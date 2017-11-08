using SilverzoneERP.Entities.Models.Common;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SilverzoneERP.Entities.Models
{
    public class StudentRegistration : Entity<long>
    {
        [MaxLength(30)]
        public string OrderNo { get; set; }
        [MaxLength(40)]
        public string EventCode { get; set; }
        [MaxLength(40)]
        public string SchCode { get; set; }
        public int RegSrlNo { get; set; }
        public int ExamDateOpted { get; set; }
        public Nullable<DateTime> ExamDate { get; set; }
        public long Class { get; set; }
        public long NoOfStudent { get; set; }
        public bool IsConfirmed { get; set; }
        public Nullable<DateTime> RegistrationDate { get; set; }
        [Column(TypeName = "varchar")]
        public string RegistrationMode { get; set; }

        
        public Nullable<long> UserId { set; get; }

        [ForeignKey("UserId")]
        public virtual User User { set; get; }


    }
}
