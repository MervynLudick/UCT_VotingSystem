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
    internal class CandidateDataModel
    {
        [FirestoreDocumentId]
        public string Id { get; set; }
        [FirestoreProperty]
        public string ElectionId { get; set; }
        [FirestoreProperty]
        public string Name { get; set; }
        [FirestoreProperty]
        public string Slogan { get; set; }
        [FirestoreProperty]
        public string Bio { get; set; }
        [FirestoreProperty]
        public string ImageUrl { get; set; }
        [FirestoreProperty]
        public int Votes { get; set; }

        private CandidateDataModel()
        {

        }

        internal CandidateDataModel(Candidate candidate)
        {
            Id = candidate.Id.ToString();
            ElectionId = candidate.ElectionId.ToString();
            Name = candidate.Name;
            Slogan = candidate.Slogan;
            Bio = candidate.Bio;
            ImageUrl = candidate.ImageUrl;
            Votes = candidate.Votes;
        }

        internal Candidate ConvertToCandidate()
        {
            return new Candidate
            {
                Bio = Bio,
                ElectionId = Guid.Parse(ElectionId),
                Name = Name,
                Slogan = Slogan,
                ImageUrl = ImageUrl,
                Votes = Votes,
                Id = Guid.Parse(Id)
            };
        }
    }
}
