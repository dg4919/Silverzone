using SilverzoneERP.Context;
using SilverzoneERP.Entities.Models;
using System.Collections.Generic;
using System.Linq;

namespace SilverzoneERP.Data
{
    public  class TeacherQuestionRepository:BaseRepository<TeacherQuestion>,ITeacherQuestionRepository
    {
        public  TeacherQuestionRepository(SilverzoneERPContext context):base(context) { }
        public TeacherQuestion GetById(int id)
        {
            return _dbset.Where(x => x.Id == id).FirstOrDefault();
        }
        public IEnumerable<TeacherQuestion> GetByUserId(int userid)
        {
            return FindBy(x => x.UserId == userid).AsEnumerable();
        }
        public IEnumerable<TeacherQuestion> GetByStatus(bool status)
        {
            return FindBy(x => x.IsActive == status).AsEnumerable();
        }
        public dynamic Get_Active()
        {
            return _dbset.Where(x=>x.IsActive==true)
                .Select(x => new {
                    x.Id,
                    x.QuestionText,
                    x.ImageName,
                    x.SubjectId,
                    SubjectName= x.Subject.SubjectName,
                    Options = x.TeacherQuestionOption.Select(o => new {
                        o.Id,
                        o.OptionText,
                        o.ImageName,
                        o.IsAnswer
                    })
                });
        }
        public string WriteImage(string Base64image,string imgPath)
        {
            return ClassUtility.WriteImage(Base64image,imgPath);
        }

        public bool DeleteImage(string ImagePath)
        {
            return ClassUtility.DeleteImage(ImagePath);
        }
    }
}
