using SilverzoneERP.Entities.Models.Common;

namespace SilverzoneERP.Entities.Models
{
    public  class MetaTag:Entity<long>
    {
        public string Title { get; set; }
        public string Link { get; set;}
        public string Description { get; set; }
        public string Keyword { get; set; }
    }
}
