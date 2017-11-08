using SilverzoneERP.Entities.Models.Common;
using System.ComponentModel.DataAnnotations;
namespace SilverzoneERP.Entities.Models
{
    public class Award : AuditableEntity<long>
    {
        [Required]
        [MaxLength(100)]
        public string Name { set; get; }

        public string Year { set; get; } = ServerKey.Event_Current_Year;
        public int YearCode { set; get; } = ServerKey.Event_Current_YearCode;

    }    
}
