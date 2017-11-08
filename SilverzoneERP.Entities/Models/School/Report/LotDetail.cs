using SilverzoneERP.Entities.Models.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace SilverzoneERP.Entities.Models
{
    public class LotDetail : Entity<long>
    {       
        [Required]
        public long Objectid { set; get; }

        [Required]
        public int ObjectType { set; get; }

        [Required]
        public long LotId { set; get; }

        [ForeignKey("LotId")]
        public virtual Lot Lot { get; set; }
    }
}
