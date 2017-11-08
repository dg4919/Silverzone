using SilverzoneERP.Context;
using SilverzoneERP.Entities.Models;
using System.Collections.Generic;
using System.Linq;

namespace SilverzoneERP.Data
{
    public class TeacherQuestionOptionRepository:BaseRepository<TeacherQuestionOption>,ITeacherQuestionOptionRepository
    {
        public TeacherQuestionOptionRepository(SilverzoneERPContext context) : base(context) { }

        public string WriteImage(string Base64image,string imgPath)
        {
            return ClassUtility.WriteImage(Base64image,imgPath);
        }

        public IEnumerable<TeacherQuestionOption> GetQuestionOptionByQuestionId( int id)
        {
            return FindBy(x => x.QuestionId == id).AsEnumerable();
        }
        public bool DeleteImage(string ImagePath)
        {
            return ClassUtility.DeleteImage(ImagePath);
        }
    }
}
