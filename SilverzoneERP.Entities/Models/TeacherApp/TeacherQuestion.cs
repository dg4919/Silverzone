using SilverzoneERP.Entities.Models.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SilverzoneERP.Entities.Models
{
    public class TeacherQuestion:AuditableEntity<long>
    {
        public long SubjectId { get; set; }

        [ForeignKey("SubjectId")]
        public  virtual  Event Subject { get; set; }
        public string QuestionText { get; set; }
        public string ImageName { get; set; }
        public long UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }
        public bool IsActive { get; set; }
        public long? AnswerId { get; set; }
        public virtual IList<TeacherQuestionOption> TeacherQuestionOption { get; set; }
    }
}
