using MvcCourse.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umbraco.Web.WebApi;

namespace MvcCourse.Controllers
{
    public class DocumentationHistoryController : UmbracoApiController
    {
        //https://our.umbraco.com/forum/umbraco-7/using-umbraco-7/55360-Web-Api-405-Method-Not-Allowed
        //verb [HttpGet] + prefix 'Get' in naming method

        //umbraco/api/DocumentationHistory/Versions/[the id]
        //[HttpPost]
        [HttpGet]
        //public IEnumerable<DocumentationVersion> Versions(int id)

        ////umbraco/api/DocumentationHistory/GetVersions/[the id]
        public IEnumerable<DocumentationVersion> GetVersions(int id)
        {
            var versions = Services.ContentService.GetVersions(id)
                .Select(x => new DocumentationVersion()
                {   
                    Name = x.Name,
                    PublishDate = x.UpdateDate,
                    VersionId = x.Version
                });

            return versions;
        }

        ///umbraco/api/DocumentationHistory/PublishVersion/?version= param guid verison
        [HttpGet]
        //public string PublishVersion(Guid version)
        /////umbraco/api/DocumentationHistory/GetPublishVersion/?version= param guid verison
        public string GetPublishVersion(Guid version)
        {
            var content = Services.ContentService.GetByVersion(version);
            Services.ContentService.Publish(content);
            var newUrl = Umbraco.Url(content.Id);
            return newUrl;
        }
    }
}