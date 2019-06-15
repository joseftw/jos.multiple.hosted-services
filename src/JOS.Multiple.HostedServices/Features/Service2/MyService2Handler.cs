using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace JOS.Multiple.HostedServices.Features.Service2
{
    public class MyService2Handler
    {
        private readonly ILogger<MyService2Handler> _logger;

        public MyService2Handler(ILogger<MyService2Handler> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task ListenForMessages(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                _logger.LogInformation($"Hello from {nameof(MyService2Handler)}");
                await Task.Delay(1000, cancellationToken);
            }
        }
    }
}
