using Mesi.Io.IdentityServer4.Config;
using Mesi.Io.IdentityServer4.Data;
using Mesi.Io.IdentityServer4.Data.Users;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Mesi.Io.IdentityServer4
{
    public class Startup
    {
        public IWebHostEnvironment Environment { get; }
        public IConfiguration Configuration { get; }

        public Startup(IWebHostEnvironment environment, IConfiguration configuration)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            
            services.AddRouting(options => options.LowercaseUrls = true);
            
            services.AddCors(options =>
            {
                options.AddPolicy("dev", policy =>
                {
                    policy.WithOrigins("http://localhost:8080")
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });
            
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlite(Configuration.GetSection("UserDatabase:ConnectionString").Value);
            });
            
            services.AddIdentity<ApplicationUser, IdentityRole>(options => { })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            var builder = services.AddIdentityServer()
                .AddInMemoryIdentityResources(IdentityServerConfig.IdentityResources)
                .AddInMemoryApiResources(IdentityServerConfig.ApiResources)
                .AddInMemoryApiScopes(IdentityServerConfig.ApiScopes)
                .AddInMemoryClients(IdentityServerConfig.Clients)
                .AddAspNetIdentity<ApplicationUser>();
                // .AddTestUsers(TestUsers.Users);

            builder.AddDeveloperSigningCredential();

            services.AddApplicationDependencies();
        }

        public void Configure(IApplicationBuilder app)
        {
            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseRouting();

            if (Environment.IsDevelopment())
            {
                app.UseCors("dev");
            }

            app.UseIdentityServer();

            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}