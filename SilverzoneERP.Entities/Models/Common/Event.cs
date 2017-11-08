using SilverzoneERP.Entities.Models.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SilverzoneERP.Entities.Models
{
    // event will contain subject + its shortcut Name (i.e > event)
    public class Event:AuditableEntity<long>
    {
        [Required, MaxLength(100)]
        public string SubjectName { get; set; }

        [Required, MaxLength(200)]
        public string EventName { get; set; }

        [Required, MaxLength(5)]
        public string EventCode { get; set; }

        public string EventImage { get; set; }

        [MaxLength(1)]
        public string EventLater { get; set; }
        //public virtual ICollection<Book> Books { get; set; }

        public ICollection<classSubject> classSubjects { get; set; }
    }
}
