using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.WebJobs.Extensions.Kafka;

namespace KafkaTest
{
    public class ConsumeKafkaTopic
    {
        [FunctionName(nameof(ConsumeKafkaTopic))]
        public void Run([KafkaTrigger(
            "LocalBroker",
            "tibiaTopic",
            ConsumerGroup = "$Default")] KafkaEventData<string>[] kafkaEvents,
            ILogger logger)
        {
            foreach(var kafkaEvent in kafkaEvents)
                logger.LogInformation($"KAFKA ON FIRE: {kafkaEvent.Value.ToString()}");
        }
    }
}