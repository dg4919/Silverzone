using SilverzoneERP.Context;
using SilverzoneERP.Entities.Models;

namespace SilverzoneERP.Data
{
    class RcSchoolsRepository : BaseRepository<RcSchools>, IRcSchoolsRepository
    {
        public RcSchoolsRepository(SilverzoneERPContext context) : base(context) { }

    }
}
