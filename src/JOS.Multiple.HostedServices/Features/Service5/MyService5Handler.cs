using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace JOS.Multiple.HostedServices.Features.Service5
{
    public class MyService5Handler
    {
        private readonly ILogger<MyService5Handler> _logger;

        public MyService5Handler(ILogger<MyService5Handler> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task ListenForMessages(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Hello from {nameof(MyService5Handler)}");
            await Task.Delay(5000, cancellationToken);
        }
    }
}
