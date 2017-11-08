using SilverzoneERP.Entities.Models;

namespace SilverzoneERP.Data
{
    public interface ISiteConfigurationRepository : IRepository<SiteConfiguration>
    {
        SiteConfiguration GetRecord();
    }
}
