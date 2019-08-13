using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TrisBluetooth
{
	public partial class Gioca : ContentPage
	{
      
        public static bool[] cliccati = new bool[9];
        public static bool[] occupati = new bool[9];

        public static Button[] cellButton = new Button[9];
        public static Image[] cellImageX = new Image[9];
        public static Image[] cellImageO = new Image[9];

        int mioPunteggio = 0;
        int suoPunteggio = 0;
        //master sarà true solo se il giocatore è client
        public static bool master = false;

        public static Button riv;
        public static Button x;
        public static Button o;
        public static Image xImage;
        public static Image oImage;
        public static Image rivincitaImage;
        public static Label punteggio;
        public static Label display;


        public Gioca ()
		{
			InitializeComponent ();

            riv = FindByName("Revenge") as Button;
            x = FindByName("X") as Button;
            o = FindByName("O") as Button;
            xImage = FindByName("XImage") as Image;
            oImage = FindByName("OImage") as Image;
            rivincitaImage = FindByName("RivincitaImage") as Image;
            punteggio = FindByName("Punteggio") as Label;
            display = FindByName("Display") as Label;

            //rivincita = FindByName("Rivincita") as Button;
            punteggio.Text = mioPunteggio.ToString() + " : " + suoPunteggio.ToString();
            display.Text = "Scegli il tuo simbolo...";

            //assegna agli array le immagini e i bottoni della griglia

            cellButton[0] = FindByName("cella0") as Button;
            cellImageX[0] = FindByName("XImg0") as Image;
            cellImageO[0] = FindByName("OImg0") as Image;
            cellButton[0].Clicked += (sender, args) => { ButtonClicked(0); };


            cellButton[1] = FindByName("cella1") as Button;
            cellImageX[1] = FindByName("XImg1") as Image;
            cellImageO[1] = FindByName("OImg1") as Image;
            cellButton[1].Clicked += (sender, args) => { ButtonClicked(1); };


            cellButton[2] = FindByName("cella2") as Button;
            cellImageX[2] = FindByName("XImg2") as Image;
            cellImageO[2] = FindByName("OImg2") as Image;
            cellButton[2].Clicked += (sender, args) => { ButtonClicked(2); };


            cellButton[3] = FindByName("cella3") as Button;
            cellImageX[3] = FindByName("XImg3") as Image;
            cellImageO[3] = FindByName("OImg3") as Image;
            cellButton[3].Clicked += (sender, args) => { ButtonClicked(3); };


            cellButton[4] = FindByName("cella4") as Button;
            cellImageX[4] = FindByName("XImg4") as Image;
            cellImageO[4] = FindByName("OImg4") as Image;
            cellButton[4].Clicked += (sender, args) => { ButtonClicked(4); };


            cellButton[5] = FindByName("cella5") as Button;
            cellImageX[5] = FindByName("XImg5") as Image;
            cellImageO[5] = FindByName("OImg5") as Image;
            cellButton[5].Clicked += (sender, args) => { ButtonClicked(5); };


            cellButton[6] = FindByName("cella6") as Button;
            cellImageX[6] = FindByName("XImg6") as Image;
            cellImageO[6] = FindByName("OImg6") as Image;
            cellButton[6].Clicked += (sender, args) => { ButtonClicked(6); };


            cellButton[7] = FindByName("cella7") as Button;
            cellImageX[7] = FindByName("XImg7") as Image;
            cellImageO[7] = FindByName("OImg7") as Image;
            cellButton[7].Clicked += (sender, args) => { ButtonClicked(7); };


            cellButton[8] = FindByName("cella8") as Button;
            cellImageX[8] = FindByName("XImg8") as Image;
            cellImageO[8] = FindByName("OImg8") as Image;
            cellButton[8].Clicked += (sender, args) => { ButtonClicked(8); };

            reset();
            
        }

        // alert per istruzioni gioco
        public void Info(object sender, EventArgs e)
        {
            Application.Current.MainPage.DisplayAlert("Istruzioni", "1)Decidere chi tra te e i tuo avversario inizia a giocare, \n2)Colui che inizia sarà la X e l'altro giocatore il O, \n3)Vince il giocatore che riesce a disporre tre dei propri simboli in linea retta orizzontale, verticale o diagonale \n4)Per continuare il gioco cliccate entrambi Rivincita, in ogni caso il punteggio continuerà ad essere presente nella parte altra dello schermo", "OK");
        }

        public void Client(object sender, EventArgs args)
        {
            MessagingCenter.Send<Object, String>(this, "Request", "Client");
            xoisEnable(false);
            display.Text = "Tocca a te";
            ButtonEnable(true);
            master = true;
        }

        public void Server(object sender, EventArgs args)
        {
            MessagingCenter.Send<Object, String>(this, "Request", "Server");
            xoisEnable(false);
            display.Text = "Attendi...";
            master = false; 
        }

        public void Rivincita(object sender, EventArgs args)
        {
            MessagingCenter.Send<Object, String>(this, "Request", "Reset");
            reset();
            xoisEnable(true);
        }

        public void ButtonClicked(int position)
        {
            ButtonEnable(false);
            display.Text = "Attendi...";
            cliccati[position] = true;
            occupati[position] = true;
            if (master) cellImageX[position].IsVisible = true;
            else cellImageO[position].IsVisible = true;
            MessagingCenter.Send<Object, int>(this, "IncomingMessage", position);
        }

        //invocata ogni volta che si inizia la partitra
        public void reset()
        {
            master = false;
            for (int i = 0; i < 9; i++)
            {
                cellButton[i].IsEnabled = false;
                cellImageX[i].IsVisible = false;
                cellImageO[i].IsVisible = false;
                cliccati[i] = false;
                occupati[i] = false;
            }

            xoisEnable(true);
        }

        //cambia isEnable dei bottoni della griglia 
        public void ButtonEnable(bool enable)
        {
            for (int i = 0; i < 9; i++)
            {
               cellButton[i].IsEnabled = enable;
            }
        }


        //cambia enable per i bottoni Server e Client
        public void xoisEnable (bool enable)
        {
            x.IsEnabled = enable;
            x.IsVisible = enable;
            o.IsEnabled = enable;
            o.IsVisible = enable;
            xImage.IsVisible = enable;
            oImage.IsVisible = enable;
        }

        //rende enable solo i bottoni che non sono occupati
        public void  ButtonClick(bool enable)
        {
            for (int i = 0; i < 9; i++)
            {
                if(!occupati[i]) cellButton[i].IsEnabled = enable;
            }
        }

        //funzione che verifica se il giocatore ha vinto (da rivedere)
        private Boolean victory()
        {
            if (cliccati[0] && cliccati[1] && cliccati[2]) return true;
            if (cliccati[3] && cliccati[4] && cliccati[5]) return true;
            if (cliccati[6] && cliccati[7] && cliccati[8]) return true;
            if (cliccati[0] && cliccati[3] && cliccati[6]) return true;
            if (cliccati[1] && cliccati[4] && cliccati[7]) return true;
            if (cliccati[2] && cliccati[5] && cliccati[8]) return true;
            if (cliccati[0] && cliccati[4] && cliccati[8]) return true;
            if (cliccati[2] && cliccati[4] && cliccati[6]) return true;

            return false;
        }

        //funzione che verifica se c'è pareggio

        private Boolean draw()
        {
            for (int i = 0; i < 9; i++)
            {
                if (!cliccati[i] & !occupati[i]) return false;
            }
            return true;
        }

        public void messageReceived(String arg)
        {

            Device.BeginInvokeOnMainThread(() => {
                Thread.CurrentThread.IsBackground = true;
                Int32.TryParse(arg, out int position);
                System.Diagnostics.Debug.WriteLine("is visible");
                if (master) cellImageO[position].IsVisible = true;
                else cellImageX[position].IsVisible = true;
                System.Diagnostics.Debug.WriteLine("occupati = true");
                occupati[position] = true;
                System.Diagnostics.Debug.WriteLine("Button Click");
                ButtonClick(true);
                System.Diagnostics.Debug.WriteLine("display text");
                display.Text = "Tocca a te";
            });

        }
    }
}