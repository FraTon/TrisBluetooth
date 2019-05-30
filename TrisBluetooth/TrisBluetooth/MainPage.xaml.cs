using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace TrisBluetooth
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

        }

        async void Cerca(object sender, EventArgs args)
        {
            DependencyService.Get<IBluetooth>().StartDiscovery();
        }


    }
}
