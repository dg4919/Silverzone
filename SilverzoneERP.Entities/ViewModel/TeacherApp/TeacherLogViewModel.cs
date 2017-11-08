using System;

namespace SilverzoneERP.Entities.ViewModel.TeacherApp
{
    public class TeacherLogViewModel
    {
        public int Id { get; set; }
        public string Remarks { get; set; }
        public  int UserId { get; set; }
        public DateTime ActionDate { get; set; }
        public  int ActionId { get; set; }
    }
}