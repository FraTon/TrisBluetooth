using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Bluetooth;
using Java.Util;
using System.Threading.Tasks;
using Java.IO;
using TrisBluetooth.Droid;
using System.Threading;
using System.Collections.ObjectModel;
using Java.Lang;
using Java.Nio.Charset;

[assembly: Xamarin.Forms.Dependency(typeof(Bth))]
namespace TrisBluetooth.Droid
{
    public class Bth : IBluetooth
    {
        public static BluetoothAdapter mBluetoothAdapter;
        public static BluetoothDevice serverDevice;


        public Bth()
        {
                mBluetoothAdapter = BluetoothAdapter.DefaultAdapter;

        }

        public void Cancel()
        {
            System.Console.WriteLine("Ricerca disattivata");
            if (mBluetoothAdapter.IsDiscovering)
            {
                mBluetoothAdapter.CancelDiscovery();
            }
        }

        public void CreateBond(string Mac)
        {
            Cancel();
            foreach (BluetoothDevice device in MainActivity.devices)
            {
                if (device.Address.Equals(Mac))
                {
                    serverDevice = device;
                    device.CreateBond();
                    break;
                }
            }
        }

        public void StartDiscovery()
        {
            if (mBluetoothAdapter.IsDiscovering)
            {
                Cancel();
                System.Console.WriteLine("Ricerca attivata");
                mBluetoothAdapter.StartDiscovery();
            }
            else
            {
                System.Console.WriteLine("Ricerca attivata");
                mBluetoothAdapter.StartDiscovery();
            }

        }

    }
}