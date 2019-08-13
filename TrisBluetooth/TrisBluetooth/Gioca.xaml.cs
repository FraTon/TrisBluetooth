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
      
        private bool[] cliccati = new bool[9];
        private bool[] occupati = new bool[9];

        Button[] cellButton = new Button[9];
        Image[] cellImageX = new Image[9];
        Image[] cellImageO = new Image[9];

        int mioPunteggio = 0;
        int suoPunteggio = 0;
        //master sarà true solo se il giocatore è client
        bool master = false;

        Button riv;
        Button x;
        Button o;
        Image xImage;
        Image oImage;
        Image rivincitaImage;
        Label punteggio;
        Label display;


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
            for (int i=0; i<9; i++)
            {
                String elemento = i.ToString();
                cellButton[i] = FindByName("cella" + elemento) as Button;
                cellImageX[i] = FindByName("XImg" + elemento) as Image;
                cellImageO[i] = FindByName("OImg" + elemento) as Image;
            }
            cellButton[0].Clicked += (sender, args) => { ButtonClicked(0); };
            cellButton[1].Clicked += (sender, args) => { ButtonClicked(1); };
            cellButton[2].Clicked += (sender, args) => { ButtonClicked(2); };
            cellButton[3].Clicked += (sender, args) => { ButtonClicked(3); };
            cellButton[4].Clicked += (sender, args) => { ButtonClicked(4); };
            cellButton[5].Clicked += (sender, args) => { ButtonClicked(5); };
            cellButton[6].Clicked += (sender, args) => { ButtonClicked(6); };
            cellButton[7].Clicked += (sender, args) => { ButtonClicked(7); };
            cellButton[8].Clicked += (sender, args) => { ButtonClicked(8); };
            reset();

            MessagingCenter.Subscribe<Object, String>(this, "OutgoingMessage",
            (sender, arg) =>
            {
                Int32.TryParse(arg, out int position);
                occupati[position] = true;
                ButtonClick(true);
                if (master) cellImageO[position].IsVisible = true;
                else cellImageX[position].IsVisible = true;
                display.Text = "Tocca a te";
            });

            
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
            //trasforma da string ad int position; il TryParse non genera un'eccezione se la conversione non riesce
            //Int32.TryParse(position, out i);
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
        public void ButtonClick(bool enable)
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
    }
}