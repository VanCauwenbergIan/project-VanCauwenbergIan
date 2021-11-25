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

            MainPage = new NavigationPage(new OverviewPage());
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
