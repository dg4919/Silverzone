using SilverzoneERP.Entities.Models.Common;

namespace SilverzoneERP.Entities.Models
{
    public class NewsUpdates : AuditableEntity<long>
    {
        public string Title { get; set; }
        public string NewsUrl { get; set; }

        public string PageUrl { get; set; }
    }
}
