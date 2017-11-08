using SilverzoneERP.Entities.Models.Common;
using System;
using System.ComponentModel.DataAnnotations;


namespace SilverzoneERP.Entities.Models
{
    public class Remarks : AuditableEntity<long>
    {

        #region Property
        
        public Nullable<long> UserId { set; get; }

        [Required]
        [MaxLength(200)]
        public string Description { get; set; }

        public string OrderNo { set; get; }
     
        #endregion
     
    }
}
