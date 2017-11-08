using SilverzoneERP.Context;
using SilverzoneERP.Entities.Models;

namespace SilverzoneERP.Data
{
    public class TeacherDetailRepository : BaseRepository<TeacherDetail>, ITeacherDetailRepository
    {
        public TeacherDetailRepository(SilverzoneERPContext dbcontext) : base(dbcontext) { }
       
    }
}
