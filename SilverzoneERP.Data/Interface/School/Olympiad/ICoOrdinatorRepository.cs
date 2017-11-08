using SilverzoneERP.Entities.Models;
using SilverzoneERP.Entities.ViewModel.School;
using System.Collections.Generic;

namespace SilverzoneERP.Data
{
    public interface ICoOrdinatorRepository : IRepository<CoOrdinator>
    {
        List<CoOrdinator> GetByEventCoOrdId(long EventManagementId);
        List<CoOrdinator_AutoFill> Get_CoOrdinator_AutoFill(string CoOrdName);
    }
}
