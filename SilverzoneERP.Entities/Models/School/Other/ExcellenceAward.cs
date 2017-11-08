using SilverzoneERP.Entities.Models.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace SilverzoneERP.Entities.Models
{
    public class ExcellenceAward : AuditableEntity<long>
    {
        [Required]
        public long SchoolId { set; get; }
        [ForeignKey("SchoolId")]
        public School School { set; get; }

        [Required]
        public long AwardId { set; get; }
        [ForeignKey("AwardId")]
        public Award Award { set; get; }

        [Required]
        [MaxLength(200)]
        public string NomineeName { set; get; }

        [Required]
        [MaxLength(500)]
        public string Remarks { set; get; }

        [Required]
        public long EventId { set; get; }
        [ForeignKey("EventId")]
        public Event Event { set; get; }
    }    
}
