using System;
using System.ComponentModel.DataAnnotations;

namespace SilverzoneERP.Entities.ViewModel.TeacherApp
{
    public class StudentRegistrationViewModel
    {
        public int Id { get; set; }
        public bool IsDelete { get; set; }
        public string EventCode { get; set; }
        public int ExamDateOpted { get; set; }
        public DateTime ExamDate { get; set; }
        public  string OrderNo { set; get; }
        [Required]
        public int Class { get; set; }

        [Required]
        public int NoOfStudent { get; set; }
    }

    public class GooglePlayViewModel
    {
        [Required]
        public string GooglePlayVersion { get; set; }
    }
}