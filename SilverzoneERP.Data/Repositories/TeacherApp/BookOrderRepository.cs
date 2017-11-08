using SilverzoneERP.Context;
using SilverzoneERP.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SilverzoneERP.Data
{
    public class BookOrderRepository:BaseRepository<BookOrder>,IBookOrderRepository
    {
        public BookOrderRepository(SilverzoneERPContext context) : base(context) { }

        public BookOrder GetById(int id)
        {
            return _dbset.Where(x => x.Id == id).FirstOrDefault();
        }
        public BookOrder GetByDate(DateTime orderDate)
        {
            return FindBy(x => x.OrderDate == orderDate).FirstOrDefault();
        }
        public IEnumerable<BookOrder> GetByDateRange(DateTime startDate, DateTime endDate)
        {
            return FindBy(x => x.OrderDate >= startDate && x.OrderDate <= endDate).AsEnumerable();
        }
        public IEnumerable<BookOrder> GetBySchCode(string scode)
        {
            return FindBy(x => x.SchCode == scode).AsEnumerable();
        }
        public IEnumerable<BookOrder> GetByEventCode(string ecode)
        {
            return FindBy(x => x.EventCode == ecode).AsEnumerable();
        }        
        public string GenerateOrderNo()
        {
            if (_dbset.Count() > 0)
            {
                string _orderNo = _dbset.OrderByDescending(x => x.Id).Take(1).Select(x => x.OrderNo).ToList().FirstOrDefault();
                _orderNo = "OrdSzOn" + (Convert.ToInt32(_orderNo.Replace("OrdSzOn", "")) + 1);
                return _orderNo;
            }
            else
                return "OrdSzOn100000";


        }
    }
}
