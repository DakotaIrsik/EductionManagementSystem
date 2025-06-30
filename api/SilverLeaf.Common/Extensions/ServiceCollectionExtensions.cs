using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.OpenApi.Models;
using Neleus.DependencyInjection.Extensions;
using Refit;
using Serilog;
using SilverLeaf.common.Services;
using SilverLeaf.Common.Models.Images;
using SilverLeaf.Common.Models.Images.Interfaces;
using SilverLeaf.Common.Models.Images.Watermarks;
using SilverLeaf.Common.Services;
using System;
using System.IO;

namespace SilverLeaf.Common.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAppSettings(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AppSettings>(configuration);
            return services;
        }

        public static IServiceCollection AddElasticSearch(this IServiceCollection services)
        {
            return services.AddScoped<IElasticSearchService, ElasticSearchService>();
        }

        public static IServiceCollection AddLogging(this IServiceCollection services, IConfigurationRoot configuration)
        {

            services.AddSingleton((Serilog.ILogger)new LoggerConfiguration()
                                                        .MinimumLevel.Information()
                                                        .ReadFrom.Configuration(configuration)
                                                        .CreateLogger());

            return services;
        }

        public static void UseVirtualDirectory(this IApplicationBuilder app, string virtualDirectory, string alias)
        {
            if (!Directory.Exists(virtualDirectory))
            {
                Directory.CreateDirectory(virtualDirectory);
            }

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(virtualDirectory),
                RequestPath = $"/{alias}"
            });
        }


        public static IServiceCollection AddRefit<T>(this IServiceCollection services, string url, TimeSpan? timeout = null) where T : class
        {
            services.AddRefitClient<T>()
                .ConfigureHttpClient(c =>
                {
                    c.BaseAddress = new Uri(url);
                    if (timeout != null)
                    {
                        c.Timeout = (TimeSpan)timeout;
                    }
                });

            return services;
        }

        public static IServiceCollection AddImaging(this IServiceCollection services)
        {
            services.AddSingleton<IImageService, ImageService>();
            services.AddSingleton<IWatermark, Watermark>();
            services.AddTransient<Thumbnail>();
            services.AddTransient<StandardDefinition>();
            services.AddTransient<HighDefinition>();
            services.AddTransient<Original>();
            services.AddTransient<Card>();
            services.AddByName<IApplicationImage>()
                .Add<Thumbnail>(nameof(Thumbnail))
                .Add<StandardDefinition>(nameof(StandardDefinition))
                .Add<HighDefinition>(nameof(HighDefinition))
                .Add<Original>(nameof(Original))
                .Add<Card>(nameof(Card))
                .Build();
            return services;
        }

        public static IServiceCollection AddImageSharp(this IServiceCollection services)
        {
            services.AddScoped<IImageSharpService, ImageSharpService>();
            return services;
        }

        // TODO Move this to a net core 3 project.
        #region IdentityServer
        //public static IServiceCollection AddIdentityServerToWebApi(this IServiceCollection services, AppSettings settings)
        //{
        //    services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
        //            .AddIdentityServerAuthentication(options =>
        //            {
        //                options.Authority = settings.ConnectionStrings.IdentityServer;
        //                options.ApiName = $"{settings.Suite}.{settings.Name}.API";
        //                options.SupportedTokens = SupportedTokens.Jwt;
        //                options.RequireHttpsMetadata = (settings.Environment != "Development");
        //            });

        //    return services;
        //}

        //public static IServiceCollection AddIdentityServerToWebClient(this IServiceCollection services, AppSettings settings)
        //{
        //    JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
        //    services
        //        .AddAuthentication(options =>
        //        {
        //            options.DefaultScheme = settings.DefaultScheme;
        //            options.DefaultChallengeScheme = settings.DefaultChallengeScheme;
        //        })
        //        .AddCookie(settings.DefaultScheme)
        //        .AddOpenIdConnect(settings.DefaultChallengeScheme, options =>
        //        {
        //            options.SignInScheme = settings.DefaultScheme;
        //            options.Authority = settings.ConnectionStrings.IdentityServer;
        //            options.RequireHttpsMetadata = false;
        //            options.ClientId = $"{settings.Suite}.{settings.Name}";
        //            options.ResponseType = "code id_token";
        //            options.SaveTokens = true;
        //            options.Scope.Clear();
        //            options.Scope.Add(AuthenticationConsts.ScopeOpenId);
        //            options.Scope.Add(AuthenticationConsts.ScopeProfile);
        //            options.Scope.Add(AuthenticationConsts.ScopeRoles);
        //            options.GetClaimsFromUserInfoEndpoint = true;
        //            options.TokenValidationParameters = new TokenValidationParameters
        //            {
        //                NameClaimType = "name",
        //                RoleClaimType = "role",
        //            };
        //            options.ClaimActions.MapJsonKey("role", "role");
        //        });
        //    return services;
        //}
        #endregion

        public static IServiceCollection AddHttpContext(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            //services.AddTransient(
            //    provider => provider.GetService<IHttpContextAccessor>().HttpContext.User);

            return services;
        }

        public static IServiceCollection AddNoCachingService(this IServiceCollection services)
        {
            services.AddScoped<ICacheService, NoCacheService>();
            return services;
        }

        public static IServiceCollection AddMemoryCaching(this IServiceCollection services)
        {
            //services.AddScoped<IMemoryCache, MemoryCacheService>();
            services.AddScoped<ICacheService, MemoryCacheService>();
            return services;
        }

        //private static IServiceCollection AddRedisCachingService(this IServiceCollection services)
        //{
        //    services.AddScoped<ICacheService, RedisCacheService>();
        //    return services;
        //}

        public static IServiceCollection AddSwagger(this IServiceCollection services, AppSettings settings)
        {
            services.AddSwaggerGen(c =>
            {

                c.SwaggerDoc("v1", new OpenApiInfo { Title = $"{settings.Suite}.{settings.Name} - {settings.Environment}", Version = settings.Version });
                //c.AddSecurityDefinition("Bearer",
                //    new ApiKeyScheme
                //    {
                //        In = "header",
                //        Description = "Please enter into field the word 'Bearer' following by space and JWT",
                //        Name = "Authorization",
                //        Type = "apiKey"
                //    });
                //c.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>> {
                //            { "Bearer", Enumerable.Empty<string>() },
                //    });
            });
            return services;
        }
    }
}
