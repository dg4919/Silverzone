using SilverzoneERP.Entities.Models;

namespace SilverzoneERP.Data
{
    public interface IEventYearRepository : IRepository<EventYear>
    {
        EventYear Get(long EventYearId);      
        bool Exists(long EventId, string EventYear);
        bool Exists(long EventYearId, long EventId, string EventYear);
        dynamic Get(bool IsStatus = false);
        dynamic Get(string EventYear, bool Status, string EventCode = null);
    }    
}
