using SilverzoneERP.Context;
using SilverzoneERP.Entities.Models;
using System.Linq;

namespace SilverzoneERP.Data
{
    public class Result_L1Repository : BaseRepository<Result_L1>, IResult_L1Repository
    {
        public Result_L1Repository(SilverzoneERPContext dbcontext) : base(dbcontext) { }
    }
    
    public class Result_L2FinalRepository : BaseRepository<Result_L2Final>, IResult_L2FinalRepository
    {
        public Result_L2FinalRepository(SilverzoneERPContext dbcontext) : base(dbcontext) { }
    }

    public class ResultEventRepository : BaseRepository<ResultEvent>, IResultEventRepository
    {
        public ResultEventRepository(SilverzoneERPContext dbcontext) : base(dbcontext) { }

        public ResultEvent findBy_shortCode(string yearCode)
        {
           return _dbset.SingleOrDefault(x => x.shortCode == yearCode);
        }
        
    }

    public class ResultEvent_DetailRepository : BaseRepository<ResultEvent_Detail>, IResultEvent_DetailRepository
    {
        public ResultEvent_DetailRepository(SilverzoneERPContext dbcontext) : base(dbcontext) { }
    }

    public class Result_levelClassRepository : BaseRepository<Result_levelClass>, IResult_levelClassRepository
    {
        public Result_levelClassRepository(SilverzoneERPContext dbcontext) : base(dbcontext) { }
    }

    public class ResultRequestRepository : BaseRepository<ResultRequest>, IResultRequestRepository
    {
        public ResultRequestRepository(SilverzoneERPContext dbcontext) : base(dbcontext) { }
    }
}
