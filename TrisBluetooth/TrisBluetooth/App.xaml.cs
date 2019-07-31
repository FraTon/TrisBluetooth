using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace TrisBluetooth
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            
            var navigationPage = new NavigationPage(new SplashPage());
            MainPage = navigationPage;
          
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            var navigationPage = new NavigationPage(new MainPage());
            MainPage = navigationPage;
        }
    }
}
