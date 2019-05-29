using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Bluetooth;
using Android.Support.V4.App;
using Android;

namespace TrisBluetooth.Droid
{
    [Activity(Label = "TrisBluetooth", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());
            addPermissionLocation();
            addPermissionBluetooth();
        }

        private void addPermissionLocation()
        {
            int MY_PERMISSIONS_REQUEST_ACCESS_COARSE_LOCATION = 1;
            ActivityCompat.RequestPermissions(this,
                    new String[] { Manifest.Permission.AccessCoarseLocation },
                    MY_PERMISSIONS_REQUEST_ACCESS_COARSE_LOCATION);
        }
        private void addPermissionBluetooth()
        {
            int MY_PERMISSIONS_REQUEST_BLUETOOTH_ADMIN = 1;
            ActivityCompat.RequestPermissions(this,
                    new String[] { Manifest.Permission.BluetoothAdmin },
                    MY_PERMISSIONS_REQUEST_BLUETOOTH_ADMIN);
        }
    }
}