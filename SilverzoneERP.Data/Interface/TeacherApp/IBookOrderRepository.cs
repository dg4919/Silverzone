
using SilverzoneERP.Entities.Models;
using System;
using System.Collections.Generic;

namespace SilverzoneERP.Data
{
    public interface IBookOrderRepository:IRepository<BookOrder>
    {
        BookOrder GetById(int id);
        BookOrder GetByDate(DateTime orderDate);
        IEnumerable<BookOrder> GetByDateRange(DateTime startDate,DateTime endDate);
        IEnumerable<BookOrder> GetBySchCode(string scode);
        IEnumerable<BookOrder> GetByEventCode(string ecode);       
        string GenerateOrderNo();
    }
}
