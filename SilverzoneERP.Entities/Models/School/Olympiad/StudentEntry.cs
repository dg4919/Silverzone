using SilverzoneERP.Entities.Models.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace SilverzoneERP.Entities.Models
{
    public class StudentEntry : AuditableEntity<long>
    {
        #region Property

        [Required]
        public long EnrollmentOrderDetailId { set; get; }

        //[Required]        
        //public long ClassId { get; set; }

        [Required]
        public string Section { get; set; }

        [Required]
        public long RollNo { get; set; }

        [Required]
        [MaxLength(50)]
        public string StudentName { get; set; }

        
        [MaxLength(30)]
        public string EnrollmentNo { get; set; }

        
        [MaxLength(15)]
        public string AuthenticationCode { get; set; }

        [MaxLength(15)]
        public string NIORollNo { get; set; }

        #endregion

        #region ForeignKey

        [ForeignKey("EnrollmentOrderDetailId")]
        public virtual EnrollmentOrderDetail EnrollmentOrderDetail { get; set; }

        //[ForeignKey("ClassId")]
        //public virtual Class Class { get; set; }

        #endregion        

        
    }
}
