using DndApp.Models;
using DndApp.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DndApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GraphPage : ContentPage
    {
        StatsViewModel Obj;
        Monster SelectedMonster { get; set; }
        public GraphPage(Monster monster)
        {
            // save monster received from detail page
            SelectedMonster = monster;
            InitializeComponent();
            LoadIcons();
            CheckConnection();
            LoadBarChartData();
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
            // load icon for return button and asign click event to it
            btnBack.Source = ImageSource.FromResource("DndApp.Assets.buttonBack.png");

            TapGestureRecognizer recognizer_return = new TapGestureRecognizer();

            recognizer_return.Tapped += Recognizer_Tapped_return;
            btnBack.GestureRecognizers.Add(recognizer_return);
        }

        private void Recognizer_Tapped_return(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        private void LoadBarChartData()
        {
            lblTitle.Text = SelectedMonster.Name + " Stats";

            Obj = new StatsViewModel();

            //Ability Score Modifier radar chart
            Obj.DataAS.Clear();
            Obj.DataAS.Add(new StatsModel("STR", 0));
            Obj.DataAS.Add(new StatsModel("DEX", 0));
            Obj.DataAS.Add(new StatsModel("CON", 0));
            Obj.DataAS.Add(new StatsModel("INT", 0));
            Obj.DataAS.Add(new StatsModel("WIS", 0));
            Obj.DataAS.Add(new StatsModel("CHA", 0));

            List<int> abilityScoreModifiers = new List<int>();

            abilityScoreModifiers.Add(MonsterMethods.getAbilityScoreModifier(SelectedMonster.Strength));
            abilityScoreModifiers.Add(MonsterMethods.getAbilityScoreModifier(SelectedMonster.Dexterity));
            abilityScoreModifiers.Add(MonsterMethods.getAbilityScoreModifier(SelectedMonster.Constitution));
            abilityScoreModifiers.Add(MonsterMethods.getAbilityScoreModifier(SelectedMonster.Intelligence));
            abilityScoreModifiers.Add(MonsterMethods.getAbilityScoreModifier(SelectedMonster.Wisdom));
            abilityScoreModifiers.Add(MonsterMethods.getAbilityScoreModifier(SelectedMonster.Charisma));

            for (int i = 0; i <= 5; i++)
            {
                int asm = abilityScoreModifiers[i];

                Obj.DataAS[i].Value = asm;
            }

            // saving throws radar chart (normally it's the same as the asm, unless proficient)
            Obj.DataST.Clear();
            Obj.DataST.Add(new StatsModel("STR", abilityScoreModifiers[0]));
            Obj.DataST.Add(new StatsModel("DEX", abilityScoreModifiers[1]));
            Obj.DataST.Add(new StatsModel("CON", abilityScoreModifiers[2]));
            Obj.DataST.Add(new StatsModel("INT", abilityScoreModifiers[3]));
            Obj.DataST.Add(new StatsModel("WIS", abilityScoreModifiers[4]));
            Obj.DataST.Add(new StatsModel("CHA", abilityScoreModifiers[5]));

            List<ProficiencyAndValue> savingThrows = MonsterMethods.getSavingThrowsRaw(SelectedMonster);

            // overwrite the saving throws that have a proficiency or expertise on them
            foreach (ProficiencyAndValue savingThrow in savingThrows)
            {
                int abilityIndex = -1;
                // check which value / position in the savingthrow chart needs to be overwritten
                if (savingThrow.Proficiency.ProficiencyId == "saving-throw-str")
                {
                    abilityIndex = 0;
                }
                else if (savingThrow.Proficiency.ProficiencyId == "saving-throw-dex")
                {
                    abilityIndex = 1;
                }
                else if (savingThrow.Proficiency.ProficiencyId == "saving-throw-con")
                {
                    abilityIndex = 2;
                }
                else if (savingThrow.Proficiency.ProficiencyId == "saving-throw-int")
                {
                    abilityIndex = 3;
                }
                else if (savingThrow.Proficiency.ProficiencyId == "saving-throw-wis")
                {
                    abilityIndex = 4;
                }
                else if (savingThrow.Proficiency.ProficiencyId == "saving-throw-cha")
                {
                    abilityIndex = 5;
                }

                Obj.DataST[abilityIndex].Value = savingThrow.Value;
            }

            BindingContext = Obj;
        }
    }
}