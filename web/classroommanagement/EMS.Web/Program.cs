using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Serilog;

namespace EMS.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateWebHostBuilder(args);
            //shouldSeed = true;
            //shouldDeleteStaticFiles = true;
            //await DbInitializer.Seed(host, shouldSeed, shouldDeleteStaticFiles);
            host.Run();
        }

        public static IWebHost CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
             .UseSerilog((ctx, config) => { config.ReadFrom.Configuration(ctx.Configuration); })
             .UseStartup<Startup>()
            .Build();
    }
}
