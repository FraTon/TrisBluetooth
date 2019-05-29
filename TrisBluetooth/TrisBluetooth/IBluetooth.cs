using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace TrisBluetooth
{
    public interface IBluetooth
    {
        void Start(string name, int sleepTime, bool readAsCharArray);
        void Cancel();
        ObservableCollection<string> PairedDevices();
      
    }
}
