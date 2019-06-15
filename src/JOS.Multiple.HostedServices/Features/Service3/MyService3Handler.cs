using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace JOS.Multiple.HostedServices.Features.Service3
{
    public class MyService3Handler
    {
        private readonly ILogger<MyService3Handler> _logger;

        public MyService3Handler(ILogger<MyService3Handler> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task DoWork(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Hello from {nameof(MyService3Handler)}");
            await Task.Delay(10000, cancellationToken);
        }
    }
}
