using SilverzoneERP.Context;
using SilverzoneERP.Entities.Models;

namespace SilverzoneERP.Data
{
    public class ExcellenceAwardRepository : BaseRepository<ExcellenceAward>, IExcellenceAwardRepository
    {
        public ExcellenceAwardRepository(SilverzoneERPContext context) : base(context) { }
       
    }
}
