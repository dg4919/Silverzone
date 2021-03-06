﻿using SilverzoneERP.Entities.Models.Common;
using System.ComponentModel.DataAnnotations;
namespace SilverzoneERP.Entities.Models
{
    public class SchoolCategory : AuditableEntity<long>
    {        
        [Required]
        [MaxLength(100)]        
        public string CategoryName { get; set; }        
    }
}
