using SilverzoneERP.Entities.Models.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SilverzoneERP.Entities.Models
{
    public class TeacherDetail : Entity<long>
    {
        [ForeignKey("Id"), Column(Order = 0)]
        public virtual User User { get; set; }

        [Required]
        public string SchoolName { get; set; }

        public string SchoolCode { get; set; }

        [MaxLength(200)]
        public string SchoolAddress { get; set; }

        [Required]
        public int PinCode { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string State { get; set; }

        [Required]
        public string Country { get; set; }


        public long ProfileId { get; set; }
      
        public virtual Profile Profile { get; set; }

        [Required]
        public bool is_Active { get; set; }       

        public long RegSrlNo { get; set; }

        
        //[MaxLength(100)]
        //[EmailAddress(ErrorMessage = "Enter Valid Email ID")]
        //public string Email { set; get; }
    }
}
