using SilverzoneERP.Context;
using SilverzoneERP.Entities.Models;
using System.Linq;

namespace SilverzoneERP.Data
{
    public class LotDetailRepository : BaseRepository<LotDetail>, ILotDetailRepository
    {
        public LotDetailRepository(SilverzoneERPContext context) : base(context) { }

        public bool Exist(long ObjectId, int ObjectType)
        {
            return _dbset.Any(x=>x.Objectid==ObjectId && x.ObjectType==ObjectType);
        }       
    }
}
