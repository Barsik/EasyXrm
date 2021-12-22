using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;

namespace EasyXrm.Extensions
{
    public static class ServiceProviderExtensions
    {
        public static IPluginExecutionContext GetPluginExecutionContext(this IServiceProvider serviceProvider) =>
            (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));

        public static IOrganizationServiceFactory GetOrganizationServiceFactory(this IServiceProvider serviceProvider) =>
            (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));

        public static ITracingService GetTracingService(this IServiceProvider serviceProvider) =>
            (ITracingService)serviceProvider.GetService(typeof(ITracingService));

    }
}