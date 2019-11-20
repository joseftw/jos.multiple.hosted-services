using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace JOS.Multiple.HostedServices.Features.KafkaDaemon
{
    public class KafkaDaemon : BaseBackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public KafkaDaemon(
            IServiceScopeFactory serviceScopeFactory,
            ILogger<KafkaDaemon> logger) : base(typeof(KafkaDaemon), logger)
        {
            _serviceScopeFactory = serviceScopeFactory ?? throw new ArgumentNullException(nameof(serviceScopeFactory));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    using (var scope = _serviceScopeFactory.CreateScope())
                    {
                        var kafkaConsumer = scope.ServiceProvider.GetRequiredService<KafkaConsumer>();
                        await kafkaConsumer.Consume();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                await StopAsync(stoppingToken);
            }
            
        }
    }
}
