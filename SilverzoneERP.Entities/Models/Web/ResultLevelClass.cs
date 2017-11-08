using SilverzoneERP.Entities.Models.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace SilverzoneERP.Entities.Models
{
    public class Result_levelClass: Entity<long>
    {
        public long subjectId{ get; set; }
        [ForeignKey("subjectId")]
        public virtual Event EventInfo { get; set; }

        public int LevelId { get; set; }
    }
}
