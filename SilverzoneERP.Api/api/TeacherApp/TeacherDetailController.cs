using SilverzoneERP.Data;
using SilverzoneERP.Entities.Models;
using SilverzoneERP.Entities.ViewModel.TeacherApp;
using System;
using System.IO;
using System.Web.Http;

namespace SilverzoneERP.Api.api.TeacherApp
{
    [Authorize]
    public class TeacherDetailController : ApiController
    {
        private ITeacherDetailRepository teacherDetailRepository { get; set; }
        private ITeacherEventRepository teacherEventRepository { get; set; }
        private IProfileRepository profileRepository { get; set; }
        private IEventRepository eventRepository { get; set; }
        private IAccountRepository accountRepository;
        private ITeacherLogRepository teacherLogRepository;
        private IBookOrderRepository bookOrderRepository;
        private IStudentRegistrationRepository studentRegistrationRepository;

        
        [HttpPost]
        public IHttpActionResult create_Profile(string profileName)
        {
            if (profileRepository.isExist(profileName))
                return Ok(new { result = "exist" });

            profileRepository.Create(new Profile()
            {
                ProfileName = profileName,
                is_Active = true
            });
            return Ok(new { result = "success" });
        }

        [HttpGet]
        public IHttpActionResult get_Profiles()
        {
            var result = profileRepository.GetByStatus(true);
            return Ok(new { result = result });
        }
       
        [HttpPost]
        public IHttpActionResult save_userDetail(UserDetailViewModel userModel)
        {
            if (ModelState.IsValid)
            {
                int userId = Convert.ToInt32(User.Identity.Name);

                var userInfo = accountRepository.FindById(userId);

                userInfo.GenderType = userModel.gender;
                userInfo.DOB = userModel.Age;
                userInfo.UserName = userModel.UserName;
                userInfo.EmailID = userModel.Email;
                accountRepository.Update(userInfo);

                teacherDetailRepository.Create(new TeacherDetail()
                {
                    Id = userId,
                    City = userModel.City,
                    Country = userModel.Country,
                    ProfileId = userModel.ProfileId,
                    SchoolAddress = userModel.SchoolAddress,
                    SchoolName = userModel.SchoolName,
                    State = userModel.State,
                    PinCode = userModel.PinCode,
                    //ActionId=2,
                    //Email= userModel.Email,
                    is_Active = false,
                    Status = true
                }, false);

                foreach (var eventId in userModel.Events)
                {
                    teacherEventRepository.Create(new TeacherEvent()
                    {
                        EventId = eventId,
                        UserId = userId,
                        Status = true
                    }, false);      // bulk insert only .. not saved data Yet !
                }

                teacherDetailRepository.Save(); // finally save changes :)

                string path = System.Web.Hosting.HostingEnvironment.MapPath("~/Format/Notification.html");
                string HtmlTemplate = File.ReadAllText(path);
                string content = "<strong>Teacher has been registered.</strong><br /><br />"
                                 + "<div style = \"text-align:left\" ><strong>"
                                 + "<table>"
                                 + "<tr>"
                                 + "<td>Teacher Name</td><td>:</td><td style = 'color:darkblue'>"+ userInfo.UserName + "</td>"
                                 + "</tr>"
                                 + "<tr>"
                                 + "<td>Email-Id</td><td>:</td><td style = 'color:darkblue'>" + userInfo.EmailID + "</td>"
                                 + "</tr>"
                                 + "<tr>"
                                 + "<td>Mobile No.</td><td>:</td><td style = 'color:darkblue'>" + userInfo.MobileNumber + "</td>"
                                 + "</tr>"
                                 + "<tr>"
                                 + "<td>School</td><td>:</td><td style = 'color:darkblue'>"+ userModel.SchoolName + "</td>"
                                 + "</tr>"
                                 + "<tr>"
                                 + "<td>Address</td><td>:</td><td style = 'color:darkblue'>" + userModel.SchoolAddress + ",</td>"
                                 + "</tr>"
                                 + "<tr>"
                                 + "<td></td><td></td><td style = 'color:darkblue' >" + userModel.City + ", " + userModel.State + ",</td>"
                                 + "</tr>"
                                 + "<tr>"
                                 + "<td></td><td></td><td style = 'color:darkblue' >" + userModel.Country + " - " + userModel.PinCode + " </td >"
                                 + "</tr>"
                                 + "</table>"
                                 + "</strong></div>";
                                                                                         
                HtmlTemplate = HtmlTemplate.Replace("{{Details1}}", content);

                accountRepository.sendMail("info@silverzone.org", "Teacher Registration - Silverzone", HtmlTemplate);

                return Ok(new { result = "success" });
            }
            return Ok(new { result = "error" });
        }

        [HttpPost]
        public IHttpActionResult update_userDetail(UserDetailViewModel userModel)
        {
            if (ModelState.IsValid)
            {
                int userId = Convert.ToInt32(User.Identity.Name);

                var userInfo = accountRepository.FindById(userId);

                userInfo.GenderType = userModel.gender;
                userInfo.DOB = userModel.Age;
                userInfo.UserName = userModel.UserName;
                userInfo.EmailID = userModel.Email;
                accountRepository.Update(userInfo);

                var entity = teacherDetailRepository.FindById(userId);
                entity.City = userModel.City;
                entity.Country = userModel.Country;
                entity.ProfileId = userModel.ProfileId;
                entity.SchoolAddress = userModel.SchoolAddress;
                entity.SchoolName = userModel.SchoolName;
                entity.State = userModel.State;
                entity.PinCode = userModel.PinCode;
                //entity.Email = userModel.Email;
                teacherDetailRepository.Update(entity);

                teacherEventRepository.DeleteWhere(
                teacherEventRepository.FindBy(x => x.UserId == userId));

                foreach (var eventId in userModel.Events)
                {
                    teacherEventRepository.Create(new TeacherEvent()
                    {
                        EventId = eventId,
                        UserId = userId,
                        Status=true

                    }, false);      // bulk insert only .. not saved data Yet !
                }

                teacherDetailRepository.Save(); // finally save changes :)

                return Ok(new { result = "success" });
            }
            return Ok(new { result = "error" });
        }

    

        [HttpGet]
        public IHttpActionResult GetTeacherList()
        {
            var teacherList = accountRepository.FindBy(x => x.SchCode != null);
            return Ok(new { result = teacherList });
        }
        [HttpPost]

        public IHttpActionResult ConfirmBookOrder(ConfirmBookOrderViewModel model)
        {
            if(ModelState.IsValid)
            {
                string msg = string.Empty;
                try {

                    
                        var _reg = bookOrderRepository.GetById(model.Id);
                    if (_reg != null)
                    {
                        _reg.IsConfirmed = true;
                        bookOrderRepository.Update(_reg);
                        msg = "Order confirmed.";
                    }
                    else
                    {
                        return Ok(new { result = "Error", Message = "Record not found" });
                    }
                }
                catch(Exception ex)
                {
                    return Ok(new { result = "Error", Message = ex.Message });
                }
                return Ok(new { result = "Success", Message = msg});
            }
           
           
            
            return Ok(new { result = "Error", Message = "Error" });
        }     
        public TeacherDetailController(
            ITeacherDetailRepository _teacherDetailRepository,
            ITeacherEventRepository _teacherEventRepository,
            IProfileRepository _profileRepository,
            IEventRepository _eventRepository,
            IAccountRepository _accountRepository,
            ITeacherLogRepository _teacherLogRepository,
            IBookOrderRepository _bookOrderRepository,
            IStudentRegistrationRepository _studentRegistrationRepository
            )
        {
            teacherDetailRepository = _teacherDetailRepository;
            teacherEventRepository = _teacherEventRepository;
            profileRepository = _profileRepository;
            eventRepository = _eventRepository;
            accountRepository = _accountRepository;
            teacherLogRepository = _teacherLogRepository;
            bookOrderRepository = _bookOrderRepository;
            studentRegistrationRepository = _studentRegistrationRepository;
        }
    }
}
