using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TrisBluetooth
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Gioca : ContentPage
	{
		public Gioca ()
		{
			InitializeComponent ();
		}

        public void Client(object sender, EventArgs args)
        {
            MessagingCenter.Send<Object, String>(this, "C-S", "Client");
        }

        public void Server(object sender, EventArgs args)
        {
            MessagingCenter.Send<Object, String>(this, "C-S", "Server");
        }

        public void Info(object sender, EventArgs e)
        {
            Application.Current.MainPage.DisplayAlert("Istruzioni", "1)Decidere chi tra te e i tuo avversario inizia a giocare, \n2)Colui che inizia sarà la X e l'altro giocatore il O, \n3)Vince il giocatore che riesce a disporre tre dei propri simboli in linea retta orizzontale, verticale o diagonale \n4)Per continuare il gioco cliccate entrambi Rivincita, in ogni caso il punteggio continuerà ad essere presente nella parte altra dello schermo", "OK");
        }

    }
}