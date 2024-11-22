using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;
using System.Linq;
using VotingApp.Core.Entities;
using VotingApp.Core.Services.Interfaces;
using VotingApp.Core.Services.Models;
using VotingApp.Data.Models;
using static System.Collections.Specialized.BitVector32;

namespace VotingApp.Data.Repositories
{
    public class ElectionRepository : IElectionRepository
    {
        private async Task<FirestoreDb> CreateAsync()
        {
            return await FirestoreDb.CreateAsync("votingapp-21e49");
        }

        public async Task AddElectionAsync(Election  election)
        {
            var db = await CreateAsync();

            DocumentReference electionDoc = db.Collection("Elections").Document(election.Id.ToString());

            var electionData = new Dictionary<string, object>
            {
                { "Name", election.Name }
            };

            await electionDoc.SetAsync(electionData);
        }


        public async Task AddCandidateAsync(Candidate candidate)
        {
            var db = await CreateAsync();


            var candidateDataModel = new CandidateDataModel(candidate);

            DocumentReference electionDocRef = db.Collection("Elections").Document(candidateDataModel.ElectionId);

            // Reference to the candidates subcollection
            CollectionReference candidatesCollection = electionDocRef.Collection("Candidates");

            // Reference to the candidate document
            DocumentReference candidateDocRef = candidatesCollection.Document(candidateDataModel.Id);


            // Set the candidate data in Firestore
            await candidateDocRef.SetAsync(candidateDataModel);
        }

        public async Task<List<Candidate>> GetCandidatesAsync(Guid electionId)
        {
            var db = await CreateAsync();

            // Reference to the candidates subcollection
            CollectionReference candidatesCollection = db.Collection("Elections")
                .Document(electionId.ToString())
                .Collection("Candidates");

            // Fetch all candidate documents
            QuerySnapshot snapshot = await candidatesCollection.GetSnapshotAsync();

            var candidates = new List<Candidate>();
            foreach (DocumentSnapshot doc in snapshot.Documents)
            {
                if (doc.Exists)
                {
                    var candidateDM = doc.ConvertTo<CandidateDataModel>();

                    var candidate = candidateDM.ConvertToCandidate();

                    candidates.Add(candidate);
                }
            }

            return candidates;
        }

        public async Task AddUserAsync(User user)
        {
            var db = await CreateAsync();

            DocumentReference electionDoc = db.Collection("Users").Document(user.Id.ToString());
            var UserDM = new UserDataModel(user);
            await electionDoc.SetAsync(UserDM);
        }

        public async Task<AddVoteResult> AddVoteAsync(Guid userId, Guid electionId, Guid candidateId)
        {
            var db = await CreateAsync();

            // References to the necessary documents
            DocumentReference userDocRef = db.Collection("Users").Document(userId.ToString());
            DocumentReference candidateDocRef = db.Collection("Elections")
                .Document(electionId.ToString())
                .Collection("Candidates")
                .Document(candidateId.ToString());
            DocumentReference voteDocRef = db.Collection("Votes").Document($"{userId}_{electionId}");

            AddVoteResult? result = null;

            // Start a transaction
            await db.RunTransactionAsync(async transaction =>
            {
                // Fetch the user document
                DocumentSnapshot userSnapshot = await transaction.GetSnapshotAsync(userDocRef);
                if (!userSnapshot.Exists)
                {
                    throw new Exception("User does not exist.");
                }
                var userdm = userSnapshot.ConvertTo<UserDataModel>();
                var user = userdm.ConvertToUser();

                // Check if the user has already voted in this election
                if (user.HasVoted(electionId))
                {
                    result = new AddVoteResult
                    {
                        Success = false,
                        Error = "ALREADYVOTED"
                    };
                    return;
                }

                // Fetch the candidate document
                DocumentSnapshot candidateSnapshot = await transaction.GetSnapshotAsync(candidateDocRef);
                if (!candidateSnapshot.Exists)
                {
                    throw new Exception("Candidate does not exist.");
                }
                var candidateDm = candidateSnapshot.ConvertTo<CandidateDataModel>();
                var candidate = candidateDm.ConvertToCandidate();

                // Verify the candidate belongs to the specified election
                if (!candidate.BelongsToElection(electionId))
                {
                    throw new Exception("Candidate does not belong to the specified election.");
                }

                // Create the vote record
                Vote vote = new Vote
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    CandidateId = candidateId,
                    ElectionId = electionId,
                    DateTimeVoted = DateTimeOffset.UtcNow
                };

                var voteDm = new VoteDataModel(vote);

                // Store the vote record
                transaction.Set(voteDocRef, voteDm);

                // Increment the candidate's vote count
                transaction.Update(candidateDocRef, new Dictionary<string, object>
                {
                    { "Votes", FieldValue.Increment(1) }
                });

                // Update the user's VotedElectionIds list
                transaction.Update(userDocRef, new Dictionary<string, object>
                {
                    { "VotedElectionIds", FieldValue.ArrayUnion(electionId.ToString()) }
                });
            });

            if (result is null)
            {
                result = new AddVoteResult
                {
                    Success = true,
                };
            }
            return result;
        }
    }
}
