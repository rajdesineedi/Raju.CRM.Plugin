using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace Raju.CRM.Plugin
{
   public class mySecondPlugin : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            ITracingService tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));
            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            IOrganizationServiceFactory serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);

            if(context.MessageName.ToLower() == "create" && context.InputParameters.Contains("Target") && context.InputParameters["Target"] is Entity)
            {
                try
                {
                    tracingService.Trace("Entered in try block");

                    Entity contact = (Entity)context.InputParameters["Target"];
                    string emailID = contact.GetAttributeValue<string>("emailaddress1");

                    QueryExpression query = new QueryExpression();
                    query.ColumnSet = new ColumnSet("emailaddress1");
                    query.Criteria.AddCondition("emailaddress1", ConditionOperator.Equal, emailID);
                    EntityCollection collection = service.RetrieveMultiple(query);

                    if(collection.Entities.Count >= 1)
                    {
                        throw new InvalidPluginExecutionException("Duplicates values has been found");
                    }



                }
                catch (Exception)
                {

                    throw new InvalidPluginExecutionException("Invalide Plugin Execution");
                }
            }

        }
    }
}
