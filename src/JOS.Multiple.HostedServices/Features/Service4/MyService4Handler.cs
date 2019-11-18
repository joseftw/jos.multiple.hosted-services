using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace JOS.Multiple.HostedServices.Features.Service4
{
    public class MyService4Handler
    {
        private readonly ILogger<MyService4Handler> _logger;

        public MyService4Handler(ILogger<MyService4Handler> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task ListenForMessages(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Hello from {nameof(MyService4Handler)}");
            await Task.Delay(5000, cancellationToken);
        }
    }
}
