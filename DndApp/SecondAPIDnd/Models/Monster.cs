using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DndApp.Models
{
    public class Monster
    {
        // ** PROPERTIES **
        // DESCRIPTIVE PROPERTIES
        [JsonProperty(PropertyName = "index")]
        public string MonsterId { get; set; }
        public string Name { get; set; }
        // MonsterId is a lowercase no space version of the name used in the url for a specific monster
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

        // ** METHODS **
        public override string ToString()
        {
            return $"{this.Name} - {this.Size} {this.Type}, {this.Alignment} ({this.Strength},{this.Dexterity},{this.Constitution},{this.Intelligence},{this.Wisdom},{this.Charisma}) => CR: {this.ChallengeRating}";
        }
        // END OF METHODS
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
