using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace JOS.Multiple.HostedServices.Features.KafkaDaemon
{
    public class KafkaConsumer
    {
        private readonly ILogger<KafkaConsumer> _logger;
        private int _count;

        public KafkaConsumer(ILogger<KafkaConsumer> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Consume()
        {
            _count++;
            _logger.LogInformation($"Consuming message {_count} from Kafka");
            await Task.Yield();
        }
    }
}
