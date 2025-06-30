using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Serilog;
using SilverLeaf.API.Extensions;
using SilverLeaf.API.Interfaces;
using SilverLeaf.Common;
using SilverLeaf.Common.Constants;
using SilverLeaf.Common.Extensions;
using SilverLeaf.CommonNetCore;
using SilverLeaf.CommonWeb.Extensions;
using SilverLeaf.CommonWeb.Hubs;
using SilverLeaf.Core.Mapping;
using SilverLeaf.Entities.Models;
using System.IO;

namespace SilverLeaf.API
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; }
        private AppSettings Settings => Configuration.Get<AppSettings>();
        public IWebHostEnvironment HostingEnvironment { get; }
        public Startup(IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                    .SetBasePath(env?.ContentRootPath)
                    .AddEnvironmentVariables()
                    .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);
            //File.WriteAllText($"appsettings.{env.EnvironmentName}", "Sanity check for environment. No use.");
            Configuration = builder.Build();
            HostingEnvironment = env;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSignalR();
            services.AddHttpContext();
            services.AddAppSettings(Configuration);
            services.AddCrossOriginPolicy(CrossOrigins.Policies.Loose, Settings);
            services.AddLogging(Configuration);
            services.AddAutoMapper(typeof(MappingProfile)); 
            services.AddElasticSearch();
            services.AddRefit<IEMSGeneralAPI>(Settings.ConnectionStrings.GeneralApi, Settings.Timers.Apis.General);
            services.AddRefit<ISeedEMSGeneralAPI>(Settings.ConnectionStrings.GeneralApi);
            services.AddDbContext<EMSContext>(options => options.UseSqlServer(Settings.ConnectionStrings.MSSQL));
            services.AddBusinessLogic();
            services.AddSwagger(Configuration.Get<AppSettings>());
            //TODO add this back when moving identity server from .net standard library.
            //services.AddIdentityServerToWebApi(Configuration.Get<AppSettings>());
            services.AddNoCachingService();
            services.AddControllers();
        }
        public void Configure(IApplicationBuilder app)
        {
            if (!Directory.Exists(Settings.StaticFilePath))
            {
                Directory.CreateDirectory(Settings.StaticFilePath);
            }

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Settings.StaticFilePath),
                RequestPath = Settings.StaticFileAlias
            });
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", $"{Settings.Suite}.{Settings.Name} - {Settings.Environment} {Settings.Version}");
                c.RoutePrefix = string.Empty;
            });
            app.UseMiddleware(typeof(ErrorHandlingMiddleware));
            app.UseAuthentication();
            app.UseSerilogRequestLogging();
            app.UseRouting();
            //WARNING With endpoint routing, the CORS middleware must be configured to execute between the calls to UseRouting 
            //and UseEndpoints. Incorrect configuration will cause the middleware to stop functioning correctly.
            //https://docs.microsoft.com/en-us/aspnet/core/security/cors?view=aspnetcore-3.0
            app.UseCors(CrossOrigins.Policies.Loose);
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<ChatHub>("/chat");
            });
        }
    }
}
