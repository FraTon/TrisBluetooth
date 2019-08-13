using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.Xaml;
using Xamarin.Forms;

namespace TrisBluetooth
{
    public partial class MainPage : ContentPage
    {

        public ObservableCollection<ListViewTemplate> data = new ObservableCollection<ListViewTemplate>();
        Button play;
        Image playImage;
        public Gioca page = new Gioca();

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

            MessagingCenter.Subscribe<Object, String>(this, "OutgoingMessage",
            (sender, arg) =>
            {
                page.messageReceived(arg);
            });

            play = FindByName("gioca") as Button;
            playImage = FindByName("giocaImage") as Image;
        }
        
        async void Cerca(object sender, EventArgs args)
        {
            MessagingCenter.Send<Object, String>(this, "Request", "Discovery");
            DependencyService.Get<IBluetooth>().StartDiscovery();
        }


        async private void MainListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var Selected = e.Item as ListViewTemplate;
            DependencyService.Get<IBluetooth>().CreateBond(Selected.Mac);
            play.IsVisible = true;
            playImage.IsVisible = true;
        }

        private void Gioca(object sender, EventArgs e)
        {
            Navigation.PushAsync(page);
        }

        public void Info(object sender, EventArgs e)
        {
            Application.Current.MainPage.DisplayAlert("Istruzioni", "1) Cliccare il pulsante Cerca e attendere la ricerca dei dispositivi bluetooth \n2) Selezionare il device del tuo avversario \n3) Cliccare Gioca", "OK");
        }
    }
}
