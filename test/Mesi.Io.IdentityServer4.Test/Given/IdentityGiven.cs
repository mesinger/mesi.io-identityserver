using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Mesi.Io.IdentityServer4.Test.Given
{
    public partial class Given
    {
        public static IdentityError[] AIdentityErrorsList() =>
            new[]
            {
                new IdentityError
                {
                    Code = "code1",
                    Description = "description1"
                },
                new IdentityError
                {
                    Code = "code2",
                    Description = "description2"
                },
            };
    }
}