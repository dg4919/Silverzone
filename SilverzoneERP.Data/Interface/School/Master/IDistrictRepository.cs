using SilverzoneERP.Entities.Models;

namespace SilverzoneERP.Data
{
    public interface IDistrictRepository:IRepository<District>
    {
        bool Exists(string DistrictName, long CountryId,long ZoneId,long StateId);
        bool Exists(long DistrictId, string DistrictName, long CountryId, long ZoneId, long StateId);
        District Get(long DistrictId);
        bool Exists(string DistrictName,string country,string zone,string state);
    }
}
