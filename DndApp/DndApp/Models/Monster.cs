﻿using DndApp.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DndApp.Models
{
    // main class for all monsters in the listview
    public class Monster
    {
        // ** PROPERTIES **
        // DESCRIPTIVE PROPERTIES
        [JsonProperty(PropertyName = "index")]
        public string MonsterId { get; set; }
        // MonsterId is a lowercase no space version of the name used in the url for a specific monster
        public string Name { get; set; }
        public string Size { get; set; }
        public string Type { get; set; }
        public string Alignment { get; set; }

        // STAT PROPERTIES
        [JsonProperty(PropertyName = "armor_class")]
        public int ArmorClass { get; set; }
        [JsonProperty(PropertyName = "hit_points")]
        public int HitPoints { get; set; }
        [JsonProperty(PropertyName = "hit_dice")]
        public string HitDice { get; set; }
        public SpeedProperties Speed { get; set; }

        public int Strength { get; set; }
        public int Dexterity { get; set; }
        public int Constitution { get; set; }
        public int Intelligence { get; set; }
        public int Wisdom { get; set; }
        public int Charisma { get; set; }

        // PROFICIENCIES AND EXPERTISE PROPERTIES 
        public List<ProficiencyAndValue> Proficiencies { get; set; }

        // VULNERABLE AND RESISTANT DAMAGE TYPE PROPERTIES
        [JsonProperty(PropertyName = "damage_vulnerabilities")]
        public List<string> DamageVulnerabilities { get; set; }
        [JsonProperty(PropertyName = "damage_resistances")]
        public List<string> DamageResistances { get; set; }
        [JsonProperty(PropertyName = "damage_immunities")]
        public List<string> DamageImmunities { get; set; }
        [JsonProperty(PropertyName = "condition_immunities")]
        public List<ConditionImmunity> ConditionImmunities { get; set; }
        // SENSE AND LANGUAGE PROPERTIES
        public SensesObject Senses { get; set; }
        public string Languages { get; set; }

        // CHALLENGE PROPERTIES
        [JsonProperty(PropertyName = "challenge_rating")]
        public double ChallengeRating { get; set; }
        // is an int most of the time, but sometimes you have creatures with a CR of for example 0.25
        [JsonProperty(PropertyName = "xp")]
        public int ExperiencePoints { get; set; }

        // ABILITY AND ACTION PROPERTIES 
        [JsonProperty(propertyName: "special_abilities")]
        public List<Action> SpecialAbilities { get; set; }
        public List<Action> Actions { get; set; }

        [JsonProperty(propertyName: "legendary_actions")]
        public List<Action> LegendaryActions { get; set; }

        // END OF PROPERTIES

        // ** CALCULATED PROPERTIES **
        public string Fraction
        {
            get
            {
                return ToFraction(ChallengeRating);
            }
        }
        // converts doubles into a fraction to display in the listview

        private static string ToFraction(double number)
        {
            int w, n, d;

            RoundToFraction(number, out w, out n , out d);

            string returnValue = $"{w}";

            // handling a double that's larger or equal to 1 
            if (w > 0)
            {
                if (n > 0)
                {
                    returnValue = $"{w} {n}/{d}";
                }
            }
            else
            {
                if (n > 0)
                {
                    returnValue = $"{n}/{d}";
                }
            }
            return returnValue;
        }

        static void RoundToFraction(double number, out int whole, out int numerator, out int denominator)
        {
            //smallest fraction we want to show is 1/8 => accuracy of 8
            int accuracy = 8; 
            double dblAccuracy = (double)accuracy;
            whole = (int)(Math.Truncate(number));
            var fraction = Math.Abs(number - whole);

            if (fraction == 0)
            {
                numerator = 0;
                denominator = 1;
                return;
            }
            
            // here we want to find the smallest possible gcd (so by extension the most simplified form of our fraction), while still meeting our accuracy
            var n = Enumerable.Range(0, accuracy + 1).SkipWhile(e => (e / dblAccuracy) < fraction).First();
            var hi = n / dblAccuracy;
            var lo = (n - 1) / dblAccuracy;

            if ((fraction - lo) < (hi - fraction))
            {
                n--;
            }
            if (n == accuracy)
            {
                whole++;
                numerator = 0;
                denominator = 0;
                return;
            }

            var gcd = GCD(n, accuracy);
            numerator = n / gcd;
            denominator = accuracy / gcd;
        }

        static int GCD(int n1, int n2)
        {
            if (n2 == 0)
            {
                return n1;
            }
            else
            {
                return GCD(n2, n1 % n2);
            }
        }

        // if you wonder where this comes from, it's just following the standard dnd5e rules for monsters
        public int ProficiencyBonus
        {
            get
            {
                if (5 <= this.ChallengeRating && this.ChallengeRating < 9)
                {
                    return 3;
                }
                else if (9 <= this.ChallengeRating && this.ChallengeRating < 13)
                {
                    return 4;
                }
                else if (13 <= this.ChallengeRating && this.ChallengeRating < 17)
                {
                    return 5;
                }
                else if (17 <= this.ChallengeRating && this.ChallengeRating < 21)
                {
                    return 6;
                }
                else if (21 <= this.ChallengeRating && this.ChallengeRating < 25)
                {
                    return 7;
                }
                else if (25 <= this.ChallengeRating && this.ChallengeRating < 29)
                {
                    return 8;
                }
                else if (29 <= this.ChallengeRating)
                {
                    return 9;
                }
                else
                {
                    return 2;
                }
            }
        }

        // just how dice rolls are formatted, nothing special here
        public int AmountOfHPDice
        {
            get
            {
                string[] num = this.HitDice.Split('d');

                return Convert.ToInt32(num[0]);
            }
        }

        // actual AC - standard 5e formula to get AC, that way we know if a creature has a boosted AC (aka natural armor)
        public int NaturalArmor
        {
            get
            {
                int regularAC = 10 + MonsterMethods.getAbilityScoreModifier(this.Dexterity);
                int extraAC = this.ArmorClass - regularAC;

                return extraAC;
            }
        }
        // END OF CALCULATED PROPERTIES
    }

    public class SpeedProperties
    {
        // most creatures won't have all of these (most likely one or two, with walk being the most common)
        [JsonProperty(PropertyName = "walk")]
        public string WalkingSpeed { get; set; }
        [JsonProperty(PropertyName = "swim")]
        public string SwimmingSpeed { get; set; }
        [JsonProperty(PropertyName = "fly")]
        public string FlyingSpeed { get; set; }
        [JsonProperty(PropertyName = "burrow")]
        public string BurrowingSpeed { get; set; }
        [JsonProperty(PropertyName = "climb")]
        public string ClimbingSpeed { get; set; }
        // a creature without walking speed, but with flying speed will often have hover
        public bool Hover { get; set; }
    }

    public class UsageObject
    {
        // from as far as I know you have two types:
        // 1)limited amount within a time period (or till a long or short rest)
        // 2)single use or more with a recharge when a dice roll is above a certain value
        public string Type { get; set; }
        public int Times { get; set; }
        public string Dice { get; set; }
        [JsonProperty(PropertyName = "min_value")]
        public int MinimumValue { get; set; }
    }

    public class ProficiencyAndValue
    {
        // a monster can have multiple proficiencies for different skills, wether it's a single proficiency for a skill or expertise (double proficiency) is decided partially by the value
        public ProficiencyObject Proficiency { get; set; }
        public class ProficiencyObject
        {
            [JsonProperty(PropertyName = "index")]
            public string ProficiencyId { get; set; }
            public string Name { get; set; }
        }
        public int Value { get; set; }
    }
    public class Action
    {
        public string Name { get; set; }
        [JsonProperty(PropertyName = "desc")]
        public string Description { get; set; }
        public UsageObject Usage { get; set; }
    }

    public class ConditionImmunity
    {
        [JsonProperty(PropertyName = "index")]
        public string ConditionID { get; set; }
        public string Name { get; set; }
    }

    public class SensesObject
    {
        public string Blindsight { get; set; }
        public string Darkvision { get; set; }
        public string Tremorsense { get; set; }
        public string Truesight { get; set; }
        [JsonProperty(PropertyName = "passive_perception")]
        public int PassivePerception { get; set; }
    }
}

