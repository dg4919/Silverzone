using SilverzoneERP.Entities.Models.Common;
using System;

namespace SilverzoneERP.Entities.Models
{
    public class DateOpted:Entity<long>
    { 
        public string EventCode { get; set; }
        public string DateName { get; set; }
        public DateTime EventDate { get; set; }
        public string EventYear { get; set; }
        public bool IsActive { get; set; }
    }
}
