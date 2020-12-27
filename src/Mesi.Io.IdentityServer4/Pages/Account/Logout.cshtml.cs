using System.Threading.Tasks;
using IdentityServer4.Events;
using IdentityServer4.Extensions;
using IdentityServer4.Services;
using Mesi.Io.IdentityServer4.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Mesi.Io.IdentityServer4.Pages.Account
{
    public class LogoutModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IEventService _events;


        public LogoutModel(
            SignInManager<ApplicationUser> signInManager,
            IIdentityServerInteractionService interaction,
            IEventService events)
        {
            _signInManager = signInManager;
            _interaction = interaction;
            _events = events;
        }

        public async Task<IActionResult> OnGet(string logoutId)
        {
            if (string.IsNullOrWhiteSpace(logoutId))
            {
                return BadRequest();
            }
            
            var logout = await _interaction.GetLogoutContextAsync(logoutId);

            if (User?.Identity?.IsAuthenticated == true)
            {
                await _signInManager.SignOutAsync();
                await _events.RaiseAsync(new UserLogoutSuccessEvent(User.GetSubjectId(), User.GetDisplayName()));
            }

            var redirectUri = logout?.PostLogoutRedirectUri;
            
            return string.IsNullOrWhiteSpace(redirectUri) 
                ? Page() 
                : Redirect(redirectUri);
        }
    }
}