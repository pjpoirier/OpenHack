using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace Company.Function
{
    public static class GetRatings
    {
        [FunctionName("GetRatings")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "ratings/{userId}")] HttpRequest req,
            [CosmosDB(
                databaseName: "BFYOCDB",
                collectionName: "Rating",
                ConnectionStringSetting = "CosmosDbConnectionString",
                SqlQuery = "SELECT * FROM c WHERE c.userId = {userId}"
            )] IEnumerable<RatingItem> ratingItems,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            List<RatingItem> foundRatingItems = ratingItems.ToList();
            if (foundRatingItems.Count <= 0) 
            {
                return new NotFoundObjectResult(req);
            }
            return new OkObjectResult(foundRatingItems);
        }
    }
}
