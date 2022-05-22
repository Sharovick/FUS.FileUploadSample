using FUS.Common.Exceptions;
using FUS.File.Messages;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using NServiceBus;

namespace FUS.FileUploadSample
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                }).UseNServiceBus(context =>
                {
                    return ConfigureNServiceBus(context);
                });


        private static EndpointConfiguration ConfigureNServiceBus(HostBuilderContext context)
        {
            var endpointConfiguration = new EndpointConfiguration(context.Configuration["NServiceBus:EndpointName"]);
            endpointConfiguration.SendOnly();
            endpointConfiguration.EnableInstallers();
            var transport = endpointConfiguration.UseTransport<LearningTransport>();

            var routing = transport.Routing();
            routing.RouteToEndpoint(typeof(SendFilesCommand), context.Configuration["NServiceBus:FileEndpointName"]);

            var recoverabilitySettings = endpointConfiguration.Recoverability();
            recoverabilitySettings.AddUnrecoverableException(typeof(IntegrationException));
            return endpointConfiguration;
        }
    }
}
