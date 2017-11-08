using SilverzoneERP.Entities.Models.Common;

namespace SilverzoneERP.Entities.Models
{
    public class PreviousYrQP : AuditableEntity<long>
    {
        public int year { get; set; }
    }
}
