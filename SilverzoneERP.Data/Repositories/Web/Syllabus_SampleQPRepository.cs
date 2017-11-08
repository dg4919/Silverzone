using SilverzoneERP.Context;
using SilverzoneERP.Entities.Models;

namespace SilverzoneERP.Data
{
    public class Syllabus_SampleQPRepository : BaseRepository<Syllabus_SampleQP>, ISyllabus_SampleQPRepository
    {
        public Syllabus_SampleQPRepository(SilverzoneERPContext context) : base(context) { }

    }
}
