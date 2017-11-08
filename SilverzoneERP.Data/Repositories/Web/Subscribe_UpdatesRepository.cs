using SilverzoneERP.Context;
using SilverzoneERP.Entities.Models;

namespace SilverzoneERP.Data
{
    public class Subscribe_UpdatesRepository : BaseRepository<Subscribe_Updates>, ISubscribe_UpdatesRepository
    {
        public Subscribe_UpdatesRepository(SilverzoneERPContext dbcontext) : base(dbcontext) { }    
    }    
}
