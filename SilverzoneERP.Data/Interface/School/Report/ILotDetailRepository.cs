using SilverzoneERP.Entities.Models;

namespace SilverzoneERP.Data
{
    public interface ILotDetailRepository : IRepository<LotDetail>
    {
        bool Exist(long ObjectId, int ObjectType);
    }
}
