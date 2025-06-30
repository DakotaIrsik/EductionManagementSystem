using Autofac;
using Foundation;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Plugin.DownloadManager;
using Plugin.DownloadManager.Abstractions;
using SilverLeaf.Screener.iOS.IoC;
using System;
using System.IO;
using UIKit;

namespace SilverLeaf.Screener.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            AppCenter.Start("0539fb4c-373f-4737-90d4-4087231cf3c0",
                   typeof(Analytics), typeof(Crashes));

            global::Xamarin.Forms.Forms.SetFlags("CollectionView_Experimental");
            global::Xamarin.Forms.Forms.Init();
            Downloaded();
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterModule<iOSModule>();
            LoadApplication(new App(containerBuilder));
            App.DeviceScreenHeight = (int)UIScreen.MainScreen.Bounds.Height;
            return base.FinishedLaunching(app, options);
        }

        public void Downloaded()
        {
            CrossDownloadManager.Current.PathNameForDownloadedFile = new Func<IDownloadFile, string>(file =>
            {
                string fileName = (new NSUrl(file.Url, false)).LastPathComponent;
                return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), fileName);
            });
        }
    }
}
