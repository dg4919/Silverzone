using SilverzoneERP.Entities.Models.Common;

namespace SilverzoneERP.Entities.Models
{
    public class RcSchools : AuditableEntity<long>
    {
        public long RCId { get; set; }
        public string Schools { get; set; }
    }
}