using DndApp.Models;
using DndApp.Repositories;
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
    public partial class DetailPage : ContentPage
    {
        public Monster Monster { get; set; }
        public List<Monster> OriginalMonsters { get; set; }
        public List<Monster> HomebrewMonsters { get; set; }

        public DetailPage(Monster selectedMonster, List<Monster> oMonsters, List<Monster> hbMonsters)
        {
            // save monster received from overviewpage
            Monster = selectedMonster;

            InitializeComponent();
            LoadIcons();
            CheckConnection();
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
            // load in the right images for icons/buttons
            btnBack.Source = ImageSource.FromResource("DndApp.Assets.buttonBack.png");
            btnEdit.Source = ImageSource.FromResource("DndApp.Assets.buttonEdit.png");
            btnRadarChart.Source = ImageSource.FromResource("DndApp.Assets.buttonRadarRed.png");

            // make the right ones clickable
            TapGestureRecognizer recognizer_return = new TapGestureRecognizer();
            TapGestureRecognizer recognizer_editdetails = new TapGestureRecognizer();
            TapGestureRecognizer recognizer_barchart = new TapGestureRecognizer();

            recognizer_return.Tapped += Recognizer_Tapped_return;
            recognizer_editdetails.Tapped += Recognizer_Tapped_editdetails;
            recognizer_barchart.Tapped += Recognizer_Tapped_barchart;
            btnBack.GestureRecognizers.Add(recognizer_return);
            btnEdit.GestureRecognizers.Add(recognizer_editdetails);
            btnRadarChart.GestureRecognizers.Add(recognizer_barchart);
        }

        private void Recognizer_Tapped_return(object sender, EventArgs e)
        {
            // return to overviewpage
            Navigation.PopAsync();
        }

        private void Recognizer_Tapped_editdetails(object sender, EventArgs e)
        {
            if (srvActions.IsVisible == true)
            {
                Navigation.PushAsync(new ModifyActionsPage(Monster, OriginalMonsters, HomebrewMonsters));
            }
            else
            {
                Navigation.PushAsync(new AddMonsterPage(Monster, OriginalMonsters, HomebrewMonsters));
            }
        }

        private void Recognizer_Tapped_barchart(object sender, EventArgs e)
        {
            Navigation.PushAsync(new GraphPage(Monster));
        }

        private void LoadMonster()
        {
            // fill in all the labels and spans with the right values of the monster
            lblPageTitle.Text = Monster.Name;
            lblDescriptionString.Text = $"{Monster.Size} {Monster.Type}, {Monster.Alignment}";
            spnArmorClass.Text = $"{Monster.ArmorClass} {MonsterMethods.CheckForNaturalArmor(Monster)}";
            spnHitPoints.Text = $"{Monster.HitPoints} ({Monster.HitDice})";
            spnSpeedString.Text = MonsterMethods.StringifySpeed(Monster);
            lblStr.Text = $"{Monster.Strength} {MonsterMethods.getAbilityScoreModifierString(Monster.Strength)}";
            lblDex.Text = $"{Monster.Dexterity} {MonsterMethods.getAbilityScoreModifierString(Monster.Dexterity)}";
            lblCon.Text = $"{Monster.Constitution} {MonsterMethods.getAbilityScoreModifierString(Monster.Constitution)}";
            lblInt.Text = $"{Monster.Intelligence} {MonsterMethods.getAbilityScoreModifierString(Monster.Intelligence)}";
            lblWis.Text = $"{Monster.Wisdom} {MonsterMethods.getAbilityScoreModifierString(Monster.Wisdom)}";
            lblCha.Text = $"{Monster.Charisma} {MonsterMethods.getAbilityScoreModifierString(Monster.Charisma)}";
            spnSavingThrowsString.Text = MonsterMethods.getSavingThrows(Monster);
            spnSkillsString.Text = MonsterMethods.getSkills(Monster);
            spnSensesString.Text = MonsterMethods.getSenses(Monster);
            spnLanguages.Text = MonsterMethods.checkLanguages(Monster.Languages);
            spnChallengeRatingString.Text = $"{Monster.ChallengeRating} ({Monster.ExperiencePoints} XP)";
            spnDamageVulnerabilities.Text = MonsterMethods.stringifyListStrings(Monster.DamageVulnerabilities);
            spnDamageResistances.Text = MonsterMethods.stringifyListStrings(Monster.DamageResistances);
            spnDamageImmunities.Text = MonsterMethods.stringifyListStrings(Monster.DamageImmunities);
            spnConditionImmunities.Text = MonsterMethods.getConditionImmunities(Monster);

            ShowFilledProperties();
            LoadMonsterAbilities();
            LoadActions();
        }

        private void ShowFilledProperties()
        {
            // only reveal the following properties if they are properly filled in
            // margin reacts to which certain properties are visible as to avoid empty spaces while hidden or no whitespace while revealed
            if (MonsterMethods.getSavingThrows(Monster) != "")
            {
                lblSavingThrowsString.IsVisible = true;
                lblSavingThrowsString.Margin = new Thickness(0, 16, 0, 16);
                lblSensesString.Margin = new Thickness(0, 0, 0, 16);
            }
            if (MonsterMethods.getSkills(Monster) != "")
            {
                lblSkillsString.IsVisible = true;
                lblSensesString.Margin = new Thickness(0, 0, 0, 16);

                if (lblSavingThrowsString.IsVisible == true)
                {
                    lblSkillsString.Margin = new Thickness(0, 0, 0, 16);
                }
                else
                {
                    lblSkillsString.Margin = new Thickness(0, 16, 0, 16);
                }
            }
            if (Monster.DamageVulnerabilities.Count() != 0)
            {
                lblDamageVulnerabilities.IsVisible = true;
                lblSensesString.Margin = new Thickness(0, 0, 0, 16);

                if (lblSavingThrowsString.IsVisible == true || lblSkillsString.IsVisible == true)
                {
                    lblDamageVulnerabilities.Margin = new Thickness(0, 0, 0, 16);
                }
                else
                {
                    lblDamageVulnerabilities.Margin = new Thickness(0, 16, 0, 16);
                }
            }
            if (Monster.DamageResistances.Count() != 0)
            {
                lblDamageResistances.IsVisible = true;
                lblSensesString.Margin = new Thickness(0, 0, 0, 16);

                if (lblSavingThrowsString.IsVisible == true || lblSkillsString.IsVisible == true || lblDamageVulnerabilities.IsVisible == true)
                {
                    lblDamageResistances.Margin = new Thickness(0, 0, 0, 16);
                }
                else
                {
                    lblDamageResistances.Margin = new Thickness(0, 16, 0, 16);
                }
            }
            if (Monster.DamageImmunities.Count() != 0)
            {
                lblDamageImmunities.IsVisible = true;
                lblSensesString.Margin = new Thickness(0, 0, 0, 16);

                if (lblSavingThrowsString.IsVisible == true || lblSkillsString.IsVisible == true || lblDamageVulnerabilities.IsVisible == true || lblDamageResistances.IsVisible == true)
                {
                    lblDamageImmunities.Margin = new Thickness(0, 0, 0, 16);
                }
                else
                {
                    lblDamageImmunities.Margin = new Thickness(0, 16, 0, 16);
                }
            }
            if (MonsterMethods.getConditionImmunities(Monster) != "")
            {
                lblConditionImmunities.IsVisible = true;
                lblSensesString.Margin = new Thickness(0, 0, 0, 16);

                if (lblSavingThrowsString.IsVisible == true || lblSkillsString.IsVisible == true || lblDamageVulnerabilities.IsVisible == true || lblDamageResistances.IsVisible == true || lblDamageImmunities.IsVisible == true)
                {
                    lblConditionImmunities.Margin = new Thickness(0, 0, 0, 16);
                }
                else
                {
                    lblConditionImmunities.Margin = new Thickness(0, 16, 0, 16);
                }
            }
        }

        private void LoadMonsterAbilities()
        {
            // dynamically add a monster's abilities to the bottom of the page with the corresponding layout
            if (Monster.SpecialAbilities != null)
            {
                foreach (Action ability in Monster.SpecialAbilities)
                {
                    Label title = new Label();
                    Label body = new Label();
                    title.Text = $"{ability.Name} {MonsterMethods.getUsage(ability)}";
                    title.FontSize = 16;
                    title.FontAttributes = FontAttributes.Bold;
                    title.TextColor = Color.FromHex("#F4F7FB");
                    body.Text = ability.Description;
                    body.FontSize = 14;
                    body.TextColor = Color.FromHex("#E4E4E4");
                    body.Margin = new Thickness(0, 0, 0, 8);

                    stlAbilities.Children.Add(title);
                    stlAbilities.Children.Add(body);
                }
            }
        }

        private void LoadActions()
        {
            // same, but for actions in the actions tab
            if (Monster.Actions != null)
            {
                foreach (Action action in Monster.Actions)
                {
                    Label title = new Label();
                    Label body = new Label();
                    title.Text = $"{action.Name} {MonsterMethods.getUsage(action)}";
                    title.FontSize = 16;
                    title.FontAttributes = FontAttributes.Bold;
                    title.TextColor = Color.FromHex("#F4F7FB");
                    body.Text = action.Description;
                    body.FontSize = 14;
                    body.TextColor = Color.FromHex("#E4E4E4");
                    body.Margin = new Thickness(8, 0, 0, 8);

                    stlActions.Children.Add(title);
                    stlActions.Children.Add(body);
                }
            }

            // same, but for legendaryactions in the actions tab
            if (Monster.LegendaryActions != null)
            {
                Label instruction = new Label();
                instruction.Text = $"After each other creature's turn the {Monster.Name} may make a legendary action. The {Monster.Name} has 3 legendary action points, which it regains at the start of its own turn. Each legendary action costs 1 of these points unless otherwise specified.";
                instruction.FontSize = 14;
                instruction.TextColor = Color.FromHex("#E4E4E4");
                instruction.Margin = new Thickness(0, 0, 0, 8);

                stlLegendaryActions.Children.Add(instruction);

                foreach (Action action in Monster.LegendaryActions)
                {
                    Label title = new Label();
                    Label body = new Label();
                    title.Text = $"{action.Name} {MonsterMethods.getUsage(action)}";
                    title.FontSize = 16;
                    title.FontAttributes = FontAttributes.Bold;
                    title.TextColor = Color.FromHex("#E40712");
                    body.Text = action.Description;
                    body.FontSize = 14;
                    body.TextColor = Color.FromHex("#E4E4E4");
                    body.Margin = new Thickness(8, 0, 0, 8);

                    stlLegendaryActions.Children.Add(title);
                    stlLegendaryActions.Children.Add(body);
                }
            }
        }

        private void btnActionsClicked(object sender, EventArgs e)
        {
            // reveals the actions section and hides the stats section when the right tab is clicked (while also changing which tab looks selected)
            srvActions.IsVisible = true;
            srvStats.IsVisible = false;
            btnActions.TextColor = Color.FromHex("#F4F7FB");
            btnStats.TextColor = Color.FromHex("#8999A6");
            bxvActions.BackgroundColor = Color.FromHex("E40712");
            bxvStats.BackgroundColor = Color.Transparent;
        }

        private void btnStatsClicked(object sender, EventArgs e)
        {
            // return to the stats section
            srvActions.IsVisible = false;
            srvStats.IsVisible = true;
            btnActions.TextColor = Color.FromHex("#8999A6");
            btnStats.TextColor = Color.FromHex("#F4F7FB");
            bxvActions.BackgroundColor = Color.Transparent;
            bxvStats.BackgroundColor = Color.FromHex("E40712");
        }
    }
}