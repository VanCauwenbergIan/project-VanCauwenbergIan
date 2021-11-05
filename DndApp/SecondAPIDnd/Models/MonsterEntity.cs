using Microsoft.Azure.Cosmos.Table;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DndApp.Models
{
    public class MonsterEntity : TableEntity
    {
        // ** CONSTRUCTORS **
        public MonsterEntity()
        {

        }

        public MonsterEntity(string monsterId, string type)
        {
            this.PartitionKey = type;
            this.RowKey = monsterId;
        }
        // END OF CONSTRUCTORS

        // ** PROPERTIES **
        // DESCRIPTIVE PROPERTIES
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
    }
}
