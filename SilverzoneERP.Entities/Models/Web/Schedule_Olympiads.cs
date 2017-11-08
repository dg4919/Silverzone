using SilverzoneERP.Entities.Models.Common;

namespace SilverzoneERP.Entities.Models
{
    public class Schedule_Olympiads : Entity<long>
    {
        public string ImageName { get; set; }
        public string Caption { get; set; }
        public string Link { get; set; }

        //[NotMapped]
        //public bool isDelete { get; set; }
    }
}
