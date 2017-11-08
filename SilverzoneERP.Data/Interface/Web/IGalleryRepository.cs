using SilverzoneERP.Entities.Models;
using System.Collections.Generic;
using System.Linq;

namespace SilverzoneERP.Data
{
    public interface IGalleryImageRepository : IRepository<GalleryImage> {
        List<string> upload_gallery_Image_toTemp(string tempPath);

        void save_Image_fromTemp(IEnumerable<string> imageName, string tempPath, string finalPath);
        IQueryable<GalleryImage> finadAll();
    }

    public interface IGalleryImageDetailRepository : IRepository<GalleryImageDetail> {
        void copyImage_toTemp(string imgName);

        void rotateImag(string imgName, bool isNew);
        void removeImag(string imgName);
    }

    public interface IGalleryImageCategoryRepository : IRepository<GalleryImageCategory> { }
}
