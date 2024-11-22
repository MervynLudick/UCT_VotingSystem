using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VotingApp.Core.Entities;
using VotingApp.Core.Services.Models;

namespace VotingApp.Core.Services.Interfaces
{
    public interface IElectionRepository
    {
        Task AddElectionAsync(Election election);
        Task AddCandidateAsync(Candidate candidate);
        Task<List<Candidate>> GetCandidatesAsync(Guid electionId);
        Task AddUserAsync(User user);
        Task<AddVoteResult> AddVoteAsync(Guid userId, Guid electionId, Guid candidateId);
    }
}
