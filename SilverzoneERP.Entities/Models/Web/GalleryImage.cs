using SilverzoneERP.Entities.Models.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SilverzoneERP.Entities.Models
{
    public class GalleryImageCategory : AuditableEntity<int>
    {
        public string Name { get; set; }
    }

    public class GalleryImage : AuditableEntity<long>
    {
        public string ImageUrl { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public virtual GalleryImageCategory GalleryImageType { get; set; }
        public int year { get; set; }
        public int? galleryOrder { get; set; }

        public virtual ICollection<GalleryImageDetail> GalleryImageDetails { get; set; }
    }

    public class GalleryImageDetail : Entity<long>
    {
        public long GalleryImageId { get; set; }

        [ForeignKey("GalleryImageId")]
        public virtual GalleryImage GalleryImage { get; set; }
        public string ImageUrl { get; set; }
    }

}
