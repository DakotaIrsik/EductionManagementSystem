
using Acr.UserDialogs;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Autofac;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Plugin.DownloadManager;
using Plugin.DownloadManager.Abstractions;
using SilverLeaf.Screener.Admin.Droid.IoC;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xamarin.Forms;

namespace SilverLeaf.Screener.Admin.Droid
{
    [Activity(Label = "Screener", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        public static MainActivity Instance { get; private set; }

        public override void OnBackPressed()
        {
            if (!App.BlockBack)
            {
                base.OnBackPressed();
            }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            AppCenter.Start("75913158-4498-48b3-bf6e-a23978e30fca",
                               typeof(Analytics), typeof(Crashes));

            Analytics.TrackEvent($"Class: {nameof(MainActivity)} - Method: {nameof(OnCreate)}");
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;
            base.OnCreate(savedInstanceState);
            Analytics.TrackEvent($"Class: {nameof(MainActivity)} - Method: {nameof(OnCreate)} - onCreate() Success.");

            Forms.SetFlags("CollectionView_Experimental");
            Analytics.TrackEvent($"Class: {nameof(MainActivity)} - Method: {nameof(OnCreate)} - setFlags() Success.");

            Instance = this;
            Downloaded();
            Analytics.TrackEvent($"Class: {nameof(MainActivity)} - Method: {nameof(OnCreate)} - downloaded() Success.");

            Forms.Init(this, savedInstanceState);
            Analytics.TrackEvent($"Class: {nameof(MainActivity)} - Method: {nameof(OnCreate)} - forms.Init() Success.");

            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterModule<DroidModule>();
            Analytics.TrackEvent($"Class: {nameof(MainActivity)} - Method: {nameof(OnCreate)} - containerBuilder.RegisterModule() Success.");

            UserDialogs.Init(this);
            Analytics.TrackEvent($"Class: {nameof(MainActivity)} - Method: {nameof(OnCreate)} - userDialogs.Init() Success.");

            LoadApplication(new App(containerBuilder));
            Analytics.TrackEvent($"Class: {nameof(MainActivity)} - Method: {nameof(OnCreate)} - loadApplication() Success.");

            App.DeviceScreenHeight = (int)(Resources.DisplayMetrics.HeightPixels / Resources.DisplayMetrics.Density);
            Analytics.TrackEvent($"Class: {nameof(MainActivity)} - Method: {nameof(OnCreate)} - setDeviceScreenHeight() Success.");
        }

        public void Downloaded()
        {
            CrossDownloadManager.Current.PathNameForDownloadedFile = new Func<IDownloadFile, string>(file =>
            {
                string fileName = Android.Net.Uri.Parse(file.Url).Path.Split('/').Last();
                return Path.Combine(ApplicationContext.GetExternalFilesDir(Android.OS.Environment.DirectoryDownloads).AbsolutePath, fileName);
            });
        }
    }
}