using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using DndApp.Models;
using Microsoft.Azure.Cosmos.Table;
using System.Diagnostics;

namespace SecondAPIDnd
{
    public static class MonsterFunctions
    {
        [FunctionName("AddMonster")]
        public static async Task<IActionResult> AddMonster(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "/monsters")] HttpRequest req,
            ILogger log)
        {
            try
            {
                var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var m = JsonConvert.DeserializeObject<Monster>(requestBody);
                var connectionString = Environment.GetEnvironmentVariable("ConnectionStringStorage");
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);
                CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
                CloudTable table = tableClient.GetTableReference("monsters");
                string monsterID = m.Name.ToLower().Replace(' ', '-');

                Debug.WriteLine($"MonsterId: {monsterID} registered");

                MonsterEntity monsterEntity = new MonsterEntity(monsterID, m.Type)
                {
                    Name = m.Name,
                    Size = m.Size,
                    Type = m.Type,
                    Alignment = m.Alignment,
                    ArmorClass = m.ArmorClass,
                    HitPoints = m.HitPoints,
                    HitDice = m.HitDice,
                    Speed = m.Speed,
                    Strength = m.Strength,
                    Dexterity = m.Dexterity,
                    Constitution = m.Constitution,
                    Intelligence = m.Intelligence,
                    Wisdom = m.Wisdom,
                    Charisma = m.Charisma,
                    Proficiencies = m.Proficiencies,
                    DamageVulnerabilities = m.DamageVulnerabilities,
                    DamageResistances = m.DamageResistances,
                    DamageImmunities = m.DamageImmunities,
                    ConditionImmunities = m.ConditionImmunities,
                    Senses = m.Senses,
                    Languages = m.Languages,
                    ChallengeRating = m.ChallengeRating,
                    ExperiencePoints = m.ExperiencePoints,
                    SpecialAbilities = m.SpecialAbilities,
                    Actions = m.Actions,
                    LegendaryActions = m.LegendaryActions
                };

                TableOperation insertOperation = TableOperation.Insert(monsterEntity);

                await table.ExecuteAsync(insertOperation);

                return new OkObjectResult(m);
            }
            catch (Exception ex)
            {
                log.LogError(ex.ToString());
                return new StatusCodeResult(500);
            }
        }
    }
}
