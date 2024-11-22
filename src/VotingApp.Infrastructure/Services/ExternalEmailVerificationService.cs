using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using VotingApp.Core.Services.Interfaces;
using VotingApp.Core.Services.Models;

namespace VotingApp.Infrastructure.Services
{
    public class ExternalEmailVerificationService : IEmailValidation
    {
        private static string BaseUrl = "https://api.usercheck.com";
        private static string apiKey = "2oe4NCOALA7IvJkbFMeAgvu8ie5bpksv";

        
        public async Task<EmailVerificationResult> ValidateEmailAddress(string email)
        {
            using var client = new HttpClient();

            var response = await client.GetAsync($"{BaseUrl}/email/{email}?key={apiKey}");

            if (response.IsSuccessStatusCode)
            {
                var SuccessResponse = await response.Content.ReadFromJsonAsync<EmailVerificationResponse>();

                return new EmailVerificationResult
                {
                    Email = email,
                    Success = true,
                    IsTemporary = SuccessResponse.Disposable
                };
            }

            else
            {
                var errorResponse = await response.Content.ReadFromJsonAsync<EmailVerifcationResponseError>();

                return new EmailVerificationResult
                {
                    Email = email,
                    Success = false,
                    Error = errorResponse.Error,
                    IsTemporary = false
                };
            }
        }
        
    }

    public class EmailVerificationResponse
    {
        [JsonPropertyName("status")]
        public int Status { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("domain")]
        public string Domain { get; set; }

        [JsonPropertyName("mx")]
        public bool Mx { get; set; }

        [JsonPropertyName("disposable")]
        public bool Disposable { get; set; }

        [JsonPropertyName("public_domain")]
        public bool PublicDomain { get; set; }

        [JsonPropertyName("alias")]
        public bool Alias { get; set; }

        [JsonPropertyName("did_you_mean")]
        public string DidYouMean { get; set; }
    }

    public class EmailVerifcationResponseError
    {
        [JsonPropertyName("status")]
        public int Status { get; set; }
        [JsonPropertyName("error")]
        public string Error { get; set; }
    }
}
