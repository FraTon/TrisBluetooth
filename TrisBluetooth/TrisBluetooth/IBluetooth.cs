using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace TrisBluetooth
{
    public interface IBluetooth
    {
        void StartDiscovery();
        void Cancel();
        ObservableCollection<string> PairedDevices();
      
    }
}
