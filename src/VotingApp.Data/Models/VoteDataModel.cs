using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VotingApp.Core.Entities;

namespace VotingApp.Data.Models
{
    [FirestoreData]
    internal class VoteDataModel
    {
        [FirestoreDocumentId]
        public string Id { get; set; }
        [FirestoreProperty]
        public string UserId { get; set; }
        [FirestoreProperty]
        public string CandidateId { get; set; }
        [FirestoreProperty]
        public string ElectionId { get; set; }
        [FirestoreProperty]
        public DateTimeOffset DateTimeVoted { get; set; }


        public VoteDataModel()
        {

        }

        public VoteDataModel(Vote vote)
        {
            Id = vote.Id.ToString();
            UserId = vote.UserId.ToString();
            CandidateId = vote.CandidateId.ToString();
            ElectionId = vote.ElectionId.ToString();
            DateTimeVoted = vote.DateTimeVoted;
        }
    }
}
