using SilverzoneERP.Entities.Models.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SilverzoneERP.Entities.Models
{
    public partial class Order : Entity<long>
    {
        public string OrderNumber { get; set; }

        public decimal Total_Shipping_Amount { get; set; }
        public decimal Total_Shipping_Charges { get; set; }

        public System.DateTime OrderDate { get; set; }

        public long UserId { get; set; }
        public virtual User User { get; }

        //public orderType Payment_ModeType { get; set; }

        public long Shipping_addressId { get; set; }
        [ForeignKey("Shipping_addressId")]
        public virtual UserShippingAddress UserShippingAddress { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; }

        public int? Quiz_Points_Deduction { get; set; }

        // bcoz shiping charge can be change as it dynamic, so preserve value of it
        public long First_Shipping_Charge { get; set; }
        public long Other_Shipping_Charge { get; set; }
    }


}