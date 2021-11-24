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
using System.Collections.Generic;

namespace SecondAPIDnd
{
    public static class MonsterFunctions
    {
        [FunctionName("AddMonster")]
        public static async Task<IActionResult> AddMonster(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "monsters")] HttpRequest req, ILogger log)
        {
            try
            {
                // read incoming json
                var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var m = JsonConvert.DeserializeObject<Monster>(requestBody);

                // establish a connection with the table
                var connectionString = Environment.GetEnvironmentVariable("ConnectionStringStorage");
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);
                CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
                CloudTable table = tableClient.GetTableReference("monsters");
                string monsterID = m.Name.ToLower().Replace(' ', '-');
                m.MonsterId = monsterID;

                Debug.WriteLine($"MonsterId: {monsterID} registered");

                // change the received monster into something that's saveable in our table storage (practically the same as monster, but with a row and partitionkey).
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

                // write new monster away to table storage
                TableOperation insertOperation = TableOperation.Insert(monsterEntity);

                await table.ExecuteAsync(insertOperation);

                m.MonsterId = monsterID;

                return new OkObjectResult(m);
            }
            catch (Exception ex)
            {
                log.LogError(ex.ToString());
                return new StatusCodeResult(500);
            }
        }

        [FunctionName("UpdateMonster")]
        public static async Task<IActionResult> UpdateMonster(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "monsters/{id}")] HttpRequest req, string id, ILogger log)
        {
            try
            {
                var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var m = JsonConvert.DeserializeObject<Monster>(requestBody);

                var connectionString = Environment.GetEnvironmentVariable("ConnectionStringStorage");
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);
                CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
                CloudTable table = tableClient.GetTableReference("monsters");

                // if the user changes the name, the id will remain the same, keep that in mind! 
                // The only way to change this would be creating a new one if the name has been changed
                // if the type changes a new monster will be created in another partition!

                // change the received monster into something that's saveable in our table storage (practically the same as monster, but with a row and partitionkey).
                MonsterEntity monsterEntity = new MonsterEntity(id, m.Type)
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

                TableOperation replaceOperation = TableOperation.InsertOrReplace(monsterEntity);

                await table.ExecuteAsync(replaceOperation);

                m.MonsterId = id;

                return new OkObjectResult(m);
            }
            catch (Exception ex)
            {
                log.LogError(ex.ToString());
                return new StatusCodeResult(500);
            }
        }
        [FunctionName("SelectMonster")]
        public static async Task<IActionResult> SelectMonster(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "monsters")] HttpRequest req, ILogger log)
        {
            try
            {
                List<Monster> monsters = new List<Monster>();

                var connectionString = Environment.GetEnvironmentVariable("ConnectionStringStorage");
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);
                CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
                CloudTable table = tableClient.GetTableReference("monsters");
                TableQuery<MonsterEntity> query = new TableQuery<MonsterEntity>();

                var result = await table.ExecuteQuerySegmentedAsync<MonsterEntity>(query, null);

                foreach (var m in result.Results)
                {
                    monsters.Add(new Monster()
                    {
                        MonsterId = m.PartitionKey,
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
                    });
                }

                return new OkObjectResult(monsters);
            }
            catch (Exception ex)
            {
                log.LogError(ex.ToString());
                return new StatusCodeResult(500);
            }
        }

        [FunctionName("DeleteMonster")]
        public static async Task<IActionResult> DeleteMonster(
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "monsters/{type}/{id}")] HttpRequest req, string type, string id, ILogger log)
        {
            try
            {
                var connectionString = Environment.GetEnvironmentVariable("ConnectionStringStorage");
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);
                CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
                CloudTable table = tableClient.GetTableReference("monsters");

                // check which row has the right Partition and Rowkey
                var conditionPK = TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, type);
                var conditionFK = TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, id);
                var Filter = TableQuery.CombineFilters(conditionPK, TableOperators.And, conditionFK);

                var query = new TableQuery<MonsterEntity>().Where(Filter);
                string returnValue = "";

                var results = await table.ExecuteQuerySegmentedAsync<MonsterEntity>(query, null);
                // delete the item, foreach is only used as an easy way to get item out of our results, there should always only be one since the combination of PK and FK should be unique!
                if (results.Results.Count > 0)
                {
                    foreach (var item in results)
                    {
                        var deleteOperation = TableOperation.Delete(item);
                        await table.ExecuteAsync(deleteOperation);

                        returnValue = $"Deleted monster {type}/{id}!";
                    }
                }
                else
                {
                    returnValue = "No monster with those credentials found...";
                }

                return new OkObjectResult(returnValue);
            }
            catch (Exception ex)
            {
                log.LogError(ex.ToString());
                return new StatusCodeResult(500);
            }
        }
    }
}

