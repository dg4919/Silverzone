using SilverzoneERP.Entities.Models;
using System;

namespace SilverzoneERP.Data
{
    public interface IEnrollmentOrderRepository : IRepository<EnrollmentOrder>
    {
        dynamic EnrollmentOrderSummary(Nullable<DateTime> From, Nullable<DateTime> To, Nullable<long> EventId, Nullable<long> UserId, ref long Count);
        dynamic BookOrderSummary(Nullable<DateTime> From, Nullable<DateTime> To, Nullable<long> EventId, Nullable<long> UserId, ref long Count);        
    }    
}
