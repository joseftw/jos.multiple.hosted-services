using System.Threading.Tasks;
using JOS.Multiple.HostedServices.Features.Service1;
using JOS.Multiple.HostedServices.Features.Service2;
using JOS.Multiple.HostedServices.Features.Service3;
using JOS.Multiple.HostedServices.Features.Shared.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace JOS.Multiple.HostedServices
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var hostBuilder = CreateHostBuilder(args);

            var host = hostBuilder.Build();

            var serviceScopeFactory = host.Services.GetRequiredService<IServiceScopeFactory>();

            using (var scope = serviceScopeFactory.CreateScope())
            {
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
                var hostingEnvironment = scope.ServiceProvider.GetRequiredService<IHostingEnvironment>();
                logger.LogInformation($"Starting application. Environment: {hostingEnvironment.EnvironmentName}");
            }

            await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            var hostBuilder = new HostBuilder()
                .ConfigureHostConfiguration((configurationBuilder) =>
                {
                    configurationBuilder.AddCommandLine(args);
                })
                .ConfigureAppConfiguration((context, configurationBuilder) =>
                {
                    configurationBuilder.AddJsonFile("appsettings.json", false);
                    configurationBuilder.AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json", true);
                    configurationBuilder.AddJsonFile("appsettings.Local.json", true);
                })
                .ConfigureServices((services) =>
                {
                    services.AddSingleton<MyService1Handler>();
                    services.AddSingleton<MyService2Handler>();
                    services.AddSingleton<MyService3Handler>();

                    services.AddHostedService<MyService1>();
                    services.AddHostedService<MyService2>();
                    services.AddHostedService<MyService3>();
                })
                .ConfigureLogging((context, loggingBuilder) =>
                {
                    loggingBuilder.ConfigureLoggingFeature(
                        context.HostingEnvironment,
                        context.Configuration,
                        loggingBuilder.Services);
                });

            return hostBuilder;
        }
    }
}
