using System.Linq;
using System.Collections.Generic;
using SilverzoneERP.Context;
using SilverzoneERP.Entities.Models;

namespace SilverzoneERP.Data
{
    public class NotificationRepository : BaseRepository<Notification>, INotificationRepository
    {
        public NotificationRepository(SilverzoneERPContext context) : base(context) { }

        public Notification GetById(int id)
        {
            return _dbset.Where(x => x.Id == id).FirstOrDefault();
        }
        public IEnumerable<Notification> GetBySubject(int id)
        {
            return FindBy(x => x.SubjectId == id).AsEnumerable();
        }
        public IEnumerable<Notification> GetByStatus(bool status)
        {
            return FindBy(x => x.IsActive == status).AsEnumerable();
        }
    }
}