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

[assembly: Xamarin.Forms.Dependency(typeof(Bth))]
namespace TrisBluetooth.Droid
{
    public class Bth : IBluetooth
    {
        BluetoothAdapter mBluetoothAdapter;

        public Bth()
        {
                mBluetoothAdapter = BluetoothAdapter.DefaultAdapter;

        }

        public void Cancel()
        {
            System.Console.WriteLine("Ricerca disattivata");
            mBluetoothAdapter.CancelDiscovery();
        }

        public void CreateConnection(Object device)
        {

        }

        public void CreateBond(String Mac)
        {
            this.Cancel();
            foreach (BluetoothDevice device in MainActivity.devices)
            {
                if (device.Address.Equals(Mac))
                {
                    device.CreateBond();
                    break;
                }
            }
        }

        public void StartDiscovery()
        {
            System.Console.WriteLine("Ricerca attivata");
            mBluetoothAdapter.StartDiscovery();

        }

    }
}