using SilverzoneERP.Entities.Models;
using System;

namespace SilverzoneERP.Data
{
    public interface ISMS_EmailLogRepository : IRepository<SMS_EmailLog>
    {
        bool Log(Nullable<long> MobileNo, string EmailId, string Purpose, string Content, string FormName, Nullable<long> SchCode);
        dynamic Search(Nullable<long> MobileNo, string EmailId, string Content, Nullable<DateTime> From, Nullable<DateTime> To, Nullable<long> SchCode);
    }
}
