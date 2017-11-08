using SilverzoneERP.Entities.Models.Common;
using System.ComponentModel.DataAnnotations;

namespace SilverzoneERP.Entities.Models
{
    public class Notification:AuditableEntity<long>
    {
        public int SubjectId { get; set; }

        [MaxLength(50)]
        public string Heading { get; set; }

        [MaxLength(300)]
        public string Message { get; set; }

        public bool IsActive { get; set; }
    }
}
