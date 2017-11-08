using SilverzoneERP.Entities.Models;
using SilverzoneERP.Entities.ViewModel.School;
using System;
using System.Collections.Generic;

namespace SilverzoneERP.Data
{
    public interface ILotRepository : IRepository<Lot>
    {
        long GetLotNo();
        dynamic GetLotDetail(long EventId, Nullable<long> LotNo, Nullable<long> SchoolCode, Nullable<DateTime> ExamDate);
        List<RegNoList> RegNoList(long LotId);
        QPDispatchDetail GetQPDispatchDetail(long EventManagementId);
    }
}
