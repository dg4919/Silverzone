using SilverzoneERP.Data;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Web.Http;
using SilverzoneERP.Entities.ViewModel.School;
using SilverzoneERP.Entities.Models;
using System.Net;
using System.Web;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Net.Sockets;
using SilverzoneERP.Entities;
using System.Globalization;
using Newtonsoft.Json.Linq;



namespace SilverzoneERP.Api.api.School
{
    [Authorize]
    public class accountController : ApiController
    {
        IUserRepository _userRepository;
        ISchoolRepository _schoolRepository;
        IFormManagementRepository _formManagementRepository;
        IRolePermissionRepository _rolePermissionRepository;
        IRoleRepository _roleRepository;
        IUserPermissionRepository _userPermissionRepository;
        IEventYearRepository _eventYearRepository;
        //*****************  Constructor********************************

        public accountController(IUserRepository _userRepository, IFormManagementRepository _formManagementRepository, IRolePermissionRepository _rolePermissionRepository, IUserPermissionRepository _userPermissionRepository, IEventYearRepository _eventYearRepository, ISchoolRepository _schoolRepository, IRoleRepository _roleRepository)
        {
            this._userRepository = _userRepository;
            this._formManagementRepository = _formManagementRepository;
            this._rolePermissionRepository = _rolePermissionRepository;
            this._userPermissionRepository = _userPermissionRepository;
            this._eventYearRepository = _eventYearRepository;
            this._schoolRepository = _schoolRepository;
            this._roleRepository = _roleRepository;
        }

        [AllowAnonymous]
        [HttpPost]
        public IHttpActionResult login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                ERPuser user = _userRepository.SignIn(model.EmailID, model.Password,model.RoleId);
                if (user != null)
                {
                    dynamic school = new { SchName="", SchCode =0};
                    if(user.SchId != null)
                    school = _schoolRepository.FindBy(x=>x.Id==user.SchId).Select(x=>new {
                        x.SchName,
                        x.SchCode
                    }).FirstOrDefault();
                   
                    return Ok(new {
                        result = "Success",
                        user = user,
                        token = getToken(user),
                        school= school,
                        menu = GetFormUrlByUserId(user.RoleId,user.Id),
                        EventYearInfo = new {Previous=new {Year= ServerKey.Event_Previous_Year,Code=ServerKey.Event_Previous_YearCode}, Current = new { Year = ServerKey.Event_Current_Year, Code = ServerKey.Event_Current_YearCode }, Next = new { Year = ServerKey.Event_Next_Year, Code = ServerKey.Event_Next_YearCode } },
                        Event= _eventYearRepository.Get(ServerKey.Event_Current_Year, true)
                    });
                    //return Ok(new { result = "Success", user = user, token = getToken(user)});
                }
                else
                    return Ok(new { result = "error", message = "Email-Id and password does not matched. !" });
            }
            return Ok(new { result = "error", message = "error" });
        }

        private AccessTokenViewModel getToken(ERPuser model)
        {
            var url = Request.RequestUri.Scheme + "://" + Request.RequestUri.Authority;
            HttpWebRequest myWebRequest = (HttpWebRequest)WebRequest.Create(url + "/token");
            myWebRequest.ContentType = "application/x-www-form-urlencoded";
            myWebRequest.Method = "POST";

            var _role = model.Role.RoleName;



            var request = string.Format("grant_type=password&UserName={0}&Password={1}",
                                                                         HttpUtility.UrlEncode(model.Id.ToString()),
                                                                         HttpUtility.UrlEncode(model.Password)
                                                                         );

            byte[] bytes = Encoding.ASCII.GetBytes(request);
            myWebRequest.ContentLength = bytes.Length;
            using (Stream outputStream = myWebRequest.GetRequestStream())
            {
                outputStream.Write(bytes, 0, bytes.Length);
            }

            using (WebResponse webResponse = myWebRequest.GetResponse())
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(AccessTokenViewModel));

                //Get deserialized object from JSON stream
                AccessTokenViewModel token = (AccessTokenViewModel)serializer.ReadObject(webResponse.GetResponseStream());
                return token;
            }
        }

        [AllowAnonymous]
        [HttpPost]
        public IHttpActionResult registration(ViewModel model)
        {
            if (ModelState.IsValid)
            {                
                if (model.Id == 0)
                {
                    var IsExist = false;
                    if (_roleRepository.FindById(model.RoleId).RoleName.Trim().ToLower().Equals("regional coordinator"))
                        IsExist = _userRepository.Exist(model.EmailID, model.RoleId);
                    else if (_userRepository.GetByEmail(model.EmailID) == null)
                        IsExist = false;

                    if (IsExist == false)
                    {
                        ERPuser _user = new ERPuser();
                        _user.EmailID = model.EmailID;
                        _user.Status = true;

                        SetValue(model, ref _user);

                        _userRepository.Create(_user);

                        return Ok(new { result = "Success", message = "Successfully user created!" });
                    }
                    else
                        return Ok(new { result = "error", message = "User alredy Created !" });
                }
                else
                {
                    var _user = _userRepository.GetById(model.Id);
                    if (_user != null)
                    {
                        string Preserver_ProfilePic = _user.ProfilePic;
                        SetValue(model, ref _user);                        
                        _userRepository.Update(_user);

                       _userRepository.DeleteImage(Preserver_ProfilePic);
                        return Ok(new { result = "Success", message = "Successfully user updated !" });
                    }
                    return Ok(new { result = "error", message = "Failled user updation !" });
                }
            }
            return Ok(new { result = "error", message = "error" });
        }

        private void SetValue(ViewModel model,ref ERPuser _user)
        {
            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
            var ipAddress = host.AddressList.FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork);
            var ctx = HttpContext.Current;

            _user.UserName = model.UserName;
            _user.ProfilePic = _userRepository.WriteImage(model.ProfilePic);
            _user.Password = model.Password;
            _user.MobileNumber = model.MobileNumber;
            _user.GenderType = (genderType)model.GenderType;
            _user.RoleId = model.RoleId;
            _user.DOB = model.DOB;
            _user.Browser = ctx.Request.Browser.Browser;
            _user.OperatingSystem = Environment.OSVersion.ToString();
            _user.IPAddress = ipAddress.ToString();
            _user.UserAddress = model.UserAddress;
            _user.Location = RegionInfo.CurrentRegion.DisplayName;
            _user.SrcFrom = model.SrcFrom;
            _user.Qualification = model.Qualification;
            _user.OtherQualification = model.OtherQualification;
            _user.Profession = model.Profession;
            _user.HowDid = model.HowDid;
        }
        [HttpGet]
        public IHttpActionResult GetUser(int StartIndex,int Limit)
        {
            try
            {
                long Count;
                return Ok(new { result = _userRepository.Get(StartIndex,Limit,out Count),Count= Count });
            }
            catch (Exception ex)
            {
                throw ex;
            }            
        }

        [HttpGet]
        public IHttpActionResult GetAllUser()
        {
            try
            {               
                return Ok(new { result = _userRepository.Get()});
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public IHttpActionResult GetUserPermission(long UserId,long RoleId)
        {
            return Ok(new { result = GetFormUrlByUserId( RoleId, UserId) });
        }

        private dynamic GetFormUrlByUserId(long RoleId, long UserId)
        {
            try
            {
                var FormPermissionList = from rp in _rolePermissionRepository.FindBy(x => x.RoleId == RoleId && x.Status == true)
                                         join up in _userPermissionRepository.FindBy(x => x.UserId == UserId && x.Status == true)
                                         on rp.FormId equals up.FormId
                                         into Details
                                         from a in Details.DefaultIfEmpty()
                                         select new
                                         {
                                             rp.FormId,
                                             Permission = a == null ? new { rp.Add, rp.Edit, rp.Delete, rp.Read, rp.Print } : new { a.Add, a.Edit, a.Delete, a.Read, a.Print }
                                         };

                var FormList = FormPermissionList.Select(x => x.FormId).ToArray();

                var data = from frm in _formManagementRepository.FindBy(x => x.FormParentId == null && x.Status == true).OrderBy(x => x.FormOrder)
                           select new
                           {
                               FormId = frm.Id,
                               frm.FormName,
                               frm.FormUrl,
                               Active = false,
                               Forms = frm.ChildFormManagement.Where(x => FormList.Contains(x.Id) && x.FormName.ToLower() != "divider" && x.Status == true).OrderBy(x => x.FormOrder).Select(subFrm => new
                               {
                                   FormId = subFrm.Id,
                                   subFrm.FormName,
                                   subFrm.FormUrl,
                                   Permission = FormPermissionList.AsEnumerable().FirstOrDefault(x => x.FormId == subFrm.Id).Permission,
                                   Active = false,
                                   subForms = subFrm.ChildFormManagement.OrderBy(x => x.FormOrder).Select(y => new {
                                       FormId = y.Id,
                                       y.FormName,
                                       y.FormUrl,
                                       Active = false,
                                   })

                               })

                           };
                return data;
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        private JObject ParsePermission(string Permission)
        {
            var data = JObject.Parse(Permission);
            return data;
        }
        
        [HttpPost]
        public IHttpActionResult Active_Deactive(List<multiSelect> model)
        {
            try
            {
                foreach (var item in model)
                {
                    var _user = _userRepository.Get(item.Id);
                    if (_user != null)
                    {
                        _user.Status = !_user.Status;
                        _userRepository.Update(_user);
                    }
                }

                return Ok(new { result = "Success", message = "Successfully Save Changed !" });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [AllowAnonymous]
        [HttpPost]
        public IHttpActionResult SendOTP(Entities.ViewModel.Site.emailViewModel model)
        {
            
            if (!_userRepository.Exist(model.emailId,(long)model.RoleId))
            {
                string OTP = GenerateOTP();
                model.HtmlTemplate = model.HtmlTemplate.Replace("_OTP", OTP);
                if (_userRepository.sendMail(model.emailId, "Verfy OTP - Silverzone", model.HtmlTemplate))
                    return Ok(new { result = "Success", OTP = OTP, message = "OTP sent your email-Id !" });
                else
                    return Ok(new { result = "Success", message = "Failed !" });
            }
            else
                return Ok(new { result = "error", message = "User alredy Exist !" });           
        }

        private string GenerateOTP()
        {
            string alphabets = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string small_alphabets = "abcdefghijklmnopqrstuvwxyz";
            string numbers = "1234567890";

            string characters = numbers+ alphabets + small_alphabets + numbers;
            
            int length = 5;
            string otp = string.Empty;
            for (int i = 0; i < length; i++)
            {
                string character = string.Empty;
                do
                {
                    int index = new Random().Next(0, characters.Length);
                    character = characters.ToCharArray()[index].ToString();
                } while (otp.IndexOf(character) != -1);
                otp += character;
            }
            
            return otp;
        }
    }
}
