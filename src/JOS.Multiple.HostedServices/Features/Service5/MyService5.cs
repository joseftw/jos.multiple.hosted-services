using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace JOS.Multiple.HostedServices.Features.Service5
{
    public class MyService5 : IHostedService
    {
        private readonly MyService5Handler _myService5Handler;
        private readonly ILogger<MyService5> _logger;

        public MyService5(
            MyService5Handler myService5Handler,
            ILogger<MyService5> logger)
        {
            _myService5Handler = myService5Handler ?? throw new ArgumentNullException(nameof(myService5Handler));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{nameof(MyService5)} is starting...");
            var task = ExecuteAsync(cancellationToken);
            _logger.LogInformation($"{nameof(MyService5)} has started");
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{nameof(MyService5)} received stop signal...");
            _logger.LogInformation($"{nameof(MyService5)} has stopped");

            return Task.CompletedTask;
        }

        private async Task ExecuteAsync(CancellationToken stopToken)
        {
            while (!stopToken.IsCancellationRequested)
            {
                await _myService5Handler.ListenForMessages(stopToken);
            }
        }
    }
}
