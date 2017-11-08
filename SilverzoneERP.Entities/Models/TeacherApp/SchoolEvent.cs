using SilverzoneERP.Entities.Models.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SilverzoneERP.Entities.Models
{
    public  class SchoolEvent:Entity<long>
    {
        [Required]
        public long SchCode { get; set; }

        [Required]
        public long EventId { get; set; }

        [ForeignKey("EventId")]
        public virtual Event Event { get; set; }

        [Required]
        public int EventYear { get; set; }
    }
}
