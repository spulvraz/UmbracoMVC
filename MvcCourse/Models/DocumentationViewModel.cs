using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Core.Models;
using Umbraco.Web.PublishedContentModels;       //generated models
//namespace Umbraco.Web.PublishedContentModels

namespace MvcCourse.Models
{
    public class DocumentationViewModel : Documentation     //PublishedContentModel
    {
        //this assure alle the inherited properties to be added to the Vmodel
        public DocumentationViewModel(IPublishedContent content) : base(content)
        {
        }
        //add here extra custom property to avoid service logic on the html page
        public IEnumerable<LandingPage> LandingPages { get; internal set; }
        //where this LandingPage is defined? this is aanother Umbraco generated model that  will be included in the viewModel

        //add Images to display in the view to via this view model
        //this value will be gathered from the IPublished content available from the Documentations generated model and casted to this type
        public IEnumerable<Image> AllImages { get; internal set; }
    }
}