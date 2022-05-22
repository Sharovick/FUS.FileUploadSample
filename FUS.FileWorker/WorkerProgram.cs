using FUS.Common.Exceptions;
using FUS.Core.EFCore;
using FUS.Infrastrucure.Interfaces;
using FUS.Infrastrucure.Repository;
using FUS.Infrastrucure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;
using System;

namespace FUS.FileWorker
{
    public partial class Program
    {
        private static void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services)
        {
            services.AddDbContext<FUSContext>(opt => opt.UseInMemoryDatabase("Test"));
            services.AddScoped(typeof(ICustomerService), typeof(CustomerService));
            services.AddScoped(typeof(IUserService), typeof(UserService));
            services.AddScoped(typeof(IFileService), typeof(FileService));
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        }

        private static EndpointConfiguration ConfigureNServiceBus(HostBuilderContext context)
        {
            var endpointConfiguration = new EndpointConfiguration(context.Configuration["NServiceBus:EndpointName"]);
            endpointConfiguration.EnableInstallers();
            endpointConfiguration.UseTransport<LearningTransport>();
            endpointConfiguration.UsePersistence<LearningPersistence>();
            endpointConfiguration.SendFailedMessagesTo("error");
            endpointConfiguration.AuditProcessedMessagesTo("audit");

            var recoverabilitySettings = endpointConfiguration.Recoverability();
            recoverabilitySettings.AddUnrecoverableException(typeof(IntegrationException));
            recoverabilitySettings.Immediate(immidiate =>
            {
                immidiate.NumberOfRetries(1);
            });
            recoverabilitySettings.Delayed(delayed =>
            {
                delayed.NumberOfRetries(2).TimeIncrease(TimeSpan.FromSeconds(10));
            });

            return endpointConfiguration;
        }
    }
}
