using SilverzoneERP.Entities;
using SilverzoneERP.Entities.Models;

namespace SilverzoneERP.Data
{
    public interface IDND_MobileNoRepository : IRepository<DND_MobileNo>
    {
        dynamic Get(string Search, long StartIndex, long Limit, DNDType DNDType, out long Count);        
    }
}
