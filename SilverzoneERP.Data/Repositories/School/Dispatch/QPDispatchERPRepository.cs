using SilverzoneERP.Context;
using SilverzoneERP.Entities.Models;
using System.Linq;

namespace SilverzoneERP.Data
{
    public class QPDispatchERPRepository : BaseRepository<QPDispatchERP>, IQPDispatchERPRepository
    {
        public QPDispatchERPRepository(SilverzoneERPContext context) : base(context) { }

        public dynamic RegNoListByLot(long LotId) {
            var data =_dbset.Where(x => x.LotId == LotId).Select(x => new { x.Id,x.EventManagementId,x.EventManagement.RegNo,x.JSONData }).ToList();
            return data;
        }
    }
}
