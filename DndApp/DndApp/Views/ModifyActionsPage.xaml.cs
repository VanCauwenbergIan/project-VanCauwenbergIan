using DndApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Action = DndApp.Models.Action;

namespace DndApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ModifyActionsPage : ContentPage
    {
        AddMonsterViewModel Obj;
        public Monster SelectedMonster { get; set; }

        public ModifyActionsPage(Monster selectedMonster, List<Monster> oMonsters, List<Monster> hbMonsters)
        {
            SelectedMonster = selectedMonster;
            InitializeComponent();
            CheckConnection();
            LoadIcons();
            LoadMonster();
        }

        private void CheckConnection()
        {
            Connectivity.ConnectivityChanged += ToNoNetworkPage;
        }

        private void ToNoNetworkPage(object sender, ConnectivityChangedEventArgs e)
        {
            if (e.ConnectionProfiles.Contains(ConnectionProfile.WiFi) == false && e.ConnectionProfiles.Contains(ConnectionProfile.Cellular) == false)
            {
                Navigation.PushAsync(new NoNetworkPage());
            }
        }

        private void LoadIcons()
        {
            btnBack.Source = ImageSource.FromResource("DndApp.Assets.buttonBack.png");
            btnAddAction.Source = ImageSource.FromResource("DndApp.Assets.buttonAddRed.png");
            btnAddLegendaryAction.Source = ImageSource.FromResource("DndApp.Assets.buttonAddRed.png");

            TapGestureRecognizer recognizer_return = new TapGestureRecognizer();

            recognizer_return = new TapGestureRecognizer();

            recognizer_return.Tapped += Recognizer_Tapped_return;
            btnBack.GestureRecognizers.Add(recognizer_return);
        }

        private void Recognizer_Tapped_return(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        private void LoadMonster()
        {
            Obj = new AddMonsterViewModel();

            if (SelectedMonster.Actions != null)
            {
                foreach (Action action in SelectedMonster.Actions)
                {
                    Obj.Actions.Add(action);
                }

                bdlActions.BindingContext = Obj;
            }
            if (SelectedMonster.LegendaryActions != null)
            {
                foreach (Action action in SelectedMonster.LegendaryActions)
                {
                    Obj.LegendaryActions.Add(action);
                }

                bdlLegendaryActions.BindingContext = Obj;
            }
        }

        private void btnConfirmClicked(object sender, EventArgs e)
        {

        }
    }
}