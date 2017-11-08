using SilverzoneERP.Entities.Models.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace SilverzoneERP.Entities.Models
{
    public class Syllabus_SampleQP : AuditableEntity<long>
    {
        public long EventId { get; set; }

        [ForeignKey("EventId")]
        public virtual Event Event { get; set; }

        public long ClassId { get; set; }

        [ForeignKey("ClassId")]
        public virtual Class Class { get; set; }

        public string DownloadLink { get; set; }
        public pdfType PdfType { get; set; }
    }
}
