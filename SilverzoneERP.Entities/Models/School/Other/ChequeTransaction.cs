using SilverzoneERP.Entities.Models.Common;
using System.ComponentModel.DataAnnotations;
namespace SilverzoneERP.Entities.Models
{
    public class ChequeTransaction : AuditableEntity<long>
    {
        [Required]
        public string PartyName { set; get; }
        [Required]
        public long Amount { set; get; }
        [Required]
        public string Remarks { set; get; }

        public bool IsVerified { set; get; }
        [Required]
        public BankType BankType { set; get; }
    }    
}
