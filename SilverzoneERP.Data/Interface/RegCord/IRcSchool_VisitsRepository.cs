using SilverzoneERP.Entities.Models;

namespace SilverzoneERP.Data
{
    public interface IRcSchool_VisitsRepository : IRepository<RcSchool_Visits>
    {

    }

    public interface IRcSchool_VisitsInfoRepository : IRepository<RcSchool_VisitsInfo>
    {
        bool isAny(long rcId, long schId, long eventId);
    }
}
