namespace SilverzoneERP.Entities.ViewModel.TeacherApp
{
    public class TeacherDetailViewModel
    {
        public  int Id { get; set; }
        public string SchoolName { get; set; }
        public string SchoolCode { get; set; }
        public string SchoolAddress { get; set; }
        public int PinCode { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public int ProfileId { get; set; }
        public bool is_Active { get; set; }
        public int ActionId { get; set; }
        public int RegSrlNo { get; set; }
    }
}