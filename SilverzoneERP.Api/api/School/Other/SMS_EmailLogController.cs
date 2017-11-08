using SilverzoneERP.Data;
using System;
using System.Web.Http;

namespace SilverzoneERP.Api.api.School
{
    public class SMS_EmailLogController : ApiController
    {
        ISMS_EmailLogRepository _SMS_EmailLogRepository;
        public SMS_EmailLogController(ISMS_EmailLogRepository _SMS_EmailLogRepository)
        {
            this._SMS_EmailLogRepository = _SMS_EmailLogRepository;
        }
        
        [HttpGet]
        public IHttpActionResult Search(Nullable<long> MobileNo, string EmailId, string Content, Nullable<DateTime> From, Nullable<DateTime> To, Nullable<long> SchCode)
        {
            var data = _SMS_EmailLogRepository.Search(MobileNo, EmailId, Content, From,To, SchCode);
            return Ok(new { SMS_EmailLog =data});
        }
    }
}
