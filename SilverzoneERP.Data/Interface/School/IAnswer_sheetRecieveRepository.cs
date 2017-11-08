using SilverzoneERP.Entities.Models;
using System.Linq;

namespace SilverzoneERP.Data
{
    public interface IAnswer_sheetRecieveRepository : IRepository<Answer_sheetRecieve>
    {
        long get_bundleId();
        long getNext_bundleNo();
        IQueryable<Answer_sheetRecieve> findBy_EventMgtId(long EventMgtId);
    }

    public interface IScanned_answerSheetRepository : IRepository<Scanned_answerSheet>
    {
    }
}
