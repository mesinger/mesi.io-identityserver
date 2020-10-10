using System;
using Microsoft.AspNetCore.Identity;

namespace Mesi.Io.IdentityServer4.Data.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public DateTime RegisteredAt { get; set; }
        public DateTime LastModified { get; set; }
    }
}