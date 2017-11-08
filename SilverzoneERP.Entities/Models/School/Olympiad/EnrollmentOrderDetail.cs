using SilverzoneERP.Entities.Models.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;



namespace SilverzoneERP.Entities.Models
{
    public class EnrollmentOrderDetail : Entity<long>
    {        
        public long EnrollmentOrderId { set; get; }

        [ForeignKey("EnrollmentOrderId")]
        public virtual EnrollmentOrder EnrollmentOrder { set; get; }

        public long ClassId { set; get; }
        [ForeignKey("ClassId")]
        public virtual Class Class { set; get; }

        public int No_Of_Student { set; get; }

        public virtual IList<StudentEntry> StudentEntry { set; get; }
    }
}
