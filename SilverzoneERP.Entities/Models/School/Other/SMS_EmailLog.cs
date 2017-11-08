using SilverzoneERP.Entities.Models.Common;
using System;
using System.ComponentModel.DataAnnotations;
namespace SilverzoneERP.Entities.Models
{
    public class SMS_EmailLog : AuditableEntity<long>
    {
        public Nullable<long> SchCode { set; get; }
        public Nullable<long> MobileNo { set; get; }
        public string EmailId { set; get; }
        [Required]
        public string Purpose{ set; get; }
        [Required]
        public string Content { set; get; }
        [Required]
        public string FormName { set; get; }
    }
}
