using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VotingApp.Core.Entities
{
    public class Election
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public required string Name { get; set; }
        public List<Candidate> Candidates { get; set; }
        public int Population { get; } = 100;

        public int GetTotalVotes()
        {
            if (Candidates == null) return 0;

            return Candidates.Sum(c => c.Votes);
        }

        public int GetPercentageOfVotes()
        {
            var totalVotes = GetTotalVotes();

            var total = (double)totalVotes / Population * 100.0;

            return (int)Math.Round(total);
        }
    }
}