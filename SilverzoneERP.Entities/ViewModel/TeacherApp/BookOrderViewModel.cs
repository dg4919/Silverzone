namespace SilverzoneERP.Entities.ViewModel.TeacherApp
{
    public class BookOrderViewModel
    {
        public int Id { get; set; }
        public string EventCode { get; set; }
        public string SchCode { get; set; }
        public string OrderNo { get; set; }
        public int StdClassId { get; set; }
        public int CategoryId { get; set; }
        public int Quantity { get; set; }
        //public decimal UnitPrice { get; set; }
        //public decimal Total { get; set; }
        //public bool IsConfirmed { get; set; }
        //public DateTime OrderDate { get; set; }
        //public orderType OrderType { get; set; }
        //public orderSourceType OrderSource { get; set; }
        //public orderStatusType OrderStatus { get; set; }
        public bool IsDelete { get; set; }
    }
}