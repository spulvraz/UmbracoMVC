using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcCourse.Models
{
    //this is the model used in the form to post changes to an object-content inside the document type 'Documentation'
    public class DocumentationFormModel
    {
        //ex.4
        [Required]
        [AllowHtml]
        public string BodyText { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        //Ex. 5
        public IEnumerable<HttpPostedFileBase> Images { get; set; }
    }
}