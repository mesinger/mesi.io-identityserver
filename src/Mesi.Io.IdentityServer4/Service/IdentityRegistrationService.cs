using System.Linq;
using System.Threading.Tasks;
using Mesi.Io.IdentityServer4.Data.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Mesi.Io.IdentityServer4.Service
{
    /// <inheritdoc />
    public class IdentityRegistrationService : IRegistrationService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<IdentityRegistrationService> _logger;

        public IdentityRegistrationService(UserManager<ApplicationUser> userManager, ILogger<IdentityRegistrationService> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        /// <inheritdoc />
        public async Task<RegistrationResult> RegisterUser(RegistrationRequest request)
        {
            var user = new ApplicationUser
            {
                UserName = request.Name,
                Email = request.Email
            };

            var createdResult = await _userManager.CreateAsync(user, request.Password);

            if (createdResult.Succeeded)
            {
                _logger.LogInformation("Registered user '{name}' with email '{email}'", request.Name, request.Email);
            }

            return createdResult.Succeeded
                ? RegistrationResult.SuccessFul()
                : RegistrationResult.Failure(from error in createdResult.Errors select error.Description);
        }
    }
}