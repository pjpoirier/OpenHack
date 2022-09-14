using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;

namespace Brandie.Test
{
    public static class CreateRating
    {
        //TODO AuthorizationLevel????
        [FunctionName("CreateRating")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            [CosmosDB(databaseName: "BFYOCDB",
                    collectionName: "Rating",
                    ConnectionStringSetting = "CosmosDbConnectionString")]IAsyncCollector<dynamic> ratingsOut,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string hostName = "https://serverlessohapi.azurewebsites.net/api";
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);            
            string userId = data.userId;
            string productId = data.productId;
            int rating =  data.rating ?? -1;
            dynamic userResponse;
            dynamic productResponse;
            bool userValid, productValid;
            
            // Validate User & Product
            using (var client = new HttpClient()){
                var resp = await client.GetAsync(hostName + "/GetUser?userId=" + userId);
                userResponse = await resp.Content.ReadAsStringAsync();
                userValid = resp.IsSuccessStatusCode;
                resp = await client.GetAsync(hostName + "/GetProduct?productId=" + productId);
                productResponse = await resp.Content.ReadAsStringAsync();
                productValid = resp.IsSuccessStatusCode;
            }

            string responseMessage = "";

            // IF BAD USER
            if (!userValid) return new NotFoundObjectResult(userResponse);

            // IF BAD PRODUCT 
            else if (!productValid) return new NotFoundObjectResult(productResponse);
            
            else if (rating > 5 || rating < 0) {
                responseMessage = "Please provide a valid rate.";
                return new BadRequestObjectResult(responseMessage);
            }

            // Product & User are valid. Now we need to add field & write to DB
            else {
                
                // Add required fields
                data["id"] = Guid.NewGuid().ToString();
                data["timestamp"] = DateTime.Now.ToUniversalTime().ToString("u");
                responseMessage = data.ToString();

                //TODO Write to DB
                await ratingsOut.AddAsync(data);
            }

            return new OkObjectResult(responseMessage);
        }
    }
}
