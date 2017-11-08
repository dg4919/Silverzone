
using SilverzoneERP.Entities.Models.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace SilverzoneERP.Entities.Models
{
    public class TeacherQuestionOption:Entity<long>
    {
        public string OptionText { get; set; }
        public string ImageName { get; set; }
        public bool IsAnswer { get; set; }
        public long QuestionId { get; set; }

        [ForeignKey("QuestionId")]
        public virtual TeacherQuestion TeacherQuestion { get; set; }
    }
}
