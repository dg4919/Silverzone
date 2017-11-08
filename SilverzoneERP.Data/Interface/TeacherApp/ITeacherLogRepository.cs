using SilverzoneERP.Entities.Models;
using System;
using System.Collections.Generic;

namespace SilverzoneERP.Data
{
    public interface ITeacherLogRepository:IRepository<TeacherLog>
    {
        TeacherLog GetById(int id);
        IEnumerable<TeacherLog> GetByUserId(int userid);
       // IEnumerable<TeacherLog> GetByAction(int id);
        IEnumerable<TeacherLog> GetByActionDate(DateTime adate);
    }
}
