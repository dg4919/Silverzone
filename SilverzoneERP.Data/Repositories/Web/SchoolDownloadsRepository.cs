using SilverzoneERP.Context;
using SilverzoneERP.Entities.Models;

namespace SilverzoneERP.Data
{
    public class SchoolDownloadsRepository : BaseRepository<SchoolDownloads>, ISchoolDownloadsRepository
    {
        public SchoolDownloadsRepository(SilverzoneERPContext context) : base(context) { }

    }
}
