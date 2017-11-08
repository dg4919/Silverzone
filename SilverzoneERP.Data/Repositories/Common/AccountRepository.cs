using System.Collections.Generic;
using System.Linq;
using SilverzoneERP.Entities.Models;
using SilverzoneERP.Entities;
using SilverzoneERP.Context;
using System;

namespace SilverzoneERP.Data
{
    public class AccountRepository : BaseRepository<User>, IAccountRepository
    {
        public AccountRepository(SilverzoneERPContext dbcontext) : base(dbcontext) { }

        public bool sms_verification(string mobileNo, int verfiy_code, verficationType Type)
        {
            string msg = string.Empty;
            string _smsCode = verfiy_code.ToString();

            switch (Type)
            {
                case verficationType.register:
                    msg = string.Format(smsTemplates.new_registration, _smsCode);
                    break;
                case verficationType.forget:
                    msg = string.Format(smsTemplates.foget_password, _smsCode);
                    break;
                case verficationType.change:
                    msg = string.Format(smsTemplates.change_mobile, _smsCode);
                    break;
            }

            return ClassUtility.send_message(mobileNo, msg);
        }

        public int get_smsCode()
        {
            return ClassUtility.get_smsCode();
        }

        public User GetById(long id)
        {
            return FindBy(x => x.Id == id).FirstOrDefault();
        }

        public User user_login(User model)
        {
            return FindBy(x => x.MobileNumber == model.MobileNumber && x.Password == model.Password).FirstOrDefault();
        }

        public bool is_userExist(string userName, verificationMode inputType, long RoleId)
        {
            bool result = false;

            if (inputType == verificationMode.email)
                result = _dbset.Any(x => x.EmailID == userName && x.RoleId == RoleId);
            else if (inputType == verificationMode.mobile)
                result = _dbset.Any(x => x.MobileNumber == userName && x.RoleId == RoleId);

            return result;
        }

        //public User GetByEmail(string Email)
        //{
        //    return FindBy(x => x.MobileNumber == model.MobileNumber && x.Password == model.Password).FirstOrDefault();
        //}

        public User check_User(string userName, verificationMode inputType, Nullable<long> RoleId=null)
        {
            var user = new User();

            if (inputType == verificationMode.email)
                user = findByEmailId(userName).FirstOrDefault();
            else if (inputType == verificationMode.mobile && RoleId!=null)
                user = _dbContext.Users.Where(x=>x.MobileNumber== userName && x.RoleId==RoleId).FirstOrDefault();
            else if (inputType == verificationMode.mobile && RoleId == null)
                user = findByMobile(userName).FirstOrDefault();

            return user;
        }

        public bool check_User(string email, int roleId)
        {
            return _dbset.Any(x => x.EmailID.Equals(email) && x.RoleId == roleId);
        }

        public IEnumerable<User> findByMobile(string mobileNo)
        {
            return FindBy(x => x.MobileNumber == mobileNo);
        }

        public IEnumerable<User> findByEmailId(string emailId)
        {
            return FindBy(x => x.EmailID == emailId);
        }

        public bool VerifyPassword(string password, string passwordHash)
        {
            if (string.IsNullOrWhiteSpace(passwordHash)) return false;
            if (string.IsNullOrWhiteSpace(password)) return false;

            return BCrypt.Net.BCrypt.Verify(password, passwordHash);
        }

        public string GetPasswordHash(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }


        public List<string> upload_profile_Image_toTemp(string tempPath)
        {
            return ClassUtility.upload_Images_toTemp(tempPath);
        }

        public void save_Image_fromTemp(IEnumerable<string> imageName, string tempPath, string finalPath)
        {
            // image name contain full relative path like > '/Image/User/Profile/abc.jpg', So we just want image name
            var images = imageName.Select(x => x.Substring(x.LastIndexOf('/') + 1));
            ClassUtility.save_Images_toPhysical(images, tempPath, finalPath, "profilePic.jpg");
        }
      
        public void save_Image_Profile(IEnumerable<string> imagePath, string tempPath, string finalPath,string ImageName)
        {
            // image name contain full relative path like > '/Image/User/Profile/abc.jpg', So we just want image name
            var images = imagePath.Select(x => x.Substring(x.LastIndexOf('/') + 1));
            ClassUtility.save_Images_toPhysical(images, tempPath, finalPath, ImageName);
        }
        public void DeleteImage(string ImagePath)
        {
            ClassUtility.DeleteImage(ImagePath);
        }
        public bool IsTeacher(long RoleId)
        {
            return _dbset.Any(x => x.RoleId == RoleId && x.Role.RoleName.Trim().ToLower().Equals("teacher"));
        }
        public bool sendMail(string EmailId, string subject, string body)
        {
            return ClassUtility.sendMail(EmailId, subject, body, emailSender.emailInfo);
        }
    }
}
