using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace JOS.Multiple.HostedServices.Features.Service1
{
    public class MyService1Handler
    {
        private readonly ILogger<MyService1Handler> _logger;

        public MyService1Handler(ILogger<MyService1Handler> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task ListenForMessages(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Hello from {nameof(MyService1Handler)}");
            await Task.Delay(5000, cancellationToken);
        }
    }
}
