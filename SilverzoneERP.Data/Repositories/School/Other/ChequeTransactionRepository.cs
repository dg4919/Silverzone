using SilverzoneERP.Context;
using SilverzoneERP.Entities.Models;

namespace SilverzoneERP.Data
{
    public class ChequeTransactionRepository : BaseRepository<ChequeTransaction>, IChequeTransactionRepository
    {
        public ChequeTransactionRepository(SilverzoneERPContext context) : base(context) { }
       
    }
}
