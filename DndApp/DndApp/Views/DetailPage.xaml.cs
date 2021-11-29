using DndApp.Models;
using DndApp.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Action = DndApp.Models.Action;

namespace DndApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DetailPage : ContentPage
    {
        public Monster Monster { get; set; }
        public DetailPage(Monster selectedMonster)
        {
            // save monster received from overviewpage
            Monster = selectedMonster;

            InitializeComponent();
            LoadIcons();
            LoadMonster();
        }

        private void LoadIcons()
        {
            // load in the right images for icons/buttons
            btnBack.Source = ImageSource.FromResource("DndApp.Assets.buttonBack.png");
            btnEdit.Source = ImageSource.FromResource("DndApp.Assets.buttonEdit.png");
            btnRadarChart.Source = ImageSource.FromResource("DndApp.Assets.buttonRadarRed.png");

            // make the right ones clickable
            TapGestureRecognizer recognizer_return = new TapGestureRecognizer();

            recognizer_return.Tapped += Recognizer_Tapped_return;
            btnBack.GestureRecognizers.Add(recognizer_return);
        }

        private void Recognizer_Tapped_return(object sender, EventArgs e)
        {
            // return to overviewpage
            Navigation.PopAsync();
        }

        private void LoadMonster()
        {
            // fill in all the labels and spans with the right values of the monster
            lblPageTitle.Text = Monster.Name;
            lblDescriptionString.Text = $"{Monster.Size} {Monster.Type}, {Monster.Alignment}";
            spnArmorClass.Text = $"{Monster.ArmorClass} {MonsterMethodRepository.CheckForNaturalArmor(Monster)}";
            spnHitPoints.Text = $"{Monster.HitPoints} ({Monster.HitDice})";
            spnSpeedString.Text = MonsterMethodRepository.StringifySpeed(Monster);
            lblStr.Text = $"{Monster.Strength} {MonsterMethodRepository.getAbilityScoreModifierString(Monster.Strength)}";
            lblDex.Text = $"{Monster.Dexterity} {MonsterMethodRepository.getAbilityScoreModifierString(Monster.Dexterity)}";
            lblCon.Text = $"{Monster.Constitution} {MonsterMethodRepository.getAbilityScoreModifierString(Monster.Constitution)}";
            lblInt.Text = $"{Monster.Intelligence} {MonsterMethodRepository.getAbilityScoreModifierString(Monster.Intelligence)}";
            lblWis.Text = $"{Monster.Wisdom} {MonsterMethodRepository.getAbilityScoreModifierString(Monster.Wisdom)}";
            lblCha.Text = $"{Monster.Charisma} {MonsterMethodRepository.getAbilityScoreModifierString(Monster.Charisma)}";
            spnSavingThrowsString.Text = MonsterMethodRepository.getSavingThrows(Monster);
            spnSkillsString.Text = MonsterMethodRepository.getSkills(Monster);
            spnSensesString.Text = MonsterMethodRepository.getSenses(Monster);
            spnLanguages.Text = MonsterMethodRepository.checkLanguages(Monster.Languages);
            spnChallengeRatingString.Text = $"{Monster.ChallengeRating} ({Monster.ExperiencePoints} XP)";
            spnDamageVulnerabilities.Text = MonsterMethodRepository.stringifyListStrings(Monster.DamageVulnerabilities);
            spnDamageResistances.Text = MonsterMethodRepository.stringifyListStrings(Monster.DamageResistances);
            spnDamageImmunities.Text = MonsterMethodRepository.stringifyListStrings(Monster.DamageImmunities);
            spnConditionImmunities.Text = MonsterMethodRepository.getConditionImmunities(Monster);

            ShowFilledProperties();
            LoadMonsterAbilities();
            LoadActions();
        }

        private void ShowFilledProperties()
        {
            // only reveal the following properties if they are properly filled in
            // margin reacts to which certain properties are visible as to avoid empty spaces while hidden or no whitespace while revealed
            if (MonsterMethodRepository.getSavingThrows(Monster) != "")
            {
                lblSavingThrowsString.IsVisible = true;
                lblSavingThrowsString.Margin = new Thickness(0, 16, 0, 16);
                lblSensesString.Margin = new Thickness(0, 0, 0, 16);
            }
            if (MonsterMethodRepository.getSkills(Monster) != "")
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
            if (MonsterMethodRepository.getConditionImmunities(Monster) != "")
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
                    title.Text = $"{ability.Name} {MonsterMethodRepository.getUsage(ability)}";
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
                    title.Text = $"{action.Name} {MonsterMethodRepository.getUsage(action)}";
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
                    title.Text = $"{action.Name} {MonsterMethodRepository.getUsage(action)}";
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