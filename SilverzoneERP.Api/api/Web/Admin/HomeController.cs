using SilverzoneERP.Data;
using SilverzoneERP.Entities.Constant;
using SilverzoneERP.Entities.Models;
using SilverzoneERP.Entities.ViewModel.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace SilverzoneERP.Api.api.Web.Admin
{
    public class HomeController : ApiController
    {
        IMetaTagRepository metaTagRepository { get; set; }
        IGalleryImageCategoryRepository galleryImageCategoryRepository { get; set; }
        IGalleryImageRepository galleryImageRepository { get; set; }
        IGalleryImageDetailRepository galleryImageDetailRepository { get; set; }
        IErrorLogsRepository errorLogsRepository { get; set; }
        INewsUpdatesRepository newsUpdatesRepository { get; set; }
        ISiteConfigurationRepository siteConfigurationRepository { get; set; }
        private IPreviousYrQPRepository previousYrQPRepository { get; set; }
        private IClassRepository classRepository { get; set; }
        private IEventRepository eventRepository { get; set; }
        private IInstantDownload_SubjectsMappingPRepository instantDownload_SubjectsMappingPRepository { get; set; }
        private ICarrierMasterRepository carrierMasterRepository { get; set; }
        private ISchedule_OlympiadsRepository schedule_OlympiadsRepository { get; set; }

        public IHttpActionResult get_allMetaTagList()
        {
            var res = metaTagRepository.GetAll();

            return Ok(new { result = res });
        }

        [HttpPost]
        public IHttpActionResult save_metaTag(MetaTag model)
        {
            if (metaTagRepository.FindBy(x => x.Id != model.Id
                                           && x.Link == model.Link
                                       ).Any())
                return Ok(new { result = "exist" });

            if (model.Id == 0)                      // create
            {
                model.Status = true;
                metaTagRepository.Create(model);
            }
            else
                metaTagRepository.Update(model);    // update

            return Ok(new { result = "ok" });
        }

        [HttpPost]
        public IHttpActionResult delete_metaTag(long Id, bool status)
        {
            var metaTag = metaTagRepository.FindBy(x => x.Id == Id).SingleOrDefault();

            if (metaTag == null)
                return Ok(new { result = "notfound" });

            metaTag.Status = status;
            metaTagRepository.Update(metaTag);    // update

            return Ok(new { result = "ok" });
        }

        //***********************  Gallery  *****************************

        public IHttpActionResult get_galleryCategory()
        {
            var res = galleryImageCategoryRepository.FindBy(x => x.Status == true)
                     .Select(x => new
                     {
                         x.Id,
                         x.Name
                     });

            return Ok(new { result = res });
        }

        public IHttpActionResult get_gallerys()
        {
            var res = galleryImageRepository
                     .finadAll()
                     .OrderBy(x => x.galleryOrder == null)    /*-  first perform order on values after nullable list -*/
                     .ThenBy(x => x.galleryOrder)
                     .ToList();

            return Ok(new
            {
                result = galleryImgViewModel.parse(res)
            });
        }

        [HttpPost]
        public IHttpActionResult uploadGallaryImg()
        {
            var save_Imagespath = galleryImageRepository
                                 .upload_gallery_Image_toTemp(image_urlResolver.galleryImg_temp);

            return Ok(save_Imagespath);
        }

        [HttpPost]
        public IHttpActionResult upload_Schedule_olympadsImg()
        {
            var save_Imagespath = schedule_OlympiadsRepository
                                 .uploadImage_toTemp(image_urlResolver.SOL_temp);

            return Ok(save_Imagespath);
        }


        [HttpPost]
        public IHttpActionResult save_Schedule_olympadsImg(Schedule_Olympiads model)
        {
            model.ImageName = image_urlResolver.parseImage(model.ImageName);

            if (model.Id != 0)
                schedule_OlympiadsRepository
               .Update(model);
            else
            {
                model.Status = true;

                schedule_OlympiadsRepository
               .Create(model);
            }

            schedule_OlympiadsRepository
                .save_Image_fromTemp(
                    new string[] { model.ImageName },
                    image_urlResolver.SOL_temp,
                    image_urlResolver.SOL_main);

            return Ok();
        }

        public IHttpActionResult get_Schedule_olympads()
        {
            var res = schedule_OlympiadsRepository
                .GetAll()
                .ToList()
                .Select(x => new
                {
                    x.Id,
                    ImageName = image_urlResolver.getSOFImage(x.ImageName),
                    x.Caption,
                    x.Link,
                    x.Status
                });

            return Ok(new { result = res });
        }


        [HttpGet]
        public IHttpActionResult changeSts_Gallary(long Id, bool status)
        {
            var gallery = galleryImageRepository.FindById(Id);
            if (gallery == null)
                return Ok(new { result = "notfound" });

            gallery.Status = status;
            galleryImageRepository.Update(gallery);

            return Ok();
        }

        [HttpPost]
        public IHttpActionResult setGallery_Order(List<galleryOrder_ViewModel> model)
        {
            using (var transaction = galleryImageRepository.BeginTransaction())
            {
                try
                {
                    foreach (var gallery in model)
                    {
                        var galleryInfo = galleryImageRepository.FindById(gallery.galleryId);
                        galleryInfo.galleryOrder = gallery.order;

                        galleryImageRepository.Update(galleryInfo, false);
                    }

                    galleryImageRepository.Save();
                    transaction.Commit();
                    return Ok(new { result = "ok" });
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    errorLogsRepository.logError(ex);
                    return Ok(new { result = "error" });
                }
            }
        }

        [HttpGet]
        public IHttpActionResult rotateGallaryImg(string imagName, bool isNew)
        {
            galleryImageDetailRepository
                .rotateImag(image_urlResolver.parseImage(imagName), isNew);

            return Ok();
        }

        [HttpGet]
        public IHttpActionResult removeGallaryImg(string imagName)
        {
            string Img = image_urlResolver.parseImage(imagName);

            var galleryDetail = galleryImageDetailRepository
                               .FindBy(x => x.ImageUrl == Img)
                               .SingleOrDefault();

            if (galleryDetail == null)
                return Ok(new { result = "notfound" });

            galleryImageDetailRepository.Delete(galleryDetail);

            galleryImageDetailRepository
                .removeImag(Img);

            return Ok();
        }


        [HttpPost]
        public IHttpActionResult saveGallery(galleryImageViewModel model)
        {
            model.ImageUrl = image_urlResolver.parseImage(model.ImageUrl);
            if (model.Id != 0)
            {
                var gallery = galleryImageRepository.FindById(model.Id);

                if (gallery.ImageUrl != model.ImageUrl)
                    galleryImageRepository.save_Image_fromTemp(new string[] { model.ImageUrl },
                    image_urlResolver.galleryImg_temp,
                    image_urlResolver.galleryImg_main);

                galleryImageViewModel.Parse(model, gallery);
                galleryImageRepository.Update(gallery);
            }
            else
            {
                galleryImageRepository.Create(galleryImageViewModel.Parse(model));

                galleryImageRepository.save_Image_fromTemp(new string[] { model.ImageUrl },
                    image_urlResolver.galleryImg_temp,
                    image_urlResolver.galleryImg_main);
            }
            return Ok();
        }

        [HttpPost]
        public IHttpActionResult save_galleryDetail(galleryImage_detailViewModel model)
        {
            // isNew to check user upload a new image or image existed already
            foreach (var dt in model.imageGallerys)
            {
                if (dt.isNew)
                    galleryImageDetailRepository.Create(
                        galleryImage_detailViewModel
                       .parse(model.Id, image_urlResolver.parseImage(dt.ImageUrls)),
                              false);
            }
            galleryImageDetailRepository.Save();

            var ImageUrls = model.imageGallerys
                                 .Where(x => x.isNew == true)
                                 .Select(x => x.ImageUrls);

            galleryImageRepository.save_Image_fromTemp(ImageUrls,
                image_urlResolver.galleryImg_temp,
                image_urlResolver.galleryImg_main);

            return Ok();
        }

        public IHttpActionResult get_galleryDetail(long gallaryId)
        {
            var res = galleryImageDetailRepository
                     .FindBy(x => x.GalleryImageId == gallaryId
                               && x.Status == true)
                     .ToList()
                     .Select(x => new
                     {
                         x.Id,
                         ImageUrl = image_urlResolver.getGalleryImage(x.ImageUrl),
                         isNew = false
                     });

            return Ok(new { result = res });
        }

        [HttpPost]
        public IHttpActionResult save_newsUpdates(newsUpdates_ViewModel model)
        {
            if (model.Id == 0)
            {
                var entity = new NewsUpdates();
                newsUpdates_ViewModel.parse(model, entity);
                newsUpdatesRepository.Create(entity);
            }
            else
            {
                var entity = newsUpdatesRepository.FindById(model.Id);
                newsUpdates_ViewModel.parse(model, entity);
                newsUpdatesRepository.Update(entity);
            }

            return Ok();
        }

        [HttpGet]
        public IHttpActionResult delete_newsUpdates(long newsId)
        {
            var entity = newsUpdatesRepository.FindById(newsId);
            entity.Status = !entity.Status;
            newsUpdatesRepository.Update(entity);

            return Ok();
        }

        public IHttpActionResult get_newsUpdates()
        {
            var list = newsUpdatesRepository
                        .GetAll()
                        .Select(x => new
                        {
                            x.Id,
                            title = x.Title,
                            news_url = x.NewsUrl,
                            page_url = x.PageUrl,
                            x.Status
                        });
            return Ok(new { result = list });
        }

        public IHttpActionResult get_siteConfiguration()
        {
            var res = siteConfigurationRepository.GetRecord();

            if (res == null)
                return Ok(new { result = "" });

            return Ok(new { result = res });
        }

        [HttpPost]
        public IHttpActionResult save_siteConfiguration(SiteConfiguration model)
        {
            model.Status = true;
            if (model.Id == 0)
                siteConfigurationRepository.Create(model);
            else
                siteConfigurationRepository.Update(model);

            return Ok();
        }

        public IHttpActionResult get_InstantDnd_SubjectsMapping_json()
        {
            var dt = new List<int>();
            var _classList = classRepository.GetAll().Select(x => new
            {
                classId = x.Id,
                x.className,
                subjectIds = dt
            });

            var _previousYrQp = previousYrQPRepository
                                       .FindBy(x => x.Status == true)
                                       .Select(x => new
                                       {
                                           x.Id,
                                           x.year
                                       });

            var _subjectList = eventRepository
                                   .GetAll()
                                   .OrderBy(x => x.Id)
                                   .Select(x => new
                                   {
                                       x.Id,
                                       Name = x.SubjectName
                                   });

            var _data = new
            {
                classList = _classList,
                subjectList = _subjectList,
                previousYrQp = _previousYrQp,
            };

            return Ok(new { result = _data });
        }

        [HttpPost]
        public IHttpActionResult save_InstantDnd_mapping(InstantDnd_mapping_ViewModel model)
        {
            if (instantDownload_SubjectsMappingPRepository.FindBy(x => x.YearId == model.yearId).Any())
                return Ok(new { result = "exist" });

            foreach (var dt in model.mappingInfo)
            {
                foreach (var subjectId in dt.subjectIds)
                {
                    instantDownload_SubjectsMappingPRepository
                    .Create(
                        InstantDnd_mapping_ViewModel.parse(
                            model.yearId,
                            subjectId,
                            dt.classId), false);
                }
            }
            instantDownload_SubjectsMappingPRepository.Save();

            return Ok(new { result = "ok" });
        }

        [HttpPost]
        public IHttpActionResult save_carrier(CarrierViewModel model)
        {
            if (model.Id == 0)
            {
                CarrierMaster entity = new CarrierMaster();

                CarrierViewModel.parse(model, entity);
                carrierMasterRepository.Create(entity);
            }
            else
            {
                var entity = carrierMasterRepository.FindById(model.Id);
                if (model.isDelete)
                    entity.Status = false;      // change status to false
                else
                    CarrierViewModel.parse(model, entity);  // update record

                carrierMasterRepository.Update(entity);
            }

            return Ok(new { result = "ok" });
        }

        public IHttpActionResult get_carrierList()
        {
            var _carrierList = carrierMasterRepository.GetAll()
                            .Select(x => new
                            {
                                x.Id,
                                x.JobTitle,
                                x.Experience,
                                x.Qualification,
                                x.Skills,
                                x.Vacancies,
                                x.Status
                            });

            return Ok(new { result = _carrierList });
        }



        public HomeController(
            IMetaTagRepository _metaTagRepository,
            IGalleryImageCategoryRepository _galleryImageCategoryRepository,
            IGalleryImageRepository _galleryImageRepository,
            IGalleryImageDetailRepository _galleryImageDetailRepository,
            IErrorLogsRepository _errorLogsRepository,
            INewsUpdatesRepository _newsUpdatesRepository,
            ISiteConfigurationRepository _siteConfigurationRepository,
            IPreviousYrQPRepository _previousYrQPRepository,
            IClassRepository _classRepository,
            IEventRepository _eventRepository,
            IInstantDownload_SubjectsMappingPRepository _instantDownload_SubjectsMappingPRepository,
            ICarrierMasterRepository _carrierMasterRepository,
            ISchedule_OlympiadsRepository _schedule_OlympiadsRepository)
        {
            metaTagRepository = _metaTagRepository;
            galleryImageCategoryRepository = _galleryImageCategoryRepository;
            galleryImageRepository = _galleryImageRepository;
            galleryImageDetailRepository = _galleryImageDetailRepository;
            errorLogsRepository = _errorLogsRepository;
            newsUpdatesRepository = _newsUpdatesRepository;
            siteConfigurationRepository = _siteConfigurationRepository;
            previousYrQPRepository = _previousYrQPRepository;
            classRepository = _classRepository;
            eventRepository = _eventRepository;
            instantDownload_SubjectsMappingPRepository = _instantDownload_SubjectsMappingPRepository;
            carrierMasterRepository = _carrierMasterRepository;
            schedule_OlympiadsRepository = _schedule_OlympiadsRepository;
        }

    }
}
