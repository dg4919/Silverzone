using SilverzoneERP.Context;
using SilverzoneERP.Entities.Models;

namespace SilverzoneERP.Data
{
    public class MetaTagRepository : BaseRepository<MetaTag>, IMetaTagRepository
    {
        public MetaTagRepository(SilverzoneERPContext dbcontext) : base(dbcontext) { }
    }
}
