using System.Collections.Generic;
using SilverzoneERP.Entities.Models;
using SilverzoneERP.Entities;
using System;

namespace SilverzoneERP.Data
{
    public interface IAccountRepository : IRepository<User>
    {
        bool sms_verification(string mobileNo, int verfiy_code, verficationType Type);
        int get_smsCode();

        User GetById(long id);
        IEnumerable<User> findByMobile(string mobileNo);
        IEnumerable<User> findByEmailId(string emailId);

        User user_login(User model);

        bool VerifyPassword(string password, string passwordHash);
        string GetPasswordHash(string password);

        List<string> upload_profile_Image_toTemp(string tempPath);
        void save_Image_fromTemp(IEnumerable<string> imageName, string tempPath, string finalPath);
        void save_Image_Profile(IEnumerable<string> imagePath, string tempPath, string finalPath, string ImageName);

        User check_User(string userName, verificationMode inputType,Nullable<long> RoleId=null);
        bool check_User(string email, int roleId);
        void DeleteImage(string ImagePath);
        bool IsTeacher(long RoleId);
        bool sendMail(string EmailId, string subject, string body);
        bool is_userExist(string userName, verificationMode inputType, long RoleId);
    }
}
