using DndApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DndApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddMonsterPage : ContentPage
    {
        public AddMonsterPage(Monster selectedMonster, List<Monster> oMonsters, List<Monster> hbMonsters)
        {
            InitializeComponent();
            LoadIcons();
        }
        private void LoadIcons()
        {
            btnBack.Source = ImageSource.FromResource("DndApp.Assets.buttonBack.png");
            btnAddProf.Source = ImageSource.FromResource("DndApp.Assets.buttonAddRed.png");
            btnAddExpert.Source = ImageSource.FromResource("DndApp.Assets.buttonAddRed.png");
            btnAddAbility.Source = ImageSource.FromResource("DndApp.Assets.buttonAddRed.png");
            btnAddResitance.Source = ImageSource.FromResource("DndApp.Assets.buttonAddRed.png");
            btnAddDamageImmunity.Source = ImageSource.FromResource("DndApp.Assets.buttonAddRed.png");
            btnAddVulnerability.Source = ImageSource.FromResource("DndApp.Assets.buttonAddRed.png");
            btnAddCoditionImmunity.Source = ImageSource.FromResource("DndApp.Assets.buttonAddRed.png");

            TapGestureRecognizer recognizer_return = new TapGestureRecognizer();

            recognizer_return.Tapped += Recognizer_Tapped_return;
            btnBack.GestureRecognizers.Add(recognizer_return);
        }

        private void Recognizer_Tapped_return(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}