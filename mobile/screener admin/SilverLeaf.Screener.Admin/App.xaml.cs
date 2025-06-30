using Autofac;
using SilverLeaf.Screener.Admin.IoC;
using SilverLeaf.Screener.Admin.Views;
using System.Reflection;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace SilverLeaf.Screener.Admin
{
    public partial class App : Application
    {
        public App(ContainerBuilder containerBuilder)
        {
            var assembly = typeof(App).GetTypeInfo().Assembly;
            foreach (var res in assembly.GetManifestResourceNames())
            {
                System.Diagnostics.Debug.WriteLine("found resource: " + res);
            }

            InitializeComponent();
            containerBuilder.RegisterModule<CoreModule>();
            Container = containerBuilder.Build();
            Current.MainPage = new HomePage();
        }

        public static IContainer Container { get; private set; }
        public static bool BlockBack { get; set; }
        public static int DeviceScreenHeight { get; set; }
    }
}
