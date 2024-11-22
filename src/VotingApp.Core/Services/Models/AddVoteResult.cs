using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VotingApp.Core.Services.Models
{
    public class AddVoteResult
    {
        public bool Success { get; set; }
        public string? Error { get; set; }
    }
}
