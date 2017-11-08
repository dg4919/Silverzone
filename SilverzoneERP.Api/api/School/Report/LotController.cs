using SilverzoneERP.Data;
using SilverzoneERP.Entities;
using SilverzoneERP.Entities.Models;
using SilverzoneERP.Entities.ViewModel.School;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace SilverzoneERP.Api.api.School
{
    
    public class LotController : ApiController
    {
        IEventManagementRepository _eventManagementRepository;
        IStudentEntryRepository _studentEntryRepository;
        ILotRepository _lotRepository;
        ILotDetailRepository _lotDetailRepository;
        IUserRepository _userRepository;
        IExaminationDateRepository _examinationDateRepository;
        IEnrollmentOrderRepository _enrollmentOrderRepository;
        IEnrollmentOrderDetailRepository _enrollmentOrderDetailRepository;
        IDispatch_MasterRepository _dispatch_MasterRepository;
        IDispatchRepository _dispatchRepository;
        IDispatchLogsRepository _dispatchLogsRepository;

        public LotController(IEventManagementRepository _eventManagementRepository, IStudentEntryRepository _studentEntryRepository, ILotRepository _lotRepository, ILotDetailRepository _lotDetailRepository, IUserRepository _userRepository, IExaminationDateRepository _examinationDateRepository, IEnrollmentOrderRepository _enrollmentOrderRepository, IEnrollmentOrderDetailRepository _enrollmentOrderDetailRepository, IDispatch_MasterRepository _dispatch_MasterRepository,IDispatchRepository _dispatchRepository, IDispatchLogsRepository _dispatchLogsRepository)
        {
            this._eventManagementRepository = _eventManagementRepository;
            this._studentEntryRepository = _studentEntryRepository;
            this._lotRepository = _lotRepository;
            this._lotDetailRepository = _lotDetailRepository;
            this._userRepository = _userRepository;
            this._examinationDateRepository = _examinationDateRepository;
            this._enrollmentOrderRepository = _enrollmentOrderRepository;
            this._enrollmentOrderDetailRepository = _enrollmentOrderDetailRepository;
            this._dispatch_MasterRepository = _dispatch_MasterRepository;
            this._dispatchRepository = _dispatchRepository;
            this._dispatchLogsRepository = _dispatchLogsRepository;
        }
        [HttpGet]
        public IHttpActionResult Get_Option()
        {
            var LotType = Enum.GetValues(typeof(LotType))
                       .Cast<LotType>()
                       .Select(v => new
                       {
                           Id = v.GetHashCode(),
                           Name = v.ToString().Replace("_", " ")
                       }).ToList();
            var Level = Enum.GetValues(typeof(Level))
                        .Cast<Level>()
                        .Select(v => new
                        {
                            Id = v.GetHashCode(),
                            Name = v.ToString().Replace("_", " ")
                        }).ToList();

            var LotFilter = Enum.GetValues(typeof(LotFilter))
                   .Cast<LotFilter>()
                   .Select(v => new
                   {
                       Id = v.GetHashCode(),
                       Name = v.ToString().Replace("__", "/").Replace("_", " ")
                   }).ToList();

            return Ok(new {
                ExamDateList = _examinationDateRepository.GetExamDate(),
                LotType,
                Level,
                LotFilter
            });
        }
       
        [HttpGet]
        public IHttpActionResult SearchSchool(long EventId, long ExamDateId, Nullable<DateTime> ChangeDate, bool IsZeroEnroll, bool IsZeroStuReg, bool IsDiff, bool IsNotInSelectedDate)
        {
            var LotDetailList = _lotDetailRepository.FindBy(x=>x.ObjectType== 1).Select(x=>x.Objectid).ToList();


            var res = from evm in _eventManagementRepository.FindBy(x => x.EventId == EventId && x.Status == true && x.EventManagement_YearCode == ServerKey.Event_Current_YearCode)
                      join o in _enrollmentOrderRepository.FindBy(e => e.Status == true
                                                    && IsNotInSelectedDate == true ? (e.ExaminationDateId != ExamDateId) : (e.ExaminationDateId == (ExamDateId < 1 ? null : (long?)ExamDateId)
                                                    && e.ChangeExamDate == (ExamDateId < 1 ? ChangeDate : e.ChangeExamDate))
                                                )
                     on evm.Id equals o.EventManagementId
                      where !LotDetailList.Contains(o.Id)
                      group o by o.EventManagement.School.SchCode into g
                      select new
                      {
                          g.FirstOrDefault().EventManagement.SchId,
                          g.FirstOrDefault().EventManagement.School.SchName,
                          g.FirstOrDefault().EventManagement.School.SchCode,
                          g.FirstOrDefault().EventManagement.Event.EventCode,
                          No_Of_Student = g.Sum(s => (s.EnrollmentOrderDetail.Count() == 0 ? 0 : s.EnrollmentOrderDetail.Sum(ss => ss.No_Of_Student))),// o.EnrollmentOrderDetail.Sum(eo =>eo.No_Of_Student),
                          EnrollmentOrders = g.Select(eo => new { eo.Id, EventId }),
                          CoOrdinator = g.FirstOrDefault().EventManagement.CoOrdinator.Select(c => new { c.CoOrdName, c.CoOrdMobile, c.CoOrdEmail }),
                          StudentEntry = g.Sum(s => (s.EnrollmentOrderDetail.Count() == 0 ? 0 : s.EnrollmentOrderDetail.Sum(ss =>ss.StudentEntry.Count(st=>st.Status==true))))//g.FirstOrDefault().EventManagement.StudentEntry.Count(s => s.Status == true),
                      };
           
            if (IsZeroEnroll == false && IsZeroStuReg == false && IsDiff == false)
            {
                res = res.Where(x => x.StudentEntry== x.No_Of_Student && x.No_Of_Student != 0);
            }
            else if (IsZeroEnroll)
            {
                res = res.Where(x => x.No_Of_Student >= 0);
            }
            else if (IsZeroStuReg)
            {
                res = res.Where(x => x.StudentEntry >= 0);
            }
            else if (IsDiff)
            {
                res = res.Where(x => x.StudentEntry != x.No_Of_Student && x.No_Of_Student != 0 && x.StudentEntry != 0);
            }

            return Ok(new { SchoolList = res });
        }        

        [HttpPost]
        public IHttpActionResult CreateLot(List<CreateLot> model,long ExaminationDateId,Level _level,LotType _lotType)
        {
            if (ModelState.IsValid)
            {
                using (var transaction = _lotRepository.BeginTransaction())
                {
                    try
                    {                        
                        long LotNo = _lotRepository.GetLotNo();
                        long LotId = _lotRepository.Create(new Lot
                        {
                            LotNo = LotNo,
                            ExaminationDateId = ExaminationDateId,
                            LotType = _lotType,
                            ExamLevel = _level,                            
                            EventId = model.FirstOrDefault().EnrollmentOrders.FirstOrDefault().EventId,
                            Status = true
                        }).Id;

                        var _dispatch_Master = _dispatch_MasterRepository.Create(new Dispatch_Master {
                            OrderSource = orderSourceType.Lot,
                            LotId= LotId,
                            Status=true
                        });

                        var _dispatch = _dispatchRepository.Create(new Dispatch
                        {
                            Id= _dispatch_Master.Id,
                            Packet_Id = _dispatchRepository.get_Packet_Number(_dispatch_Master.Id, orderSourceType.Lot),
                            Invoice_No = _dispatchRepository.get_Invoice_Number(_dispatch_Master.Id),
                            Order_StatusType = orderStatusType.Pending,
                            Status = true
                        });

                        _dispatchLogsRepository.Create(new DispatchLogs()
                        {
                            DispatchId = _dispatch_Master.Id,
                            Action = "Packet is created",
                            Action_Date = DateTime.Now,
                            Status = true
                        });

                        foreach (CreateLot _createLot in model)
                        {
                            foreach (EnrollmentOrderInfo _enrollmentOrderInfo in _createLot.EnrollmentOrders)
                            {
                                if(!_lotDetailRepository.Exist(_enrollmentOrderInfo.Id, (int)LotType.Question_Paper))
                                {
                                    _lotDetailRepository.Create(new LotDetail
                                    {
                                        LotId= LotId,
                                        Objectid = _enrollmentOrderInfo.Id,
                                        ObjectType = 1,
                                        Status = true
                                    });
                                }                                
                             }
                        }
                        transaction.Commit();
                        return Ok(new { result = "Success", message = "Successfully lot created..." });
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return Ok(new { result = "error", message = ex.Message });
                    }
                }
            }
            return Ok(new { result = "error", message = "Error" });   
        }

        [HttpGet]        
        public IHttpActionResult GetQuestionPaperLot(long EventId, Nullable<long> LotNo, Nullable<long> SchoolCode, Nullable<DateTime> ExamDate)
        {
            var data = _lotRepository.GetLotDetail(EventId,LotNo,SchoolCode,ExamDate);                           
            return Ok(new { result = data });
        }

        [HttpGet]
        public IHttpActionResult GenerateReport(Nullable<long> LotNo,long EventId, long ExaminationDateId, int ExamLevel, string ReportName,Nullable<LotFilter> LotFilter)
        {
            try
            {                
                string ProcedureName = "GET_QPDispatchLatter";
                string TabelName = "QPDispatchLatter";

                if (ReportName.Trim().ToLower().Equals("studentattendance")|| ReportName.Trim().ToLower().Equals("enrollmentticket"))
                {
                    ProcedureName = "GET_StudentAttendance";
                    TabelName = "StudentAttendance";
                }
                else if (ReportName.Trim().ToLower().Equals("OMRPrintForClass3To5".ToLower()))
                {
                    ProcedureName = "GET_OMRPrintForClass3To5";
                    TabelName = "StudentAttendance";
                }
                else if (ReportName.Trim().ToLower().Equals("teachercorrectionlist"))
                {
                    ProcedureName = "GET_TeacherCorrectionList";
                    TabelName = "TeacherSummary";
                }
                else if (ReportName.Trim().ToLower().Equals("rptboxsticker_3x8") || ReportName.Trim().ToLower().Equals("QPAddressLebel_1x4".ToLower()) || ReportName.Trim().ToLower().Equals("RptQPSelfArressSticker_1x4".ToLower()))
                {
                    ProcedureName = "GET_OMRClass6To12";
                    TabelName = "OMRClass6To12";
                }
                HttpContext.Current.Session["LotNo"] = LotNo;
                HttpContext.Current.Session["EventId"] = EventId;
                HttpContext.Current.Session["ExaminationDateId"] = ExaminationDateId;
                HttpContext.Current.Session["ReportName"] = ReportName + ".rpt";
                HttpContext.Current.Session["ProcedureName"] = ProcedureName;
                HttpContext.Current.Session["TabelName"] = TabelName;                
                
                HttpContext.Current.Session["LotFilter"] = LotFilter==null?0:(int)LotFilter;

                var domain = HttpContext.Current.Request.Url.Host;
                string url = "http://" + domain + "/FinalReport/Index.aspx";
                if (domain == "localhost")
                    url = "http://localhost:55615/FinalReport/Index.aspx";

                System.Uri uri = new System.Uri(url);

                return Redirect(uri);
            }
            catch (Exception ex)
            {
                return Ok(new { result = "error", message = ex.Message });
            }                        
        }

        [HttpGet]
        public IHttpActionResult IsLotCreated(long EnrollmentOrderId)
        {            
            return Ok(new { IsLotCreated = _lotDetailRepository.Exist(EnrollmentOrderId, 1) });
        }
    }
}
