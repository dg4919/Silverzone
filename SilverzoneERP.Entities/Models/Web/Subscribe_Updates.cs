using SilverzoneERP.Entities.Models.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace SilverzoneERP.Entities.Models
{
    public class Subscribe_Updates : AuditableEntity<long>
    {
        public string UserName { get; set; }
        public long classId { get; set; }

        [ForeignKey("classId")]
        public Class Class{ get; set; }

        public string Address { get; set; }
        public string EmailId { get; set; }
        public long Mobile { get; set; }
        public string City { get; set; }

        public long StateId { get; set; }
        [ForeignKey("StateId")]
        public State state { get; set; }

        public long CountryId { get; set; }
        [ForeignKey("CountryId")]
        public Country country { get; set; }
    }
}
