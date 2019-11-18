using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace JOS.Multiple.HostedServices.Features.Service2
{
    public class MyService2 : BackgroundService
    {
        private readonly MyService2Handler _myService2Handler;
        private readonly ILogger<MyService2> _logger;

        public MyService2(
            MyService2Handler myService2Handler,
            ILogger<MyService2> logger)
        {
            _myService2Handler = myService2Handler ?? throw new ArgumentNullException(nameof(myService2Handler));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{nameof(MyService2)} is starting...");
            await base.StartAsync(cancellationToken);
            _logger.LogInformation($"{nameof(MyService2)} has started");
        }

        protected override async Task ExecuteAsync(CancellationToken stopToken)
        {
            while (!stopToken.IsCancellationRequested)
            {
                await _myService2Handler.ListenForMessages(stopToken);
            }
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{nameof(MyService2)} received stop signal...");
            await base.StopAsync(cancellationToken);
            _logger.LogInformation($"{nameof(MyService2)} has stopped");
        }
    }
}
