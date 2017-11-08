using SilverzoneERP.Entities.Models.Common;

namespace SilverzoneERP.Entities.Models
{
    public class PaymentInfo : Entity<int>
    {
        public long RefrenceId { get; set; }
        public Payment_refrenceType RefrenceType { get; set; }

        public decimal Amount { get; set; }
        public string Tracking_Id { get; set; }
        public string Bank_Ref_No { get; set; }

        public string Payment_Status { get; set; }
    }
}
