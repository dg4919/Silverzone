using SilverzoneERP.Entities.Models.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SilverzoneERP.Entities.Models
{
    public class SchoolDetail : AuditableEntity<long>
    {
        #region Property
           
        public string SchCode { get; set; }
        
        [Required]
        [MaxLength(50)]        
        public string SchName { get; set; }

        [Required]
        [MaxLength(150)]        
        public string SchAddress { get; set; }             
       
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }

        [MaxLength(50)]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string SchEmail { get; set; }

       
        [MaxLength(100)]
        public string SchWebSite { get; set; }

       
        public Nullable<long> SchPhoneNo { get; set; }

        
        public Nullable<long> SchFaxNo { get; set; }

        [Required]      
        public long SchPinCode { get; set; }


      
        [MaxLength(50)]
        public string SchBoard { get; set; }

      
        [MaxLength(50)]
        public string SchAffiliationNo { get; set; }

        #endregion

       

        #region IEnumerable

        public virtual IEnumerable<SchoolRemarks> SchoolRemarks { get; set; }
        public virtual IEnumerable<SchoolShippingAddress> ShippingAddress { get; set; }
        public virtual IList<BlackListedSchool> BlackListed { get; set; }
        public virtual IList<EventManagement> EventManagement { get; set; }
        public virtual IList<Contact> Contact { get; set; }

        public virtual IList<SchoolLog> SchoolLog { get; set; }
        #endregion
    }
}
