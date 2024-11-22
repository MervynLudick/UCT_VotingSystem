using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VotingApp.Core.Entities
{
    public class Candidate
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public required Guid ElectionId { get; set; }
        public required string Name { get; set; }
        public required string Slogan { get; set; }
        public required string Bio { get; set; }
        public required string ImageUrl { get; set; }
        public int Votes { get; set; }
        
        public void AddVote()
        {
            Votes++;
        }

        public bool BelongsToElection(Guid electionId)
        {
            return electionId == ElectionId;
        }

        public int GetVotePercentage(int totalVotesCasted) 
        {
            return (int)Math.Round((double)Votes / totalVotesCasted * 100.0);
        }
    }
}