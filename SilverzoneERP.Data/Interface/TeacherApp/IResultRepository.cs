using SilverzoneERP.Entities.Models;
using System.Collections.Generic;

namespace SilverzoneERP.Data
{
    public interface IResultRepository:IRepository<Result>
    {
        Result GetById(int id);
        IEnumerable<Result> GetResultBySchoolCodeAndEventCode(string schcode, string eventCode);
        Result GetResultBySchoolCodeEventIdAndEnrollNo(string scode, string eventCode, string enroll);
    }
}
