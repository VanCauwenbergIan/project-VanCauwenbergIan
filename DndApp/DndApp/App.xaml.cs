using Android.Views;
using DndApp.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DndApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            // if you leave the backgroundColor out it will be white by default... which we don't want since we'll get white flashes when navigating between pages
            MainPage = new NavigationPage(new OverviewPage()) { BackgroundColor = Color.Black };
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
