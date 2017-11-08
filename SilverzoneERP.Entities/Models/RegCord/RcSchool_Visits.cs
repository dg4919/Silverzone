using SilverzoneERP.Entities.Models.Common;
using System;
using System.Collections.Generic;

namespace SilverzoneERP.Entities.Models
{
    public class RcSchool_Visits : Entity<long>
    {
        public long RCId { get; set; }

        public long SchoolId { get; set; }
        public School SchoolInfo { get; set; }

        public VisitType VisitType { get; set; }
        public DateTime VisitDate { get; set; }

        public DateTime? FoolowUpDate { get; set; }

        public string Contact_Peron_Name { get; set; }
        public string Contact_Peron_Mobile { get; set; }

        public string Remarks { get; set; }

        public ICollection<RcSchool_VisitsInfo> RcSchool_VisitsInfo { get; set; }
    }

    public class RcSchool_VisitsInfo : Entity<long>
    {

        public long RcSchool_VisitId { get; set; }
        public virtual RcSchool_Visits RcSchool_Visit { get; set; }

        public long EventId { get; set; }
        public virtual Event Event { get; set; }

        public VisitStatus VisitStatus { get; set; }
    }
}
