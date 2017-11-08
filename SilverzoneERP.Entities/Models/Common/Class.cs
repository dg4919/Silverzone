using SilverzoneERP.Entities.Models.Common;
using System.Collections.Generic;

namespace SilverzoneERP.Entities.Models
{
    public class Class : AuditableEntity<long>
    {
        public string className { get; set; }

        public long QPStartNo { set; get; } = 1000;
        public virtual ICollection<classSubject> classSubjects { get; set; }
        //public virtual ICollection<Book> books { get; set; }
    }

}
