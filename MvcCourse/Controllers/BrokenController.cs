using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcCourse.Models;
using Umbraco.Web.Mvc;

namespace MvcCourse.Controllers
{
    public class BrokenController : SurfaceController
    {
        // GET: Broken
        public ActionResult Submit(BrokenFormModel model)
        {
            //In cases the model does not validate
            if (ModelState.IsValid == false)
                return CurrentUmbracoPage();

            //this will explode when = 0
            var result = 1000/model.ANumber;

            
            return RedirectToCurrentUmbracoPage();
        }
    }
}