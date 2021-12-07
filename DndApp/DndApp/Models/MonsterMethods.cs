using DndApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Action = DndApp.Models.Action;

namespace DndApp.Repositories
{
    // other file, because tha main class is already crowded enough
    public class MonsterMethods
    {
        public static string StringifySpeed(Monster m)
        {
            // what the name says, it adds substrings to a string if the properties of the Speed class have the right value
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
            // a simple way turn a list of strings into a string with the right formatting
            if (list.Count() > 0)
            {
                return string.Join(", ", list);
            }
            else
            {
                return "";
            }
        }

        public static int getAbilityScoreModifier(int a)
        {
            // this is also just a standard dnd5e formula to calculate the ability score modifier for a certain stat / ability score
            int asm = (int)Math.Floor((a - 10) / 2.0);

            return asm;
        }

        public static string getAbilityScoreModifierString(int a)
        {
            // all this method does is adding a '+' in front of positive ability score modifiers
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

        public static string getSavingThrows(Monster m)
        {
            // filters object of the proficiency class into only those that are saving throws and returns them as a formatted string
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
            // filters object of the proficiency class into only those that are skills and returns them as a formatted string
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
            // converts the list of conditionImmunities into a list of strings and returns them
            List<string> conditionImmunities = new List<string>();

            foreach (ConditionImmunity conditionImmunity in m.ConditionImmunities)
            {
                conditionImmunities.Add(conditionImmunity.Name);
            }

            return stringifyListStrings(conditionImmunities);
        }

        public static string getSenses(Monster m)
        {
            // filters the senses class of a monster and returns it as a formatted string
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
            // returns the usage subclass of the acion class as a formatted string
            if (action.Usage != null)
            {
                if (action.Usage.Type == "recharge on roll")
                {
                    return $"(Recharge {action.Usage.MinimumValue}-{getDiceRoll(action.Usage.Dice)})";
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

        public static int getDiceRoll(string dice)
        {
            // some actions require a min roll of dice to recharge them (HP can also be rolled, but a lot of people just take the average result instead), these values are formatted as amount d maxValueOfDice
            string[] num = dice.Split('d');

            return (Convert.ToInt32(num[0]) * Convert.ToInt32(num[1]));
        }

        // these are pretty self-explanatory

        public static List<string> getAllTypes(List<Monster> monsters)
        {
            List<string> types = new List<string>();

            foreach (Monster monster in monsters)
            {
                if (types.Contains(monster.Type) == false)
                {
                    types.Add(monster.Type);
                }
            }

            return types;
        }

        public static List<string> getAllSizes(List<Monster> monsters)
        {
            List<string> sizes = new List<string>();

            foreach (Monster monster in monsters)
            {
                if (sizes.Contains(monster.Size) == false)
                {
                    sizes.Add(monster.Size);
                }
            }

            return sizes;
        }

        public static List<string> getAllAlignments(List<Monster> monsters)
        {
            List<string> alignements = new List<string>();

            foreach (Monster monster in monsters)
            {
                if (alignements.Contains(monster.Alignment) == false)
                {
                    alignements.Add(monster.Alignment);
                }
            }

            return alignements;
        }

        public static List<SubOptionCheckbox> getFilterCheckboxes(List<Monster> monsters,string chosenOption)
        {
            List<string> subOptions = new List<string>();
            List<SubOptionCheckbox> checkboxes = new List<SubOptionCheckbox>();

            if (chosenOption.ToLower() == "type")
            {
                subOptions = getAllTypes(monsters);
            }
            else if (chosenOption.ToLower() == "size")
            {
                subOptions = getAllSizes(monsters);
            }
            else if (chosenOption.ToLower() == "alignment")
            {
                subOptions = getAllAlignments(monsters);
            }
            else if (chosenOption.ToLower() == "legendary")
            {
                subOptions = new List<string> { "Yes", "No" };
            }

            subOptions = subOptions.OrderBy(o => o).ToList();

            foreach (string name in subOptions)
            {
                SubOptionCheckbox subOptionCheckbox = new SubOptionCheckbox()
                {
                    Name = name,
                    Status = false
                };

                checkboxes.Add(subOptionCheckbox);
            }

            return checkboxes;
        }

        public static List<SubOptionEntry> getFilterEntries()
        {
            List<string> subOptions = new List<string> {"From", "To"};
            List<SubOptionEntry> entries = new List<SubOptionEntry>();
            
            foreach (string name in subOptions)
            {
                SubOptionEntry subOptionEntry = new SubOptionEntry()
                {
                    Name = name,
                    Limit = null
                };

                entries.Add(subOptionEntry);
            }

            return entries;

        }

        public static string CheckForNaturalArmor(Monster m)
        {
            // compares their AC to the dnd5e formula (which would be their AC if they didn't have extra armor)
            if (m.NaturalArmor > 0)
            {
                return "(natural armor)";
            }
            else
            {
                return "";
            }
        }

        public static string checkLanguages(string languages)
        {
            // returns "-" for monsters that can't understand or speak any languages that are listed in the sourcebooks (so mostly normal animals)
            if (languages != "" && languages != null)
            {
                return languages;
            }
            else
            {
                return "-";
            }
        }

        public static List<Monster> sortListBy (List<Monster> originalMonsters, string parameter)
        {
            parameter = parameter.ToLower();

            List<Monster> customSorted = new List<Monster>();

            if (parameter == "type")
            {
                return originalMonsters.OrderBy(o => o.Type).ThenBy(o => o.Name).ToList();
            }
            else if (parameter == "cr")
            {
                return originalMonsters.OrderBy(o => o.ChallengeRating).ThenBy(o => o.Name).ToList();
            }
            else if (parameter == "size")
            {
                // custom sort by size
                List<Monster> tiny = new List<Monster>();
                List<Monster> small = new List<Monster>();
                List<Monster> medium = new List<Monster>();
                List<Monster> large = new List<Monster>();
                List<Monster> huge = new List<Monster>();
                List<Monster> gargantuan = new List<Monster>();

                foreach (Monster monster in originalMonsters)
                {
                    if (monster.Size == "Tiny")
                    {
                        tiny.Add(monster);
                    }
                    else if (monster.Size == "Small")
                    {
                        small.Add(monster);
                    }
                    else if (monster.Size == "Medium")
                    {
                        medium.Add(monster);
                    }
                    else if (monster.Size == "Large")
                    {
                        large.Add(monster);
                    }
                    else if (monster.Size == "Huge")
                    {
                        huge.Add(monster);
                    }
                    else if (monster.Size == "Gargantuan")
                    {
                        gargantuan.Add(monster);
                    }
                    else
                    {
                        medium.Add(monster);
                    }
                }

                tiny = tiny.OrderBy(o => o.Name).ToList();
                small = small.OrderBy(o => o.Name).ToList();
                medium = medium.OrderBy(o => o.Name).ToList();
                large = large.OrderBy(o => o.Name).ToList();
                huge = huge.OrderBy(o => o.Name).ToList();
                gargantuan = gargantuan.OrderBy(o => o.Name).ToList();

                return (List<Monster>)(tiny.Concat(small).Concat(medium).Concat(large).Concat(huge).Concat(gargantuan).ToList());
            }
            else if (parameter == "alignment")
            {
                // custom sort by alignment
                List<Monster> chaoticEvil = new List<Monster>();
                List<Monster> neutralEvil = new List<Monster>();
                List<Monster> lawfulEvil = new List<Monster>();
                List<Monster> chaoticNeutal = new List<Monster>();
                List<Monster> unaligned = new List<Monster>();
                List<Monster> trueNeutral = new List<Monster>();
                List<Monster> lawfulNeutral = new List<Monster>();
                List<Monster> chaoticGood = new List<Monster>();
                List<Monster> neutralGood = new List<Monster>();
                List<Monster> lawfulGood = new List<Monster>();

                foreach (Monster monster in originalMonsters)
                {
                    if (monster.Alignment == "chaotic evil")
                    {
                        chaoticEvil.Add(monster);
                    }
                    else if (monster.Alignment == "neutral evil")
                    {
                        neutralEvil.Add(monster);
                    }
                    else if (monster.Alignment == "lawful evil")
                    {
                        lawfulEvil.Add(monster);
                    }
                    else if (monster.Alignment == "chaotic neutral")
                    {
                        chaoticNeutal.Add(monster);
                    }
                    else if (monster.Alignment == "unaligned" || monster.Alignment == "any alignment")
                    {
                        unaligned.Add(monster);
                    }
                    else if (monster.Alignment == "neutral")
                    {
                        trueNeutral.Add(monster);
                    }
                    else if (monster.Alignment == "lawful neutral")
                    {
                        lawfulNeutral.Add(monster);
                    }
                    else if (monster.Alignment == "chaotic good")
                    {
                        chaoticGood.Add(monster);
                    }
                    else if (monster.Alignment == "neutral good")
                    {
                        neutralGood.Add(monster);
                    }
                    else if (monster.Alignment == "lawful good")
                    {
                        lawfulGood.Add(monster);
                    }
                    // sometimes you'll have something along the lines of 'any alignment as long as it's evil'
                    else
                    {
                        // monsters with for example 'any non-good alignment' will be treated as unaligned by the sort method
                        if (monster.Alignment.Contains("non-evil") == true || monster.Alignment.Contains("non-good") == true)
                        {
                            unaligned.Add(monster);
                        }
                        else if (monster.Alignment.Contains("good") == true)
                        {
                            neutralGood.Add(monster);
                        }
                        else if (monster.Alignment.Contains("neutral") == true)
                        {
                            trueNeutral.Add(monster);
                        }
                        else if (monster.Alignment.Contains("evil") == true)
                        {
                            neutralEvil.Add(monster);
                        }
                        else
                        {
                            unaligned.Add(monster);
                        }
                    }
                }

                // now sort them all alphabetically by name and add them together
                chaoticEvil = chaoticEvil.OrderBy(o => o.Name).ToList();
                neutralEvil = neutralEvil.OrderBy(o => o.Name).ToList();
                lawfulEvil = lawfulEvil.OrderBy(o => o.Name).ToList();
                chaoticNeutal = chaoticNeutal.OrderBy(o => o.Name).ToList();
                unaligned = unaligned.OrderBy(o => o.Name).ToList();
                trueNeutral = trueNeutral.OrderBy(o => o.Name).ToList();
                lawfulNeutral = lawfulNeutral.OrderBy(o => o.Name).ToList();
                chaoticGood = chaoticGood.OrderBy(o => o.Name).ToList();
                neutralGood = neutralGood.OrderBy(o => o.Name).ToList();
                lawfulGood = lawfulGood.OrderBy(o => o.Name).ToList();

                return (List<Monster>)(chaoticEvil.Concat(neutralEvil).Concat(lawfulEvil).Concat(chaoticNeutal).Concat(unaligned).Concat(trueNeutral).Concat(lawfulNeutral).Concat(chaoticGood).Concat(neutralGood).Concat(lawfulGood).ToList());
            }
            else if (parameter == "ac")
            {
                return originalMonsters.OrderBy(o => o.ArmorClass).ThenBy(o => o.Name).ToList();
            }
            else if (parameter == "hp")
            {
                return originalMonsters.OrderBy(o => o.HitPoints).ThenBy(o => o.Name).ToList();
            }
            else if (parameter == "la")
            {
                // custom sort by legendary acions
                List<Monster> nonlegendary = new List<Monster>();
                List<Monster> legendary = new List<Monster>();
 
                foreach (Monster monster in originalMonsters)
                {
                    if (monster.LegendaryActions != null)
                    {
                        legendary.Add(monster);
                    }
                    else
                    {
                        nonlegendary.Add(monster);
                    }
                }

                nonlegendary = nonlegendary.OrderBy(o => o.Name).ToList();
                legendary = legendary.OrderBy(o => o.LegendaryActions.Count()).ThenBy(o => o.Name).ToList();

                return (List<Monster>)(nonlegendary.Concat(legendary).ToList());
            }
            else
            {
                return originalMonsters;
            }
        }

        public static List<Monster> filterByCheckboxes (List<Monster> monsters, List<SubOptionCheckbox> checkboxes, string stringId)
        {
            List<Monster> filteredList = new List<Monster>();
            // I only stumbled upon this one recently, probably a lot of ifs I can replace
            List<SubOptionCheckbox> options = checkboxes.Where(o => o.Status == true).ToList();

            if (stringId == "Type")
            {
                foreach (Monster monster in monsters)
                {
                    foreach (SubOptionCheckbox option in options)
                    {
                        if (monster.Type == option.Name)
                        {
                            filteredList.Add(monster);
                        }
                    }
                }
            }
            else if (stringId == "Size")
            {

                foreach (Monster monster in monsters)
                {
                    foreach (SubOptionCheckbox option in options)
                    {
                        if (monster.Size == option.Name)
                        {
                            filteredList.Add(monster);
                        }
                    }
                }
            }
            else if (stringId == "Alignment")
            {

                foreach (Monster monster in monsters)
                {
                    foreach (SubOptionCheckbox option in options)
                    {
                        if (monster.Alignment == option.Name)
                        {
                            filteredList.Add(monster);
                        }
                    }
                }
            }
            else if (stringId == "Legendary")
            {

                foreach (Monster monster in monsters)
                {
                    foreach (SubOptionCheckbox option in options)
                    {
                        if (option.Name == "Yes" && monster.LegendaryActions != null)
                        {
                            filteredList.Add(monster);
                        }
                        else if (option.Name == "No" && monster.LegendaryActions == null)
                        {
                            filteredList.Add(monster);
                        }
                    }
                }
            }

            return filteredList;
        }

        public static List<Monster> filterByEntries(List<Monster> monsters, List<SubOptionEntry> entries, string stringId)
        {
            List<Monster> filteredList = new List<Monster>();

            if (stringId == "Challenge")
            {
                foreach (Monster monster in monsters)
                {
                    if (monster.ChallengeRating >= entries[0].Limit && monster.ChallengeRating <= entries[1].Limit)
                    {
                        filteredList.Add(monster);
                    }
                }
            }
            else if (stringId == "Armor Class")
            {
                foreach (Monster monster in monsters)
                {
                    if (monster.ArmorClass >= entries[0].Limit && monster.ArmorClass <= entries[1].Limit)
                    {
                        filteredList.Add(monster);
                    }
                }
            }
            else if (stringId == "Average Hitpoints")
            {
                foreach (Monster monster in monsters)
                {
                    if (monster.HitPoints >= entries[0].Limit && monster.HitPoints <= entries[1].Limit)
                    {
                        filteredList.Add(monster);
                    }
                }
            }

            return filteredList;
        }
    }
}
