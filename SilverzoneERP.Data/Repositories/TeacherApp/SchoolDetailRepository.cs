using SilverzoneERP.Context;
using SilverzoneERP.Entities.Models;
using System.Data;
using System.Linq;

namespace SilverzoneERP.Data
{
    public class SchoolDetailRepository : BaseRepository<SchoolDetail>, ISchoolDetailRepository
    {
        public SchoolDetailRepository(SilverzoneERPContext context) : base(context) { }

        public bool Exist(string SchoolCode)
        {            
            return _dbset.Any(x => x.SchCode == SchoolCode);            
        }

        public string GetSchoolName(string SchoolCode)
        {
            return _dbset.Where(x => x.SchCode == SchoolCode).FirstOrDefault().SchName;
        }
    }
}
