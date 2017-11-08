using SilverzoneERP.Context;
using SilverzoneERP.Entities.Models;
using System.Linq;

namespace SilverzoneERP.Data
{
    public class InstantDownloadsRepository : BaseRepository<InstantDownloads>, IInstantDownloadsRepository
    {
        public InstantDownloadsRepository(SilverzoneERPContext dbcontext) : base(dbcontext) { }

        public string get_orderNo(long orderNo)
        {
            return string.Format("{0}{1}",
                        "ORD/InstantDnd/",
                        100000 + orderNo);
        }

        public InstantDownloads findBy_OrderId(string orderId)
        {
            return _dbset.SingleOrDefault(x => x.OrderId.Contains(orderId));
        }
    }

    public class InstantDownload_SubjectsRepository : BaseRepository<InstantDownload_Subjects>, IInstantDownload_SubjectsRepository
    {
        public InstantDownload_SubjectsRepository(SilverzoneERPContext dbcontext) : base(dbcontext) { }
    }

    public class PreviousYrQPRepository : BaseRepository<PreviousYrQP>, IPreviousYrQPRepository
    {
        public PreviousYrQPRepository(SilverzoneERPContext dbcontext) : base(dbcontext) { }
    }

    public class InstantDownload_SubjectsMappingPRepository : BaseRepository<InstantDownload_SubjectsMapping>, IInstantDownload_SubjectsMappingPRepository
    {
        public InstantDownload_SubjectsMappingPRepository(SilverzoneERPContext dbcontext) : base(dbcontext) { }
    }
}
