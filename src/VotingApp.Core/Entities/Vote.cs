using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VotingApp.Core.Entities
{
    public class Vote
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid CandidateId { get; set; }
        public Guid ElectionId { get; set; }
        public DateTimeOffset DateTimeVoted { get; set; }
        public string Province { get; set; }
    }
}
