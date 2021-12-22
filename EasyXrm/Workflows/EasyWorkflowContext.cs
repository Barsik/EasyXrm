using System;
using System.Activities;
using EasyXrm.Extensions;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;

namespace EasyXrm.Workflows
{
    public class EasyWorkflowContext
    {
        public IWorkflowContext WorkflowExecutionContext { get; }
        public CodeActivityContext CodeActivityContext { get; }
        public ITracingService TracingService { get; }
        private readonly Lazy<IOrganizationService> _userOrganizationService;
        private readonly Lazy<IOrganizationService> _systemOrganizationService;
        private readonly Lazy<IOrganizationService> _initiatingUserOrganizationService;

        public IOrganizationService UserOrganizationService => _userOrganizationService.Value;
        public IOrganizationService InitiatingUserOrganizationService => _initiatingUserOrganizationService.Value;
        public IOrganizationService SystemOrganizationService => _systemOrganizationService.Value;

        public EasyWorkflowContext(CodeActivityContext context)
        {
            CodeActivityContext = context;
            var organizationServiceFactory = context.GetExtension<IOrganizationServiceFactory>();
            WorkflowExecutionContext = context.GetExtension<IWorkflowContext>();
            TracingService = context.GetExtension<ITracingService>();

            _userOrganizationService =
                organizationServiceFactory.CreateLazyOrganizationService(WorkflowExecutionContext.UserId);
            _initiatingUserOrganizationService =
                organizationServiceFactory.CreateLazyOrganizationService(WorkflowExecutionContext.InitiatingUserId);
            _systemOrganizationService = organizationServiceFactory.CreateLazyOrganizationService(null);
        }

        public void Trace(string message)
        {
            if (string.IsNullOrWhiteSpace(message) || TracingService == null)
            {
                return;
            }

            TracingService.Trace($@"Trace details ->
Initiating User: {WorkflowExecutionContext.InitiatingUserId}
Correlation Id: {WorkflowExecutionContext.CorrelationId}
Message: {message}");
        }
    }
}
