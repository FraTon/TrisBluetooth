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
            throw new NotImplementedException();
        }

        public ObservableCollection<string> PairedDevices()
        {
            throw new NotImplementedException();
        }

        // Start the "reading" loop 
        public void StartDiscovery()
        {
            System.Console.WriteLine("Cerca attivata");
            mBluetoothAdapter.StartDiscovery();
        }

       
        }
}