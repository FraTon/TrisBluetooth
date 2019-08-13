using Android.OS;
using Rox;
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
	public partial class SplashPage : ContentPage
	{
        //VideoView bumper;
        public SplashPage ()
		{
			InitializeComponent ();
           // bumper = FindByName("video") as VideoView;
            // bumper.Source = "android.resource://com.example.progettoandroid/" +  Resource.logobumper;
            //bumper.Start();
            Handler h = new Handler();
            Action start = () =>
              {
                  var page = new MainPage();
                  Navigation.PushAsync(page);
                //  bumper.Stop();
              };
            h.PostDelayed(start, 10000);
            
        }

        public void OnRestart()
        {
            //bumper.VideoState
            
           // bumper.Stop();
        }
    }
}