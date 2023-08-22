using System;
using Microsoft.Xrm.Sdk;

namespace Raju.CRM.Plugin
{
    public class myFirstPlugin : IPlugin //Interface to create Plugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            ITracingService tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));
            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            IOrganizationServiceFactory serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);
            if (context.MessageName.ToLower() == "update" && context.InputParameters.Contains("Target") && context.InputParameters["Target"] is Entity)
            {
                Entity contact = (Entity)context.InputParameters["Target"];
                try
                {

                    if (contact.LogicalName == "contact")
                    {
                        tracingService.Trace("Entered into Contact Entity");

                        string lastName = contact.GetAttributeValue<string>("lastname");
                        string upperCaseLastName = lastName.ToUpper();
                        contact["lastname"] = upperCaseLastName;
                        service.Update(contact);

                    }
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }
    }
}



