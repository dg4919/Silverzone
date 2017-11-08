
using SilverzoneERP.Entities.Models;
using System.Collections.Generic;
namespace SilverzoneERP.Data
{
   public  interface ITeacherQuestionRepository:IRepository<TeacherQuestion>
    {
        TeacherQuestion GetById(int id);
        IEnumerable<TeacherQuestion> GetByUserId(int userid);
        IEnumerable<TeacherQuestion> GetByStatus(bool status);
        dynamic Get_Active();
        string WriteImage(string Base64image,string imgPath);
        bool DeleteImage(string ImagePath);
    }
}
