using Mesi.Io.IdentityServer4.Data.Users;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Mesi.Io.IdentityServer4.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>(b =>
            {
                b.HasKey(u => u.Id);

                b.Property(u => u.UserName).HasMaxLength(20);
                b.Property(u => u.Email).HasMaxLength(100);
                
                b.ToTable("users");
            });
        }
    }
}