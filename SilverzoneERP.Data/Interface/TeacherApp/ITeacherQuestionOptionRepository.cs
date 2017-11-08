using SilverzoneERP.Entities.Models;
using System.Collections.Generic;

namespace SilverzoneERP.Data
{
    public interface ITeacherQuestionOptionRepository:IRepository<TeacherQuestionOption>
    {
        string WriteImage(string Base64image,string imgPath);
        IEnumerable<TeacherQuestionOption> GetQuestionOptionByQuestionId(int id);
        bool DeleteImage(string ImagePath);
    }
}
