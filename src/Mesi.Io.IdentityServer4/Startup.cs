using IdentityServer4.Stores;
using Mesi.Io.IdentityServer4.Config;
using Mesi.Io.IdentityServer4.Data;
using Mesi.Io.IdentityServer4.Data.Entities;
using Mesi.Io.IdentityServer4.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

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
            services.AddRazorPages();
            services.AddRouting(options => options.LowercaseUrls = true);
            services.Configure<ForwardedHeadersOptions>(options => options.ForwardedHeaders = ForwardedHeaders.All);

            services.AddDbContext<ApplicationUserDbContext>(options =>
            {
                options.UseNpgsql(Configuration.GetConnectionString("UserDatabase"));
            });

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseNpgsql(Configuration.GetConnectionString("UserDatabase"));
            });
            
            services.AddIdentity<ApplicationUser, IdentityRole>(options => { })
                .AddEntityFrameworkStores<ApplicationUserDbContext>()
                .AddDefaultTokenProviders();

            var builder = services.AddIdentityServer()
                .AddInMemoryIdentityResources(IdentityServerConfig.IdentityResources)
                .AddInMemoryApiResources(IdentityServerConfig.ApiResources)
                .AddInMemoryApiScopes(IdentityServerConfig.ApiScopes)
                .AddAspNetIdentity<ApplicationUser>();
            
            services.AddScoped<ISigningCredentialStore, SigningCredentialStore>();
            services.AddScoped<IValidationKeysStore, ValidationKeyStore>();
            
            services.AddScoped<IClientStore, ClientStore>();

            services.AddScoped<IRegistrationService, IdentityRegistrationService>();
        }

        public void Configure(IApplicationBuilder app, ILogger<Startup> logger)
        {
            logger.LogInformation("----- Settings -----");
            logger.LogInformation($"Configuration: {Environment.EnvironmentName}");
            logger.LogInformation($"UserDB: {Configuration.GetConnectionString("UserDatabase")}");
            logger.LogInformation($"Private cert path: {Configuration.GetSection("Certificate:Private").Value}");
            logger.LogInformation($"Public cert path: {Configuration.GetSection("Certificate:Public").Value}");
            
            // this shouldn't be necessary as the scheme should be taken from the forwarded headers, but on aws this does not work ...
            if (!Environment.IsDevelopment())
            {
                app.Use((context, next) =>
                {
                    context.Request.Scheme = "https";
                    return next();
                });
            }

            app.UseForwardedHeaders();

            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseIdentityServer();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapGet("/heartbeat", async context => await context.Response.WriteAsync("heartbeat"));
            });
        }
    }
}