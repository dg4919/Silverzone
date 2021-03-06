﻿using SilverzoneERP.Context;
using SilverzoneERP.Entities.Models;
using System.Linq;

namespace SilverzoneERP.Data
{
    public class ClassRepository : BaseRepository<Class>, IClassRepository
    {
        public ClassRepository(SilverzoneERPContext dbcontext) : base(dbcontext) { }

        public dynamic GetAllClass()
        {
            return _dbset.OrderByDescending(x=>x.UpdationDate).Select(x => new { x.Id, ClassName= x.className,x.RowVersion,x.Status,x.QPStartNo });
        }
        public dynamic Get()
        {
            return _dbset.Where(x => x.Status==true).Select(x=>new { ClassId=x.Id, ClassName=x.className,});
        }
        public Class Get(long ClassId)
        {
            return _dbContext.Classes.FirstOrDefault(x => x.Id == ClassId);
        }
        public bool Exists(string ClassName)
        {
            return _dbContext.Classes.FirstOrDefault(x => x.className == ClassName) == null ? false : true;
        }
        public bool Exists(long ClasssId, string ClassName)
        {
            return _dbContext.Classes.FirstOrDefault(x => x.Id != ClasssId && x.className == ClassName) == null ? false : true;
        }
    }
}
