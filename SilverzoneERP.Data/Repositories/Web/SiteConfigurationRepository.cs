using SilverzoneERP.Context;
using SilverzoneERP.Entities.Models;
using System.Linq;

namespace SilverzoneERP.Data
{
    public class SiteConfigurationRepository : BaseRepository<SiteConfiguration>, ISiteConfigurationRepository
    {
        public SiteConfigurationRepository(SilverzoneERPContext dbcontext) : base(dbcontext) { }

        public SiteConfiguration GetRecord()
        {
            return GetAll().SingleOrDefault();
        }
    }
}
