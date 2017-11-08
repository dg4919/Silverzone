using SilverzoneERP.Entities.Models;

namespace SilverzoneERP.Data
{
    public interface IQPDispatchERPRepository : IRepository<QPDispatchERP>
    {
        dynamic RegNoListByLot(long LotId);
    }
}
