using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace JOS.Multiple.HostedServices.Features.Service2
{
    public class MyService2 : BaseBackgroundService
    {
        private readonly MyService2Handler _myService2Handler;

        public MyService2(
            MyService2Handler myService2Handler,
            ILogger<MyService2> logger) : base(typeof(MyService2), logger)
        {
            _myService2Handler = myService2Handler ?? throw new ArgumentNullException(nameof(myService2Handler));
        }

        protected override async Task ExecuteAsync(CancellationToken stopToken)
        {
            while (!stopToken.IsCancellationRequested)
            {
                await _myService2Handler.ListenForMessages(stopToken);
            }
        }
    }
}
