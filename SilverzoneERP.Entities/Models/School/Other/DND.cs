using SilverzoneERP.Entities.Models.Common;
using System.ComponentModel.DataAnnotations;
namespace SilverzoneERP.Entities.Models
{
    public class DND_MobileNo : AuditableEntity<long>
    {
        [Required]
        public long MobileNo { set; get; }
        [Required]
        public string Remarks { set; get; }
    }
    public class DND_EmailId : AuditableEntity<long>
    {
        [Required]
        public string EmailId { set; get; }
        [Required]
        public string Remarks { set; get; }
    }
}
