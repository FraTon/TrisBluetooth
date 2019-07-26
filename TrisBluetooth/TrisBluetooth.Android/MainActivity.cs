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
using Android.Support.V4.Content;
using Android.Content;
using Xamarin.Forms;
using System.Collections;

namespace TrisBluetooth.Droid
{
    [Activity(Label = "TrisBluetooth", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {

        // Broadcast receiver per quando si scoprono nuovi dispositivi
        private DeviceDiscoveredReceiver receiver = new DeviceDiscoveredReceiver();
        public class DeviceDiscoveredReceiver : BroadcastReceiver
        {

            public override void OnReceive(Context context, Intent intent)
            {
                string action = intent.Action;

                // When discovery finds a device send a messagge containing the description of the device
                if (action == BluetoothDevice.ActionFound)
                {
                    // Get the BluetoothDevice object from the Intent
                    BluetoothDevice device = (BluetoothDevice)intent.GetParcelableExtra(BluetoothDevice.ExtraDevice);
                    System.Console.WriteLine("Trovato: " + device.Name + "   " + device.Address);
                    devices.Add(device);
                    String[] Description = { device.Name, device.Address };
                    MessagingCenter.Send<Object, String[]>(this, "SaveDevices", Description);
                }

            }
        }

        //Array list che contiene i Device trovati
        public static ArrayList devices = new ArrayList();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());

            //Questa funzione controlla se il dispositivo ha i permessi necessari
            checkPermission();

            //Questa funzione registra i Broadcast Receiver necessari
            registerReceivers();

            //Questa funzione registra le caselle per lo scambio di messaggi fra la parte android e cross-platform
            setMailBoxes();

        }

        private void checkPermission()
        {
            const int locationPermissionsRequestCode = 1000;

            var locationPermissions = new[]
            {
                Manifest.Permission.AccessCoarseLocation,
                Manifest.Permission.AccessFineLocation
            };

            // check if the app has permission to access coarse location
            var coarseLocationPermissionGranted =
                ContextCompat.CheckSelfPermission(this, Manifest.Permission.AccessCoarseLocation);

            // check if the app has permission to access fine location
            var fineLocationPermissionGranted =
                ContextCompat.CheckSelfPermission(this, Manifest.Permission.AccessFineLocation);

            // if either is denied permission, request permission from the user
            if (coarseLocationPermissionGranted == Permission.Denied ||
                fineLocationPermissionGranted == Permission.Denied)
            {
                ActivityCompat.RequestPermissions(this, locationPermissions, locationPermissionsRequestCode);
            }
        }

        private void registerReceivers()
        {
            // Register for broadcasts when a device is discovered
            receiver = new DeviceDiscoveredReceiver();
            var filter = new IntentFilter(BluetoothDevice.ActionFound);
            RegisterReceiver(receiver, filter);

            // Register for broadcasts when discovery has finished
            filter = new IntentFilter(BluetoothAdapter.ActionDiscoveryFinished);
            RegisterReceiver(receiver, filter);
        }

        private void setMailBoxes()
        {
            MessagingCenter.Subscribe<Object, char>(this, "setDiscoverability",
                (sender, arg) =>
                {
                    AbilitaDiscoverabilty();
                });
        }

        //abilita la discoverability, se bt è spento lo accende
        private void AbilitaDiscoverabilty()
        {
            Intent discoverableIntent =
                    new Intent(BluetoothAdapter.ActionRequestDiscoverable);
            discoverableIntent.PutExtra(BluetoothAdapter.ExtraDiscoverableDuration, 30);
            StartActivity(discoverableIntent);
        }

    }


}