using SilverzoneERP.Entities.Models;

namespace SilverzoneERP.Data
{
    public interface ISchoolDetailRepository:IRepository<SchoolDetail>
    {
        bool Exist(string SchoolCode);
        string GetSchoolName(string SchoolCode);
    }
}
