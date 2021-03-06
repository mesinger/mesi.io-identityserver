using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using IdentityServer4;
using IdentityServer4.Events;
using IdentityServer4.Services;
using Mesi.Io.IdentityServer4.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Mesi.Io.IdentityServer4.Pages.Account
{
    [BindProperties]
    public class LoginModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IEventService _events;
        private readonly ILogger<LoginModel> _logger;

        public LoginModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IIdentityServerInteractionService interaction,
            IEventService events,
            ILogger<LoginModel> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _interaction = interaction;
            _events = events;
            _logger = logger;
        }

        [Required(ErrorMessage = "Please enter your username")] public string UserName { get; set; }

        [Required(ErrorMessage = "Please enter your password")] public string Password { get; set; }

        public string ReturnUrl { get; set; }
        
        [ReadOnly(true)] public string CurrentUrl { get; set; }

        public IActionResult OnGet(string returnUrl)
        {
            if (string.IsNullOrWhiteSpace(returnUrl))
            {
                return BadRequest();
            }
            
            ReturnUrl = returnUrl;
            CurrentUrl = HttpContext.Request.GetDisplayUrl();

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (string.IsNullOrWhiteSpace(ReturnUrl))
            {
                return BadRequest();
            }

            var context = await _interaction.GetAuthorizationContextAsync(ReturnUrl);

            if (context == null)
            {
                _logger.LogWarning("Unable to find valid context");
                return BadRequest();
            }

            var user = await _userManager.FindByNameAsync(UserName);
            if (user == null)
            {
                await _events.RaiseAsync(new UserLoginFailureEvent(UserName, "invalid credentials",
                    clientId: context.Client.ClientId));
                ModelState.AddModelError(string.Empty, "Invalid credentials");

                return Page();
            }
            
            var signInResult = await _signInManager.PasswordSignInAsync(user, Password, false, false);
            if (!signInResult.Succeeded)
            {
                await _events.RaiseAsync(new UserLoginFailureEvent(UserName, "invalid credentials",
                    clientId: context?.Client.ClientId));
                ModelState.AddModelError(string.Empty, "Invalid credentials");

                return Page();
            }

            await _events.RaiseAsync(new UserLoginSuccessEvent(user.UserName, user.Id, user.UserName,
                clientId: context?.Client.ClientId));

            var identityServerUser = new IdentityServerUser(user.Id)
            {
                DisplayName = user.UserName
            };

            await HttpContext.SignInAsync(identityServerUser);

            if (string.IsNullOrWhiteSpace(ReturnUrl))
            {
                _logger.LogWarning("Invalid return url");
                throw new Exception("Invalid return url");
            }

            return Redirect(ReturnUrl);
        }
    }
}