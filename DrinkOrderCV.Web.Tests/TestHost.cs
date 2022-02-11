using DrinkOrderCV.Core.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;
using System;

namespace DrinkOrderCV.Web.Tests
{
    public class TestHost
    {
        public static IHostBuilder CreateHost()
        {
            Environment.SetEnvironmentVariable("DOTNET_ENVIRONMENT", "Testing");
            return Host.CreateDefaultBuilder()
                .ConfigureWebHost(configure=>
                {
                    configure.ConfigureServices(services =>
                    {
                        services.AddServices();
                        services.AddAppMapping();
                    });
                })
                .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseTestServer();
                webBuilder.UseStartup<Startup>();
            });
        }
    }
}