using Mesi.Io.IdentityServer4.Data.Users;

namespace Mesi.Io.IdentityServer4.Test.Given
{
    public partial class Given
    {
        public static ApplicationUser AApplicationUser(string name, string email) =>
            new ApplicationUser {UserName = name, Email = email};
    }
}