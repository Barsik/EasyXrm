using System;
using System.Linq;
using EasyXrm.Extensions;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace EasyXrm.Plugins
{
    public partial class EasyPluginContext
    {
        public const string QueryName = "Query";
        public const string TargetName = "Target";
        public const string PreImageName = "PreImage";
        public const string PostImageName = "PostImage";
        public const string OutputBusinessEntityName = "BusinessEntity";
        public const string OutputBusinessEntityCollectionName = "BusinessEntityCollection";

        private readonly Lazy<IOrganizationService> _userOrganizationService;
        private readonly Lazy<IOrganizationService> _systemOrganizationService;
        private readonly Lazy<IOrganizationService> _initiatingUserOrganizationService;

        public ITracingService TracingService { get; }
        public IPluginExecutionContext PluginExecutionContext { get; }
        public IOrganizationService UserOrganizationService => _userOrganizationService.Value;
        public IOrganizationService SystemOrganizationService => _systemOrganizationService.Value;
        public IOrganizationService InitiatingUserOrganizationService => _initiatingUserOrganizationService.Value;

        public Entity TargetEntity => PluginExecutionContext.InputParameters.Contains(TargetName) &&
                                      PluginExecutionContext.InputParameters[TargetName] is Entity targetEntity
            ? targetEntity
            : null;

        public EntityReference TargetReference => PluginExecutionContext.InputParameters.Contains(TargetName) &&
                                                  PluginExecutionContext.InputParameters[TargetName] is EntityReference
                                                      targetReference
            ? targetReference
            : null;

        public Entity PreImage => PluginExecutionContext.PreEntityImages.Count > 0
            ? PluginExecutionContext.PreEntityImages.FirstOrDefault().Value
            : null;

        public Entity PostImage => PluginExecutionContext.PostEntityImages.Count > 0
            ? PluginExecutionContext.PostEntityImages.FirstOrDefault().Value
            : null;

        public Entity OutputBusinessEntity => GetOutputParameter<Entity>(OutputBusinessEntityName);

        public EntityCollection OutputBusinessEntityCollection =>
            GetOutputParameter<EntityCollection>(OutputBusinessEntityCollectionName);

        public QueryExpression Query =>
            PluginExecutionContext.InputParameters.ContainsKey(QueryName)
                ? PluginExecutionContext.InputParameters[QueryName] as QueryExpression
                : null;


        public TValue GetInputParameter<TValue>(string name, TValue defaultValue = default)
        {
            return PluginExecutionContext.InputParameters.ContainsKey(name)
                ? (TValue)PluginExecutionContext.InputParameters[name]
                : defaultValue;
        }

        public TValue GetOutputParameter<TValue>(string name, TValue defaultValue = default)
        {
            return PluginExecutionContext.OutputParameters.ContainsKey(name)
                ? (TValue)PluginExecutionContext.OutputParameters[name]
                : defaultValue;
        }

        public void SetOutputParameter(string name, object value) =>
            PluginExecutionContext.OutputParameters[name] = value;

        public void Trace(string message)
        {
            if (string.IsNullOrWhiteSpace(message) || TracingService == null)
            {
                return;
            }

            TracingService.Trace($@"Trace details ->
Initiating User: {PluginExecutionContext.InitiatingUserId}
Correlation Id: {PluginExecutionContext.CorrelationId}
Message: {message}");
        }

        public EasyPluginContext(IServiceProvider serviceProvider)
        {
            PluginExecutionContext = serviceProvider.GetPluginExecutionContext();
            TracingService = serviceProvider.GetTracingService();

            var organizationServiceFactory = serviceProvider.GetOrganizationServiceFactory();
            _userOrganizationService =
                organizationServiceFactory.CreateLazyOrganizationService(PluginExecutionContext.UserId);
            _initiatingUserOrganizationService =
                organizationServiceFactory.CreateLazyOrganizationService(PluginExecutionContext.InitiatingUserId);
            _systemOrganizationService = organizationServiceFactory.CreateLazyOrganizationService(null);
        }
    }
}