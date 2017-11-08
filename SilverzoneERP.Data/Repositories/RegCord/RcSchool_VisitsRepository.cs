using SilverzoneERP.Context;
using SilverzoneERP.Entities.Models;
using System.Linq;

namespace SilverzoneERP.Data
{
    class RcSchool_VisitsRepository : BaseRepository<RcSchool_Visits>, IRcSchool_VisitsRepository
    {
        public RcSchool_VisitsRepository(SilverzoneERPContext context) : base(context) { }
    }


    class RcSchool_VisitsInfoRepository : BaseRepository<RcSchool_VisitsInfo>, IRcSchool_VisitsInfoRepository
    {
        public RcSchool_VisitsInfoRepository(SilverzoneERPContext context) : base(context) { }

        public bool isAny(long rcId, long schId, long eventId)
        {
            return _dbset.Any(x => x.RcSchool_Visit.RCId == rcId
                                && x.RcSchool_Visit.SchoolId == schId
                                && x.EventId == eventId);
        }
    }
}
