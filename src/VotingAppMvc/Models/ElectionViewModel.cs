namespace VotingAppMvc.Models
{
    public class ElectionViewModel
    {
        public string ElectionName { get; set; }
        public Guid ElectionId { get; set; }
        public List<CandidateViewModel> Candidates { get; set; }
        public int TotalVotesCast { get; set; }
        public int Population { get; set; }
        public int PercentagePopulatioVoted { get; set; }
    }
}
