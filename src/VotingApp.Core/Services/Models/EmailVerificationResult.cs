using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VotingApp.Core.Services.Models
{
    public class EmailVerificationResult
    {
        public string Email { get; set; }
        public bool Success { get; set; }
        public bool IsTemporary { get; set; }
        public string? Error { get; set; }
    }
}
