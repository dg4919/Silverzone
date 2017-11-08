using SilverzoneERP.Context;
using SilverzoneERP.Entities.Models;
using System.Linq;
namespace SilverzoneERP.Data
{
    public class BundleRepository:BaseRepository<BookBundle>,IBundleRepository
    {
        public BundleRepository(SilverzoneERPContext dbcontext) : base(dbcontext) { }

        public BookBundle GetById(long id)
        {
            return _dbset.Where(x => x.Id == id).FirstOrDefault();
        }

        //public BookBundle GetByBookId(int id)
        //{
        //    return FindBy(x => x.BookId == id).FirstOrDefault();
        //}
    }

    public class BundleDetailRepository : BaseRepository<BookBundleDetails>, IBundleDetailRepository
    {
        public BundleDetailRepository(SilverzoneERPContext dbcontext) : base(dbcontext) { }

        public BookBundleDetails GetById(int id)
        {
            return _dbset.Where(x => x.Id == id).FirstOrDefault();
        }
    }

}
