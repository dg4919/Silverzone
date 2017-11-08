using System;
using System.Linq;
using SilverzoneERP.Context;
using SilverzoneERP.Entities.Models;

namespace SilverzoneERP.Data
{
    public class RemarksRepository : BaseRepository<Remarks>, IRemarksRepository
    {
        public RemarksRepository(SilverzoneERPContext context) : base(context) { }

        public dynamic GetAll(Nullable<long> UserId,string OrderNo)
        {            
            if(!string.IsNullOrWhiteSpace(OrderNo))

            return _dbset.Where(x=>x.OrderNo==OrderNo).OrderByDescending(x=>x.UpdationDate).ToList().Select(x=>new {
                x.Description,
                x.CreationDate,
                EntryBy=GetUserName(x.CreatedBy)
            });
            else
                return _dbset.Where(x =>x.UserId == UserId).OrderByDescending(x => x.UpdationDate).ToList().Select(x => new {
                    x.Description,
                    x.CreationDate,
                    EntryBy = GetUserName(x.CreatedBy)
                });
        }

        private string GetUserName(long UserId)
        {
            return _dbContext.ERPusers.FirstOrDefault(x => x.Id == UserId).UserName;
        }
    }
}
