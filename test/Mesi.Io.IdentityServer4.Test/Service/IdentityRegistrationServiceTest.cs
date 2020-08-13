using System.Linq;
using Mesi.Io.IdentityServer4.Data.Users;
using Mesi.Io.IdentityServer4.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Mesi.Io.IdentityServer4.Test.Service
{
    public class IdentityRegistrationServiceTest
    {
        private readonly IdentityRegistrationService sut;
        private readonly Mock<IUserStore<ApplicationUser>> _userStore;
        private readonly Mock<UserManager<ApplicationUser>> _userManager;
        private readonly Mock<ILogger<IdentityRegistrationService>> _logger;

        public IdentityRegistrationServiceTest()
        {
            _userStore = new Mock<IUserStore<ApplicationUser>>();
            _userManager =
                new Mock<UserManager<ApplicationUser>>(_userStore.Object, null, null, null, null, null, null, null,
                    null);
            _logger = new Mock<ILogger<IdentityRegistrationService>>();

            sut = new IdentityRegistrationService(_userManager.Object, _logger.Object);
        }

        [Fact]
        public async void ItShallRegisterUser()
        {
            var request = Given.Given.ARegistrationRequest();

            _userManager
                .Setup(mgr =>
                    mgr.CreateAsync(It.Is<ApplicationUser>(u => u.UserName == request.Name && u.Email == request.Email),
                        request.Password)).ReturnsAsync(IdentityResult.Success);

            var result = await sut.RegisterUser(request);

            Assert.True(result.Succeeded);
            
            _userManager.Verify(
                mgr => mgr.CreateAsync(
                    It.Is<ApplicationUser>(u => u.UserName == request.Name && u.Email == request.Email),
                    request.Password), Times.Once);
        }

        [Fact]
        public async void ItShallNotRegisterUser()
        {
            var request = Given.Given.ARegistrationRequest();
            var errors = Given.Given.AIdentityErrorsList();

            _userManager
                .Setup(mgr =>
                    mgr.CreateAsync(It.Is<ApplicationUser>(u => u.UserName == request.Name && u.Email == request.Email),
                        request.Password)).ReturnsAsync(IdentityResult.Failed(errors));

            var result = await sut.RegisterUser(request);

            Assert.False(result.Succeeded);
            Assert.Equal(from error in errors select error.Description, result.Errors);
        }
    }
}