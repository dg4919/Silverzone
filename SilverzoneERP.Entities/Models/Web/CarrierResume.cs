using SilverzoneERP.Entities.Models.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace SilverzoneERP.Entities.Models
{
    public class CarrierMaster : AuditableEntity<long>
    {
        public string JobTitle { get; set; }
        public int Vacancies { get; set; }
        public string Qualification { get; set; }
        public string Skills { get; set; }
        public string Experience { get; set; }
    }

    public class CarrierResume : Entity<long>
    {
        public long JobId { get; set; }
        [ForeignKey("JobId")]
        public virtual CarrierMaster CarrierMaster { get; set; }

        public string FilePath { get; set; }
    }
}
