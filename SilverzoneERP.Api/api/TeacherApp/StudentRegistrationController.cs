using SilverzoneERP.Data;
using SilverzoneERP.Entities;
using SilverzoneERP.Entities.Constant;
using SilverzoneERP.Entities.Models;
using SilverzoneERP.Entities.ViewModel.TeacherApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.UI.WebControls;


namespace SilverzoneERP.Api.api.TeacherApp
{
    [Authorize]
    public class StudentRegistrationController : ApiController
    {
        IMasterAcademicYearRepository masterAcademicYearRepository;
        IStudentRegistrationRepository studentRegistrationRepository;
        IAccountRepository accountRepository;
        ITeacherDetailRepository teacherDetailRepository;
        IEventRepository eventRepository;
        ISchoolEventRepository schoolEventRepository;
        IQueryRepository queryRepository;
        IBookDispatchRepository bookDispatchRepository;
        IQPDispatchRepository qpDispatchRepository;        
        IResultRepository resultRepository;
        IOlympiadListRepository olympiadListRepository;
        ITeacherEventRepository teacherEventRepository;
        IProfileRepository profileRepository;
        ITeacherQuestionRepository teacherQuestionRepository;
        ITeacherQuestionOptionRepository teacherQuestionOptionRepository;
        IErrorLogsRepository errorLogRepository;        
        IBookOrderRepository bookOrderRepository;
        IDateOptedRepository dateOptedRepository;        
        IBookCategoryRepository bookCategoryRepository;
        IitemTitle_MasterRepository itemTitle_MasterRepository;
        IBookRepository bookRepository;
        IEventYearRepository _eventYearRepository;
        ISchoolDetailRepository _schoolDetailRepository;
        IResult_L1Repository _result_L1Repository;
        IResult_L2FinalRepository _result_L2FinalRepository;

        public StudentRegistrationController(
            IMasterAcademicYearRepository _masterAcademicYearRepository, 
            IStudentRegistrationRepository _studentRegistrationRepository, 
            IAccountRepository _accountRepository, 
            ITeacherDetailRepository _teacherDetailRepository, 
            IEventRepository _eventRepository, 
            ISchoolEventRepository _schoolEventRepository,
            IQueryRepository _queryRepository, 
            IBookDispatchRepository _bookDispatchRepository,
            IQPDispatchRepository _qpDispatchRepository,
            IResultRepository _resultRepository, 
            IOlympiadListRepository _olympiadListRepository,
            ITeacherEventRepository _teacherEventRepository, 
            IProfileRepository _profileRepository, 
            ITeacherQuestionRepository _teacherQuestionRepository, 
            ITeacherQuestionOptionRepository _teacherQuestionOptionRepository, 
            IErrorLogsRepository _errorLogRepository, 
            IBookOrderRepository _bookOrderRepository, 
            IDateOptedRepository _dateOptedRepository, 
            IBookCategoryRepository bookCategoryRepository, 
            IitemTitle_MasterRepository itemTitle_MasterRepository,
            IBookRepository bookRepository,
            IEventYearRepository _eventYearRepository,
            ISchoolDetailRepository _schoolDetailRepository,
            IResult_L1Repository _result_L1Repository,
            IResult_L2FinalRepository _result_L2FinalRepository)
        {
            masterAcademicYearRepository = _masterAcademicYearRepository;
            studentRegistrationRepository = _studentRegistrationRepository;
            accountRepository = _accountRepository;
            teacherDetailRepository = _teacherDetailRepository;
            eventRepository = _eventRepository;
            schoolEventRepository = _schoolEventRepository;
            queryRepository = _queryRepository;
            bookDispatchRepository = _bookDispatchRepository;
            qpDispatchRepository = _qpDispatchRepository;           
            resultRepository = _resultRepository;
            olympiadListRepository = _olympiadListRepository;
            teacherEventRepository = _teacherEventRepository;
            profileRepository = _profileRepository;
            teacherQuestionRepository = _teacherQuestionRepository;
            teacherQuestionOptionRepository = _teacherQuestionOptionRepository;
            errorLogRepository = _errorLogRepository;           
            bookOrderRepository = _bookOrderRepository;
            dateOptedRepository = _dateOptedRepository;                      
            this.bookCategoryRepository = bookCategoryRepository;
            this.itemTitle_MasterRepository = itemTitle_MasterRepository;
            this.bookRepository = bookRepository;
            this._eventYearRepository = _eventYearRepository;
            this._schoolDetailRepository = _schoolDetailRepository;
            this._result_L1Repository = _result_L1Repository;
            this._result_L2FinalRepository = _result_L2FinalRepository;
        }

        [HttpGet]
        public IHttpActionResult GetGooglePlayVersion()
        {            
            return Ok(new { GooglePlayVersion = studentRegistrationRepository.GetGooglePlayVersion() });
        }

        [HttpPost]
        public IHttpActionResult UpdateGooglePlayVersion(GooglePlayViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if(studentRegistrationRepository.UpdateGooglePlayVersion(model.GooglePlayVersion))
                        return Ok(new { result = "success", message = "Successfully Google Play Version updated !" });
                    else
                        return Ok(new { result = "error", message = "Failed Google Play Version updation !" });
                }
                catch (Exception ex)
                {
                    return Ok(new { result = "error", message = ex.Message });
                }
            }
            return Ok(new { result = "error", message = "Bad Json format" });
        }

        public IHttpActionResult GetAllEvent()
        {            
            return Ok(new { result = eventRepository.GetActive() });         
        }
        
        [HttpGet]
        public IHttpActionResult GetSchoolEvent()
        {
            var PreYear = _eventYearRepository.FindBy(x=>x.Status==true && x.EventYearCode==ServerKey.Event_Previous_YearCode).OrderByDescending(x=>x.UpdationDate).Select(x=>new {
                x.Event.Id,
                x.Event.EventCode,
                x.Event.EventName,
                x.Event.EventImage

            });
            var CurrentYear = _eventYearRepository.FindBy(x => x.Status == true && x.Event_Year == ServerKey.Event_Current_Year).OrderByDescending(x => x.UpdationDate).Select(x => new {
                x.Event.Id,
                x.Event.EventCode,
                x.Event.EventName,
                x.Event.EventImage

            });
            return Ok(new
            {
                PrevoiusYear = new
                {
                    date=ServerKey.Event_Previous_Year,
                    Event = PreYear
                },
                CurrentYear = new
                {
                    date = ServerKey.Event_Current_Year,
                    Event = CurrentYear
                }
            });            
        }

        [HttpGet]
        public IHttpActionResult ConfirmOrder(int ConfirmType,string schCode, string EventCode, string Orderno)
        {
            using (var transaction = studentRegistrationRepository.BeginTransaction())
            {
                try
                {
                    if(ConfirmType==1)
                    {
                        var OrderList = studentRegistrationRepository.FindBy(x => x.SchCode == schCode && x.EventCode == EventCode && x.OrderNo == Orderno);

                        foreach (var item in OrderList)
                        {
                            item.IsConfirmed = true;
                            studentRegistrationRepository.Update(item);
                        }
                    }
                    else if(ConfirmType == 2)
                    {
                        var OrderList = bookOrderRepository.FindBy(x => x.SchCode == schCode && x.EventCode == EventCode && x.OrderNo == Orderno);

                        foreach (var item in OrderList)
                        {
                            item.IsConfirmed = true;
                            bookOrderRepository.Update(item);
                        }
                    }
                    
                    transaction.Commit();
                    return Ok(new { result = "success", message = "Order Successfully confirmed !" });                   
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return Ok(new { result = "error", message = ex.Message });
                }
            }           
        }

        [HttpGet]
        public IHttpActionResult GetStudentDetail(string ecode)
        {
            var userId = Convert.ToInt32(User.Identity.Name);
            var sCode = teacherDetailRepository.GetAll().Where(x => x.Id == userId).SingleOrDefault();

            if (sCode == null)
                return Ok(new { result = "error" });

            var tcode = sCode.SchoolCode;
            var studentList = studentRegistrationRepository.GetAll().Where(x => x.SchCode == tcode && x.EventCode == ecode);
            return Ok(new { result = studentList });
        }


        [HttpGet]
        public IHttpActionResult GetSchoolStudentDetailBySchoolCodeAndEventCode(string scode, string ecode)
        {
            try
            {
                var userId = int.Parse(User.Identity.Name);
                var evtcode = ecode.Substring(0, ecode.Length - 2).ToLower();

                var fromcls = 0;
                int tocls = 0;
                var yr = string.Empty;
                var year = ecode.Substring(ecode.Length - 2, 2);
                if (evtcode == "iio")
                {
                    fromcls = 1;
                    tocls = 12;
                }
                else if (evtcode == "iom")
                {
                    fromcls = 1;
                    tocls = 12;
                }
                else if (evtcode == "ios")
                {
                    fromcls = 1;
                    tocls = 12;
                }
                else if (evtcode == "ioel")
                {
                    fromcls = 1;
                    tocls = 12;
                }
                else if (evtcode == "iflo")
                {
                    fromcls = 6;
                    tocls = 10;
                }
                else if (evtcode == "abho")
                {
                    fromcls = 3;
                    tocls = 10;
                }
                else if (evtcode == "itho")
                {
                    fromcls = 1;
                    tocls = 10;
                }
                else if (evtcode == "skgko")
                {
                    fromcls = 1;
                    tocls = 10;
                }
                else if (evtcode == "iSSO")
                {
                    fromcls = 3;
                    tocls = 10;
                }
                else if (evtcode == "irao")
                {
                    fromcls = 6;
                    tocls = 12;
                }

                if (evtcode == "ioel" || evtcode == "iflo")
                {
                    int number = Convert.ToInt32(year);
                    number = number - 1;
                    yr = "20" + number + "-" + year;
                }
                else
                {
                    int number = Convert.ToInt32(year);
                    number = number + 1;
                    yr = "20" + year + "-" + number;
                }
                //Console.Write(evtcode);
                if (teacherDetailRepository.FindBy(x => x.Id == userId && x.is_Active == true) == null)
                {
                    return Ok(new { result = "Error", Message = "Teacher Profile is not Active" });
                }
                var dateOptedList = dateOptedRepository.FindBy(x => x.EventCode == evtcode && x.EventYear == yr).Select(x => new {
                    x.EventDate,
                    x.EventCode,
                    x.Id
                });

                //.ToString("dd-MM-yyyy")
                var _confirmstudentList = studentRegistrationRepository.FindBy(x => x.SchCode == scode && x.EventCode == ecode && x.IsConfirmed == true).ToList().OrderByDescending(x => x.RegistrationDate).GroupBy(x =>x.OrderNo)
                    .Select(x => new
                    {

                        x.FirstOrDefault().RegSrlNo,
                    // x.FirstOrDefault().DateOpted.EventDate,
                    RegistrationDate = x.FirstOrDefault().RegistrationDate.Value.ToString("dd-MM-yyyy"),
                    OrderNo=x.Key,

                        Records = x.Select(s => new
                        {
                            s.Class,
                            s.NoOfStudent,
                            eventDateId = s.ExamDateOpted,
                            ExamDate = s.ExamDate
                        })
                    });

                //.ToString("dd-MM-yyyy")
                var _unconfirmstudentlist = studentRegistrationRepository.FindBy(x => x.SchCode == scode && x.EventCode == ecode && x.IsConfirmed == false).ToList().OrderByDescending(x=>x.RegistrationDate).GroupBy(x => x.OrderNo)
                    .Select(x => new
                    {

                        x.FirstOrDefault().RegSrlNo,
                        // x.FirstOrDefault().DateOpted.EventDate,
                        RegistrationDate = x.FirstOrDefault().RegistrationDate.Value.ToString("dd-MM-yyyy"),
                        OrderNo = x.Key,
                        Records = x.Select(s => new
                        {
                            s.Id,
                            s.Class,
                            s.NoOfStudent,
                            eventDateId = s.ExamDateOpted,
                            ExamDate = s.ExamDate
                        })
                    });

                var _bookTypeList = from bc in bookCategoryRepository.FindBy(x => x.Status == true)                                   
                                    select new
                                    {
                                        bc.Id,
                                        bc.Name,
                                        bc.CouponId
                                    };

                //var _bookTypeList = categoryRepository.GetAll().Select(x => new { x.Id, x.Name, x.Price });

                var _confirmbookList = bookOrderRepository.FindBy(x => x.SchCode == scode && x.EventCode == ecode && x.IsConfirmed == true).ToList().ToList().OrderByDescending(x => x.OrderDate).GroupBy(x => x.OrderNo)
                   .Select(x => new
                   {
                       OrderNo= x.Key,                       
                       orderSource = Enum.GetName(typeof(orderSourceType), x.FirstOrDefault().OrderSource),
                       orderType = Enum.GetName(typeof(orderType), x.FirstOrDefault().OrderType),
                       OrderDate = x.FirstOrDefault().OrderDate.ToString("dd-MM-yyyy"),                       
                       Records = x.Select(s => new
                       {
                           s.StdClassId,
                           s.Quantity,
                           s.UnitPrice,
                           orderStatus = Enum.GetName(typeof(orderStatusType), s.OrderStatus),
                           bookTypeId=s.CategoryId,
                           s.Total

                       })
                   });
                var _unconfirmbookList = bookOrderRepository.FindBy(x => x.SchCode == scode && x.EventCode == ecode && x.IsConfirmed == false).ToList().ToList().OrderByDescending(x => x.OrderDate).GroupBy(x => x.OrderNo)
                    .Select(x => new
                    {

                        OrderNo = x.Key,
                        orderSource = Enum.GetName(typeof(orderSourceType), x.FirstOrDefault().OrderSource),
                        orderType = Enum.GetName(typeof(orderType), x.FirstOrDefault().OrderType),
                        OrderDate = x.FirstOrDefault().OrderDate.ToString("dd-MM-yyyy"),

                        Records = x.Select(s => new
                        {
                            s.Id,
                            s.StdClassId,
                            s.Quantity,
                            s.UnitPrice,
                            orderStatus = Enum.GetName(typeof(orderStatusType), s.OrderStatus),
                            s.Total,
                            bookTypeId = s.CategoryId
                        })
                    });

                return Ok(new { eventDates = dateOptedList, studentconfirmList = _confirmstudentList, studentunconfirmList = _unconfirmstudentlist, bookType = _bookTypeList, bookconfirmlist = _confirmbookList, bookunconfirmlist = _unconfirmbookList, fromClass = fromcls, toClass = tocls });
            }
            catch (Exception ex)
            {
                throw ex;
            }            
        }

        [HttpPost]
        public IHttpActionResult CreateUpdateBookOrder(List<BookOrderViewModel> model)
        {
            if (ModelState.IsValid)
            {               
                var _orderNo = string.Empty;             

                var data = model.Where(x => x.OrderNo != null && x.OrderNo!="").FirstOrDefault();
                if (data != null)
                {
                    _orderNo = data.OrderNo;
                }
                else
                {
                    _orderNo = bookOrderRepository.GenerateOrderNo();
                }
                if (model.Count > 0)
                {
                    string msg = string.Empty;                    
                    using (var transaction = bookOrderRepository.BeginTransaction())
                    {
                        try
                        {
                            foreach (BookOrderViewModel item in model)
                            {                               
                                if (item.Id == 0)
                                {
                                    var _bookTypeList = from bc in bookCategoryRepository.FindBy(x => x.Status == true && x.Id == item.CategoryId)
                                                        join itm in itemTitle_MasterRepository.FindBy(i => i.Status == true)
                                                        on bc.Id equals itm.CategoryId
                                                        join b in bookRepository.FindBy(x => x.Status == true)
                                                        on itm.Id equals b.Title_Mid
                                                        select new
                                                        {
                                                            b.Price
                                                        };
                                    decimal uprice = 0;
                                    if (_bookTypeList != null)
                                        uprice = _bookTypeList.FirstOrDefault().Price;
                                   
                                    BookOrder _bookOrder = new BookOrder();
                                    _bookOrder.OrderNo = _orderNo;
                                    _bookOrder.EventCode = item.EventCode;
                                    _bookOrder.SchCode = item.SchCode;
                                  
                                    _bookOrder.OrderDate = DateTime.Now;
                                    _bookOrder.UpdationDate = _bookOrder.OrderDate;
                                    _bookOrder.IsConfirmed = false;
                                    _bookOrder.Status = true;
                                    _bookOrder.UserId = int.Parse(User.Identity.Name);                                    
                                    SetbookOrder(_bookOrder, item, uprice);
                                    bookOrderRepository.Create(_bookOrder);                                    
                                }
                                else
                                {
                                    var _bookOrder = bookOrderRepository.GetById(item.Id);
                                    if (item.IsDelete)
                                    {                                        
                                        bookOrderRepository.Delete(_bookOrder);                                                                                                           
                                    }
                                    else
                                    {
                                        var _bookTypeList = from bc in bookCategoryRepository.FindBy(x => x.Status == true && x.Id == item.CategoryId)
                                                            join itm in itemTitle_MasterRepository.FindBy(i => i.Status == true)
                                                            on bc.Id equals itm.CategoryId
                                                            join b in bookRepository.FindBy(x => x.Status == true)
                                                            on itm.Id equals b.Title_Mid
                                                            select new
                                                            {
                                                                b.Price
                                                            };
                                        decimal uprice = 0;
                                        if (_bookTypeList != null)
                                            uprice = _bookTypeList.FirstOrDefault().Price;
                                        SetbookOrder(_bookOrder, item, uprice);
                                        _bookOrder.UpdationDate = DateTime.Now;

                                        bookOrderRepository.Update(_bookOrder);                                      
                                    }
                                }
                            }
                            transaction.Commit();
                            return Ok(new { result = "Success", Message = "BookOrder has been Successfully" });
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            return Ok(new { result = "Error", Message = ex.Message });
                        }
                    }
                }
            }
            return Ok(new { result = "Error", Message = "Error" });
        }

        private void SetbookOrder(BookOrder _bookOrder, BookOrderViewModel viewModel, decimal uprice)
        {
            _bookOrder.StdClassId = viewModel.StdClassId;
            _bookOrder.CategoryId = viewModel.CategoryId;
            _bookOrder.Quantity = viewModel.Quantity;
            _bookOrder.UnitPrice = Convert.ToDecimal(uprice);
            _bookOrder.Total = viewModel.Quantity * Convert.ToDecimal(uprice);                     
        }

        [HttpPost]
        public IHttpActionResult SendStudentRegistration(List<StudentRegistrationViewModel> model)
        {
            if (ModelState.IsValid)
            {
                using (var transaction = studentRegistrationRepository.BeginTransaction())
                {
                    try
                    {
                        var _orderNo = string.Empty;
                        int userId = int.Parse(User.Identity.Name);
                        var _schoolCode = teacherDetailRepository.FindBy(x => x.Id == userId).Select(x => new { x.SchoolCode, x.RegSrlNo }).FirstOrDefault().SchoolCode;
                        var EventCode = model.FirstOrDefault().EventCode;
                        var RegSNo = studentRegistrationRepository.GenerateRegNo(_schoolCode, EventCode);

                        var data = model.Where(x => x.OrderNo != null && x.OrderNo != "").FirstOrDefault();
                        if (data != null)
                            _orderNo = data.OrderNo;
                        else
                            _orderNo = studentRegistrationRepository.GenerateOrderNo(_schoolCode);


                        string msg = string.Empty;
                        foreach (StudentRegistrationViewModel _viewModel in model)
                        {
                            if (_viewModel.Id == 0)
                            {

                                StudentRegistration _studentRegistration = new StudentRegistration();
                                Set_Value_studentRegistration(_studentRegistration, _viewModel);

                                _studentRegistration.IsConfirmed = false;
                                _studentRegistration.OrderNo = _orderNo;
                                _studentRegistration.RegistrationMode = "online";
                                _studentRegistration.RegistrationDate = DateTime.Now;
                                _studentRegistration.RegSrlNo = RegSNo;
                                _studentRegistration.SchCode = _schoolCode;

                                _studentRegistration.Status = true;
                                _studentRegistration.UserId = userId;
                                studentRegistrationRepository.Create(_studentRegistration);
                            }
                            else
                            {
                                var _studentRegistration = studentRegistrationRepository.GetById(_viewModel.Id);
                                if (_studentRegistration != null)
                                {
                                    if (_viewModel.IsDelete)
                                        studentRegistrationRepository.Delete(_studentRegistration);
                                    else
                                    {
                                        Set_Value_studentRegistration(_studentRegistration, _viewModel);
                                        studentRegistrationRepository.Update(_studentRegistration);
                                    }
                                }
                                else
                                    return Ok(new { result = "Error", Message = "Record not found !" });
                            }
                        }
                        transaction.Commit();
                        return Ok(new { result = "Success", Message = "Student registration has been successfully" });
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return Ok(new { result = "Error", Message = ex.Message });
                    }
                }
            }
            return Ok(new { result = "Error", Message = "Error" });
        }

        private void Set_Value_studentRegistration(StudentRegistration _studentRegistration, StudentRegistrationViewModel _viewModel)
        {
            _studentRegistration.EventCode = _viewModel.EventCode;
            _studentRegistration.ExamDateOpted = _viewModel.ExamDateOpted;
            _studentRegistration.ExamDate = _viewModel.ExamDate;
            _studentRegistration.Class = _viewModel.Class;
            _studentRegistration.NoOfStudent = _viewModel.NoOfStudent;
        }
        [HttpGet]
        public IHttpActionResult GetAllDateOpted()
        {
            var dateOptedList = dateOptedRepository.GetAll().ToList();
            return Ok(new { result = dateOptedList });
        }
   

        [HttpGet]
        public IHttpActionResult  GetDispatchDetail(string scode, string  ecode)
        {
            var userId = Convert.ToInt32(User.Identity.Name);
         
            var orderList = (from tbl1 in bookDispatchRepository.FindBy(x => x.SchCode == scode && x.EventCode == ecode)
                               join tbl2 in teacherDetailRepository.GetAll() on tbl1.SchCode equals tbl2.SchoolCode
                               where
                                 tbl2.Id == userId &&
                                 tbl1.EventCode == ecode
                               select new
                               {
                                   tbl1.Id,
                                   tbl1.EventCode,
                                   tbl1.SchCode,
                                   tbl1.PacketID,
                                   tbl1.ItemDescription,
                                   tbl1.ReceivedBy,
                                   tbl1.Remarks,
                                   tbl1.Weight,
                                   tbl1.StatusDate,
                                   tbl1.DeliveryStatus,
                                   tbl1.DispatchDate,
                                   tbl1.DispatchMode,
                                   tbl1.ConsignmentNumber,
                                   tbl1.CourierName
                                   
                               });

            return Ok(orderList);
        }

        [HttpGet]
        public IHttpActionResult GetQPDispatchDetail(string scode, string ecode)
        {
            var userId = Convert.ToInt32(User.Identity.Name);
          
            var dispatchList = (from tbl1 in qpDispatchRepository.FindBy(x => x.SchCode == scode && x.EventCode == ecode)
                             join tbl2 in teacherDetailRepository.GetAll() on new { SchoolCode = tbl1.SchCode } equals new { SchoolCode = tbl2.SchoolCode }
                             where
                               tbl2.Id == userId &&
                               tbl1.EventCode == ecode
                             select new
                             {
                                 tbl1.Id,
                                 tbl1.EventCode,
                                 tbl1.SchCode,
                                 tbl1.PacketID,
                                 tbl1.ItemDescription,
                                 tbl1.ReceivedBy,
                                 tbl1.Remarks,
                                 tbl1.Weight,
                                 tbl1.StatusDate,
                                 tbl1.DeliveryStatus,
                                 tbl1.DispatchDate,
                                 tbl1.DispatchMode,
                                 tbl1.ConsignmentNumber,
                                 tbl1.CourierName

                             });

            return Ok(dispatchList);
        }
        
        #region Query

        [HttpGet]
        public IHttpActionResult GetQueryList()
        {
            var userid = Convert.ToInt32(User.Identity.Name);
            var queryList = (from tbl1 in queryRepository.FindBy(x => x.UserId == userid)
                             orderby tbl1.QueryDate descending
                             select new
                             {
                                 tbl1.Id,
                                 tbl1.QueryDate,
                                 tbl1.OldRef,
                                 tbl1.QueryDetail,
                                 tbl1.QueryStatus,
                                 tbl1.Subject,
                                 tbl1.UserId,
                                 tbl1.CloseDate
                             });

            return Ok(queryList);
        }


        [HttpGet]
        public IHttpActionResult GetQueryById(int id)
        {
            var userid = Convert.ToInt32(User.Identity.Name);
            var dataList = queryRepository.GetAll().Where(x => x.Id == id).ToList();

            return Ok(new { result = dataList });
        }

        [HttpPost]
        public IHttpActionResult CreateQuery(QueryViewModel model)
        {
            var userid =  Convert.ToInt32(User.Identity.Name);
            if (model != null)
            {
                try
                {
                    var _query = queryRepository.Create(new Query()
                    {
                        Subject = model.Subject,
                        QueryDetail = model.QueryDetail,
                        OldRef = 999,
                        QueryDate = DateTime.Now,
                        QueryStatus = "Open",
                        UserId = userid,
                        Status = true
                    });
                    _query.OldRef = _query.OldRef + _query.Id;
                    queryRepository.Update(_query);
                return Ok(new { result = "Success" });
                }
                catch (Exception ex)
                {
                    return Ok(new { result = "error" });
                }                               
            }
            return Ok(new { result = "error" });
        }
        [HttpPost]  // automatically assign value if send from ajax in a single model
        public IHttpActionResult EditQuery(QueryViewModel model)
        {
            var userid = Convert.ToInt32(User.Identity.Name);
            if (model != null)
            {
               
                    var query = queryRepository.GetById(model.id);
                    query.Subject = model.Subject;
                    query.QueryDetail = model.QueryDetail;                                        
                    query.QueryStatus = model.QueryStatus;
                    query.UserId = userid;
                    // update records
                    queryRepository.Update(query);
                    return Ok(new { result = "Success" });
                
            }

            return Ok(new { result = "error" });
        }

        [HttpGet]
        public IHttpActionResult DeleteQuery(int Id)
        {
            if (Id != 0)
            {
                queryRepository.Delete(queryRepository.
                    GetById(Id));

                return Ok(new { result = "Query deleted sucessfully" });
            }
            return Ok(new { result = "Query is not found" });
        }

        #endregion Query


        #region Result
        [HttpGet]
        public  IHttpActionResult GetResultBySchoolCodeAndEventCode(string scode,string ecode,string stdclass,int startIndex,int limit,int Level)
        {
            var userId =  Convert.ToInt32(User.Identity.Name);            
            if (Level==1)
            {
               var  data = (from L1 in _result_L1Repository.FindBy(x => x.SchCode == scode && (x.ResultEvent.EventInfo.EventCode + x.ResultEvent.ResultEventInfo.shortCode) == ecode && x.Class == stdclass)
                                  join td in teacherDetailRepository.GetAll() on L1.SchCode equals td.SchoolCode
                                  where
                                    td.Id == userId
                                  select new
                                  {
                                      L1.Class,
                                      L1.SchCode,
                                      L1.RollNo,
                                      L1.TotMarks,
                                      L1.ClassRank,
                                      L1.AllIndiaRank,
                                      L1.NIORollNo,
                                      L1.StudName,
                                      L1.RawScore,
                                      L1.SecondLevelEligible,
                                      L1.Medal,
                                      EventCode = L1.ResultEvent.EventInfo.EventCode + L1.ResultEvent.ResultEventInfo.shortCode,
                                      L1.ResultEvent.ResultEventInfo.EventYear,                                      
                                  }).OrderByDescending(x => x.Class);//.Skip(startIndex).Take(limit); 

                long Count = data.Count();

                var res = data.Skip(startIndex).Take(limit);

                return Ok(new { data = res, Count });

            }
            else if (Level == 2)
            {
                 var data=(from l2 in _result_L2FinalRepository.FindBy(x => x.Result_L1.SchCode == scode && (x.Result_L1.ResultEvent.EventInfo.EventCode+ x.Result_L1.ResultEvent.ResultEventInfo.shortCode) == ecode && x.Result_L1.Class == stdclass)
                         join td in teacherDetailRepository.GetAll() on l2.Result_L1.SchCode equals td.SchoolCode
                         where td.Id==userId
                         select new {
                             l2.Result_L1.Class,
                             l2.Result_L1.SchCode,
                             l2.Result_L1.RollNo,
                             l2.TotMarks,
                             l2.ClassRank,
                             l2.AllIndiaRank,
                             l2.Result_L1.NIORollNo,
                             l2.Result_L1.StudName,
                             l2.RawScore,
                             l2.Result_L1.SecondLevelEligible,
                             l2.Result_L1.Medal,
                             EventCode=l2.Result_L1.ResultEvent.EventInfo.EventCode+ l2.Result_L1.ResultEvent.ResultEventInfo.shortCode,
                             l2.Result_L1.ResultEvent.ResultEventInfo.EventYear,
                             l2.StateRank
                         }).OrderByDescending(x => x.Class);

                long Count = data.Count();

                var res = data.Skip(startIndex).Take(limit);
                return Ok(new { data = res, Count });
            }

            return Ok();

            //var resultList = (from tbl1 in resultRepository.FindBy(x => x.SchCode == scode && x.EventCode == ecode && x.Class == stdclass &&x.Level==Level)
            //                  join tbl2 in teacherDetailRepository.GetAll() on tbl1.SchCode equals tbl2.SchoolCode
            //                  where 
            //                    tbl2.Id == userId
            //                  select new
            //                  {
            //                      tbl1.Class,
            //                      tbl1.SchCode,
            //                      tbl1.RollNo,
            //                      tbl1.TotMarks,
            //                      tbl1.ClassRank,
            //                      tbl1.AllIndiaRank,
            //                      tbl1.NIORollNo,
            //                      tbl1.StudName,
            //                      tbl1.RawScore,
            //                      tbl1.SecondLevelEligible,
            //                      tbl1.Medal,
            //                      tbl1.EventCode,
            //                      tbl1.EventYear
            //                  }).OrderByDescending(x => x.Class);//.Skip(startIndex).Take(limit); 


            //long Count = resultList.Count();

            //var res= resultList.Skip(startIndex).Take(limit);

            //return Ok(new {data= res, Count } );
        }

        [HttpGet]
        public IHttpActionResult GetResultBySchoolCodeEventCodeAndRoll(string scode, string ecode,string rollno)
        {
            var data = string.Empty; 
            var userId = Convert.ToInt32(User.Identity.Name);
            
            var orderList = (from tbl1 in resultRepository.FindBy(x => x.SchCode == scode && x.EventCode == ecode && x.NIORollNo == rollno)
                             join tbl2 in teacherDetailRepository.GetAll() on new { SchoolCode = tbl1.SchCode } equals new { SchoolCode = tbl2.SchoolCode }
                             where
                               tbl2.Id == userId &&
                               tbl1.NIORollNo == rollno
                             select new
                             {
                                 tbl1.Id,
                                 tbl1.Class,
                                 tbl1.SchCode,
                                 tbl1.RollNo,
                                 tbl1.TotMarks,
                                 tbl1.ClassRank,
                                 tbl1.AllIndiaRank,
                                 tbl1.NIORollNo,
                                 tbl1.StudName,
                                 tbl1.RawScore,
                                 tbl1.SecondLevelEligible,
                                 tbl1.Medal,
                                 tbl1.EventCode,
                                 tbl1.EventYear
                             });
           
               return Ok(orderList) ;
        }
        #endregion Result

        #region OlympiadList
        [HttpGet]
        public IHttpActionResult GetOlympiadList(bool status)
        {
            var olyList = (from s in olympiadListRepository.FindBy(x => x.Status == status)
                         select new {
                             s.OlympiadName,
                             s.FirstDate,
                             s.SecondDate,
                             s.LastDateOfRegistration
                            
                             
                         });

            return Ok(olyList);
        }
        #endregion OlympiadList

        #region TeacherProfile
        [HttpGet]
        public IHttpActionResult GetSchoolCode()
        {
            var userid = Convert.ToInt32(User.Identity.Name);
            var data = teacherDetailRepository.FindById(userid);

            if (data != null)
                return Ok(new { result = data.SchoolCode });
            else
                return Ok(new { result = data });
        }

        [HttpGet]
        public IHttpActionResult GetTeacherProfile()
        {
            try
            {
                var userid =  Convert.ToInt32(User.Identity.Name);
                var allEvents = eventRepository.GetAll().ToList();
                var allprofile = profileRepository.GetAll().ToList();

                var events = teacherEventRepository.FindBy(x => x.Status == true && x.UserId == userid).Select(x => new
                {
                    x.Id,
                    x.EventId,
                    x.Event.EventCode
                });


                var data = teacherDetailRepository.FindBy(x => x.Status == true && x.Id== userid).ToList()
                    .Select(x => new {
                        x.SchoolName,
                        x.SchoolAddress,
                        x.PinCode,
                        x.City,
                        x.State,
                        x.Country,
                        x.ProfileId,
                        x.SchoolCode,
                        x.User.UserName,
                        Email=x.User.EmailID,
                        ProfilePic = image_urlResolver.getProfileImage(x.User.ProfilePic, x.User.Id),
                        GenderType = (System.Int32?)x.User.GenderType,
                        DOB = (System.DateTime?)x.User.DOB,
                        x.Profile.ProfileName,
                        events,
                        allEvents,
                        allprofile
                    }).FirstOrDefault();


                return Ok(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }            
        }


        [HttpGet]
        public IHttpActionResult GetUserMaster()
        {
            var allEvents = eventRepository.GetAll();
            var allprofile = profileRepository.GetAll();
            return Ok(new {allEvents, allprofile });
        }
        #endregion TeacherStatus

        #region TeacherQuestion

        [HttpGet]
        public IHttpActionResult GetTeacherQuestionAll()
        {
           
           return Ok(new { Subject = eventRepository.GetActive(), Questions= teacherQuestionRepository.Get_Active() });           
        }

        [HttpGet]
        public IHttpActionResult GetEventCodeList(bool IsBook)
        {
            return Ok(new { EventCodeList = studentRegistrationRepository.GetEventList(IsBook) });
        }
        [HttpGet]
        public IHttpActionResult GetStudentRegiList(string EventCode, bool IsConfirm, Nullable<DateTime> From, Nullable<DateTime> To)
        {
            var res = studentRegistrationRepository.FindBy(x => x.EventCode == EventCode && x.OrderNo != null && x.IsConfirmed == IsConfirm);
            if (From != null && To != null)
                res = res.Where(x => x.RegistrationDate >= From && x.RegistrationDate <= To);
            else if (From != null)
                res = res.Where(x => x.RegistrationDate >= From);
            else if (To != null)
                res = res.Where(x => x.RegistrationDate <= To);

            var _studentlist = res.ToList()
                .OrderByDescending(x => x.RegistrationDate)
                .GroupBy(x => x.OrderNo)                
                .Select(x => new
                {
                    GuidId = Guid.NewGuid().ToString().Replace("-", ""),
                    SchName= _schoolDetailRepository.GetSchoolName(x.FirstOrDefault().SchCode),
                    x.FirstOrDefault().SchCode,
                    x.FirstOrDefault().RegSrlNo,
                    x.FirstOrDefault().EventCode,
                    RegistrationDate = x.FirstOrDefault().RegistrationDate.Value.ToString("dd-MM-yyyy"),
                    OrderNo = x.Key,
                    Records = x.Select(s => new
                    {
                        s.Id,
                        s.Class,
                        s.NoOfStudent,
                        eventDateId = s.ExamDateOpted,
                        ExamDate = s.ExamDate
                    })
                });



            return Ok(new { StudentEnrollList = _studentlist });
        }

        [HttpGet]
        public IHttpActionResult GetBookOrderList(string EventCode, bool IsConfirm ,Nullable<DateTime> From, Nullable<DateTime> To)
        {
            var res = bookOrderRepository.FindBy(x => x.EventCode == EventCode && x.IsConfirmed == IsConfirm);
            if(From != null && To != null)
                res = res.Where(x => x.OrderDate >= From && x.OrderDate<=To);
            else if (From != null)
                res = res.Where(x => x.OrderDate >= From);
            else if (To != null)
                res = res.Where(x => x.OrderDate <= To);

            var _studentlist = res.ToList()
                .OrderByDescending(x => x.OrderDate)
                .GroupBy(x => x.OrderNo)                
                .Select(x => new
                {
                    GuidId = Guid.NewGuid().ToString().Replace("-", ""),
                    SchName = _schoolDetailRepository.GetSchoolName(x.FirstOrDefault().SchCode),
                    x.FirstOrDefault().SchCode,                    
                    x.FirstOrDefault().EventCode,
                    OrderDate = x.FirstOrDefault().OrderDate.ToString("dd-MM-yyyy"),
                    OrderNo = x.Key,
                    Records = x.Select(s => new
                    {
                        s.Id,
                        Class= s.StdClassId,
                        s.Quantity,
                        s.UnitPrice,
                        orderStatus = Enum.GetName(typeof(orderStatusType), s.OrderStatus),
                        s.Total,
                        bookTypeId = s.CategoryId
                    })
                });
          
            return Ok(new { StudentEnrollList = _studentlist });
        }

        [HttpPost]
        public IHttpActionResult CreateQuestion(TeacherQuestionViewModel model)
        {
            if (ModelState.IsValid)
            {
                using (var transaction = teacherQuestionRepository.BeginTransaction())
                {
                    try
                    {
                        string msg;
                        //string urlPath = image_urlResolver.question_image;
                        var _PreserveQuestionImage = "";
                        List<string> OptionImage = new List<string>();

                        TeacherQuestion _question;
                        if (model.Id==0)
                        {
                            _question = new TeacherQuestion();
                            _question.IsActive = true;
                            _question.Status = true;
                            SetQuestionValue(model, ref  _question);

                            _question = teacherQuestionRepository.Create(_question);
                            msg = "Successfully Question Created";
                        }
                        else
                        {
                            _question = teacherQuestionRepository.GetById(model.Id);
                            if(_question!=null)
                            {
                                 _PreserveQuestionImage = _question.ImageName;
                                SetQuestionValue(model, ref _question);
                                teacherQuestionRepository.Update(_question);
                                
                                var _questionOption= teacherQuestionOptionRepository.FindBy(x => x.QuestionId == model.Id);

                                var oldImage = _questionOption.Select(x => x.ImageName).ToList();

                                var newImage = model.Options.Select(x => x.optionsUrl).ToList();

                                OptionImage = _questionOption.Where(x => !newImage.Contains(x.ImageName)).Select(x=>x.ImageName).ToList();
                               

                                teacherQuestionOptionRepository.DeleteWhere(_questionOption);
                                msg = "Successfully Question Updated";
                            }
                            else
                                return Ok(new { result = "Error", message = "Question Not found" });
                        }


                        if (model.Options != null)
                        {
                            foreach (Options item in model.Options)
                            {
                                TeacherQuestionOption _option = teacherQuestionOptionRepository.Create(new TeacherQuestionOption()
                                {
                                    OptionText = item.option,
                                    ImageName = teacherQuestionOptionRepository.WriteImage(item.optionsUrl, image_urlResolver.question_image),
                                    IsAnswer = item.IsAnswer,
                                    QuestionId = _question.Id,
                                    Status=true
                                });
                                if (_option.IsAnswer)
                                {
                                    _question.AnswerId = _option.Id;
                                    teacherQuestionRepository.Update(_question);
                                }
                            }
                        }

                        transaction.Commit();

                        teacherQuestionRepository.DeleteImage(_PreserveQuestionImage);
                        foreach (var item in OptionImage)
                        {
                            teacherQuestionOptionRepository.DeleteImage(item);
                        }
                        return Ok(new { result = "Success", message = msg });
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return Ok(new { result = "Error", message = ex.Message  });
                    }                    
                }               
            }
            return Ok(new { result = "Error", message = "Error" });
        }

        private void SetQuestionValue(TeacherQuestionViewModel model, ref TeacherQuestion _question)
        {
            //string urlPath = image_urlResolver.question_image;
            _question.QuestionText = model.QuestionText;
            _question.ImageName = teacherQuestionRepository.WriteImage(model.ImageName, image_urlResolver.question_image);
            _question.SubjectId = model.SubjectId;
            _question.UserId =  Convert.ToInt32(User.Identity.Name);
        }

        #endregion TeacherQuestion

# region UserDetail Section
        [HttpGet]
        public IHttpActionResult GetUserList()
        {
            var userList = accountRepository.GetAll().Where(x=>x.Role.RoleName.ToLower()=="teacher");
            return Ok(new { result = userList });
        }

#endregion
    }
}
