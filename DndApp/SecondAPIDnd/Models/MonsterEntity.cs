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
        [IgnoreProperty]
        public SpeedProperties Speed
        {
            get
            {
                return JsonConvert.DeserializeObject<SpeedProperties>(SpeedString);
            }
            set
            {
                SpeedString = JsonConvert.SerializeObject(value);
            }
        }
        public int Strength { get; set; }
        public int Dexterity { get; set; }
        public int Constitution { get; set; }
        public int Intelligence { get; set; }
        public int Wisdom { get; set; }
        public int Charisma { get; set; }

        // PROFICIENCIES AND EXPERTISE PROPERTIES 
        [IgnoreProperty]
        public List<ProficiencyAndValue> Proficiencies
        {
            get
            {
                return JsonConvert.DeserializeObject<List<ProficiencyAndValue>>(ProficienciesString);
            }
            set
            {
                ProficienciesString = JsonConvert.SerializeObject(value);
            }
        }

        // VULNERABLE AND RESISTANT DAMAGE TYPE PROPERTIES
        [IgnoreProperty]
        [JsonProperty(PropertyName = "damage_vulnerabilities")]
        public List<string> DamageVulnerabilities
        {
            get
            {
                return JsonConvert.DeserializeObject<List<string>>(DamageVulnerabilitiesString);
            }
            set
            {
                DamageVulnerabilitiesString = JsonConvert.SerializeObject(value);
            }
        }

        [IgnoreProperty]
        [JsonProperty(PropertyName = "damage_resistances")]
        public List<string> DamageResistances
        {
            get
            {
                return JsonConvert.DeserializeObject<List<string>>(DamageResistancesString);
            }
            set
            {
                DamageResistancesString = JsonConvert.SerializeObject(value);
            }
        }

        [IgnoreProperty]
        [JsonProperty(PropertyName = "damage_immunities")]
        public List<string> DamageImmunities
        {
            get
            {
                return JsonConvert.DeserializeObject<List<string>>(DamageImmunitiesString);
            }
            set
            {
                DamageImmunitiesString = JsonConvert.SerializeObject(value);
            }
        }

        [IgnoreProperty]
        [JsonProperty(PropertyName = "condition_immunities")]
        public List<ConditionImmunity> ConditionImmunities
        {
            get
            {
                return JsonConvert.DeserializeObject<List<ConditionImmunity>>(ConditionImmunitiesString);
            }
            set
            {
                ConditionImmunitiesString = JsonConvert.SerializeObject(value);
            }
        }

        // SENSE AND LANGUAGE PROPERTIES
        [IgnoreProperty]
        public SensesObject Senses
        {
            get
            {
                return JsonConvert.DeserializeObject<SensesObject>(SensesObjectString);
            }
            set
            {
                SensesObjectString = JsonConvert.SerializeObject(value);
            }
        }
        public string Languages { get; set; }

        // CHALLENGE PROPERTIES
        [JsonProperty(PropertyName = "challenge_rating")]
        public double ChallengeRating { get; set; }
        // is an int most of the time, but sometimes you have creatures with a CR of for example 0.25
        [JsonProperty(PropertyName = "xp")]
        public int ExperiencePoints { get; set; }

        // ABILITY AND ACTION PROPERTIES 
        [IgnoreProperty]
        [JsonProperty(propertyName: "special_abilities")]
        public List<Action> SpecialAbilities
        {
            get
            {
                return JsonConvert.DeserializeObject<List<Action>>(SpecialAbilitiesString);
            }
            set
            {
                SpecialAbilitiesString = JsonConvert.SerializeObject(value);
            }
        }

        [IgnoreProperty]
        public List<Action> Actions
        {
            get
            {
                return JsonConvert.DeserializeObject<List<Action>>(ActionsString);
            }
            set
            {
                ActionsString = JsonConvert.SerializeObject(value);
            }
        }

        [IgnoreProperty]
        [JsonProperty(propertyName: "legendary_actions")]
        public List<Action> LegendaryActions
        {
            get
            {
                return JsonConvert.DeserializeObject<List<Action>>(LegendaryActionsString);
            }
            set
            {
                LegendaryActionsString = JsonConvert.SerializeObject(value);
            }
        }

        // END OF PROPERTIES

        // we all know you can't save an array in a column, this is a workaround

        // SERIALIZED PROPERTIES
        [JsonIgnore]
        public string SpeedString { get; set; }

        [JsonIgnore]
        public string ProficienciesString { get; set; }

        [JsonIgnore]
        public string DamageVulnerabilitiesString { get; set; }

        [JsonIgnore]
        public string DamageResistancesString { get; set; }

        [JsonIgnore]
        public string DamageImmunitiesString { get; set; }

        [JsonIgnore]
        public string ConditionResistancesString { get; set; }

        [JsonIgnore]
        public string ConditionImmunitiesString { get; set; }

        [JsonIgnore]
        public string SpecialAbilitiesString { get; set; }

        [JsonIgnore]
        public string ActionsString { get; set; }

        [JsonIgnore]
        public string LegendaryActionsString { get; set; }

        [JsonIgnore]
        public string SensesObjectString { get; set; }

        // END OF SERIALIZED PROPERTIES
    }
}

