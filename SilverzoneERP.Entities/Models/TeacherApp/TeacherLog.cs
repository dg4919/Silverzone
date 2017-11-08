using SilverzoneERP.Entities.Models.Common;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SilverzoneERP.Entities.Models
{
    public class TeacherLog:Entity<long>
    {
        [MaxLength(200)]
        public string Remarks { get; set; }

        public long UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        public DateTime ActionDate { get; set; }

        //public int ActionId { get; set; }

        //[ForeignKey("ActionId")]
        //public virtual  Action Action { get; set; }
    }
}
