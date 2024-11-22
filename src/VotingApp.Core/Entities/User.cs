using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VotingApp.Core.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public List<Guid> VotedElectionIds { get; set; } = new List<Guid>();

        public void MarkAsVoted(Guid electionId)
        {
            if (VotedElectionIds is null)
            {
                VotedElectionIds = new List<Guid>();
            }

            VotedElectionIds.Add(electionId);
        }

        public bool HasVoted(Guid electionId)
        {
            if (VotedElectionIds is null) return false;
            return VotedElectionIds.Any(x => x ==  electionId);
        }
    }
}