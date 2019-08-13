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
using System.Threading;
using Java.Util;
using System.IO;
using Java.IO;
using System.Text;
using Java.Nio.Charset;

namespace TrisBluetooth.Droid
{
    [Activity(Label = "TrisBluetooth", Icon = "@mipmap/icona", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global:: Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        // thread per gestire lo scambio di messaggi
        private static ConnectedThread mConnectedThread;

        // ruolo gestito dal dispositivo nella connessione bluetooth (true client, false server)
        private static bool ruolo = false;

        // Broadcast receiver per quando si scoprono nuovi dispositivi
        private BluetoothReceiver receiver = new BluetoothReceiver();
        private class BluetoothReceiver : BroadcastReceiver
        {

            public override void OnReceive(Context context, Intent intent)
            {
                string action = intent.Action;

                // When discovery finds a device send a messagge containing the description of the device
                if (action == BluetoothDevice.ActionFound)
                {
                    System.Console.WriteLine(action);
                    // Get the BluetoothDevice object from the Intent
                    BluetoothDevice device = (BluetoothDevice)intent.GetParcelableExtra(BluetoothDevice.ExtraDevice);
                    System.Console.WriteLine("Trovato: " + device.Name + " " + device.Address);
                    devices.Add(device);
                    String[] Description = { device.Name, device.Address };
                    MessagingCenter.Send<Object, String[]>(this, "SaveDevices", Description);
                } else if(action == BluetoothDevice.ActionAclConnected)
                {
                } else if(action == BluetoothDevice.ActionAclDisconnected)
                {
                    System.Console.WriteLine(action);
                    reset();
                }

            }
        }



        //Array list che contiene i Device trovati
        public static System.Collections.ArrayList devices = new System.Collections.ArrayList();

        // Thread per gestire la connessione
        private static AcceptThread server;
        private static ConnectThread client;

        // UUID con cui fare la connessione
        private static readonly UUID MY_UUID =
    UUID.FromString("8ce255c0-200a-11e0-ac64-0800200c9a66");

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());

            Window.SetStatusBarColor(Android.Graphics.Color.Rgb(255, 100, 60));

            //Questa funzione controlla se il dispositivo ha i permessi necessari
            checkPermission();

            //Questa funzione registra i Broadcast Receiver necessari
            registerReceivers();

            //Questa funzione registra le caselle per lo scambio di messaggi fra la parte android e cross-platform
            setMailBoxes();
   
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            System.Console.WriteLine("OnDestroy called");
            reset();
        }

        private static void reset()
        {
            System.Console.WriteLine("Reset Chiamata");

            if (mConnectedThread != null)
            {
                mConnectedThread.Cancel();
                mConnectedThread = null;
            }
            if (client != null)
            {
                client.Cancel();
                client = null;
            }
            if (server != null)
            {
                server.Cancel();
                server = null;
            }
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
            receiver = new BluetoothReceiver();
            var filter = new IntentFilter(BluetoothDevice.ActionFound);
            RegisterReceiver(receiver, filter);

            // Register for broadcasts when discovery has finished
            filter = new IntentFilter(BluetoothAdapter.ActionDiscoveryFinished);
            RegisterReceiver(receiver, filter);
        }

        private void setMailBoxes()
        {

            MessagingCenter.Subscribe<Object, String>(this, "Request",
                (sender, arg) =>
                {
                    String request = arg;

                    if (request.Equals("Server"))
                    {
                        ruolo = false;
                        System.Console.WriteLine("Richiesta di Server");
                        server = new AcceptThread();
                        server.Start();
                    }
                    else if (request.Equals("Client"))
                    {
                        ruolo = true;
                        System.Console.WriteLine("Richiesta di Client");
                        client = new ConnectThread(Bth.serverDevice, MY_UUID);
                        client.Start();
                    } else if(request.Equals("Discovery"))
                    {
                        System.Console.WriteLine("Richiesta di Discoverability");
                        AbilitaDiscoverabilty();
                    } else if (request.Equals("Reset"))
                    {
                        System.Console.WriteLine("Richiesta di Reset");
                        reset();
                    }
                });

            MessagingCenter.Subscribe<Object, int>(this, "IncomingMessage",
                (sender, arg) =>
                {
                    System.Console.WriteLine("posizione cliccata: " + arg);
                    string messaggio =  arg.ToString();
                    byte[] bytes = Encoding.Unicode.GetBytes(messaggio);
                    mConnectedThread.Write(bytes);
                });

        }

        //abilita la discoverability, se bt è spento lo accende
        private void AbilitaDiscoverabilty()
        {
            Intent discoverableIntent =
                    new Intent(BluetoothAdapter.ActionRequestDiscoverable);
            discoverableIntent.PutExtra(BluetoothAdapter.ExtraDiscoverableDuration, 30);
            StartActivity(discoverableIntent);
            System.Console.WriteLine("Discoverability Attivata");
        }



        //Classe per Gestire il Client
        private class ConnectThread : Java.Lang.Thread
        {
            private readonly BluetoothSocket mmSocket;
            private readonly BluetoothDevice mmDevice;


            public ConnectThread(BluetoothDevice device, UUID myUuid)
            {
                // Use a temporary object that is later assigned to mmSocket
                // because mmSocket is final.
                BluetoothSocket tmp = null;
                mmDevice = device;

                try
                {
                    // Get a BluetoothSocket to connect with the given BluetoothDevice.
                    // MY_UUID is the app's UUID string, also used in the server code.
                    tmp = device.CreateRfcommSocketToServiceRecord(myUuid);
                }
                catch (Java.IO.IOException e)
                {
                    System.Console.WriteLine("Errore: " + e);
                }
                mmSocket = tmp;
            }

            override
            public void Run()
            {
                // Cancel discovery because it otherwise slows down the connection.
                Bth.mBluetoothAdapter.CancelDiscovery();

                try
                {
                    // Connect to the remote device through the socket. This call blocks
                    // until it succeeds or throws an exception.
                    System.Console.WriteLine("Tentativo di Conessione come Client");
                    mmSocket.Connect();
                }
                catch (Java.IO.IOException connectException)
                {
                    System.Console.WriteLine("Errore: " + connectException);
                    // Unable to connect; close the socket and return.
                    try
                    {
                        mmSocket.Close();
                    }
                    catch (Java.IO.IOException closeException)
                    {
                        System.Console.WriteLine("Errore: " + closeException);
                    }
                    return;
                }

                System.Console.WriteLine("Connesso come Client");



                // The connection attempt succeeded. Perform work associated with
                // the connection in a separate thread.
                mConnectedThread = new ConnectedThread(mmSocket);
                mConnectedThread.Start();
            }

            // Closes the client socket and causes the thread to finish.
            public void Cancel()
            {
                try
                {
                    mmSocket.Close();
                }
                catch (Java.IO.IOException e)
                {
                    System.Console.WriteLine("Errore: " + e);
                }
            }
        }

        //classe per gestire il Server
        private class AcceptThread : Java.Lang.Thread
        {
            private readonly BluetoothServerSocket mmServerSocket;


            public AcceptThread()
            {
                // Use a temporary object that is later assigned to mmServerSocket
                // because mmServerSocket is final.
                BluetoothServerSocket tmp = null;
                try
                {
                    // MY_UUID is the app's UUID string, also used by the client code.
                    tmp = Bth.mBluetoothAdapter.ListenUsingRfcommWithServiceRecord("NAME", MY_UUID);

                }
                catch (Java.IO.IOException e)
                {
                    System.Console.WriteLine("Errore: " + e);
                }
                mmServerSocket = tmp;
            }

            override
            public void Run()
            {
                BluetoothSocket socket = null;
                // Keep listening until exception occurs or a socket is returned.
                while (true)
                {
                    try
                    {
                        System.Console.WriteLine("Tentativo di Conessione come Server");

                        socket = mmServerSocket.Accept();

                    }
                    catch (Java.IO.IOException e)
                    {
                        System.Console.WriteLine("Errore: " + e);
                        break;
                    }
                    System.Console.WriteLine("Connesso come Server");

                    if (socket != null)
                    {
                        mConnectedThread = new ConnectedThread(socket);
                        mConnectedThread.Start();
                        break;
                    }
                }
            }

            // Closes the connect socket and causes the thread to finish.
            public void Cancel()
            {
                try
                {
                    mmServerSocket.Close();
                }
                catch (Java.IO.IOException e)
                {
                    System.Console.WriteLine("Errore: " + e);
                }
            }

        }
        //questa classe gestisce lo scambio di messaggi
        private class ConnectedThread: Java.Lang.Thread 
        {
            private Handler handler;

            private readonly BluetoothSocket mmSocket;
            private readonly InputStream mmInStream;
            private readonly OutputStream mmOutStream;
            private byte[] mmBuffer; // mmBuffer store for the stream

            public ConnectedThread(BluetoothSocket socket)
            {
                mmSocket = socket;
                InputStream tmpIn = null;
                OutputStream tmpOut = null;


                // Get the input and output streams; using temp objects because
                // member streams are final.
                try
                {
                    tmpIn = ((Android.Runtime.InputStreamInvoker)socket.InputStream).BaseInputStream;
                }
                catch (System.IO.IOException e)
                {
                    System.Console.WriteLine("Errore: " + e);
                }
                try
                {
                    tmpOut = ((Android.Runtime.OutputStreamInvoker)socket.OutputStream).BaseOutputStream;
                }
                catch (Java.IO.IOException e)
                {
                    System.Console.WriteLine("Errore: " + e);
                }

                mmInStream = tmpIn;
                mmOutStream = tmpOut;
            }

            override
            public void Run()
            {
                mmBuffer = new byte[1024];
                int numBytes; // bytes returned from read()

                // Keep listening to the InputStream until an exception occurs.
                while (true)
                {
                    try
                    {
                        // Read from the InputStream.
                        numBytes = mmInStream.Read(mmBuffer);
                        // Send the obtained bytes to the UI activity.
                        System.Console.WriteLine(numBytes);
                        String message = Encoding.Unicode.GetString(mmBuffer, 0, numBytes);
                        System.Console.WriteLine("Messaggio: " + message);
                        MessagingCenter.Send<Object, String>(this, "OutgoingMessage", message);
                        break;
                    }
                    catch (Java.IO.IOException e)
                    {
                        System.Console.WriteLine("Errore: " + e);
                        break;
                    }
                }

            }

            // Call this from the main activity to send data to the remote device.
            public void Write(byte[] bytes)
            {
                string text = System.Text.Encoding.Default.GetString(bytes);
                try
                {
                    mmOutStream.Write(bytes);
                }
                catch (Java.IO.IOException e)
                {
                    System.Console.WriteLine("Errore: " + e);
                    //reset();
                }

            }
                // Call this method from the main activity to shut down the connection.
            public void Cancel()
            {
                try
                {
                    mmSocket.Close();
                }
                catch (Java.IO.IOException e)
                {
                System.Console.WriteLine("Errore: " + e);
                }
            }
        }
       
    }


}
