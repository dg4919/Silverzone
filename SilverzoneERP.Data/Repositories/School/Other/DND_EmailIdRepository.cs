using SilverzoneERP.Context;
using SilverzoneERP.Entities.Models;

namespace SilverzoneERP.Data
{
    public class DND_EmailRepository : BaseRepository<DND_EmailId>, IDND_EmailIdRepository
    {
        public DND_EmailRepository(SilverzoneERPContext context) : base(context) { }
       
    }
}
