using DndApp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
    }
}
