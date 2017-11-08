using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;

namespace SilverzoneERP.Entities.Models
{
    public class ERPuser : UserCommon
    {
        [MaxLength(200)]
        public string UserAddress { set; get; }

        public virtual IEnumerable<UserPermission> UserPermission { set; get; }

        [Required]
        public string SrcFrom { set; get; }

        [MaxLength(50)]
        public string Qualification { set; get; }

        [MaxLength(50)]
        public string OtherQualification { set; get; }

        [MaxLength(50)]
        public string Profession { set; get; }

        [MaxLength(50)]
        public string HowDid { set; get; }

        public Nullable<long> SchId { set; get; }

        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public int? Pincode { get; set; }
        public int? Age { get; set; }
    }
}
