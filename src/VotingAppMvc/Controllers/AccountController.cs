using Auth0.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VotingApp.Core.Entities;
using VotingApp.Core.Services.Interfaces;

namespace VotingAppMvc.Controllers
{
    public class AccountController : Controller
    {
        private readonly IEmailValidation _emailValidation;
        private readonly IElectionRepository _electionRepository;

        public AccountController(IEmailValidation emailValidation, IElectionRepository electionRepository)
        {
            _emailValidation = emailValidation;
            _electionRepository = electionRepository;
        }

        public async Task Login(string returnUrl = "/")
        {
            var authenticationProperties = new LoginAuthenticationPropertiesBuilder()
                // Indicate here where Auth0 should redirect the user after a login.
                // Note that the resulting absolute Uri must be added to the
                // **Allowed Callback URLs** settings for the app.
                .WithRedirectUri(returnUrl)
                .Build();

            await HttpContext.ChallengeAsync(Auth0Constants.AuthenticationScheme, authenticationProperties);
        }

        [Authorize]
        public async Task Logout()
        {
            var authenticationProperties = new LogoutAuthenticationPropertiesBuilder()
              // Indicate here where Auth0 should redirect the user after a logout.
              // Note that the resulting absolute Uri must be added to the
              // **Allowed Logout URLs** settings for the app.
              .WithRedirectUri(Url.Action("Index", "Home"))
              .Build();

            // Logout from Auth0
            await HttpContext.SignOutAsync(
              Auth0Constants.AuthenticationScheme,
              authenticationProperties
            );
            // Logout from the application
            await HttpContext.SignOutAsync(
              CookieAuthenticationDefaults.AuthenticationScheme
            );
        }

        [HttpPost]
        public async Task<IActionResult> ValidateEmail([FromBody] ValidateEmailRequest request)
        {
            var response = await _emailValidation.ValidateEmailAddress(request.Email);

            return Ok(response);
        }

        public async Task<IActionResult> CreateUser()
        {
            var t = User;
            var user = new User
            {
                Id = Guid.NewGuid(),
            };

            await _electionRepository.AddUserAsync(user);

            return Ok(user.Id.ToString());
        }

    }


    public class ValidateEmailRequest
    {
        public string Email { get; set; }
    }
}
