using SilverzoneERP.Data;
using System.Linq;
using System.Web.Http;

namespace SilverzoneERP.Api.api.Teacher
{
    public class studentEnrollmentController : ApiController
    {
        ISchoolRepository _schoolRepository;
        IEnrollmentOrderRepository _enrollmentOrderRepository;
        IEventManagementRepository _eventManagementRepository;
        public studentEnrollmentController(ISchoolRepository _schoolRepository, IEnrollmentOrderRepository _enrollmentOrderRepository, IEventManagementRepository _eventManagementRepository)
        {
            this._schoolRepository = _schoolRepository;
            this._enrollmentOrderRepository = _enrollmentOrderRepository;
            this._eventManagementRepository = _eventManagementRepository;
        }

        [HttpGet]
        public IHttpActionResult GetEnrollmentOrder(long SchoolId)
        {
            var confirm = _eventManagementRepository.FindBy(x => x.SchId == SchoolId &&x.EnrollmentOrder.Count(o=>o.IsConfirm==true)!=0 && x.Status == true).ToList().Select(x=>new {
                x.Id,
                x.EventId,
                EventCode=x.Event.EventCode+x.EventManagementYear.ToString().Substring(2,2),
                TotalEnrollmentSummary = x.EnrollmentOrder.Where(o => o.IsConfirm == true).Sum(ss => ss.EnrollmentOrderDetail.Sum(eod => eod.No_Of_Student)),
                x.UpdationDate, 
                Order=x.EnrollmentOrder.Where(o=>o.IsConfirm==true).Select(o=>new {
                    o.Id,
                    o.OrderNo,
                    //o.TotlaEnrollment,
                    TotlaEnrollment = o.EnrollmentOrderDetail.Sum(eod => eod.No_Of_Student),
                    //EnrollmentOrderDetail= JArray.Parse(o.EnrollmentOrderDetail),
                    o.EnrollmentOrderDetail,
                    o.IsConfirm,
                    o.SrcFrom
                })               
            });

            var notconfirm = _eventManagementRepository.FindBy(x => x.SchId == SchoolId && x.EnrollmentOrder.Count(o => o.IsConfirm == false) != 0 && x.Status == true).ToList().Select(x => new {
                x.Id,
                x.EventId,
                EventCode = x.Event.EventCode + x.EventManagementYear.ToString().Substring(2, 2),
                TotalEnrollmentSummary= x.EnrollmentOrder.Where(o => o.IsConfirm == true).Sum(ss => ss.EnrollmentOrderDetail.Sum(eod => eod.No_Of_Student)),//= x.EnrollmentOrder.Where(o => o.IsConfirm == false).Sum(s=>s.TotlaEnrollment),
                x.UpdationDate,
                Order = x.EnrollmentOrder.Where(o => o.IsConfirm == false).Select(o => new {
                    o.Id,
                    o.OrderNo,
                    //o.TotlaEnrollment,
                    TotlaEnrollment = o.EnrollmentOrderDetail.Sum(eod => eod.No_Of_Student),
                    o.ExaminationDateId,
                    //EnrollmentOrderDetail = JArray.Parse(o.EnrollmentOrderDetail),
                    o.EnrollmentOrderDetail,
                    o.IsConfirm,
                    o.SrcFrom
                })
            });
            return Ok(new { EnrollmentOrderList = new {
                confirm= confirm,
                notconfirm= notconfirm
            } });
        }
    }
}
