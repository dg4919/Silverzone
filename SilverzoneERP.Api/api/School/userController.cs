using System.Web.Http;
using SilverzoneERP.Entities.Models;
using SilverzoneERP.Data;
using System;
using System.Linq;
using SilverzoneERP.Entities.ViewModel.School;
using System.IO;
using SilverzoneERP.Entities;
using System.Collections.Generic;

namespace SilverzoneERP.Api.api.School
{
    [Authorize]
    public class userController : ApiController
    {
        IUserRepository _userRepository;
        IAccountRepository _accountRepository;
        IRoleRepository _roleRepository;
        IRolePermissionRepository _rolePermissionRepository;
        IFormManagementRepository _formManagementRepository;
        IUserPermissionRepository _userPermissionRepository;
        ITeacherDetailRepository _teacherDetailRepository;
        ISchoolDetailRepository _schoolDetailRepository;
        ICityRepository _cityRepository;
        IRemarksRepository _remarksRepository;
        ISMS_EmailLogRepository _SMS_EmailLogRepository;
        //*****************  Constructor********************************

        public userController(IUserRepository _userRepository, IRoleRepository _roleRepository, IRolePermissionRepository _rolePermissionRepository, IFormManagementRepository _formManagementRepository, IUserPermissionRepository _userPermissionRepository, IAccountRepository _accountRepository, ITeacherDetailRepository _teacherDetailRepository, ISchoolDetailRepository _schoolDetailRepository, ICityRepository _cityRepository, IRemarksRepository _remarksRepository, ISMS_EmailLogRepository _SMS_EmailLogRepository)
        {
            this._userRepository = _userRepository;
            this._roleRepository = _roleRepository;
            this._rolePermissionRepository = _rolePermissionRepository;
            this._formManagementRepository = _formManagementRepository;
            this._userPermissionRepository = _userPermissionRepository;
            this._accountRepository = _accountRepository;
            this._teacherDetailRepository = _teacherDetailRepository;
            this._schoolDetailRepository = _schoolDetailRepository;
            this._cityRepository = _cityRepository;
            this._remarksRepository = _remarksRepository;
            this._SMS_EmailLogRepository = _SMS_EmailLogRepository;
        }

        #region GET       
        //Use
        [HttpGet]
        public IHttpActionResult GetUserSummary(int UserId, int RoleId, int StartIndex, int Limit)
        {
            try
            {
                if (UserId == 0 && RoleId == 0)
                {
                    var data = from u in _userRepository.FindBy(x => x.Status == true)
                               .OrderByDescending(x => x.UpdationDate)
                                .Skip(StartIndex).Take(Limit)
                               group u by u.RoleId into g
                               select new
                               {
                                   RoleId = g.Key,
                                   RoleName = g.FirstOrDefault().Role.RoleName,
                                   Users = g.Select(x => new
                                   {
                                       UserId = x.Id,
                                       x.UserName,
                                       x.ProfilePic,
                                       x.EmailID
                                   })
                               };

                    var count = _userRepository.FindBy(x => x.Status == true).Count();

                    return Ok(new { result = data, count = data.Count() });
                }

                else if (UserId != 0 && RoleId != 0)
                {
                    var data = from u in _userRepository.FindBy(x => x.Id == UserId && x.RoleId == RoleId && x.Status == true)
                               .OrderByDescending(x => x.UpdationDate)
                               .Skip(StartIndex).Take(Limit)
                               group u by u.RoleId into g
                               select new
                               {
                                   RoleId = g.Key,
                                   RoleName = g.FirstOrDefault().Role.RoleName,
                                   Users = g.Select(x => new
                                   {
                                       UserId = x.Id,
                                       x.UserName,
                                       x.ProfilePic,
                                       x.EmailID
                                   })
                               };

                    var count = _userRepository.FindBy(x => x.Id == UserId && x.RoleId == RoleId && x.Status == true).Count();

                    return Ok(new { result = data, count = data.Count() });
                }
                else if (UserId == 0 && RoleId != 0)
                {
                    var data = from u in _userRepository.FindBy(x => x.RoleId == RoleId && x.Status == true)
                               .OrderByDescending(x => x.UpdationDate)
                               .Skip(StartIndex).Take(Limit)
                               group u by u.RoleId into g
                               select new
                               {
                                   RoleId = g.Key,
                                   RoleName = g.FirstOrDefault().Role.RoleName,
                                   Users = g.Select(x => new
                                   {
                                       UserId = x.Id,
                                       x.UserName,
                                       x.ProfilePic,
                                       x.EmailID
                                   })
                               };

                    var count = _userRepository.FindBy(x => x.RoleId == RoleId && x.Status == true).Count();

                    return Ok(new { result = data, count = data.Count() });
                }
                else if (UserId != 0 && RoleId == 0)
                {
                    var data = from u in _userRepository.FindBy(x => x.Id == UserId && x.Status == true)
                               .OrderByDescending(x => x.UpdationDate)
                               .Skip(StartIndex).Take(Limit)
                               group u by u.RoleId into g
                               select new
                               {
                                   RoleId = g.Key,
                                   RoleName = g.FirstOrDefault().Role.RoleName,
                                   Users = g.Select(x => new
                                   {
                                       UserId = x.Id,
                                       x.UserName,
                                       x.ProfilePic,
                                       x.EmailID
                                   })
                               };

                    var count = _userRepository.FindBy(x => x.Id == UserId && x.Status == true).Count();

                    return Ok(new { result = data, count = data.Count() });
                }

                return Ok();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        [HttpGet]
        public IHttpActionResult GetRole()
        {
            return Ok(new { result = _roleRepository.FindBy(x => x.Status == true) });
        }

        private Nullable<bool> GetGroup(Nullable<long> ParentId)
        {
            Nullable<bool> val = null;
            if (ParentId == null)
                val = true;
            return val;
        }

        #endregion

        [HttpGet]
        public IHttpActionResult GetTeacherlist(int startIndex, int limit, bool IsTeacherApp, Nullable<DateTime> From, Nullable<DateTime> To, string AnySearch,Nullable<bool>IsActive)
        {
            long Count = 0;
            long AllCount = 0;
            if (IsTeacherApp)
                return Ok(new { TeacherList = _userRepository.GetTeacherAppList(startIndex, limit, From, To, AnySearch,IsActive, ref Count,ref AllCount), Count = Count, AllCount });
            else
                return Ok(new { TeacherList = _userRepository.GetTeacherList(startIndex, limit, From, To, AnySearch,IsActive, ref Count, ref AllCount), Count = Count, AllCount });
        }

        [HttpGet]
        public IHttpActionResult GetTeacherDetail(long UserId)
        {
            var data = _teacherDetailRepository.FindById(UserId);
            return Ok(new { TeacherDetail = data });
        }

        [HttpPost]
        public IHttpActionResult ActivateTeacher(Remarks model, string SchoolCode, bool IsTeacherApp, bool IsConfirm)
        {
            if (ModelState.IsValid)
            {
                using (var transaction = _teacherDetailRepository.BeginTransaction())
                {
                    try
                    {
                        string msg = "";
                        if (IsTeacherApp)
                        {
                            var _teacherUser = _accountRepository.FindById((long)model.UserId);
                            if (_teacherUser != null)
                            {
                                var _teacherDetail = _teacherDetailRepository.FindById(_teacherUser.Id);
                                if (_teacherUser.SchCode != SchoolCode)
                                {   
                                    if(string.IsNullOrWhiteSpace(_teacherUser.SchCode))
                                    {
                                        if (!_schoolDetailRepository.Exist(SchoolCode))
                                        {
                                            var _schoolDetail = _schoolDetailRepository.Create(new Entities.Models.SchoolDetail
                                            {
                                                SchCode = SchoolCode,
                                                SchName = _teacherDetail.SchoolName,
                                                SchAddress = _teacherDetail.SchoolAddress,
                                                City = _teacherDetail.City,
                                                Country = _teacherDetail.Country,
                                                State = _teacherDetail.State,
                                                SchPinCode = _teacherDetail.PinCode,
                                                Status = true
                                            });
                                        }
                                        else
                                        {
                                            msg = "School Code("+ SchoolCode + ") already assigned to another user(" + string.Join(",", _accountRepository.FindBy(x => x.SchCode == SchoolCode).Select(x => x.EmailID).ToList()) + ")";
                                            return Ok(new { result = "error", message = msg });
                                        }
                                    }
                                    else
                                    {
                                        var _schoolDetail=_schoolDetailRepository.FindBy(x => x.SchCode.Trim() == SchoolCode.Trim()).FirstOrDefault();
                                        if (!_schoolDetail.SchName.Trim().ToLower().Equals(_teacherDetail.SchoolName.Trim().ToLower()))
                                        {
                                            msg = "School Code("+ SchoolCode + ") already assigned to another school("+ _schoolDetail.SchName + ")";
                                            return Ok(new { result = "error", message = msg });
                                        }                                        
                                    }                                    
                                    _teacherDetail.SchoolCode = SchoolCode;
                                    _teacherDetail.Status = true;
                                   _teacherUser.SchCode = _teacherDetail.SchoolCode;                                    
                                }
                                _teacherDetail.is_Active = IsConfirm;
                                _teacherDetailRepository.Update(_teacherDetail);

                                _teacherUser.IsActive = IsConfirm;

                                msg = _teacherUser.IsActive == true ? "Successfully teacher activated !" : "Successfully teacher Deactivated !";
                                _accountRepository.Update(_teacherUser);
                                if (_teacherUser.FireBaseToken != null)
                                {
                                    string notificationTitle = "Account Activation";
                                    string notificationMsg = "Your account has been activated";
                                    if (_teacherUser.IsActive != true)
                                    {
                                        notificationTitle = "Account De-Activation";
                                        notificationMsg = "Your account has been De-activated";
                                    }

                                    FCMPushNotification.SendPushNotification(_teacherUser.FireBaseToken, ServerKey.serverapikey, ServerKey.senderId, _teacherUser.Role.RoleName, notificationTitle, notificationMsg, "");
                                }
                                Nullable<long> _SchCode = long.Parse(SchoolCode);
                                Send_Email_On_Teacher(_teacherUser, _SchCode);
                                Send_Email_On_Admin(_teacherUser, _SchCode);                                
                            }
                        }
                        else
                        {
                            var _teacher = _userRepository.FindById((long)model.UserId);
                            if (_teacher != null)
                            {
                                _teacher.IsActive = IsConfirm;
                                msg = _teacher.IsActive == true ? "Successfully teacher activated !" : "Successfully teacher Deactivated !";
                                _userRepository.Update(_teacher);
                                if (_teacher.FireBaseToken != null)
                                    FCMPushNotification.SendPushNotification(_teacher.FireBaseToken, ServerKey.serverapikey, ServerKey.senderId, _teacher.Role.RoleName, "Account Activation", "your account has been activated", "");

                                string path = System.Web.Hosting.HostingEnvironment.MapPath("~/Format/Notification.html");
                                string HtmlTemplate = File.ReadAllText(path);

                                _userRepository.sendMail(_teacher.EmailID, "Account Activation - Silverzone", HtmlTemplate);
                            }
                        }

                        _remarksRepository.Create(model);

                        transaction.Commit();
                        return Ok(new { result = "Success", message = msg });
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return Ok(new { result = "error", message = "error" });
                    }
                }
            }
            return Ok(new { result = "error", message = "error" });
        }
        private void Send_Email_On_Teacher(dynamic _teacherUser,Nullable<long> SchoolCode)
        {
            string path = System.Web.Hosting.HostingEnvironment.MapPath("~/Format/Notification.html");
            string HtmlTemplate = File.ReadAllText(path);
            string Content = "<strong>Sorry User,<br/> Your account has been De-activated due to missuse.<br/> Have you any query please contact on Email-Id: info@silverzone.org</strong>";
            string Title = "Account De-Activation - Silverzone";
            if (_teacherUser.IsActive == true)
            {
                Content = "<strong>Dear User,<br/> Your account has been activated.</strong>";
                Title = "Account Activation - Silverzone";
            }
                            
            HtmlTemplate = HtmlTemplate.Replace("{{Details1}}", Content);


            if (_userRepository.sendMail(_teacherUser.EmailID, "Account Activation - Silverzone", HtmlTemplate))
                _SMS_EmailLogRepository.Log(null,_teacherUser.EmailID, Title, ServerKey.HtmlToPlainText(Content), "Teacher/User", SchoolCode);

        }
        private void Send_Email_On_Admin(dynamic _teacherUser, Nullable<long> SchoolCode)
        {
            long ActionBy = Convert.ToInt32(User.Identity.Name);
            var ActionBy_Detail = _userRepository.FindById(ActionBy);
            string HtmlTemplate = File.ReadAllText(System.Web.Hosting.HostingEnvironment.MapPath("~/Format/Notification.html"));

            string Content = "<strong>Account has been De-activated by <spam style='color:darkblue'>" + ActionBy_Detail.UserName + "(Email-Id: " + ActionBy_Detail.EmailID + ")</spam>.</strong><br /><br /><div style = \"text-align:left\"><strong><ul><li> School Code: " + _teacherUser.SchCode + " </li><li> Teacher Name: " + _teacherUser.UserName + "(Email-Id: " + _teacherUser.EmailID + ") </li></ul> <br/></strong></div>";
            string Title = "Account De-Activation - Silverzone";

            if (_teacherUser.IsActive == true)
            {
                Content = "<strong>Account has been activated by <spam style='color:darkblue'>" + ActionBy_Detail.UserName + "(Email-Id: " + ActionBy_Detail.EmailID + ")</spam>.</strong><br /><br /><div style = \"text-align:left\" ><strong><ul><li> School Code: " + _teacherUser.SchCode + " </li><li> Teacher Name: " + _teacherUser.UserName + "(Email-Id: " + _teacherUser.EmailID + ")</li></ul> <br/></strong></div>";
                Title = "Account Activation - Silverzone";
            }

            HtmlTemplate = HtmlTemplate.Replace("{{Details1}}", Content);
            //info@silverzone.org
            if (_userRepository.sendMail("info@silverzone.org", "Account Activation - Silverzone", HtmlTemplate))
                _SMS_EmailLogRepository.Log(null, _teacherUser.EmailID, Title, ServerKey.HtmlToPlainText(Content), "Teacher/User", SchoolCode);
        }
        [HttpPost]
        public IHttpActionResult DeleteTeacher(List<multiSelect> model)
        {
            if (ModelState.IsValid && model.Count != 0)
            {
                try
                {                   
                    string Query =string.Join(",", model.Select(x=>x.Id));
                    if(_userRepository.DeleteTeacher(Query))
                        return Ok(new { result = "Success", message = "Sucessfully teacher deleted !" });
                    else
                        return Ok(new { result = "Success", message = "Failed teacher deletion !" });

                }
                catch (Exception ex)
                {
                    return Ok(new { result = "error", message = ex.Message });
                }
            }
            return Ok(new { result = "error", message = "Error" });           
        }

        [HttpPost]
        public IHttpActionResult userPermission(UserPermissionViewModel models)
        {
            if (models != null)
            {
                //Delete All permission of user
                if (models.Forms.Count != 0)
                {
                    //var _userPermission = userPermissionRepository.FindBy(x => x.UserId == models.UserId);

                    //foreach (var item in _userPermission)
                    //{
                    //    var _userPermission = userPermissionRepository.FindBy(x => x.UserId == models.UserId);
                    //    userPermissionRepository.Delete(item, false);
                    //}
                    //userPermissionRepository.Save();

                    foreach (var item in models.Forms)
                    {
                        var _UserPermission = _userPermissionRepository.FindBy(x => x.UserId == models.UserId && x.FormId == item.FormId).FirstOrDefault();
                        if (_UserPermission == null)
                        {
                            _userPermissionRepository.Create(new UserPermission
                            {
                                UserId = models.UserId,
                                FormId = item.FormId,
                                Add = item.Permission.Add,
                                Edit = item.Permission.Edit,
                                Delete = item.Permission.Delete,
                                Read = item.Permission.Read,
                                Print = item.Permission.Print,
                                Status = true
                            });
                        }
                        else
                        {
                            _UserPermission.Add = item.Permission.Add;
                            _UserPermission.Edit = item.Permission.Edit;
                            _UserPermission.Delete = item.Permission.Delete;
                            _UserPermission.Read = item.Permission.Read;
                            _UserPermission.Print = item.Permission.Print;

                            _userPermissionRepository.Update(_UserPermission);
                        }

                    }
                }
                return Ok(new { result = "Success", message = "User Permission created Successfully!" });
            }
            return Ok(new { result = "error", message = "error" });
        }
        // #endregion

        #region Delete
        [HttpDelete]
        public IHttpActionResult DeleteUser(int Id)
        {
            if (Id != 0)
            {
                var _user = _userRepository.GetById(Id);
                _user.Status = false;
                _userRepository.Update(_user);
                return Ok(new { result = "Success", message = "Role deleted sucessfully" });
            }
            return Ok(new { result = "error", message = "" });
        }
        [HttpDelete]
        public IHttpActionResult DeleteRole(long Id)
        {
            if (Id != 0)
            {
                var _role = _roleRepository.GetById(Id);
                _role.Status = false;
                _roleRepository.Update(_role);
                DeleteRolePermission(Id);
                return Ok(new { result = "Success", message = "Role deleted sucessfully" });
            }
            return Ok(new { result = "error", message = "" });
        }

        private void DeleteRolePermission(long Id)
        {
            var rolePermissionList = _rolePermissionRepository.FindBy(x => x.RoleId == Id);
            _rolePermissionRepository.DeleteWhere(rolePermissionList);
            _rolePermissionRepository.Save();
        }
        //[HttpDelete]
        //public IHttpActionResult DeleteRolePermission(int Id)
        //{
        //    if (Id != 0)
        //    {
        //        rolePermissionRepository.Delete(rolePermissionRepository.GetById(Id));
        //        return Ok(new { result = "Success", message = "Role Permission deleted sucessfully" });
        //    }
        //    return Ok(new { result = "error", message = "error" });
        //}

        //public IHttpActionResult DeleteUserRole(int Id)
        //{
        //    if (Id != 0)
        //    {                              
        //        userRoleRepository.Delete(userRoleRepository.GetById(Id));
        //        return Ok(new { result = "Success", message = "Role deleted sucessfully" });
        //    }
        //    return Ok(new { result = "error", message = "" });
        //}

        #endregion


    }
}
