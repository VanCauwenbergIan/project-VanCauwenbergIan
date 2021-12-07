﻿using DndApp.Models;
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
        public Monster SelectedMonster { get; set; }
        public List<string> OptionsSize = new List<string> { "Tiny", "Small", "Medium", "Large", "Huge", "Gargantuan" };
        // 1d4, 1d6, 1d8, 1d10, 1d12, 1d20
        public List<string> OptionsAlignment = new List<string> { "chaotic evil", "neutral evil", "lawful evil", "chaotic neutral", "neutral", "lawful neutral", "unaligned", "chaotic good", "neutral good", "lawful good" };

        // constructor for put 
        public AddMonsterPage(Monster selectedMonster, List<Monster> oMonsters, List<Monster> hbMonsters)
        {
            SelectedMonster = selectedMonster;
            InitializeComponent();
            LoadIcons();
            LoadPickers();
            LoadMonsterInfo();
        }

        // constructor for post
        public AddMonsterPage(List<Monster> oMonsters, List<Monster> hbMonsters)
        {
            InitializeComponent();
            LoadIcons();
            LoadPickers();
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

        private void LoadMonsterInfo()
        {
            entName.Text = SelectedMonster.Name;

            entType.Text = SelectedMonster.Type;
            pckSize.SelectedItem = SelectedMonster.Size;

            if (OptionsAlignment.Contains(SelectedMonster.Alignment) == true)
            {
                pckAlignment.SelectedItem = SelectedMonster.Alignment;
            }
            else
            {
                pckAlignment.SelectedItem = "unaligned";
            }

            entProficiencyBonus.Text = SelectedMonster.ProficiencyBonus.ToString();
            entHitDice.Text = SelectedMonster.AmountOfHPDice.ToString();

            entStrength.Text = SelectedMonster.Strength.ToString();
            entDexterity.Text = SelectedMonster.Dexterity.ToString();
            entConstitution.Text = SelectedMonster.Dexterity.ToString();
            entIntelligence.Text = SelectedMonster.Intelligence.ToString();
            entWisdom.Text = SelectedMonster.Wisdom.ToString();
            entCharisma.Text = SelectedMonster.Charisma.ToString();

            entNaturalArmor.Text = SelectedMonster.NaturalArmor.ToString();
            entChallengeRating.Text = SelectedMonster.ChallengeRating.ToString();

            entWalkingSpeed.Text = SelectedMonster.Speed.WalkingSpeed;
            entFlyingSpeed.Text = SelectedMonster.Speed.FlyingSpeed;
            entClimbingSpeed.Text = SelectedMonster.Speed.ClimbingSpeed;
            entSwimmingSpeed.Text = SelectedMonster.Speed.SwimmingSpeed;
            entBurrowingSpeed.Text = SelectedMonster.Speed.BurrowingSpeed;
            cbxHover.IsChecked = SelectedMonster.Speed.Hover;

            entBlindsight.Text = SelectedMonster.Senses.Blindsight;
            entDarkvision.Text = SelectedMonster.Senses.Darkvision;
            entTremorsense.Text = SelectedMonster.Senses.Tremorsense;
            entTruesight.Text = SelectedMonster.Senses.Truesight;
            entLanguages.Text = SelectedMonster.Languages;
        }

        private void LoadPickers()
        {
            foreach (string size in OptionsSize)
            {
                pckSize.Items.Add(size);
            }

            foreach (string alignment in OptionsAlignment)
            {
                pckAlignment.Items.Add(alignment);
            }
        }
    }
}