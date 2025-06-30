using Autofac;
using Refit;
using SilverLeaf.Common.Interfaces;
using SilverLeaf.Screener.Admin.Services;
using System;
using System.Net.Http;

namespace SilverLeaf.Screener.Admin.Droid.IoC
{
    public class DroidModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<Localize>()
               .AsImplementedInterfaces();

            builder.Register(c =>
            {
                var api = Config.Current.GetApi("PhonicsScreener");
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(api.BaseUrl);
                var restClient = RestService.For<IPhonicsScreenerAPI>(client);
                return restClient;
            })
                .As<IPhonicsScreenerAPI>()
                .SingleInstance();

            builder.Register(c =>
            {
                var api = Config.Current.GetApi("ComprehensionScreener");
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(api.BaseUrl);
                var restClient = RestService.For<IComprehensionScreenerAPI>(client);
                return restClient;
            })
               .As<IComprehensionScreenerAPI>()
               .SingleInstance();

            builder.Register(c =>
            {
                var api = Config.Current.GetApi("OralScreener");
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(api.BaseUrl);
                var restClient = RestService.For<IOralScreenerAPI>(client);
                return restClient;
            })
               .As<IOralScreenerAPI>()
               .SingleInstance();

            builder.Register(c =>
            {
                var api = Config.Current.GetApi("PendingScreener");
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(api.BaseUrl);
                var restClient = RestService.For<IPendingScreenerAPI>(client);
                return restClient;
            })
               .As<IPendingScreenerAPI>()
               .SingleInstance();

            builder.Register(c =>
            {
                var api = Config.Current.GetApi("CompletionScreener");
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(api.BaseUrl);
                var restClient = RestService.For<ICompletionScreenerAPI>(client);
                return restClient;
            })
             .As<ICompletionScreenerAPI>()
             .SingleInstance();

            builder.Register(c =>
            {
                var api = Config.Current.GetApi("User");
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(api.BaseUrl);
                var restClient = RestService.For<IUserAPI>(client);
                return restClient;
            })
            .As<IUserAPI>()
            .SingleInstance();

            builder.Register(c =>
            {
                var api = Config.Current.GetApi("Student");
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(api.BaseUrl);
                var restClient = RestService.For<IStudentAPI>(client);
                return restClient;
            })
          .As<IStudentAPI>()
          .SingleInstance();

        }
    }
}