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
                var reg = JsonConvert.DeserializeObject<Monster>(requestBody);
                var connectionString = Environment.GetEnvironmentVariable("ConnectionStringStorage");
            }
            catch (Exception ex)
            {
                log.LogError(ex.ToString());
                return new StatusCodeResult(500);
            }

            return new OkObjectResult("");
        }
    }
}
