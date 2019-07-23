using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace TrisBluetooth
{
    public partial class MainPage : ContentPage
    {

        public ObservableCollection<ListViewTemplate> data = new ObservableCollection<ListViewTemplate>();

        private bool search = true;

        public MainPage()
        {
            InitializeComponent();
            data.Add(new ListViewTemplate("Droga", "One"));
            data.Add(new ListViewTemplate("Sesso", "Two"));
            MainListView.ItemsSource = data;

            MessagingCenter.Subscribe<Object, string>(this, "ParsedSmsReceived",
            (sender, arg) =>
            {
                string message = arg;
                data.Add(new ListViewTemplate(message, " "));
                MainListView.ItemsSource = data;
            });

        }

        async void Cerca(object sender, EventArgs args)
        {
            DependencyService.Get<IBluetooth>().StartDiscovery();
        }


        async private void MainListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var Selected = e.Item as ListViewTemplate;

            switch (Selected.Mac)
            {
                case "One":
                    System.Console.WriteLine("Cliccato: " + Selected.Mac);
                    break;
                case "Two":
                    data.Add( new ListViewTemplate("Pippo", "Three"));
                    MainListView.ItemsSource = data;
                    break;
                case "Three":
                    break;
                case "Four":
                    break;
            }

       ((ListView)sender).SelectedItem = null;


        }

    }
}
