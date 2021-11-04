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
    public class MonsterRepository
    {
        private const string _BASEURI = "https://www.dnd5eapi.co/api";

        private static HttpClient GetHttpClient()
        {
            HttpClient client = new HttpClient();

            client.DefaultRequestHeaders.Add("Accept", "application/json");

            return client;
        }

        public static async Task<List<Monster>> GetMonstersAsync()
        {
            string url = $"{_BASEURI}/monsters";

            using (HttpClient client = GetHttpClient())
            {
                try
                {
                    // PROBLEM: the monsters we want are referred to in a url within a object part of a results array.


                    string json = await client.GetStringAsync(url);
                    List<Monster> monsters = new List<Monster>();
                    List<JsonToMonster.Result> results = new List<JsonToMonster.Result>();
                    // first we deserialize the jsonstring into an object of the the class we created
                    // that class has one property which is an array / list of results, within the class we have another class which gets the id and name property of each of the objects within that results array
                    var data = JsonConvert.DeserializeObject<JsonToMonster>(json);
                    results = data.results;
                    
                    foreach (JsonToMonster.Result result in results)
                    {
                        Debug.WriteLine(result);
                    }
                    return monsters;
                }
                catch (Exception ex)
                {

                    throw ex;
                }
            }
        }
    }
}
