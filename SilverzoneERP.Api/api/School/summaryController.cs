using SilverzoneERP.Data;
using System;
using System.Linq;
using System.Web.Http;

namespace SilverzoneERP.Api.api.School
{
    public class summaryController : ApiController
    {
        IEnrollmentOrderRepository _enrollmentOrderRepository;
        IUserRepository _userRepository;
        IBookCategoryRepository _bookCategoryRepository;


        public summaryController(IEnrollmentOrderRepository _enrollmentOrderRepository, IUserRepository _userRepository, IBookCategoryRepository _bookCategoryRepository)
        {
            this._enrollmentOrderRepository = _enrollmentOrderRepository;
            this._userRepository = _userRepository;
            this._bookCategoryRepository = _bookCategoryRepository;
        }
        
        [HttpGet]
        public IHttpActionResult GetUserList()
        {
            var data = _userRepository.GetAll().ToList().Select(x => new
            {
                UserId= x.Id,
               x.UserName
            }).Distinct();
            return Ok(new { UserList= data });
        }

        [HttpGet]
        public IHttpActionResult GetEnrollmentOrderSummary(Nullable<DateTime>From, Nullable<DateTime> To, Nullable<long> EventId, Nullable<long> UserId)
        {
            long Count = 0;
            return Ok(new { History = _enrollmentOrderRepository.EnrollmentOrderSummary(From,To,EventId,UserId,ref Count), Count = Count });
        }

        [HttpGet]
        public IHttpActionResult GetBookOrderSummary(Nullable<DateTime> From, Nullable<DateTime> To, Nullable<long> EventId, Nullable<long> UserId)
        {
            long Count = 0;
            var _bookCategory = _bookCategoryRepository.GetAll().Select(x=>new { Title=x.Name,Total=0}).ToArray();
            return Ok(new { History = _enrollmentOrderRepository.BookOrderSummary(From, To, EventId, UserId, ref Count), BookCategory= _bookCategory, Count = Count });
        }

       
    }
}
