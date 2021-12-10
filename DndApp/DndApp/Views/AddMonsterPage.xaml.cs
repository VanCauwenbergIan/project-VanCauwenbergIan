using DndApp.Models;
using DndApp.Repositories;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    public partial class AddMonsterPage : ContentPage
    {
        public Monster SelectedMonster { get; set; }
        public List<string> OptionsSize = new List<string> { "Tiny", "Small", "Medium", "Large", "Huge", "Gargantuan" };
        // 1d4, 1d6, 1d8, 1d10, 1d12, 1d20
        public List<string> OptionsAlignment = new List<string> { "chaotic evil", "neutral evil", "lawful evil", "chaotic neutral", "neutral", "lawful neutral", "unaligned", "chaotic good", "neutral good", "lawful good" };
        public List<Monster> OriginalMonsters { get; set; }
        public List<Monster> HomebrewMonsters { get; set; }

        public List<ProficiencyAndValue> Proficiencies { get; set; }
        public List<ProficiencyAndValue> Expertises { get; set; }

        // constructor for put 
        public AddMonsterPage(Monster selectedMonster, List<Monster> oMonsters, List<Monster> hbMonsters)
        {
            SelectedMonster = selectedMonster;
            OriginalMonsters = oMonsters;
            HomebrewMonsters = hbMonsters;

            InitializeComponent();
            LoadIcons();
            LoadPickers();
            CheckConnection();
            LoadMonsterInfo();
        }

        // constructor for post
        public AddMonsterPage(List<Monster> oMonsters, List<Monster> hbMonsters)
        {
            InitializeComponent();
            LoadIcons();
            LoadPickers();
            CheckConnection();
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
            btnAddProf.Source = ImageSource.FromResource("DndApp.Assets.buttonAddRed.png");
            btnAddExpert.Source = ImageSource.FromResource("DndApp.Assets.buttonAddRed.png");
            btnAddAbility.Source = ImageSource.FromResource("DndApp.Assets.buttonAddRed.png");
            btnAddResitance.Source = ImageSource.FromResource("DndApp.Assets.buttonAddRed.png");
            btnAddDamageImmunity.Source = ImageSource.FromResource("DndApp.Assets.buttonAddRed.png");
            btnAddVulnerability.Source = ImageSource.FromResource("DndApp.Assets.buttonAddRed.png");
            btnAddCoditionImmunity.Source = ImageSource.FromResource("DndApp.Assets.buttonAddRed.png");
            btnSize.Source = ImageSource.FromResource("DndApp.Assets.buttonDropSmall.png");
            btnAlignment.Source = ImageSource.FromResource("DndApp.Assets.buttonDropSmall.png");

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

            if (OriginalMonsters.Contains(SelectedMonster) == true)
            {
                lblWarning.IsVisible = true;
            }

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

            entHitDice.Text = SelectedMonster.AmountOfHPDice.ToString();

            entStrength.Text = SelectedMonster.Strength.ToString();
            entDexterity.Text = SelectedMonster.Dexterity.ToString();
            entConstitution.Text = SelectedMonster.Constitution.ToString();
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

            Proficiencies = MonsterMethods.CheckProficiencies(SelectedMonster, false);
            Expertises = MonsterMethods.CheckProficiencies(SelectedMonster, true);

            lvwProficiencies.ItemsSource = Proficiencies;
            lvwExpertise.ItemsSource = Expertises;
            lvwAbilities.ItemsSource = SelectedMonster.SpecialAbilities;
            lvwResistances.ItemsSource = SelectedMonster.DamageResistances;
            lvwDamageImmunities.ItemsSource = SelectedMonster.DamageImmunities;
            lvwVulnerabilities.ItemsSource = SelectedMonster.DamageVulnerabilities;
            lvwConditionImmunities.ItemsSource = SelectedMonster.ConditionImmunities;
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

        private async void btnConfirmClicked(Object sender, EventArgs e)
        {
            List<string> warnings = CheckUserInput();

            if (warnings.Count() > 0)
            {
                lblWarning.Text = $"Something went wrong: {MonsterMethods.stringifyListStrings(warnings)}!";
                lblWarning.TextColor = Color.FromHex("#E40712");
                lblWarning.IsVisible = true;
            }
            else
            {
                // properties used for calculations
                string name = entName.Text;
                int dex = Int32.Parse(entDexterity.Text);
                int con = Int32.Parse(entConstitution.Text);
                int wis = Int32.Parse(entWisdom.Text);
                int na = Int32.Parse(entNaturalArmor.Text);
                double cr = Double.Parse(entChallengeRating.Text);

                string id = ComposeId(name);
                int ac = CalculateArmorClass(dex, na);
                string hd = ComposeHitDice(Int32.Parse(entHitDice.Text), pckSize.SelectedItem.ToString());
                int hp = CalculateHitPoints(hd, con);
                int xp = CalculateExperiencePoints(cr);
                int pp = CalculatePassivePerception(cr, wis);
                // change after adding proficiencies!!!!!!! 

                // remaining properties
                string size = pckSize.SelectedItem.ToString();
                string type = entType.Text;
                string alignment = pckAlignment.SelectedItem.ToString();
                int strength = Int32.Parse(entStrength.Text);
                int intelligence = Int32.Parse(entIntelligence.Text);
                int charisma = Int32.Parse(entCharisma.Text);
                string languages = entLanguages.Text;
                string walkingSpeed = entWalkingSpeed.Text;
                string swimmingSpeed = entSwimmingSpeed.Text;
                string flyingSpeed = entFlyingSpeed.Text;
                string burrowingSpeed = entBurrowingSpeed.Text;
                string climbingSpeed = entClimbingSpeed.Text;
                bool hover = cbxHover.IsChecked;
                string blindsight = entBlindsight.Text;
                string darkvision = entDarkvision.Text;
                string tremorsense = entTremorsense.Text;
                string truesight = entTruesight.Text;

                List<ProficiencyAndValue> proficiencies = new List<ProficiencyAndValue>();
                List<string> damageVulnerabilities = new List<string>();
                List<string> damageResistances = new List<string>();
                List<string> damageImmunities = new List<string>();
                List<ConditionImmunity> conditionImmunities = new List<ConditionImmunity>();
                List<Action> specialAbilities = new List<Action>();
                List<Action> actions = new List<Action>();
                List<Action> legendaryActions = new List<Action>();

                if (SelectedMonster == null || OriginalMonsters.Contains(SelectedMonster))
                {
                    Monster newMonster = new Monster()
                    {
                        Name = name,
                        Size = size,
                        Type = type,
                        Alignment = alignment,
                        ArmorClass = ac,
                        HitPoints = hp,
                        HitDice = hd,
                        Strength = strength,
                        Dexterity = dex,
                        Constitution = con,
                        Intelligence = intelligence,
                        Wisdom = wis,
                        Charisma = charisma,
                        Languages = languages,
                        ChallengeRating = cr,
                        ExperiencePoints = xp,
                        Speed = new SpeedProperties()
                        {
                            WalkingSpeed = walkingSpeed,
                            SwimmingSpeed = swimmingSpeed,
                            FlyingSpeed = flyingSpeed,
                            BurrowingSpeed = burrowingSpeed,
                            ClimbingSpeed = climbingSpeed,
                            Hover = hover
                        },
                        Senses = new SensesObject()
                        {
                            Blindsight = blindsight,
                            Darkvision = darkvision,
                            Tremorsense = tremorsense,
                            Truesight = truesight,
                            PassivePerception = pp
                        },
                        Proficiencies = proficiencies,
                        DamageVulnerabilities = damageVulnerabilities,
                        DamageResistances = damageResistances,
                        DamageImmunities = damageImmunities,
                        ConditionImmunities = conditionImmunities,
                    };

                    await MonsterRepository.PostHomebrewMonsterAsync(newMonster);

                    // problem: normally we would pass on data when creating a page, but then the whole overviewpage would reload for one single monster. Not exactly the best UX
                    MessagingCenter.Send<Monster>(newMonster, "refresh!");
                }
                else
                {
                    SelectedMonster.MonsterId = id;
                    SelectedMonster.Name = name;
                    SelectedMonster.Size = size;
                    SelectedMonster.Type = type;
                    SelectedMonster.Alignment = alignment;
                    SelectedMonster.ArmorClass = ac;
                    SelectedMonster.HitPoints = hp;
                    SelectedMonster.HitDice = hd;
                    SelectedMonster.Strength = strength;
                    SelectedMonster.Dexterity = dex;
                    SelectedMonster.Constitution = con;
                    SelectedMonster.Intelligence = intelligence;
                    SelectedMonster.Wisdom = wis;
                    SelectedMonster.Charisma = charisma;
                    SelectedMonster.Languages = languages;
                    SelectedMonster.ChallengeRating = cr;
                    SelectedMonster.ExperiencePoints = xp;
                    SelectedMonster.Speed.WalkingSpeed = walkingSpeed;
                    SelectedMonster.Speed.SwimmingSpeed = swimmingSpeed;
                    SelectedMonster.Speed.FlyingSpeed = flyingSpeed;
                    SelectedMonster.Speed.BurrowingSpeed = burrowingSpeed;
                    SelectedMonster.Speed.ClimbingSpeed = climbingSpeed;
                    SelectedMonster.Speed.Hover = hover;
                    SelectedMonster.Senses.Blindsight = blindsight;
                    SelectedMonster.Senses.Darkvision = darkvision;
                    SelectedMonster.Senses.Tremorsense = tremorsense;
                    SelectedMonster.Senses.Truesight = truesight;
                    SelectedMonster.Senses.PassivePerception = pp;
                    SelectedMonster.Proficiencies = proficiencies;
                    SelectedMonster.DamageVulnerabilities = damageVulnerabilities;
                    SelectedMonster.DamageResistances = damageResistances;
                    SelectedMonster.DamageImmunities = damageImmunities;
                    SelectedMonster.ConditionImmunities = conditionImmunities;

                    // note: regardless of making a put request... a post will still happen in some cases: the name has been changed, the type has been changed or both!!! (the monster's id (rowkey) is a formatted version of the name, type is the partitionkey of monsters within the table storage.
                    await MonsterRepository.PutHomebrewMonsterAsync(SelectedMonster);

                    MessagingCenter.Send<Monster>(SelectedMonster, "refresh!");
                }
            }
        }

        private string ComposeId(string name)
        {
            return name.ToLower().Replace(" ", "-");
        }

        private int CalculatePassivePerception(double cr, int wis)
        {
            // proficiency on wisdom saving throws and the perception skill also have an influence, I'll add this later.
            return 10 + CalculateProficiencyBonus(cr) + MonsterMethods.getAbilityScoreModifier(wis);
        }

        private int CalculateProficiencyBonus(double cr)
        {
            if (5 <= cr && cr < 9)
            {
                return 3;
            }
            else if (9 <= cr && cr < 13)
            {
                return 4;
            }
            else if (13 <= cr && cr < 17)
            {
                return 5;
            }
            else if (17 <= cr && cr < 21)
            {
                return 6;
            }
            else if (21 <= cr && cr < 25)
            {
                return 7;
            }
            else if (25 <= cr && cr < 29)
            {
                return 8;
            }
            else if (29 <= cr)
            {
                return 9;
            }
            else
            {
                return 2;
            }
        }

        private int CalculateArmorClass(int dex, int nac)
        {
            return 10 + MonsterMethods.getAbilityScoreModifier(dex) + nac;
        }

        private string ComposeHitDice(int amountHitdice, string size)
        {
            string typeHitDice = "";

            if (size == "Tiny")
            {
                typeHitDice = "d4";
            }
            else if (size == "Small")
            {
                typeHitDice = "d6";
            }
            else if (size == "Medium")
            {
                typeHitDice = "d8";
            }
            else if (size == "Large")
            {
                typeHitDice = "d10";
            }
            else if (size == "Huge")
            {
                typeHitDice = "d12";
            }
            else if (size == "Gargantuan")
            {
                typeHitDice = "d20";
            }

            return amountHitdice.ToString() + typeHitDice;
        }

        private int CalculateHitPoints(string hitdice, int con)
        {
            string[] num = hitdice.Split('d');
            int maxValue = Convert.ToInt32(num[1]);
            int amount = Convert.ToInt32(num[0]);

            // formula to get average hp: ((( max value single dice + 1 ) / 2) + con asm) * amount of dice
            int average = Convert.ToInt32(Math.Floor(((maxValue + 1) / 2.0) + MonsterMethods.getAbilityScoreModifier(con)) * amount);

            return average;
        }

        private int CalculateExperiencePoints(double cr)
        {
            // this is the table also used by the API https://roll20.net/compendium/dnd5e/Monsters#toc_26
            // problem is that I can't make any sense of it by CR alone. I've searched and calculated for over an hour and I can't find any generalised formula. There's definitely some external smoothing and factors at play.
            // now I'm basically forced to write 30+ cases

            if (cr == 0)
            {
                return 10;
            }
            else if (cr == 0.125)
            {
                return 25;
            }
            else if (cr == 0.25)
            {
                return 50;
            }
            else if (cr == 0.5)
            {
                return 100;
            }
            else if (cr == 1)
            {
                return 200;
            }
            else if (cr == 2)
            {
                return 450;
            }
            else if (cr == 3)
            {
                return 700;
            }
            else if (cr == 4)
            {
                return 1100;
            }
            else if (cr == 5)
            {
                return 1800;
            }
            else if (cr == 6)
            {
                return 2300;
            }
            else if (cr == 7)
            {
                return 2900;
            }
            else if (cr == 8)
            {
                return 3900;
            }
            else if (cr == 9)
            {
                return 5000;
            }
            else if (cr == 10)
            {
                return 5900;
            }
            else if (cr == 11)
            {
                return 7200;
            }
            else if (cr == 12)
            {
                return 8400;
            }
            else if (cr == 13)
            {
                return 10000;
            }
            else if (cr == 14)
            {
                return 11500;
            }
            else if (cr == 15)
            {
                return 13000;
            }
            else if (cr == 16)
            {
                return 15000;
            }
            else if (cr == 17)
            {
                return 18000;
            }
            else if (cr == 18)
            {
                return 20000;
            }
            else if (cr == 19)
            {
                return 22000;
            }
            else if (cr == 20)
            {
                return 25000;
            }
            else if (cr == 21)
            {
                return 33000;
            }
            else if (cr == 22)
            {
                return 41000;
            }
            else if (cr == 23)
            {
                return 50000;
            }
            else if (cr == 24)
            {
                return 62000;
            }
            else if (cr == 25)
            {
                return 75000;
            }
            else if (cr == 26)
            {
                return 90000;
            }
            else if (cr == 27)
            {
                return 105000;
            }
            else if (cr == 28)
            {
                return 120000;
            }
            else if (cr == 29)
            {
                return 135000;
            }
            else if (cr == 30)
            {
                return 155000;
            }
            else
            {
                return 0;
            }
        }

        private List<string> CheckUserInput()
        {
            List<string> errors = new List<string>();
            int hitdice;
            int Str, Dex, Con, Int, Wis, Cha;
            int na;
            double cr;

            if (entName.Text == null || entName.Text == "")
            {
                errors.Add("name can not be empty");
            }

            if (entType.Text == null || entType.Text == "")
            {
                errors.Add("type can not be empty");
            }

            if (pckAlignment.SelectedItem == null || pckSize.SelectedItem == null)
            {
                errors.Add("please make sure you've selected an alignment and size");
            }

            if (int.TryParse(entHitDice.Text, out hitdice) == false)
            {
                errors.Add("amount of hit dice must be a strictly positive number");
            }
            else if (hitdice < 0 || hitdice == 0)
            {
                errors.Add("amount of hit dice can not be negative or zero");
            }

            if (int.TryParse(entStrength.Text, out Str) == false || int.TryParse(entDexterity.Text, out Dex) == false || int.TryParse(entConstitution.Text, out Con) == false || int.TryParse(entIntelligence.Text, out Int) == false || int.TryParse(entWisdom.Text, out Wis) == false || int.TryParse(entCharisma.Text, out Cha) == false)
            {
                errors.Add("please make sure all base ability scores are strictly positive numbers");
            }
            else if (Str <= 0 || Dex <= 0 || Con <= 0 || Int <= 0 || Wis <= 0 || Cha <= 0)
            {
                errors.Add("please make sure all base ability scores are strictly positive numbers");
            }

            if (int.TryParse(entNaturalArmor.Text, out na) == false && entNaturalArmor.Text != null && entNaturalArmor.Text != "")
            {
                errors.Add("natural armor must be a positive number");
            }
            else if(entNaturalArmor.Text == null || entNaturalArmor.Text == "")
            {
                entNaturalArmor.Text = "0";
            }
            else if (na < 0)
            {
                errors.Add("natural armor must be a positive number");
            }

            if (double.TryParse(entChallengeRating.Text, out cr) == false && entChallengeRating.Text != null && entChallengeRating.Text != "")
            {
                errors.Add("challenge rating must be a positive number between 0 and 30");
            }
            else if (entChallengeRating.Text == null || entChallengeRating.Text == "")
            {
                entChallengeRating.Text = "0";
            }
            else if (cr < 0 || cr > 30)
            {
                errors.Add("challenge rating must be a positive number between 0 and 30");
            }
            else if ((Double.Parse(entChallengeRating.Text) <= 30 && Double.Parse(entChallengeRating.Text) >= 1) || Double.Parse(entChallengeRating.Text) == 0)
            {
                entChallengeRating.Text = Int32.Parse(entChallengeRating.Text).ToString();
            }
            else if (Double.Parse(entChallengeRating.Text) != 0.125 && Double.Parse(entChallengeRating.Text) != 0.25 && Double.Parse(entChallengeRating.Text) != 0.5)
            {
                errors.Add("challenge rating must be in line with the official manual, choose a whole number between 0 and 30 or 0.125, 0.25, 0.5");
            }

            if (entWalkingSpeed.Text != null)
            {
                if (entWalkingSpeed.Text.EndsWith(" ft.") == false)
                {
                    errors.Add("please make sure all speed properties are empty or follow the format: n ft.");
                }
            }
            if (entSwimmingSpeed.Text != null)
            {
                if (entSwimmingSpeed.Text.EndsWith(" ft.") == false)
                {
                    errors.Add("please make sure all speed properties are empty or follow the format: n ft.");
                }
            }
            if (entFlyingSpeed.Text != null)
            {
                if (entFlyingSpeed.Text.EndsWith(" ft.") == false)
                {
                    errors.Add("please make sure all speed properties are empty or follow the format: n ft.");
                }
            }
            if (entClimbingSpeed.Text != null)
            {
                if (entClimbingSpeed.Text.EndsWith(" ft.") == false)
                {
                    errors.Add("please make sure all speed properties are empty or follow the format: n ft.");
                }
            }
            if (entBurrowingSpeed.Text != null)
            {
                if (entBurrowingSpeed.Text.EndsWith(" ft.") == false)
                {
                    errors.Add("please make sure all speed properties are empty or follow the format: n ft.");
                }
            }
            if (entBlindsight.Text != null)
            {
                if (entBlindsight.Text.EndsWith(" ft.") == false)
                {
                    errors.Add("please make sure all sense properties are empty or follow the format: n ft.");
                }
            }
            if (entDarkvision.Text != null)
            {
                if (entDarkvision.Text.EndsWith(" ft.") == false)
                {
                    errors.Add("please make sure all sense properties are empty or follow the format: n ft.");
                }
            }
            if (entTremorsense.Text != null)
            {
                if (entTremorsense.Text.EndsWith(" ft.") == false)
                {
                    errors.Add("please make sure all sense properties are empty or follow the format: n ft.");
                }
            }
            if (entTruesight.Text != null)
            {
                if (entTruesight.Text.EndsWith(" ft.") == false)
                {
                    errors.Add("please make sure all sense properties are empty or follow the format: n ft.");
                }
            }

            return errors;
        }
    }
}