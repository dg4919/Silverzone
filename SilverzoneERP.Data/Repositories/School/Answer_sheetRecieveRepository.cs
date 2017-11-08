using System.Linq;
using SilverzoneERP.Context;
using SilverzoneERP.Entities.Models;

namespace SilverzoneERP.Data
{
    public class Answer_sheetRecieveRepository : BaseRepository<Answer_sheetRecieve>, IAnswer_sheetRecieveRepository
    {
        public Answer_sheetRecieveRepository(SilverzoneERPContext context) : base(context) { }

        public long get_bundleId()
        {
            var maxBundle_No = _dbset.Max(x => (long?)x.Bundle_No);
            return maxBundle_No == null ? 1 : (long)maxBundle_No;
        }

        public long getNext_bundleNo()
        {
            return _dbset.Max(x => x.Bundle_No) + 1;
        }

        public IQueryable<Answer_sheetRecieve> findBy_EventMgtId(long EventMgtId)
        {
            return _dbset.Where(x => x.EventMgtId == EventMgtId);
        }
    }

    public class Scanned_answerSheetRepository : BaseRepository<Scanned_answerSheet>, IScanned_answerSheetRepository
    {
        public Scanned_answerSheetRepository(SilverzoneERPContext context) : base(context) { }
    }
}
