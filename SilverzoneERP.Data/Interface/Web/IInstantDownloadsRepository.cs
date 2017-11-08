using SilverzoneERP.Entities.Models;

namespace SilverzoneERP.Data
{
    public interface IInstantDownloadsRepository : IRepository<InstantDownloads>
    {
        string get_orderNo(long orderNo);
        InstantDownloads findBy_OrderId(string orderId);
    }

    public interface IInstantDownload_SubjectsRepository : IRepository<InstantDownload_Subjects> { }

    public interface IPreviousYrQPRepository : IRepository<PreviousYrQP> { }

    public interface IInstantDownload_SubjectsMappingPRepository : IRepository<InstantDownload_SubjectsMapping> { }
}
