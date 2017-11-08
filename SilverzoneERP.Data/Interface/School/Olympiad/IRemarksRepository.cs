using SilverzoneERP.Entities.Models;
using System;

namespace SilverzoneERP.Data
{
    public interface IRemarksRepository : IRepository<Remarks>
    {
        dynamic GetAll(Nullable<long> UserId, string OrderNo);
    }
}
