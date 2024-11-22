using System.Security.Claims;

namespace VotingAppMvc.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static string? GetUserId(this ClaimsPrincipal user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            return user.Claims.FirstOrDefault(x => x.Type.Contains("firebaseId"))?.Value;
        }

        public static string? GetFullName(this ClaimsPrincipal user) 
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            return user.Claims.FirstOrDefault(x => x.Type.Contains("full_name"))?.Value;
        }
    }
}
