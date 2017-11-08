using SilverzoneERP.Data;
using SilverzoneERP.Entities;
using SilverzoneERP.Entities.Constant;
using SilverzoneERP.Entities.Models;
using SilverzoneERP.Entities.ViewModel.Site;
using System;
using System.Linq;
using System.Web.Http;

namespace SilverzoneERP.Api.api.Site
{
    public class HomeController : ApiController
    {
        private ISchoolDownloadsRepository schoolDownloadsRepository;
        private IMetaTagRepository metaTagRepository;
        private ISyllabus_SampleQPRepository syllabus_SampleQPRepository;
        private IEventRepository eventRepository;
        IGalleryImageRepository galleryImageRepository { get; set; }
        IGalleryImageDetailRepository galleryImageDetailRepository { get; set; }
        ICountryRepository countryRepository { get; set; }
        IResult_L1Repository result_L1Repository { get; set; }
        IResult_L2FinalRepository result_L2FinalRepository { get; set; }
        IResultEventRepository resultEventRepository { get; set; }
        IResultEvent_DetailRepository resultEvent_DetailRepository { get; set; }
        ISubscribe_UpdatesRepository subscribe_UpdatesRepository { get; set; }
        IRegisterSchoolRepository registerSchoolRepository { get; set; }
        INewsUpdatesRepository newsUpdatesRepository { get; set; }
        private IEnquiryRepository enquiryRepository { get; set; }
        private IResult_levelClassRepository result_levelClassRepository { get; set; }
        private IUserRepository userRepository { get; set; }
        private ISiteConfigurationRepository siteConfigurationRepository { get; set; }
        private ICarrierResumeRepository carrierResumeRepository { get; set; }
        private ICarrierMasterRepository carrierMasterRepository { get; set; }
        private IResultRequestRepository resultRequestRepository { get; set; }
        private ISchedule_OlympiadsRepository schedule_OlympiadsRepository { get; set; }

        [HttpPost]
        public IHttpActionResult save_enquiryForm(enquiryViewModel model)
        {
            var id = enquiryRepository.Create(enquiryViewModel.parse(model)).Id;

            if (id != 0)
                enquiryRepository.send_enquiryEmail(model.HtmlTemplate);

            return Ok(new { result = id });
        }

        [HttpGet]
        public IHttpActionResult getSchool_dndFiles()
        {
            var schoolDownloads = schoolDownloadsRepository.GetAll()
                                 .GroupBy(x => x.EventId)
                                 .OrderBy(x => x.Select(y => y.Id).Count())
                                 .ToList()
                                 .Select(y => new
                                 {
                                     EventCode = y.FirstOrDefault().Event.EventCode.ToLower(),
                                     EventName = string.Format("{0} {1}",
                                                  y.FirstOrDefault().Event.EventCode,
                                                  DateTime.Now.Year),
                                     Data = y.GroupBy(z => z.DownloadType).Select(a => new
                                     {
                                         FileType = a.Key,
                                         FileDetails = a.Select(dt => new
                                         {
                                             dt.FileName,
                                             dndUrl = DownloadViewModel.parse(dt.DownloadType,
                                              dt.DownloadLink,
                                              dt.Event.EventCode)
                                         })
                                     })
                                 });

            return Ok(new { result = schoolDownloads });
        }

        [HttpGet]
        public IHttpActionResult getMeta_tagbyUrl(string url)
        {
            var metaTag = metaTagRepository
                        .FindBy(x => x.Link == url)
                        .SingleOrDefault();

            if (metaTag == null)
                return Ok(new { result = "" });

            return Ok(new
            {
                result = new
                {
                    metaTag.Title,
                    metaTag.Description,
                    metaTag.Keyword
                }
            });
        }

        [HttpGet]
        public IHttpActionResult getSyllabus_SampleQP(string eventName)
        {
            long eventId = eventRepository
                          .FindBy(x => x.EventCode.ToLower() == eventName.ToLower())
                          .SingleOrDefault()
                          .Id;

            var res = syllabus_SampleQPRepository
             .FindBy(x => x.EventId == eventId)
             .GroupBy(x => x.ClassId)
             .ToList()
             .Select(x => new
             {
                 x.FirstOrDefault().Class.className,
                 data = x.Select(y => new
                 {
                     pdfType = y.PdfType,
                     dndUrl = string.Format("/Files/Syl_Sam_Papers/{0}/{1}.pdf",
                             eventName.ToLower(),
                             y.DownloadLink)
                 })
             });

            return Ok(new { result = res });
        }

        public IHttpActionResult get_gallerys()
        {
            var hallOf_fame = galleryImageRepository
                   .FindBy(aa => aa.CategoryId == 3 && aa.galleryOrder == 0);

            var res = galleryImageRepository
                     .GetAll()
                     .OrderBy(x => x.galleryOrder == null)  /*-  first perform order on values after nullable list -*/
                     .ThenBy(x => x.galleryOrder).AsQueryable()
                     .Union(hallOf_fame.AsEnumerable())
                     .ToList()
                     .Select(x => new
                     {
                         x.Id,
                         category = x.GalleryImageType.Name,
                         x.year,
                         x.Description,
                         ImageUrl = image_urlResolver.getGalleryImage(x.ImageUrl),
                     });

            return Ok(new { result = res });
        }

        public IHttpActionResult get_galleryDetail(long gallaryId)
        {
            var res = galleryImageRepository.FindById(gallaryId);

            var a = new
            {
                galleryName = string.Format("{0} - {1}",
                                             res.GalleryImageType.Name,
                                            res.year),
                galleryDetail = res.GalleryImageDetails
                   .Where(x => x.Status == true)
                   .ToList()
                   .Select(x => new
                   {
                       x.Id,
                       ImageUrl = image_urlResolver.getGalleryImage(x.ImageUrl),
                   })
            };

            return Ok(new { result = a });
        }

        public IHttpActionResult get_hallofFame()
        {
            var res = galleryImageRepository
                     .FindBy(x => x.CategoryId == 3 && x.galleryOrder == null)
                     .ToList()
                     .Select(x => new
                     {
                         x.Id,
                         category = x.GalleryImageType.Name,
                         x.year,
                         x.Description,
                         ImageUrl = image_urlResolver.getGalleryImage(x.ImageUrl),
                     });

            return Ok(new { result = res });
        }


        public IHttpActionResult get_MediaImg()
        {
            var res = galleryImageRepository
                     .FindBy(x => x.CategoryId == 10)
                     .ToList()
                     .Select(x => new
                     {
                         x.Id,
                         category = x.GalleryImageType.Name,
                         x.year,
                         x.Description,
                         ImageUrl = image_urlResolver.getGalleryImage(x.ImageUrl),
                     });

            return Ok(new { result = res });
        }

        public IHttpActionResult getAll_country()
        {
            var lst = countryRepository.GetAll().Select(x => new
            {
                code = x.Id,
                name = x.CountryName
            });

            return Ok(new { result = lst });
        }

        [HttpGet]
        public IHttpActionResult getAll_events()
        {
            var resultEvent = resultEventRepository
                             .findBy_shortCode(DateTime.Now.ToString("yy"));

            var res = result_levelClassRepository
                    .GetAll()
                    .GroupBy(x => x.LevelId)
                    .ToList()   // convert into list to use function inside select statement like String.Format, Convert.ToInt32 or any custom fx
                    .Select(x => new
                    {
                        x.Key,
                        resultEvent.isResult_declared,
                        eventInfo = x.Select(y => new
                        {
                            y.Id,
                            EventCode = string.Format("{0}{1}",
                                         y.EventInfo.EventCode.ToUpper(),
                                        (y.EventInfo.Id == 4 || y.EventInfo.Id == 5 ? (Convert.ToInt32(resultEvent.shortCode) + 1).ToString()
                                        : resultEvent.shortCode))
                        })
                    });

            return Ok(new { result = res });
        }

        [HttpPost]
        public IHttpActionResult register_subscribeUpdates(subscribe_updatesViewModel model)
        {
            var entity = new Subscribe_Updates();
            subscribe_updatesViewModel.parse(model, entity);
            subscribe_UpdatesRepository.Create(entity);

            return Ok();
        }

        public IHttpActionResult get_registerSchool_json()
        {
            var _genders = Enum.GetValues(typeof(genderType))
                         .Cast<genderType>()
                         .Select(v => new
                         {
                             Id = v.GetHashCode(),
                             Name = v.ToString()
                         }).ToList();

            var _profiles = Enum.GetValues(typeof(school_ProfileType))
                         .Cast<school_ProfileType>()
                         .Select(v => new
                         {
                             Id = v.GetHashCode(),
                             Name = v.ToString()
                         }).ToList();

            var res = new
            {
                genders = _genders,
                profiles = _profiles
            };

            return Ok(new { result = res });
        }

        [HttpPost]
        public IHttpActionResult register_school(school_registrationViewModel model)
        {
            var entity = new RegisterSchool();
            school_registrationViewModel.parse(model, entity);
            var id = registerSchoolRepository.Create(entity).Id;

            if (id != 0)
                registerSchoolRepository.send_schoolRegister_Email(model.HtmlTemplate);

            return Ok();
        }

        [HttpPost]
        public IHttpActionResult searchResult(resultViewModel model)
        {
            var result_L1 = result_L1Repository.FindBy(x => x.ResultEventId == model.result_eventId);

            if (!result_L1.Any())
                return Ok(new { result = 0 });  // result is not declared


            var res = result_L1.Where(x => x.NIORollNo.Equals(model.enrolmentNo)
                                        && x.Status == true)
                               .SingleOrDefault();

            if (res == null ||
               (model.levelType == levelType.Level_2
             && res.Result_L2 == null))
                return Ok(new { result = 1 });

            return Ok(new { result = resultViewModel.parse(res, model.levelType) });
        }

        public IHttpActionResult get_newsUpdates(string curentUrl)
        {
            var _result = newsUpdatesRepository
                         .FindBy(x => x.PageUrl == curentUrl
                                   && x.Status == true);

            if (!_result.Any())
                _result = newsUpdatesRepository
                         .FindBy(x => x.PageUrl == "/Home"
                                   && x.Status == true);

            return Ok(new
            {
                result = _result.Select(x => new
                {
                    x.Title,
                    x.NewsUrl
                })
            });
        }

        [HttpPost]
        public IHttpActionResult save_freelance(rcViewModel model)
        {
            userRepository.Create(rcViewModel.parse(model));
            return Ok();
        }

        public IHttpActionResult get_siteConfiguration(rcViewModel model)
        {
            return Ok(new
            {
                result = siteConfigurationRepository.GetRecord()
            });
        }

        [HttpPost]
        public IHttpActionResult uploadResume(string htmlTemplate, long jobId)
        {
            string resume_fileName = carrierResumeRepository.saveResume(htmlTemplate);

            carrierResumeRepository.Create(new CarrierResume()
            {
                JobId = jobId,
                FilePath = resume_fileName,
                Status = true
            });

            return Ok();
        }


        public IHttpActionResult get_carrier(rcViewModel model)
        {
            var _carrierList = carrierMasterRepository.FindBy(x => x.Status == true)
                            .Select(x => new
                            {
                                x.Id,
                                x.JobTitle,
                                x.Experience,
                                x.Qualification,
                                x.Skills,
                                x.Vacancies,
                            });

            return Ok(new { result = _carrierList });
        }

        [HttpPost]
        public IHttpActionResult save_resultRequest(ResultRequest model)
        {
            model.Status = true;
            resultRequestRepository.Create(model);
            return Ok();
        }

        public IHttpActionResult get_Schedule_olympads()
        {
            var res = schedule_OlympiadsRepository
                     .FindBy(x => x.Status)
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

        public HomeController(
            ISchoolDownloadsRepository _schoolDownloadsRepository,
            IMetaTagRepository _metaTagRepository,
            ISyllabus_SampleQPRepository _syllabus_SampleQPRepository,
            IEventRepository _eventRepository,
            IGalleryImageRepository _galleryImageRepository,
            IGalleryImageDetailRepository _galleryImageDetailRepository,
            ICountryRepository _countryRepository,
            IResult_L1Repository _result_L1Repository,
            IResult_L2FinalRepository _result_L2FinalRepository,
            IResultEventRepository _resultEventRepository,
            IResultEvent_DetailRepository _resultEvent_DetailRepository,
            ISubscribe_UpdatesRepository _subscribe_UpdatesRepository,
            IRegisterSchoolRepository _registerSchoolRepository,
            INewsUpdatesRepository _newsUpdatesRepository,
            IEnquiryRepository _enquiryRepository,
            IResult_levelClassRepository _result_levelClassRepository,
            IUserRepository _userRepository,
            ISiteConfigurationRepository _siteConfigurationRepository,
            ICarrierResumeRepository _carrierResumeRepository,
            ICarrierMasterRepository _carrierMasterRepository,
            IResultRequestRepository _resultRequestRepository,
            ISchedule_OlympiadsRepository _schedule_OlympiadsRepository
            )
        {
            schoolDownloadsRepository = _schoolDownloadsRepository;
            metaTagRepository = _metaTagRepository;
            syllabus_SampleQPRepository = _syllabus_SampleQPRepository;
            eventRepository = _eventRepository;
            galleryImageRepository = _galleryImageRepository;
            galleryImageDetailRepository = _galleryImageDetailRepository;
            countryRepository = _countryRepository;
            result_L1Repository = _result_L1Repository;
            result_L2FinalRepository = _result_L2FinalRepository;
            resultEventRepository = _resultEventRepository;
            resultEvent_DetailRepository = _resultEvent_DetailRepository;
            subscribe_UpdatesRepository = _subscribe_UpdatesRepository;
            registerSchoolRepository = _registerSchoolRepository;
            newsUpdatesRepository = _newsUpdatesRepository;
            enquiryRepository = _enquiryRepository;
            result_levelClassRepository = _result_levelClassRepository;
            userRepository = _userRepository;
            siteConfigurationRepository = _siteConfigurationRepository;
            carrierResumeRepository = _carrierResumeRepository;
            resultRequestRepository = _resultRequestRepository;
            carrierMasterRepository = _carrierMasterRepository;
            schedule_OlympiadsRepository = _schedule_OlympiadsRepository;
        }
    }
}
