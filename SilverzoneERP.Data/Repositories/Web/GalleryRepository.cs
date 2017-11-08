using SilverzoneERP.Entities.Models;
using SilverzoneERP.Context;
using System.Collections.Generic;
using SilverzoneERP.Entities.Constant;
using System.Web;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace SilverzoneERP.Data
{
    public class GalleryImageRepository : BaseRepository<GalleryImage>, IGalleryImageRepository
    {
        public GalleryImageRepository(SilverzoneERPContext dbcontext) : base(dbcontext) { }

        public List<string> upload_gallery_Image_toTemp(string tempPath)
        {
            return ClassUtility.upload_orignal_Images_toTemp(tempPath);
        }
       
        public void save_Image_fromTemp(IEnumerable<string> imageName, string tempPath, string finalPath)
        {
            ClassUtility.save_Images_toPhysical(imageName, tempPath, finalPath);
        }

        // override method not need to specify in interface
        public override IQueryable<GalleryImage> GetAll()
        {
            return _dbset.Where(x => x.CategoryId != 10
                                  && x.CategoryId != 3
                                  && x.Status == true);
        }

        public IQueryable<GalleryImage> finadAll()
        {
            return base.GetAll();
        }
    }

    public class GalleryImageDetailRepository : BaseRepository<GalleryImageDetail>, IGalleryImageDetailRepository
    {
        public GalleryImageDetailRepository(SilverzoneERPContext dbcontext) : base(dbcontext) { }

        public void copyImage_toTemp(string imgName)
        {
            string finalPath = HttpContext.Current.Server.MapPath(string.Format("~/{0}{1}",
                                                                image_urlResolver.galleryImg_main,
                                                                imgName));

            string tempPath = HttpContext.Current.Server.MapPath("~/" + image_urlResolver.galleryImg_temp);
            if (!Directory.Exists(tempPath))
                Directory.CreateDirectory(tempPath);

            File.Copy(finalPath, (tempPath + imgName));
        }

        public void rotateImag(string imgName, bool isNew)
        {
            string fullPath = HttpContext.Current.Server.MapPath(string.Format("~/{0}{1}",
                                                                isNew ? image_urlResolver.galleryImg_temp
                                                                      : image_urlResolver.galleryImg_main,
                                                                 imgName));

            var Img = Image.FromFile(fullPath);
            Img.RotateFlip(RotateFlipType.Rotate90FlipNone);

            Img.Save(fullPath, ImageFormat.Jpeg);
        }

        public void removeImag(string imgName)
        {
            string path = HttpContext.Current.Server.MapPath(string.Format("~/{0}{1}",
                                                              image_urlResolver.galleryImg_main,
                                                              imgName));

            File.Delete(path);
        }

    }

    public class GalleryImageCategoryRepository : BaseRepository<GalleryImageCategory>, IGalleryImageCategoryRepository
    {
        public GalleryImageCategoryRepository(SilverzoneERPContext dbcontext) : base(dbcontext) { }
    }
}
