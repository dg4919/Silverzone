using SilverzoneERP.Data;
using SilverzoneERP.Entities;
using SilverzoneERP.Entities.Models;
using System;
using System.IO;
using System.Linq;
using System.Web.Http;

namespace SilverzoneERP.Api.api.School
{
    public class remarksController : ApiController
    {
        IRemarksRepository _remarksRepository;
        IStudentRegistrationRepository _studentRegistrationRepository;
        IBookOrderRepository _bookOrderRepository;
        IUserRepository _userRepository;
        public remarksController(IRemarksRepository _remarksRepository, IStudentRegistrationRepository _studentRegistrationRepository, IBookOrderRepository _bookOrderRepository, IUserRepository _userRepository)
        {
            this._remarksRepository = _remarksRepository;
            this._studentRegistrationRepository = _studentRegistrationRepository;
            this._bookOrderRepository = _bookOrderRepository;
            this._userRepository = _userRepository;
        }

        [HttpPost]
        public IHttpActionResult Save(Remarks model,bool IsConfirm,bool IsBook)
        {
            if(ModelState.IsValid)
            {
                using (var transaction = _remarksRepository.BeginTransaction())
                {
                    try
                    {
                        model.Status = true;
                        _remarksRepository.Create(model);

                        string FireBaseToken=string.Empty;
                        string EmailID=string.Empty;
                        string RoleName = string.Empty;

                        if (!string.IsNullOrWhiteSpace(model.OrderNo))
                        {
                            if(IsBook)
                            {
                                var _bookOrder = _bookOrderRepository.FindBy(x => x.OrderNo == model.OrderNo);
                                if(_bookOrder.Count()!=0)
                                {
                                    FireBaseToken=_bookOrder.FirstOrDefault().User.FireBaseToken;
                                    EmailID = _bookOrder.FirstOrDefault().User.EmailID;
                                    RoleName = _bookOrder.FirstOrDefault().User.Role.RoleName;
                                }
                                foreach (var item in _bookOrder)
                                {
                                    item.IsConfirmed = IsConfirm;
                                    _bookOrderRepository.Update(item, false);
                                }
                                _bookOrderRepository.Save();
                            }
                            else
                            {
                                var _stuReg = _studentRegistrationRepository.FindBy(x => x.OrderNo == model.OrderNo);
                                if (_stuReg.Count() != 0)
                                {
                                    FireBaseToken = _stuReg.FirstOrDefault().User.FireBaseToken;
                                    EmailID = _stuReg.FirstOrDefault().User.EmailID;
                                    RoleName = _stuReg.FirstOrDefault().User.Role.RoleName;
                                }
                                foreach (var item in _stuReg)
                                {
                                    item.IsConfirmed = IsConfirm;
                                    _studentRegistrationRepository.Update(item, false);
                                }
                                _studentRegistrationRepository.Save();
                            }                            
                        }
                        transaction.Commit();
                        string customMsg = "Successfully " + (IsBook ? "Book " : "Enrollment");
                       string msg = IsConfirm == true ? customMsg + "Order activated !" : customMsg + " Order Deactivated !";
                        
                        if (!string.IsNullOrWhiteSpace(FireBaseToken))
                        {
                            customMsg = (IsBook ? "Book " : "Enrollment");

                            string notificationTitle = IsBook ? "Book " : "Enrollment"+" order confirmation";


                            string notificationMsg = "Your "+ customMsg + " order has been activated";
                            if (IsConfirm != true)
                            {                              
                                notificationMsg = "your "+ customMsg + " order has been De-Clined";
                            }

                            FCMPushNotification.SendPushNotification(FireBaseToken, ServerKey.serverapikey, ServerKey.senderId, RoleName, notificationTitle, notificationMsg, "");

                            if (!string.IsNullOrWhiteSpace(EmailID))
                            {
                                string path = System.Web.Hosting.HostingEnvironment.MapPath("~/Format/Notification.html");
                                string HtmlTemplate = File.ReadAllText(path);

                                _userRepository.sendMail(EmailID, customMsg + " order confirmation - Silverzone", HtmlTemplate);
                            }
                        }

                       
                        

                        return Ok(new { result = "Success", message = "Successfully remarks added!", RemarksList = _remarksRepository.GetAll(model.UserId,model.OrderNo) });
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return Ok(new { result = "error", message = ex.Message });                        
                    }
                }                                                
            }
            return Ok(new { result = "error", message = "error" });
        }

        [HttpGet]
        public IHttpActionResult RemarksList(Nullable<long> UserId,string OrderNo)
        {            
            return Ok(new { RemarksList = _remarksRepository.GetAll(UserId, OrderNo) });
        }
    }
}
