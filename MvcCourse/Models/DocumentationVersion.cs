using System;
using System.Runtime.Serialization;

namespace MvcCourse.Models
{

    [DataContract(Name = "version")]
    public class DocumentationVersion
    {
        [DataMember(Name = "name")]
        public string Name { get; set; }
        
        [DataMember(Name = "publishDate")]
        public DateTime PublishDate { get; set; }

        [DataMember(Name = "versionId")]
        public Guid VersionId { get; set; }
    }
}