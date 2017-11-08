using SilverzoneERP.Entities.Models;

namespace SilverzoneERP.Data
{
    public interface IBundleRepository:IRepository<BookBundle>
    {
        BookBundle GetById(long id);
        //BookBundle GetByBookId(int id);
    }

    public interface IBundleDetailRepository : IRepository<BookBundleDetails>
    {
        BookBundleDetails GetById(int id);
        //BookBundle GetByBookId(int id);
    }
}
