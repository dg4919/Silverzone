using SilverzoneERP.Entities.Models.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SilverzoneERP.Entities.Models
{
    public class InstantDownloads : AuditableEntity<long>
    {
        public string OrderId { get; set; }
        public string UserName { get; set; }
        public string Address { get; set; }
        public string EmailId { get; set; }
        public long Mobile { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public int Pincode { get; set; }

        public long ClassId { get; set; }
        [ForeignKey("ClassId")]
        public virtual Class Class { get; set; }

        public long YearId { get; set; }
        [ForeignKey("YearId")]
        public virtual PreviousYrQP PreviousYrQP { get; set; }

        public virtual ICollection<InstantDownload_Subjects> InstantDownload_Subjects { get; set; }
    }

    public class InstantDownload_Subjects : Entity<long>
    {
        public long InstantDownloadId { get; set; }

        [ForeignKey("InstantDownloadId")]
        public virtual InstantDownloads InstantDownloads { get; set; }

        public long LevelId { get; set; }
        public long SubjectId { get; set; }

        [ForeignKey("SubjectId")]
        public virtual Event Subject { get; set; }
    }

    public class InstantDownload_SubjectsMapping : Entity<long>
    {
        public long? ClassId { get; set; }
        [ForeignKey("ClassId")]
        public virtual Class ClassIfno { get; set; }

        public long SubjectId { get; set; }
        [ForeignKey("SubjectId")]
        public virtual Event SubjectInfo { get; set; }

        public long? YearId { get; set; }
        [ForeignKey("YearId")]
        public virtual PreviousYrQP previousYrQPInfo { get; set; }
    }

}
