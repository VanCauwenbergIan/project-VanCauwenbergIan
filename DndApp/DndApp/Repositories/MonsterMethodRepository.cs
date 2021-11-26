using DndApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Action = DndApp.Models.Action;

namespace DndApp.Repositories
{
    public class MonsterMethodRepository
    {
        public static string CheckForNaturalArmor(Monster m)
        {
            if (10 + getAbilityScoreModifier(m.Dexterity) < m.ArmorClass)
            {
                return "(natural armor)";
            }
            else
            {
                return "";
            }
        }

        public static int getAbilityScoreModifier(int a)
        {
            int asm = (int)Math.Floor((a - 10) / 2.0);

            return asm;
        }

        public static string getAbilityScoreModifierString(int a)
        {
            int asm = getAbilityScoreModifier(a);

            if (asm >= 0)
            {
                return $" (+{asm})";
            }
            else
            {
                return $"({asm})";
            }
        }

        public static string StringifySpeed(Monster m)
        {
            List<string> arrayStrings = new List<string>();

            if (m.Speed.WalkingSpeed != null && m.Speed.WalkingSpeed != "0 ft.")
            {
                arrayStrings.Add($"{m.Speed.WalkingSpeed} walking");
            }
            if (m.Speed.SwimmingSpeed != null)
            {
                arrayStrings.Add($"{m.Speed.SwimmingSpeed} swimming");
            }
            if (m.Speed.FlyingSpeed != null)
            {
                arrayStrings.Add($"{m.Speed.FlyingSpeed} flying");
            }
            if (m.Speed.BurrowingSpeed != null)
            {
                arrayStrings.Add($"{m.Speed.BurrowingSpeed} burrowing");
            }
            if (m.Speed.ClimbingSpeed != null)
            {
                arrayStrings.Add($"{m.Speed.ClimbingSpeed} climbing");
            }
            if (m.Speed.Hover != false)
            {
                arrayStrings.Add("hovers");
            }

            return string.Join(", ", arrayStrings);
        }

        public static string stringifyListStrings(List<string> list)
        {
            if (list.Count() > 0)
            {
                return string.Join(", ", list);
            }
            else
            {
                return "";
            }
        }

        public static string getSavingThrows(Monster m)
        {
            List<string> proficiencies = new List<string>();

            foreach (ProficiencyAndValue proficiency in m.Proficiencies)
            {
                if (proficiency.Proficiency.Name.StartsWith("Saving Throw: ") == true)
                {
                    proficiencies.Add($"{proficiency.Proficiency.Name.Replace("Saving Throw: ", "")} +{proficiency.Value}");
                }
            }

            return stringifyListStrings(proficiencies);
        }

        public static string getSkills(Monster m)
        {
            List<string> skills = new List<string>();

            foreach (ProficiencyAndValue proficiency in m.Proficiencies)
            {
                if (proficiency.Proficiency.Name.StartsWith("Skill: ") == true)
                {
                    skills.Add($"{proficiency.Proficiency.Name.Replace("Skill: ", "")} +{proficiency.Value}");
                }
            }

            return stringifyListStrings(skills);
        }

        public static string getConditionImmunities(Monster m)
        {
            List<string> conditionImmunities = new List<string>();

            foreach (ConditionImmunity conditionImmunity in m.ConditionImmunities)
            {
                conditionImmunities.Add(conditionImmunity.Name);
            }

            return stringifyListStrings(conditionImmunities);
        }

        public static string getSenses(Monster m)
        {
            List<string> senses = new List<string>();

            if (m.Senses.Blindsight != null)
            {
                senses.Add($"Blindsight {m.Senses.Blindsight}");
            }
            if (m.Senses.Darkvision != null)
            {
                senses.Add($"Darkvision {m.Senses.Darkvision}");
            }
            if (m.Senses.Tremorsense != null)
            {
                senses.Add($"Tremorsense {m.Senses.Tremorsense}");
            }
            if (m.Senses.Truesight != null)
            {
                senses.Add($"Truesight {m.Senses.Truesight}");
            }

            senses.Add($"Passive Perception {m.Senses.PassivePerception}");

            return stringifyListStrings(senses);
        }

        public static string getUsage(Action action)
        {
            if (action.Usage != null)
            {
                if (action.Usage.Type == "recharge on roll")
                {
                    return $"(Recharge {action.Usage.MinimumValue}-{getMinDiceRoll(action.Usage.Dice)})";
                }
                else
                {
                    return $"({action.Usage.Times} {action.Usage.Type})";
                }
            }
            else
            {
                return "";
            }
        }

        public static int getMinDiceRoll(string dice)
        {
            string[] num = dice.Split('d');

            return (Convert.ToInt32(num[0]) * Convert.ToInt32(num[1]));
        }

        public static string checkLanguages(string languages)
        {
            if (languages != "" && languages != null)
            {
                return languages;
            }
            else
            {
                return "-";
            }
        }
    }
}
