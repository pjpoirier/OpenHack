using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using MyOwn.Company.Items;
namespace MyOwn.Company
{
    public static class GetRating
    {
        [FunctionName("GetRating")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            [CosmosDB(
                databaseName: "BFYOCDB",
                collectionName: "Rating",
                ConnectionStringSetting = "CosmosDbConnectionString",
                Id = "{Query.ratingId}",
                PartitionKey = "{Query.ratingId}"
            )] ToDoItem toDoItem,
            ILogger log)
        {
          
            if (toDoItem == null) 
            {
                return new NotFoundObjectResult("Could not find any ratings today");
            } else {
             return new OkObjectResult(toDoItem);
        }
        }
    }
}