using SilverzoneERP.Entities.Models;

namespace SilverzoneERP.Data
{
    public interface IDateOptedRepository:IRepository<DateOpted>
    {
        DateOpted GetById(int id);
    }
}
