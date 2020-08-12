using Mesi.Io.IdentityServer4.Config;
using Mesi.Io.IdentityServer4.Data;
using Mesi.Io.IdentityServer4.Data.Users;
using Mesi.Io.IdentityServer4.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

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
            Log.Information($"UserDB: {Configuration.GetSection("UserDatabase:ConnectionString").Value}");
            Log.Information($"Private cert: {Configuration.GetSection("Certificate:Private").Value}");
            Log.Information($"Public cert: {Configuration.GetSection("Certificate:Public").Value}");
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
                
                options.AddPolicy("default", policy =>
                {
                    policy.WithOrigins("https://mesi.io")
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                if (Environment.IsDevelopment())
                {
                    options.UseSqlite(Configuration.GetSection("UserDatabase:ConnectionString").Value);
                }
            });   

            services.AddIdentity<ApplicationUser, IdentityRole>(options => { })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddIdentityServer()
                .AddInMemoryIdentityResources(IdentityServerConfig.IdentityResources)
                .AddInMemoryApiResources(IdentityServerConfig.ApiResources)
                .AddInMemoryApiScopes(IdentityServerConfig.ApiScopes)
                .AddInMemoryClients(Configuration.GetSection("IdentityServer:Clients"))
                .AddAspNetIdentity<ApplicationUser>();

            services.AddApplicationDependencies();

            services.Configure<CertificateOptions>(Configuration.GetSection("Certificate"));
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
            else
            {
                app.UseCors("default");
            }

            app.UseIdentityServer();

            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapGet("/heartbeat", async context => await context.Response.WriteAsync("heartbeat"));
            });
        }
    }
}