using SilverzoneERP.Entities.Models;
using System;
using System.Linq;

namespace SilverzoneERP.Data
{
    public interface IUserRepository: IRepository<ERPuser>
    {
        ERPuser GetById(long id);
        ERPuser GetByEmail(string EmailID);
        ERPuser SignIn(string EmailID, string Password, long RoleId);
        dynamic Get(int StartIndex, int limit, out long Count);
        dynamic Get();
        ERPuser Get(long UserId);
        dynamic GetCreated_UpdatedBy(long UserId);
        string WriteImage(string Base64image);
        bool DeleteImage(string ImagePath);
        string GetUserName(long UserId);
        bool sendMail(string EmailId, string subject, string body);
        dynamic GetTeacherList(int startIndex, int limit, Nullable<DateTime> From, Nullable<DateTime> To, string AnySearch, Nullable<bool> IsActive, ref long count, ref long Allcount);
        IQueryable<ERPuser> GetRCList();
        dynamic GetTeacherAppList(int startIndex, int limit, Nullable<DateTime> From, Nullable<DateTime> To, string AnySearch, Nullable<bool> IsActive, ref long count, ref long Allcount);
        bool DeleteTeacher(string Query);
        bool Exist(string EmailID,long RoleId);
    }
}
