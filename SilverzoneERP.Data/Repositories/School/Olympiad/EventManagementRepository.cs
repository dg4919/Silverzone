using System.Collections.Generic;
using System;
using System.Linq;
using SilverzoneERP.Entities.Models;
using SilverzoneERP.Context;
using SilverzoneERP.Entities;

namespace SilverzoneERP.Data
{
    public class EventManagementRepository : BaseRepository<EventManagement>, IEventManagementRepository
    {
        public EventManagementRepository(SilverzoneERPContext context) : base(context) { }

        public List<EventManagement> GetBySchoolId(long SchoolId)
        {
            try
            {
                return _dbContext.EventManagements.Where(x => x.SchId == SchoolId && x.EventManagementYear == ServerKey.Event_Current_Year).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool Exists(long SchoolId)
        {
            return _dbset.Any(x => x.SchId == SchoolId && x.EventManagementYear == ServerKey.Event_Current_Year);
        }
        public EventManagement Get(long Id)
        {
            return _dbset.FirstOrDefault(x => x.Id == Id);
        }

        public dynamic Get_FavourOf()
        {
            var data= _dbContext.InFavourOfs.Where(x => x.Status == true).ToList()
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    DepositOnBanks = x.DepositOnBank.Where(d => d.Status == true).ToList().Select(d => new
                    {
                        d.Id,
                        d.BankName,
                        d.AccountNo
                    })
                });
            return data;
        }
        public dynamic Get_DrawnOnBank()
        {
            var data = _dbContext.DrownOnBanks.Where(x => x.Status == true).ToList()
                .Select(x => new
                {
                    x.Id,
                    x.BankName                    
                });
            return data;
        }

        public dynamic AllOrderBySchool(long EventManagementId)
        {
            var data = _dbContext.PurchaseOrder_Masters.Where(x => x.From == orderSourceType.School && x.srcFrom == EventManagementId && x.To == orderSourceType.Silverzone && x.PurchaseOrders.Count(p => p.Status == true) != 0).OrderByDescending(x => x.PO_Date).Select(x => new {
                x.Id,
                x.PO_Number,
                x.PO_Date,
                x.isVerified,
                Order = x.PurchaseOrders.Where(p => p.Status == true).Select(p => new {
                    p.Id,
                    p.PO_mId,
                    p.Book.ItemTitle_Master.ClassId,
                    ClassName = p.Book.ItemTitle_Master.Class.className,
                    p.BookId,
                    BookName = p.Book.ItemTitle_Master.BookCategory.Name,
                    p.Quantity,
                    p.Rate,
                    p.Status
                })
            });
            return data;
        }

        public dynamic GetAllBook(long SubjectId,ref long CompanyId)
        {
            var data = _dbContext.Books.Where(x => x.Status == true && x.ItemTitle_Master.SubjectId == SubjectId).Select(x => new {
                BookId = x.Id,
                x.ItemTitle_Master.ClassId,
                x.ItemTitle_Master.SubjectId,
                x.ItemTitle_Master.CategoryId,
                CategoryName = x.ItemTitle_Master.BookCategory.Name,
            }).ToList();
            CompanyId =_dbContext.InventorySourceDetails.Where(x => x.SourceName.Trim().ToLower().Equals("silverzone foundation") && x.SourceId == 7).FirstOrDefault().Id;
            return data;
        }
    }
}
