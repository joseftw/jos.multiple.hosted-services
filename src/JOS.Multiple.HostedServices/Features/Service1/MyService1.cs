using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace JOS.Multiple.HostedServices.Features.Service1
{
    public class MyService1 : BackgroundService
    {
        private readonly MyService1Handler _myService1Handler;
        private readonly ILogger<MyService1> _logger;

        public MyService1(
            MyService1Handler myService1Handler,
            ILogger<MyService1> logger)
        {
            _myService1Handler = myService1Handler ?? throw new ArgumentNullException(nameof(myService1Handler));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{nameof(MyService1)} is starting...");
            return base.StartAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stopToken)
        {
            while (!stopToken.IsCancellationRequested)
            {
                await _myService1Handler.ListenForMessages(stopToken);
            }
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{nameof(MyService1)} received stop signal...");
            return base.StopAsync(cancellationToken);
        }
    }
}
