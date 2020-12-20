using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using Mesi.Io.IdentityServer4;
using Microsoft.Extensions.Configuration;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Month)
    .CreateLogger();
try
{
    Log.Information("Starting host...");
    CreateHostBuilder(args).Build().Run();
    return 0;
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly.");
    return 1;
}
finally
{
    Log.CloseAndFlush();
}

static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .ConfigureAppConfiguration(config => { config.AddEnvironmentVariables("MESI_IO_IDENTITY_SERVER_"); })
        .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });