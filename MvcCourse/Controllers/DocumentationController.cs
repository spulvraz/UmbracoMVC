using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ClientDependency.Core;
using ImageProcessor.Web.Helpers;
using Umbraco.Web.Models;       //RenderModel
using Umbraco.Web.Mvc;
using MvcCourse.Models;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.PublishedContentModels;

//for the viewModel custom model with custom property

namespace MvcCourse.Controllers
{
    public class DocumentationController : RenderMvcController
    {
        // GET: Documentation
        public override ActionResult Index(RenderModel model)
        {
            //add here the logic that was in the view to get all the product that are referenced in the Documentation content
            //PRODUCTs page URL to be displayed - use the relation service to get the relations
            //get the 'related' landing pages
            var ids = UmbracoContext
                .Application.Services
                .RelationService.GetByParentId(model.Content.Id)
                .Select(x => x.ChildId);

            ////fetch them as nodes
            //var products = Umbraco.TypedContent(ids)
            //    .Where(x => x.DocumentTypeAlias == "landingPage").ToList();
            var products = Umbraco.TypedContent(ids).OfType<LandingPage>();

            //IMAGES
            //var images = model.Content.Images.OfType<Folder>()
            //    .SelectMany(x => x.Children().OfType<Image>());
            //property in Documentation Model
            //IEnumerable<IPublishedContent> Images

            var images = model.Content.GetPropertyValue<IEnumerable<IPublishedContent>>("Images")       //doc Typr property in Documentation
            .OfType<Folder>().SelectMany(x => x.Children().OfType<Image>());                            //type to the VM property Ienum<Image>
     
            var list = CurrentPage.GetPropertyValue("Images");

            //create the custom viewmodel adding the product to the custom property LnadingPages fo rthe products referenced in the content
            var viewmodel = new DocumentationViewModel(model.Content)       // set the inherited content
            {
                //set the custom property LandingPages
                LandingPages = products,
                //get the property from the published content inherited and type to the custom property for images
                AllImages = images
            };

            //returns the new model to the view
            return CurrentTemplate(viewmodel);


            //return base.Index(model);
        }
    }
}