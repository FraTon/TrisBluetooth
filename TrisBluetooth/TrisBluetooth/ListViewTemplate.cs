using System;
using System.Collections.Generic;
using System.Text;

namespace TrisBluetooth
{
    public class ListViewTemplate
    {
        public string Name { get; set; }
        public string Mac { get; set; }

        public ListViewTemplate(String Name, String Mac)
        {
            this.Name = Name;
            this.Mac = Mac;
        }

    }
}
