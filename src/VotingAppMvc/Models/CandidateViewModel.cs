using VotingApp.Core.Entities;

namespace VotingAppMvc.Models
{
    public class CandidateViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Slogan { get; set; }
        public string Bio { get; set; }
        public string ImageUrl { get; set; }
        public int Votes { get; set; }
        public int PercentagOfVotes { get; set; }


        public static CandidateViewModel MapFromCandidate(Candidate candidate, int numberOfVotesCasted)
        {
            return new CandidateViewModel
            {
                Id = candidate.Id,
                Name = candidate.Name,
                Slogan = candidate.Slogan,
                Bio = candidate.Bio,
                ImageUrl = candidate.ImageUrl,
                Votes = candidate.Votes,
                PercentagOfVotes = candidate.GetVotePercentage(numberOfVotesCasted)
            };
        }

        public static List<CandidateViewModel> MapFromListOfCandidates(IEnumerable<Candidate> candidates, int numberOfVotesCasted)
        {
            var candidatesvm = new List<CandidateViewModel>();

            foreach (var candidate in candidates)
            {
                candidatesvm.Add(MapFromCandidate(candidate, numberOfVotesCasted));
            }

            return candidatesvm;
        }
    }


}
