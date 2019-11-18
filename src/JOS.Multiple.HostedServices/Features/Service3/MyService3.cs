using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Timer = System.Threading.Timer;

namespace JOS.Multiple.HostedServices.Features.Service3
{
    public class MyService3 : IHostedService
    {
        private Timer _timer;
        private readonly MyService3Handler _myService3Handler;
        private readonly ILogger<MyService3> _logger;

        public MyService3(
            MyService3Handler myService3Handler,
            ILogger<MyService3> logger)
        {
            _myService3Handler = myService3Handler;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{nameof(MyService3)} is starting...");

            _timer = new Timer(async state =>
            {
                _timer?.Change(Timeout.InfiniteTimeSpan, TimeSpan.Zero);
                await _myService3Handler.DoWork(cancellationToken);
                _timer?.Change(TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(5));
            }, null, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(5));

            _logger.LogInformation($"{nameof(MyService3)} has started");

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{nameof(MyService3)} received stop signal...");
            _timer?.Dispose();
            _logger.LogInformation($"{nameof(MyService3)} has stopped");

            return Task.CompletedTask;
        }
    }
}
