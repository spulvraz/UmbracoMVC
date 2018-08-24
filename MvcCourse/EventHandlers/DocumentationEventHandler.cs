using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;
using Umbraco.Core;
using Umbraco.Core.Events;
using Umbraco.Core.Services;
using Umbraco.Web;
using Umbraco.Web.PublishedContentModels;

namespace MvcCourse.EventHandlers
{
    //ex. 6 relation doc type content to doc type content and biderectional
    public class DocumentationEventHandler : ApplicationEventHandler
    {
        protected override void ApplicationStarting(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            base.ApplicationStarting(umbracoApplication, applicationContext);

            //subscribe to all documents being published.
            ContentService.Published += ContentService_Published;
        }

        void ContentService_Published(Umbraco.Core.Publishing.IPublishingStrategy sender, Umbraco.Core.Events.PublishEventArgs<Umbraco.Core.Models.IContent> e)
        {
            //the Umbraco services context
            var services = UmbracoContext.Current.Application.Services;

            //get the landing page document type
            var docs = e.PublishedEntities.Where(x => x.ContentType.Alias == Documentation.ModelTypeAlias);

            if (docs.Any())
            {
                //get all landing pages
                var doctypeId = LandingPage.GetModelContentType().Id;
                var products = services.ContentService
                    .GetContentOfContentType(doctypeId).ToList();

                //relate the pages
                var bodyTextProperty = Documentation.GetModelPropertyType(x => x.BodyText);
                var rs = services.RelationService;

                //iterate through all published items (there can be more then one)
                foreach (var doc in docs)
                {
                    var bodyText = doc.GetValue<string>(bodyTextProperty.PropertyTypeAlias);
                    foreach (var product in products)
                    {
                        if (bodyText.InvariantContains(product.Name))
                        {
                            if (rs.AreRelated(doc, product, "product") == false)
                            {
                                rs.Relate(doc, product, "product");
                                e.Messages.Add(new EventMessage(
                                    "Relations", "Page related to " + product.Name,
                                    EventMessageType.Success));
                            }
                        }
                    }
                }

            }

        }
    }
}