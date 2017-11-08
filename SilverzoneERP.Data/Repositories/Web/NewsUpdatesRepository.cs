using SilverzoneERP.Context;
using SilverzoneERP.Entities.Models;

namespace SilverzoneERP.Data
{
    public class NewsUpdatesRepository : BaseRepository<NewsUpdates>, INewsUpdatesRepository
    {
        public NewsUpdatesRepository(SilverzoneERPContext dbcontext) : base(dbcontext) { }
    }
}
