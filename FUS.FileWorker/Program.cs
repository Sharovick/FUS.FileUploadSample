using FUS.Core.EFCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NServiceBus;

namespace FUS.FileWorker
{
    public partial class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args)
                .UseDefaultServiceProvider(options => options.ValidateScopes = false)
                .Build()
                .Seed<FUSContext>()
                .Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseWindowsService()
                .ConfigureServices((hostContext, services) =>
                {
                    ConfigureServices(hostContext, services);
                })
                .UseNServiceBus(context =>
                {
                    return ConfigureNServiceBus(context);
                });
    }
    public static class HostExtention
    {
        public static IHost Seed<TContext>(this IHost host) where TContext : FUSContext
        {
            // Create a scope to get scoped services.
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var logger = services.GetRequiredService<ILogger<TContext>>();
                // get the service provider and db context.
                var context = services.GetService<TContext>();
                context.SeedData();
            }

            return host;
        }
    }
}
