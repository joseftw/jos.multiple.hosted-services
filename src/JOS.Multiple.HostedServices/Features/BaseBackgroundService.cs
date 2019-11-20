using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace JOS.Multiple.HostedServices.Features
{
    public abstract class BaseBackgroundService : BackgroundService
    {
        private readonly Type _type;
        protected readonly ILogger<BaseBackgroundService> Logger;

        protected BaseBackgroundService(Type type, ILogger<BaseBackgroundService> logger)
        {
            _type = type ?? throw new ArgumentNullException(nameof(type));
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            Logger.LogInformation($"Starting {_type.Name}...");
            await base.StartAsync(cancellationToken);
            Logger.LogInformation($"{_type.Name} has started");
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            Logger.LogInformation($"Stopping {_type.Name}...");
            await base.StopAsync(cancellationToken);
            Logger.LogInformation($"{_type.Name} has stopped");
        }

        protected abstract override Task ExecuteAsync(CancellationToken stoppingToken);
    }
}
