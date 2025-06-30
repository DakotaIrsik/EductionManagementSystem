using Autofac;
using SilverLeaf.Common.Interfaces;
using SilverLeaf.Common.Models;
using SilverLeaf.Screener.Admin.Services;
using SilverLeaf.Screener.Admin.Services.Interfaces;
using SilverLeaf.Screener.Admin.Views.Renderers;
using System.Reflection;
using Module = Autofac.Module;

namespace SilverLeaf.Screener.Admin.IoC
{
    public class CoreModule : Module
    {
        private static Assembly Assembly => typeof(CoreModule).GetTypeInfo().Assembly;

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            // Register services.

            builder.RegisterType<UserService>()
                .As<IUserService>()
                .SingleInstance();

            builder.RegisterType<StudentService>()
                .As<IStudentService>()
                .SingleInstance();

            builder.RegisterType<PhonicsScreenerService>()
                .As<IPhonicsScreenerService>()
                .SingleInstance();

            builder.RegisterType<ComprehensionScreenerService>()
               .As<IComprehensionScreenerService>()
               .SingleInstance();

            builder.RegisterType<OralScreenerService>()
               .As<IOralScreenerService>()
               .SingleInstance();

            builder.RegisterType<PendingScreenerService>()
                .As<IPendingScreenerService>()
                .SingleInstance();

            builder.RegisterType<Navigator>()
               .As<INavigator>()
               .SingleInstance();

            builder.RegisterType<Alerting>()
                .As<IAlerting>()
                .SingleInstance();

            builder.RegisterType<Processing>()
                .As<IProcessing>()
                .SingleInstance();

            builder.RegisterType<Reporting>()
                .As<IReporting>()
                .SingleInstance();

            builder.RegisterType<Reporting>()
                .As<IReporting>()
                .SingleInstance();

            builder.RegisterType<Adjustable>()
                .As<IAdjustable>()
                .SingleInstance();

            builder.RegisterType<ProgressRenderer>()
                .As<IRenderer>()
                .SingleInstance();

            builder.RegisterType<CompletionScreenerService>()
                .As<ICompletionScreenerService>()
                .SingleInstance();

            // Register view models.
            builder
                .RegisterAssemblyTypes(Assembly)
                .Where(x => x.Name.EndsWith("ViewModel"))
                .AsSelf();
        }
    }
}
