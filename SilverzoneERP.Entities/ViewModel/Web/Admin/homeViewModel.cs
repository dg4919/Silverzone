using SilverzoneERP.Entities.Constant;
using SilverzoneERP.Entities.Models;
using System.Collections.Generic;
using System.Linq;

// bcoz of same view model name confliction we r seprating each into its respective namespace
namespace SilverzoneERP.Entities.ViewModel.Admin
{
    public class galleryImgViewModel
    {
        public string fileType { get; set; }
        public int imgWidth { get; set; }
        public int imgHeight { get; set; }

        public static dynamic parse(List<GalleryImage> galleryImg_list)
        {
            return new
            {
                max_orderNum = galleryImg_list.Max(x => x.galleryOrder) == null ? 1 : galleryImg_list.Max(x => x.galleryOrder) + 1,
                galleryInfo = parse(galleryImg_list
                             .Where(x => x.CategoryId != 10
                                      && x.CategoryId != 3)),
                hallOf_fame = parse(galleryImg_list
                             .Where(x => x.CategoryId == 3)),
                media = parse(galleryImg_list.Where(x => x.CategoryId == 10))
            };
        }

        // by defualt private
        static dynamic parse(IEnumerable<GalleryImage> result)
        {
            return result.Select(x => new
            {
                x.Id,
                categoryType = x.CategoryId,
                category = x.GalleryImageType.Name,
                x.year,
                x.Description,
                ImageUrl = image_urlResolver.getGalleryImage(x.ImageUrl),
                hasOrder_number = x.galleryOrder == null ? false : true,
                status = x.Status
            });
        }

    }

    public class galleryImageViewModel
    {
        public long Id { get; set; }
        public string ImageUrl { get; set; }
        public string Description { get; set; }
        public int categoryType { get; set; }
        public int year { get; set; }

        public static GalleryImage Parse(galleryImageViewModel vm)
        {
            return new GalleryImage()
            {
                Description = vm.Description,
                CategoryId = vm.categoryType,
                year = vm.year,
                ImageUrl = vm.ImageUrl,
                Status = true
            };
        }

        // class is a refrence type so direct change into the onject pass as argument
        public static void Parse(galleryImageViewModel vm, GalleryImage model)
        {
            model.Description = vm.Description;
            model.CategoryId = vm.categoryType;
            model.year = vm.year;
            model.ImageUrl = vm.ImageUrl;
        }
    }

    public class galleryImage_detailViewModel
    {
        public long Id { get; set; }
        public ICollection<imageGallery_ViewModel> imageGallerys { get; set; }

        public static GalleryImageDetail parse(
            long Id,
            string imageName)
        {
            return new GalleryImageDetail()
            {
                GalleryImageId = Id,
                ImageUrl = imageName,
                Status = true
            };
        }
    }

    public class imageGallery_ViewModel
    {
        public string ImageUrls { get; set; }
        public bool isNew { get; set; }
    }

    public class galleryOrder_ViewModel
    {
        public long galleryId { get; set; }
        public int order { get; set; }
    }

    public class newsUpdates_ViewModel
    {
        public long Id { get; set; }
        public string title { get; set; }
        public string news_url { get; set; }
        public string page_url { get; set; }

        public static void parse(newsUpdates_ViewModel vm, NewsUpdates model)
        {
            model.Title = vm.title;
            model.NewsUrl = vm.news_url;
            model.PageUrl = vm.page_url;
            model.Status = true;
        }

    }

    public class InstantDnd_mapping_ViewModel
    {
        public long yearId { get; set; }
        public ICollection<InstantDnd_SubjectsMapping_ViewModel> mappingInfo { get; set; }

        public static InstantDownload_SubjectsMapping parse(
            long yearId,
            long subjectId,
            long classId)
        {
            return new InstantDownload_SubjectsMapping()
            {
                ClassId = classId,
                SubjectId = subjectId,
                YearId = yearId,
                Status = true,
            };
        }

    }

    public class InstantDnd_SubjectsMapping_ViewModel
    {
        public long classId { get; set; }
        public long[] subjectIds { get; set; }
    }

    public class CarrierViewModel
    {
        public long Id { get; set; }
        public string JobTitle { get; set; }
        public int Vacancies { get; set; }
        public string Qualification { get; set; }
        public string Skills { get; set; }
        public string Experience { get; set; }

        public bool isDelete { get; set; }      // for change status > by defualt is false

        public static void parse(CarrierViewModel vm, CarrierMaster model)
        {
            model.JobTitle = vm.JobTitle;
            model.Skills = vm.Skills;
            model.Vacancies = vm.Vacancies;
            model.Qualification = vm.Qualification;
            model.Experience = vm.Experience;
            model.Status = true;
        }
    }
}