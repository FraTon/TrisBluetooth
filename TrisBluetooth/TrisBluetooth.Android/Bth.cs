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
        ConnectedThread mConnectedThread;
        static readonly UUID MY_UUID =
            UUID.FromString("8ce255c0-200a-11e0-ac64-0800200c9a66");

        public Bth()
        {
                mBluetoothAdapter = BluetoothAdapter.DefaultAdapter;

        }

        public void Cancel()
        {
            System.Console.WriteLine("Ricerca disattivata");
            mBluetoothAdapter.CancelDiscovery();
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


    //Classe per Gestire il Client
    public class ConnectThread: Thread {
        private readonly BluetoothSocket mmSocket;
        private readonly BluetoothDevice mmDevice;


        public ConnectThread(BluetoothDevice device, UUID myUuid) {
            // Use a temporary object that is later assigned to mmSocket
            // because mmSocket is final.
            BluetoothSocket tmp = null;
            mmDevice = device;

            try {
                // Get a BluetoothSocket to connect with the given BluetoothDevice.
                // MY_UUID is the app's UUID string, also used in the server code.
                tmp = device.CreateRfcommSocketToServiceRecord(myUuid);
            } catch (IOException e) {
            }
            mmSocket = tmp;
        }

        public void run() {
            // Cancel discovery because it otherwise slows down the connection.
            mBluetoothAdapter.CancelDiscovery();
            BluetoothSocket socket = null;

            try {
                // Connect to the remote device through the socket. This call blocks
                // until it succeeds or throws an exception.
                mmSocket.Connect();
            } catch (IOException connectException) {
                // Unable to connect; close the socket and return.
                try {
                    mmSocket.Close();
                } catch (IOException closeException) {
                }
                return;
            }


            // The connection attempt succeeded. Perform work associated with
            // the connection in a separate thread.
            connected(mmSocket);
        }

        // Closes the client socket and causes the thread to finish.
        public void cancel() {
            try {
                mmSocket.Close();
            } catch (IOException e) {
            }
        }
    }

    //classe per gestire il Server
    public class AcceptThread: Thread {
        private readonly BluetoothServerSocket mmServerSocket;


        public AcceptThread() {
            // Use a temporary object that is later assigned to mmServerSocket
            // because mmServerSocket is final.
            BluetoothServerSocket tmp = null;
            try {
                // MY_UUID is the app's UUID string, also used by the client code.
                tmp = mBluetoothAdapter.ListenUsingRfcommWithServiceRecord("NAME", MY_UUID);
                    
            } catch (IOException e) {
            }
            mmServerSocket = tmp;
        }

        public void run() {
            BluetoothSocket socket = null;
            // Keep listening until exception occurs or a socket is returned.
            while (true) {
                try {

                    socket = mmServerSocket.Accept();
                      

                } catch (IOException e) {
                    break;
                }


                if (socket != null) {
                    connected(socket);
                }
            }
        }

        // Closes the connect socket and causes the thread to finish.
        public void cancel() {
            try {
                mmServerSocket.Close();
            } catch (IOException e) {
            }
        }

    }
    }
}