using Mesi.Io.IdentityServer4.Service;

namespace Mesi.Io.IdentityServer4.Test.Given
{
    public partial class Given
    {
        public static RegistrationRequest ARegistrationRequest() => new RegistrationRequest("username", "user@email.com", "password");
    }
}