using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcCourse.Helper;
using MvcCourse.Models;
using Umbraco.Core.Models;
using Umbraco.Web.Mvc;
using Umbraco.Web.PublishedContentModels;

namespace MvcCourse.Controllers
{
    public class DocumentationFormController : SurfaceController                //RenderMvcController
    {
        //1 rendering the form 
        //2 receiving update to post - update

        // GET: DocumentationForm - load the form filled with the content from the doc type
        //his fires once the used click on edit
        [ChildActionOnly]
        public ActionResult Render(Documentation model) //pass in the @Model.Content the content of the doc type 'Documentation'
        {
            //we receive here the model from the Documentation page
            DocumentationFormModel documentationFormModel = new DocumentationFormModel()
            {
                BodyText = model.BodyText.ToString(),
                Name = model.Name
            };
            //we pass the model to the partial view that contains the form
            //the partial view for the form accepts this model '@model MvcCourse.Models.DocumentationFormModel'
            return PartialView("~/Views/Partials/DocumentationForm.cshtml", documentationFormModel);
        }

        //here are posted the new content of the documentatation doc type
        [HttpPost]
        public ActionResult Submit(DocumentationFormModel model)
        {
            if (ModelState.IsValid == false)
                return CurrentUmbracoPage();

            var currentPageId = CurrentPage.Id;                                 //IPublishedContent
            var content = Services.ContentService.GetById(currentPageId);       //ServiceContext
            content.Name = model.Name;
            var bodyTextProperty = Documentation.GetModelPropertyType(p => p.BodyText);
            content.SetValue(bodyTextProperty.PropertyTypeAlias, model.BodyText);       // this is Dynamic

            //ex. 5 
            var mediaService = Services.MediaService;
            if (model.Images.HasFiles() && model.Images.ContainsImages())
            {
                var imagesProperty = Documentation.GetModelPropertyType(x => x.Images);
                //in BOffice Content section I can set a folder to the documentation content to store the uploaded images
                //or not. If not there isn't a folder available and the result of the following line is folderId = 0
                var folderId = content.GetValue<int>(imagesProperty.PropertyTypeAlias);

                if (folderId <= 0)
                {
                    //if no folder has been set in the BO documentation scontent in the field Images
                    //then create one using default settings
                    //here I create the folder: 
                    //("the name given to the folder", parentId, mediatypeAlias)
                    var folder = mediaService.CreateMedia(model.Name, -1, Folder.ModelTypeAlias);       //create folder

                    //save the folder to the MEDIA section too
                    mediaService.Save(folder);

                    folderId = folder.Id;
                    content.SetValue(imagesProperty.PropertyTypeAlias, folderId);
                }

                //if user uploaded a picture in the Edit form, then add to the folder that will be added to the media section folder too
                foreach (var file in model.Images)
                {
                    if (file.IsImage())
                    {
                        //process files images

                        //create Media of type Image
                        var mediaImage = mediaService.CreateMedia(file.FileName, folderId, Image.ModelTypeAlias);
                        var umbracoFileProperty = Image.GetModelPropertyType(i => i.UmbracoFile);
                        mediaImage.SetValue(umbracoFileProperty.PropertyTypeAlias, file);           //dynamic porperty for the images
                        //save image to set folder
                        mediaService.Save(mediaImage);
                    }
                }
            }

            Services.ContentService.SaveAndPublishWithStatus(content);

            return RedirectToCurrentUmbracoPage();
        }
    }
}