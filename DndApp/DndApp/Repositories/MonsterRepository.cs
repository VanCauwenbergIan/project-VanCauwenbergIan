using DndApp.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DndApp.Repositories
{
    // making requests from the APIs happens here
    public class MonsterRepository
    {
        private const string _BASEURI = "https://www.dnd5eapi.co";

        private static HttpClient GetHttpClient()
        {
            HttpClient client = new HttpClient();

            client.DefaultRequestHeaders.Add("Accept", "application/json");

            return client;
        }

        public static async Task<List<Monster>> GetMonstersAsync()
        {
            string url = $"{_BASEURI}/api/monsters";

            using (HttpClient client = GetHttpClient())
            {
                try
                {
                    // PROBLEM: the monsters we want are referred to in a url within a object part of a results array.

                    string json = await client.GetStringAsync(url);
                    List<Monster> monsters = new List<Monster>();
                    List<JsonToMonster.Result> results = new List<JsonToMonster.Result>();

                    // first we deserialize the jsonstring into an object of the the class we created
                    // that class has one property which is an array / list of results, within the class we have another class which saves the id and name property of each of the objects
                    var data = JsonConvert.DeserializeObject<JsonToMonster>(json);
                    results = data.results;
                    
                    // with the url of every result we can make another call to the API which gets the full information for each object instead of just the name
                    // We can't do this when a monster is selected for the details page, because we also want the CR, HP and AC to be shown in the listview + we need to able to sort (unless you only use the built-in methods)
                    foreach (JsonToMonster.Result result in results)
                    {
                        Monster monster = await GetMonsterAsync(result.Url);
                        monsters.Add(monster);
                    }

                    return monsters;
                }
                catch (Exception ex)
                {

                    throw ex;
                }
            }
        }

        public static async Task<Monster> GetMonsterAsync(string urlEndpoint)
        {
            string url = _BASEURI + urlEndpoint;

            using (HttpClient client = GetHttpClient())
            {
                try
                {
                    // since our main json contains almost no informaion for each object we have to make individual calls for each of them.
                    string json = await client.GetStringAsync(url);
                    Monster monster = JsonConvert.DeserializeObject<Monster>(json);

                    return monster;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public static async Task<List<ConditionImmunity>> GetConditions()
        {
            string url = $"{_BASEURI}/api/conditions";

            using (HttpClient client = GetHttpClient())
            {
                try
                {
                    List<ConditionImmunity> conditions = new List<ConditionImmunity>();

                    string json = await client.GetStringAsync(url);

                    var data = JsonConvert.DeserializeObject<JsonToMonster>(json).results;

                    // I'm just reusing the class I made to get monsters from the results in the main API call here
                    foreach (var item in data)
                    {
                        ConditionImmunity conditionImmunity = new ConditionImmunity()
                        {
                            ConditionID = item.MonsterId,
                            Name = item.Name
                        };

                        conditions.Add(conditionImmunity);
                    }

                    return conditions;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public static async Task<List<ProficiencyAndValue.ProficiencyObject>> GetProficiencies()
        {
            string url1 = $"{_BASEURI}/api/skills";
            string url2 = $"{_BASEURI}/api/proficiencies";

            using (HttpClient client = GetHttpClient())
            {
                try
                {
                    // skills and other proficiencies are split into two differnt API urls, the thing is we only want the saving throw proficiencies form that second link (not stuff like armor, instruments, weapons, ... those are for like actual npcs and pcs mostly)

                    List<ProficiencyAndValue.ProficiencyObject> skills = new List<ProficiencyAndValue.ProficiencyObject>();
                    List<ProficiencyAndValue.ProficiencyObject> allProficiencies = new List<ProficiencyAndValue.ProficiencyObject>();
                    List<ProficiencyAndValue.ProficiencyObject> usefullProficiencies = new List<ProficiencyAndValue.ProficiencyObject>();

                    string jsonSkills = await client.GetStringAsync(url1);
                    string jsonProficiencies = await client.GetStringAsync(url2);

                    var dataSkills = JsonConvert.DeserializeObject<JsonToMonster>(jsonSkills).results;
                    var dataProficiencies = JsonConvert.DeserializeObject<JsonToMonster>(jsonProficiencies).results;

                    foreach (var item in dataSkills)
                    {
                        ProficiencyAndValue.ProficiencyObject skill = new ProficiencyAndValue.ProficiencyObject()
                        {
                            ProficiencyId = item.MonsterId,
                            Name = "Skill: " + item.Name
                        };

                        skills.Add(skill);
                    }
                    foreach (var item in dataProficiencies)
                    {
                        ProficiencyAndValue.ProficiencyObject proficiency = new ProficiencyAndValue.ProficiencyObject()
                        {
                            ProficiencyId = item.MonsterId,
                            Name = item.Name
                        };

                        allProficiencies.Add(proficiency);
                    }
                    foreach (ProficiencyAndValue.ProficiencyObject proficiency in allProficiencies)
                    {
                        if (proficiency.ProficiencyId.StartsWith("saving-throw-"))
                        {
                            usefullProficiencies.Add(proficiency);
                        }
                    }

                    return usefullProficiencies.Concat(skills).ToList();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        // NOTE: the actual second API doesn't run locally (although you could for an emulated android device). I only included the second solution for easy readability. Deleting the 'SecondAPIDnd' project wouldn't affect the app.
        public static async Task PostHomebrewMonsterAsync(Monster monster)
        {
            string url = "https://secondapidnd.azurewebsites.net/api/monsters";
            // id gets handled in the api, we only need an url

            using (HttpClient client = GetHttpClient())
            {
                try
                {
                    // serializing our monster object into a json for the API
                    string json = JsonConvert.SerializeObject(monster);
                    HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
                    // the answer should the newly created json that has now been added to our storage table
                    var response = await client.PostAsync(url, content);
                }
                catch (Exception ex)
                {

                    throw ex;
                }
            }
        }

        public static async Task PutHomebrewMonsterAsync(Monster monster)
        {
            string url = $"https://secondapidnd.azurewebsites.net/api/monsters/{monster.MonsterId}";
            // id gets handled in the api, we only need an url

            using (HttpClient client = GetHttpClient())
            {
                try
                {
                    string json = JsonConvert.SerializeObject(monster);
                    HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await client.PutAsync(url, content);
                }
                catch (Exception ex)
                {

                    throw ex;
                }
            }
        }

        public static async Task<List<Monster>> GetHomebrewMonsterAsync()
        {
            string url = $"https://secondapidnd.azurewebsites.net/api/monsters";
            // id gets handled in the api, we only need an url

            using (HttpClient client = GetHttpClient())
            {
                try
                {
                    string json = await client.GetStringAsync(url);
                    List<Monster> monsters = JsonConvert.DeserializeObject<List<Monster>>(json);

                    return monsters;
                }
                catch (Exception ex)
                {

                    throw ex;
                }
            }
        }

        public static async Task DeleteHomebrewMonsterAsync(string type, string id)
        {
            string url = $"https://secondapidnd.azurewebsites.net/api/monsters/{type.ToLower()}/{id}";

            using (HttpClient client = GetHttpClient())
            {
                try
                {
                    await client.DeleteAsync(url);
                }
                catch (Exception ex)
                {

                    throw ex;
                }
            }
        }
    }
}
