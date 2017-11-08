using SilverzoneERP.Context;
using SilverzoneERP.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SilverzoneERP.Data
{
    public class TeacherLogRepository:BaseRepository<TeacherLog>,ITeacherLogRepository
    {
        public TeacherLogRepository(SilverzoneERPContext context) : base(context) { }
        public TeacherLog GetById(int id)
        {
            return _dbset.Where(x => x.Id == id).SingleOrDefault();
        }
        public IEnumerable<TeacherLog> GetByUserId(int userid)
        {
            return FindBy(x => x.UserId == userid).AsEnumerable();
        }
       
        //public IEnumerable<TeacherLog> GetByAction(int id)
        //{
        //    return FindBy(x => x.ActionId == id).AsEnumerable();
        //}
        public IEnumerable<TeacherLog> GetByActionDate(DateTime ddate)
        {
            return FindBy(x => x.ActionDate == ddate).AsEnumerable();
        }
    }
}
