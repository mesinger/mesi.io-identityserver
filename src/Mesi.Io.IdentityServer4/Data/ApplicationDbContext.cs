using System;
using Mesi.Io.IdentityServer4.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Mesi.Io.IdentityServer4.Data
{
    public class ApplicationUserDbContext : IdentityDbContext<ApplicationUser>
    { 
        public ApplicationUserDbContext(DbContextOptions<ApplicationUserDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>(b =>
            {
                b.HasKey(u => u.Id);

                b.Property(u => u.UserName)
                    .HasMaxLength(20)
                    .IsRequired();
                
                b.Property(u => u.Email)
                    .HasMaxLength(100)
                    .IsRequired();

                b.Property(u => u.PasswordHash).IsRequired();

                b.HasIndex(u => new {u.UserName}).IsUnique();
                b.HasIndex(u => new {u.Email}).IsUnique();
                
                b.ToTable("users");
            });
        }
    }

    public class ApplicationDbContext : DbContext
    {
        public DbSet<IdentityServerClient> IdentityServerClients { get; set; }

        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<IdentityServerClient>(b =>
            {
                b.HasKey(c => c.Id);

                b.Property(c => c.ClientId)
                    .HasMaxLength(50)
                    .IsRequired();
                
                b.Property(c => c.ClientName)
                    .HasMaxLength(100)
                    .IsRequired();

                b.Property(c => c.AllowedGrantTypes)
                    .HasConversion(
                        list => string.Join(";", list),
                        s => s.Split(';', StringSplitOptions.RemoveEmptyEntries));

                b.Property(c => c.RequireClientSecret)
                    .HasDefaultValue(true);
                
                b.Property(c => c.ClientSecrets)
                    .HasConversion(
                        list => string.Join(";", list),
                        s => s.Split(';', StringSplitOptions.RemoveEmptyEntries));

                b.Property(c => c.AccessTokenLifetime)
                    .HasDefaultValue(300);
                
                b.Property(c => c.RedirectUris)
                    .HasConversion(
                        list => string.Join(";", list),
                        s => s.Split(';', StringSplitOptions.RemoveEmptyEntries));

                b.Property(c => c.PostLogoutRedirectUris)
                    .HasConversion(
                        list => string.Join(";", list),
                        s => s.Split(';', StringSplitOptions.RemoveEmptyEntries));

                b.Property(c => c.AllowedScopes)
                    .HasConversion(
                        list => string.Join(";", list),
                        s => s.Split(';', StringSplitOptions.RemoveEmptyEntries));

                b.HasIndex(c => c.ClientId).IsUnique();

                b.ToTable("identityserver_clients");
            });
        }
    }
}