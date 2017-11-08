using SilverzoneERP.Context;
using SilverzoneERP.Entities;
using SilverzoneERP.Entities.Models;
using System;
using System.Linq;

namespace SilverzoneERP.Data
{
    public class EnrollmentOrderRepository : BaseRepository<EnrollmentOrder>, IEnrollmentOrderRepository
    {
        public EnrollmentOrderRepository(SilverzoneERPContext context) : base(context) { }

        public dynamic EnrollmentOrderSummary(Nullable<DateTime> From, Nullable<DateTime> To, Nullable<long> EventId, Nullable<long> UserId, ref long Count)
        {
            var data = (from eo in _dbset
                       join u in _dbContext.ERPusers on eo.UpdatedBy equals u.Id
                        where eo.UpdationDate >= (From == null ? eo.UpdationDate : From)
                        && eo.UpdationDate <= (To == null ? eo.UpdationDate : To)
                        && eo.EventManagement.EventId == (EventId == null ? eo.EventManagement.EventId : EventId)
                        && eo.UpdatedBy == (UserId == null ? eo.UpdatedBy : UserId)
                        orderby eo.OrderDate descending
                       select new
                       {
                           eo.EventManagement.School.SchCode,
                           EventCode = eo.EventManagement.Event.EventCode + eo.EventManagement.EventManagementYear.ToString().Substring(2, 2),
                           eo.OrderDate,
                           TotlaEnrollment=eo.EnrollmentOrderDetail.Sum(x=>x.No_Of_Student),
                           UserId = eo.UpdatedBy,
                           u.UserName
                       }).ToList();
            if (data.Count() == 0)
                Count = 0;
            else
                Count = data.Sum(x => x.TotlaEnrollment);
            return data;
        }
        public dynamic BookOrderSummary(Nullable<DateTime> From, Nullable<DateTime> To, Nullable<long> EventId, Nullable<long> UserId, ref long Count)
        {            
            var data = from pom in _dbContext.PurchaseOrder_Masters
                       join evm in _dbContext.EventManagements on pom.srcFrom equals evm.Id
                       join u in _dbContext.ERPusers on pom.UpdatedBy equals u.Id
                       where pom.From == orderSourceType.School
                       && evm.EventId == (EventId == null ? evm.EventId : EventId)
                       && pom.UpdatedBy == (UserId == null ? pom.UpdatedBy : UserId)
                       select new {
                           pom,
                           evm,
                           u
                       };
            if(From != null)
               data = data.Where(x=>x.pom.UpdationDate >= From);
            if (To != null)
                data = data.Where(x => x.pom.UpdationDate <= To);

            var res = data.Select(x => new {
                x.evm.School.SchCode,
                EventCode = x.evm.Event.EventCode + x.evm.EventManagementYear.ToString().Substring(2, 2),
                x.pom.UpdationDate,
                x.pom.PO_Date,                
                TotlaOrder = x.pom.PurchaseOrders.Count() == 0 ? 0 : x.pom.PurchaseOrders.Sum(s => s.Quantity),
                UserId = x.pom.UpdatedBy,
                x.u.UserName,
                PurchaseOrder = x.pom.PurchaseOrders.Select(p => new { Title=p.Book.ItemTitle_Master.BookCategory.Name, p.Quantity }).GroupBy(g=>g.Title).Select(a=> new {
                    a.FirstOrDefault().Title,
                    sumQty = a.Sum(y => y.Quantity)
                })
            }).OrderByDescending(x=>x.UpdationDate);

            //var data1=res.GroupBy(x=>x.PurchaseOrder.GroupBy)
            if (res.Count() != 0)
                Count = res.Sum(x => x.TotlaOrder);
            return res;
        }        
    }
}
