using SilverzoneERP.Context;
using SilverzoneERP.Entities.Models;
using System.Linq;

namespace SilverzoneERP.Data
{
    public class DateOptedRepository:BaseRepository<DateOpted>,IDateOptedRepository
    {
        public DateOptedRepository(SilverzoneERPContext context) : base(context) { }

        public DateOpted GetById(int id)
        {
            return _dbset.Where(x => x.Id == id).FirstOrDefault();
        }
    }
}
