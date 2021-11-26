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
            Monster = selectedMonster;

            InitializeComponent();
            LoadIcons();
            LoadMonster();
        }

        private void LoadIcons()
        {
            btnBack.Source = ImageSource.FromResource("DndApp.Assets.buttonBack.png");
            btnEdit.Source = ImageSource.FromResource("DndApp.Assets.buttonEdit.png");
            btnRadarChart.Source = ImageSource.FromResource("DndApp.Assets.buttonRadarRed.png");

            TapGestureRecognizer recognizer_return = new TapGestureRecognizer();

            recognizer_return.Tapped += Recognizer_Tapped_return;
            btnBack.GestureRecognizers.Add(recognizer_return);
        }

        private void Recognizer_Tapped_return(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        private void LoadMonster()
        {
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

            LoadMonsterAbilities();
            LoadActions();
        }

        private void LoadMonsterAbilities()
        {
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

            if (Monster.LegendaryActions != null)
            {
                Label instruction = new Label();
                instruction.Text = $"After each other creatures turn the {Monster.Name} may make a \nlegendary action. The {Monster.Name} has 3 legendary action points, \nwhich it regains at the start of its own turn. Each legendary \naction costs 1 of these points unless otherwise specified.";
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
            srvActions.IsVisible = true;
            srvStats.IsVisible = false;
            btnActions.TextColor = Color.FromHex("#F4F7FB");
            btnStats.TextColor = Color.FromHex("#8999A6");
            bxvActions.BackgroundColor = Color.FromHex("E40712");
            bxvStats.BackgroundColor = Color.Transparent;
        }

        private void btnStatsClicked(object sender, EventArgs e)
        {
            srvActions.IsVisible = false;
            srvStats.IsVisible = true;
            btnActions.TextColor = Color.FromHex("#8999A6");
            btnStats.TextColor = Color.FromHex("#F4F7FB");
            bxvActions.BackgroundColor = Color.Transparent;
            bxvStats.BackgroundColor = Color.FromHex("E40712");
        }
    }
}