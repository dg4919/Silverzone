using SilverzoneERP.Entities.Models;

namespace SilverzoneERP.Data
{
    public interface IResult_L1Repository : IRepository<Result_L1> { }

    public interface IResult_L2FinalRepository : IRepository<Result_L2Final> { }

    public interface IResultEventRepository : IRepository<ResultEvent> {
        ResultEvent findBy_shortCode(string yearCode);
    }

    public interface IResultEvent_DetailRepository : IRepository<ResultEvent_Detail> { }

    public interface IResult_levelClassRepository : IRepository<Result_levelClass>
    {
    }

    public interface IResultRequestRepository : IRepository<ResultRequest>
    {
    }
}
