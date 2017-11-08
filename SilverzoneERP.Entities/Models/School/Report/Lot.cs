using SilverzoneERP.Entities.Models.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace SilverzoneERP.Entities.Models
{
    public class Lot : AuditableEntity<long>
    {       
        public long  LotNo { get; set; }

        [Required]
        public long ExaminationDateId { get; set; }

        //[ForeignKey("ExaminationDateId")]
        //public virtual ExaminationDate ExaminationDate { get; set; }

        [Required]
        public LotType LotType { get; set; }
        [Required]
        public Level ExamLevel { get; set; }

        [Required]
        public long EventId { get; set; }

        [ForeignKey("EventId")]
        public virtual Event Event { get; set; }

        public long LotYear { get; set; } = ServerKey.Event_Current_YearCode;


        public virtual IList<LotDetail> LotDetail { get; set; }
    }
}
