using SilverzoneERP.Context;
using SilverzoneERP.Entities.Models;
using System.Collections.Generic;
using System.Linq;

namespace SilverzoneERP.Data.Repositories
{
    public class ResultRepository:BaseRepository<Result>,IResultRepository
    {
        public ResultRepository(SilverzoneERPContext dbcontext) : base(dbcontext) { }
        public Result GetById(int id)
        {
            return _dbset.Where(x => x.Id == id).FirstOrDefault();
        }
        public IEnumerable<Result> GetResultBySchoolCodeAndEventCode(string schcode, string eventCode)
        {
            return _dbset.Where(x => x.SchCode == schcode && x.EventCode == eventCode).AsEnumerable();
        }
        public Result GetResultBySchoolCodeEventIdAndEnrollNo(string scode, string eventCode, string enroll)
        {
            return _dbset.Where(x => x.SchCode == scode && x.EventCode == eventCode && x.NIORollNo == enroll).FirstOrDefault();
        }
    }
}
