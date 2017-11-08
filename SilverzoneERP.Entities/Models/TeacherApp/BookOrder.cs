using SilverzoneERP.Entities.Models.Common;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SilverzoneERP.Entities.Models
{
    public class BookOrder:Entity<long>
    {
        [MaxLength(40)]
        public string EventCode { get; set; }
        [MaxLength(40)]
        public string SchCode { get; set; }
        public string OrderNo { get; set; }
        public long StdClassId { get; set; }
        public long CategoryId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Total { get; set; }
        public bool IsConfirmed { get; set; }
        public DateTime OrderDate { get; set; }
        public orderType OrderType { get; set; } = orderType.Online;
        public orderSourceType OrderSource { get; set; } = orderSourceType.School;
        public orderStatusType OrderStatus { get; set; } = orderStatusType.Pending;

        public DateTime UpdationDate { get; set; }

        [Required]
        public long UserId { set; get; }

        [ForeignKey("UserId")]
        public virtual User User { set; get; }
    }
}
