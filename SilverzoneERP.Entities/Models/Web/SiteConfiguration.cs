using SilverzoneERP.Entities.Models.Common;

namespace SilverzoneERP.Entities.Models
{
    public class SiteConfiguration : Entity<long>
    {
        public int India_bookShiping_Charges1 { get; set; }
        public int India_bookShiping_Charges2 { get; set; }

        public int OutsideIndia_bookShiping_Charges1 { get; set; }
        public int OutsideIndia_bookShiping_Charges2 { get; set; }

        public int InstantDnd_Price { get; set; }
    }
}
