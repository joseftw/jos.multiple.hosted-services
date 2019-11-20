using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace JOS.Multiple.HostedServices.Features.Service1
{
    public class MyService1 : BaseBackgroundService
    {
        private readonly MyService1Handler _myService1Handler;

        public MyService1(
            MyService1Handler myService1Handler,
            ILogger<MyService1> logger) : base(typeof(MyService1), logger)
        {
            _myService1Handler = myService1Handler ?? throw new ArgumentNullException(nameof(myService1Handler));
        }

        protected override async Task ExecuteAsync(CancellationToken stopToken)
        {
            while (!stopToken.IsCancellationRequested)
            {
                await _myService1Handler.ListenForMessages(stopToken);
            }
        }
    }
}
