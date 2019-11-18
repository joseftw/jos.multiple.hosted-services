using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace JOS.Multiple.HostedServices.Features.Service4
{
    public class MyService4 : IHostedService
    {
        private Task _executingTask;
        private readonly MyService4Handler _myService4Handler;
        private readonly ILogger<MyService4> _logger;

        public MyService4(
            MyService4Handler myService4Handler,
            ILogger<MyService4> logger)
        {
            _myService4Handler = myService4Handler ?? throw new ArgumentNullException(nameof(myService4Handler));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{nameof(MyService4)} is starting...");
            _executingTask = ExecuteAsync(cancellationToken);
            _logger.LogInformation($"{nameof(MyService4)} has started");
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{nameof(MyService4)} received stop signal...");

            if (_executingTask == null)
            {
                return Task.CompletedTask;
            }

            var hej = _executingTask.IsCompleted;

            _logger.LogInformation($"{nameof(MyService4)} has stopped");

            return Task.CompletedTask;
        }

        private async Task ExecuteAsync(CancellationToken stopToken)
        {
            while (!stopToken.IsCancellationRequested)
            {
                await _myService4Handler.ListenForMessages(stopToken);
            }
        }
    }
}
