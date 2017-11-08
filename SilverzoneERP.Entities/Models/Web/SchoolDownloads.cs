using SilverzoneERP.Entities.Models.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace SilverzoneERP.Entities.Models
{
    public class SchoolDownloads: AuditableEntity<long>
    {
        public long EventId { get; set; }

        [ForeignKey("EventId")]
        public virtual Event Event { get; set; }

        public string FileName { get; set; }
        public string DownloadLink { get; set; }
        public downloadType DownloadType { get; set; }
    }
}
