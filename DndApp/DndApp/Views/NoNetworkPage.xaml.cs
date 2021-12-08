using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DndApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NoNetworkPage : ContentPage
    {
        public NoNetworkPage()
        {
            InitializeComponent();
            CheckConnection();
            imgDeco.Source = ImageSource.FromResource("DndApp.Assets.noInternetDecoration.jpg");
        }

        private void CheckConnection()
        {
            Connectivity.ConnectivityChanged += ToMainPage;
        }

        private void ToMainPage(object sender, EventArgs e)
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                App.Current.MainPage = new NavigationPage(new OverviewPage()) { BackgroundColor = Color.Black };
                // will always reset the app, no matter what
            }
        }
    }
}