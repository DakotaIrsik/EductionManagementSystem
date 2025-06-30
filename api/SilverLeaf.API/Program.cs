using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Serilog;
using SilverLeaf.API.Helpers;
using System.Linq;
using System.Threading.Tasks;

namespace SilverLeaf.API
{
    public static class Program
    {
        private const string RecreateArgs = "/recreate";

        private const string Seed = "/seed";

        public static async Task Main(string[] args)
        {
            var recreate = args.Any(x => x == RecreateArgs);
            var seed = args.Any(x => x == Seed);
            if (recreate)
            {
                args = args.Except(new[] { RecreateArgs }).ToArray();
            }

            if (seed)
            {
                args = args.Except(new[] { Seed }).ToArray();
            }

            var host = BuildWebHost(args);
           // recreate = true;
            //seed = true;
            await DbInitializer.Seed(host, recreate, seed).ConfigureAwait(false);
            host.Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseIIS()
                .UseSerilog((ctx, config) => { config.ReadFrom.Configuration(ctx.Configuration); })
                .UseStartup<Startup>()
                .Build();
    }
}
