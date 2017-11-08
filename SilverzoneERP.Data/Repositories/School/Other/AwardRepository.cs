using SilverzoneERP.Context;
using SilverzoneERP.Entities.Models;

namespace SilverzoneERP.Data
{
    public class AwardRepository : BaseRepository<Award>, IAwardRepository
    {
        public AwardRepository(SilverzoneERPContext context) : base(context) { }
       
    }
}
