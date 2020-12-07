using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs.Extensions.Kafka;

namespace KafkaTest
{
    public static class ServiceWorkerTrigger
    {
        [FunctionName("ServiceWorkerTrigger")]
        public static async Task Run(
            [TimerTrigger("0 */1 * * * *")]TimerInfo myTimer, 
            [Kafka("LocalBroker", "tibiaTopic")] IAsyncCollector<KafkaEventData<string>> events,
            ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            var client = new HttpClient();
            var response = await client.GetAsync("https://api.tibiadata.com/v2/characters/Trollefar.json");
            var content = await response.Content.ReadAsStringAsync();

            try
            {
                var kafkaEvent = new KafkaEventData<string>()
                {
                    Value = content,
                };

                await events.AddAsync(kafkaEvent);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            log.LogInformation($"CONTENT RETRIEVED: {content}");
        }
    }
}
