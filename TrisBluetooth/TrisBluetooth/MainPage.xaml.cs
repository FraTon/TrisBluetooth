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


        public MainPage()
        {
            InitializeComponent();
            MainListView.ItemsSource = data;

            MessagingCenter.Subscribe<Object, String[]>(this, "SaveDevices",
            (sender, arg) =>
            {
                string[] description = arg;
                string name = description[0];
                string mac = description[1];

                data.Add(new ListViewTemplate(name, mac));
                MainListView.ItemsSource = data;
            });



        }

        async void Cerca(object sender, EventArgs args)
        {
            MessagingCenter.Send<Object, char>(this, "setDiscoverability", 'd');
            DependencyService.Get<IBluetooth>().StartDiscovery();
        }


        async private void MainListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var Selected = e.Item as ListViewTemplate;
            DependencyService.Get<IBluetooth>().CreateBond(Selected.Mac);

        }

        private void Gioca(object sender, EventArgs e)
        {
            var page = new Gioca();
            Navigation.PushAsync(page);
        }
    }
}
