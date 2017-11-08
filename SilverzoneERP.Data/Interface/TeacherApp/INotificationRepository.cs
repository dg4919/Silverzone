using SilverzoneERP.Entities.Models;
using System.Collections.Generic;


namespace SilverzoneERP.Data
{
    public interface INotificationRepository:IRepository<Notification>
    {
        Notification GetById(int id);
        IEnumerable<Notification> GetBySubject(int id);
        IEnumerable<Notification> GetByStatus(bool status);
    }
}
