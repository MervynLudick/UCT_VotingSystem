using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VotingApp.Core.Services.Models;

namespace VotingApp.Core.Services.Interfaces
{
    public interface IEmailValidation
    {
        Task<EmailVerificationResult> ValidateEmailAddress(string email);
    }
}
