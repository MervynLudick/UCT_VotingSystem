using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VotingApp.Core.Entities;

namespace VotingApp.Data.Models
{
    [FirestoreData]
    internal class UserDataModel
    {
        [FirestoreDocumentId]
        public string Id { get; set; }
        [FirestoreProperty]
        public List<string> VotedElectionIds { get; set; }

        internal UserDataModel() { }
        public UserDataModel(User user)
        {
            Id = user.Id.ToString();
            VotedElectionIds = user.VotedElectionIds.Select(x => x.ToString()).ToList();
        }
        public User ConvertToUser()
        {
            return new User
            {
                Id = Guid.Parse(Id),
                VotedElectionIds = VotedElectionIds.Select(x => Guid.Parse(x)).ToList(),
            };
        }
    }
}
