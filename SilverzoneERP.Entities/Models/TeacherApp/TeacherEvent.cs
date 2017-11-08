using SilverzoneERP.Entities.Models.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace SilverzoneERP.Entities.Models
{
    public class TeacherEvent : Entity<long>
    {
        public long UserId { get; set; }
        public User User { get; set; }

        public long EventId { get; set; }

        [ForeignKey("EventId")]
        public virtual Event Event { get; set; }
        
    }
}
