using SilverzoneERP.Entities.Models.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace SilverzoneERP.Entities.Models
{
    public class QPDispatchERP : AuditableEntity<long>
    {
        [Required]
        public long EventManagementId { set; get; }
        [ForeignKey("EventManagementId")]        
        public virtual EventManagement EventManagement { set; get;}

        public long LotId { set; get; }

        [Required]
        public string JSONData { set; get; }
    }
}
