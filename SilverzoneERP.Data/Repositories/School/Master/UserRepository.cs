using SilverzoneERP.Context;
using SilverzoneERP.Entities.Constant;
using SilverzoneERP.Entities.Models;
using System;
using System.Data.SqlClient;
using System.Linq;

namespace SilverzoneERP.Data
{
    public class UserRepository: BaseRepository<ERPuser>, IUserRepository
    {
        public UserRepository(SilverzoneERPContext context) : base(context) { }

        public ERPuser GetById(long id)
        {
            try
            {
                return _dbContext.ERPusers.FirstOrDefault(x => x.Id == id && x.Status == true);
            }
            catch (Exception ex)
            {
                throw ex;
            }            
        }

        public ERPuser GetByEmail(string EmailID)
        {
            try
            {
                return _dbContext.ERPusers.FirstOrDefault(x => x.EmailID == EmailID);                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ERPuser SignIn(string EmailID, string Password,long RoleId)
        {
            try
            {
                if(RoleId!=0)
                    return _dbContext.ERPusers.FirstOrDefault(x =>x.RoleId==RoleId && x.EmailID == EmailID && x.Password == Password && x.Status == true);
                else
                    return _dbContext.ERPusers.FirstOrDefault(x => x.EmailID == EmailID && x.Password == Password && x.Status == true);                    
            }
            catch (Exception ex)
            {
                throw ex;
            }            
        }

        public IQueryable<ERPuser> GetRCList()
        {
            return _dbset.Where(x => x.RoleId == 10);
        }

        public dynamic Get(int StartIndex,int Limit,out long Count)
        {
            try
            {
                var data = _dbContext.ERPusers.OrderByDescending(x=>x.UpdationDate).Skip(StartIndex).Take(Limit).Select(x => new
                {
                    x.Id,
                    x.UserName,                    
                    x.ProfilePic,
                    x.UserAddress,
                    x.EmailID,
                    UserID = "(" + x.EmailID + ")",
                    x.MobileNumber,
                    x.Password,
                    x.GenderType,
                    x.RoleId,
                    x.DOB,
                    x.Status
                });
                Count = _dbContext.ERPusers.Count();
                return data;               
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public dynamic Get()
        {
            try
            {
                var data = _dbContext.ERPusers.Where(x=>x.Status==true).OrderByDescending(x => x.UpdationDate).Select(x => new
                {
                    x.Id,
                    x.UserName,                   
                    x.ProfilePic,
                    x.UserAddress,
                    x.EmailID,
                    UserID = "(" + x.EmailID + ")",
                    x.MobileNumber,
                    x.Password,
                    x.GenderType,
                    x.RoleId,
                    x.Role.RoleName,
                    x.DOB,
                    x.Status
                });                
                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ERPuser Get(long UserId)
        {
            return _dbContext.ERPusers.FirstOrDefault(x => x.Id == UserId);
        }
        public dynamic GetCreated_UpdatedBy(long UserId)
        {
            try
            {
                var data = _dbContext.Users.Where(x=>x.Id== UserId).Select(x => new
                {                   
                    x.UserName,                   
                    x.ProfilePic,                                                                                
                }).FirstOrDefault();
                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string WriteImage(string Base64image)
        {
           return ClassUtility.WriteImage(Base64image);
        }

        public bool DeleteImage(string ImagePath)
        {
            return ClassUtility.DeleteImage(ImagePath);
        }

        public string GetUserName(long UserId)
        {
            return _dbset.FirstOrDefault(x => x.Id == UserId).UserName;
        }
        public bool sendMail(string EmailId, string subject, string body)
        {
          return ClassUtility.sendMail(EmailId, subject,body, emailSender.emailInfo);
        }
        public dynamic GetTeacherList(int startIndex, int limit, Nullable<DateTime> From, Nullable<DateTime> To, string AnySearch, Nullable<bool> IsActive, ref long count,ref long Allcount)
        {
            try
            {               
                long RoleId = 0;
                var Role_data = _dbContext.Roles.Where(x => x.RoleName.Trim().ToLower().Equals("teacher")).FirstOrDefault();
                if (Role_data != null)
                    RoleId = Role_data.Id;

                var res = _dbContext.ERPusers
                       .Where(x =>
                                   x.RoleId == RoleId
                             );
                Allcount = res.Count();
                if (IsActive == false)
                    res = res.Where(x => x.IsActive == IsActive || x.IsActive == null);
                if (IsActive == true)
                    res = res.Where(x => x.IsActive == IsActive);

                if (From != null)
                    res = res.Where(x => x.UpdationDate.Date >= From.Value.Date);

                if (To != null)
                    res = res.Where(x => x.UpdationDate.Date <= To.Value.Date);

                if (string.IsNullOrWhiteSpace(AnySearch))
                    res = res.Where(x => (x.UserName == null ? "" : x.UserName).Contains(AnySearch) ||
                                (x.EmailID == null ? "" : x.EmailID).Contains(AnySearch) ||
                                x.MobileNumber.Contains(AnySearch));

                count = res.Count();
                var data = res
                          .OrderByDescending(x => x.UpdationDate)
                          .Skip(startIndex)
                          .Take(limit)
                          .Select(x => new {
                              x.Id,
                              x.UserName,
                              Gender = x.GenderType == null ? string.Empty : x.GenderType.ToString(),
                              x.UserAddress,
                              x.ProfilePic,
                              x.EmailID,
                              x.MobileNumber,
                              x.UpdationDate,
                              Status = x.IsActive == null ? false : x.IsActive
                          }
                                   );
                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }            
        }
        public dynamic GetTeacherAppList(int startIndex, int limit, Nullable<DateTime> From, Nullable<DateTime> To, string AnySearch, Nullable<bool> IsActive, ref long count, ref long Allcount)
        {
            try
            {
                
                long RoleId = 0;
                var Role_data = _dbContext.Roles.Where(x => x.RoleName.Trim().ToLower().Equals("teacher")).FirstOrDefault();
                if (Role_data != null)
                    RoleId = Role_data.Id;
                var res = _dbContext.Users
                       .Where(x =>
                                   x.RoleId == RoleId
                             );
                Allcount = res.Count();
                if (IsActive==false)
                    res = res.Where(x => x.IsActive == IsActive || x.IsActive==null);
                if (IsActive == true)
                    res = res.Where(x => x.IsActive == IsActive);

                if (From != null)
                    res = res.Where(x => x.UpdationDate.Date >= From.Value.Date);

                if (To != null)
                    res = res.Where(x => x.UpdationDate.Date <= To.Value.Date);

                if (!string.IsNullOrWhiteSpace(AnySearch))
                    res = res.Where(x => (x.UserName == null ? "" : x.UserName).Contains(AnySearch) ||
                                (x.EmailID == null ? "" : x.EmailID).Contains(AnySearch) ||
                                x.MobileNumber.Contains(AnySearch));

                count = res.Count();
                var data = res
                          .OrderByDescending(x => x.UpdationDate)
                          .Skip(startIndex)
                          .Take(limit) 
                          .ToList()                         
                          .Select(x => new {
                              x.Id,
                              x.UserName,
                              Gender =x.GenderType == null ? string.Empty : x.GenderType.ToString() ,
                              UserAddress = "",
                              ProfilePic = image_urlResolver.getProfileImage(x.ProfilePic, x.Id),
                              x.EmailID,
                              x.MobileNumber,
                              x.UpdationDate,
                              Status = x.IsActive == null ? false : x.IsActive
                          }
                                   );
                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }            
        }

        public bool DeleteTeacher(string Query)
        {
            var res = _dbContext.Database.ExecuteSqlCommand("DELETE_Teacher @TeachersId", new SqlParameter("@TeachersId", Query));
            return res > 0 ? true : false;
        }
       public bool Exist(string EmailID, long RoleId)
        {
            return _dbContext.ERPusers.Any(x => x.EmailID== EmailID && x.RoleId == RoleId);
        }
    }
}
