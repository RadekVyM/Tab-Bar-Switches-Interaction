﻿using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;

namespace TabBarSwitches.Droid
{
    [Activity(Label = "TabBarSwitches", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        public static MainActivity Current { get; set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            Current = this;

            Window.AddFlags(WindowManagerFlags.LayoutNoLimits);

            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        public override void OnWindowFocusChanged(bool hasFocus)
        {
            base.OnWindowFocusChanged(hasFocus);
            if (hasFocus)
                HideSoftwareMenuBars();
        }

        private void HideSoftwareMenuBars()
        {
            int uiOptions = (int)Window.DecorView.SystemUiVisibility;

            uiOptions |= (int)SystemUiFlags.LayoutHideNavigation;
            uiOptions |= (int)SystemUiFlags.HideNavigation;
            uiOptions |= (int)SystemUiFlags.ImmersiveSticky;

            Window.DecorView.SystemUiVisibility = (StatusBarVisibility)uiOptions;
        }
    }
}