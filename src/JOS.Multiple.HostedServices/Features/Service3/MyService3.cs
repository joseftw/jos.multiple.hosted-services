using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Timer = System.Threading.Timer;

namespace JOS.Multiple.HostedServices.Features.Service3
{
    public class MyService3 : BackgroundService
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

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{nameof(MyService3)} is starting...");

            _timer = new Timer(async state =>
            {
                _timer.Change(Timeout.InfiniteTimeSpan, TimeSpan.Zero);
                await _myService3Handler.DoWork(cancellationToken);
                _timer.Change(TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(5));
            }, null, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(5));

            return base.StartAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Yield();
            }
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{nameof(MyService3)} received stop signal...");
            return base.StopAsync(cancellationToken);
        }

        public override void Dispose()
        {
            _timer?.Dispose();
            base.Dispose();
        }
    }
}
