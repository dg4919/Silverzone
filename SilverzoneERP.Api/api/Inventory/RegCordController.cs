using SilverzoneERP.Data;
using SilverzoneERP.Entities;
using SilverzoneERP.Entities.ViewModel.RegCord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace SilverzoneERP.Api.api.Inventory
{
    public class RegCordController : ApiController
    {
        ISchoolRepository schoolRepository { get; set; }
        IUserRepository userRepository { get; set; }

        IStateRepository stateRepository { get; set; }
        IDistrictRepository districtRepository { get; set; }
        ICityRepository cityRepository { get; set; }
        IRcSchoolsRepository rcSchoolsRepository { get; set; }
        IRcSchool_VisitsRepository rcSchool_VisitsRepository { get; set; }
        IRcSchool_VisitsInfoRepository rcSchool_VisitsInfoRepository { get; set; }
        IEventRepository eventRepository { get; set; }
        IErrorLogsRepository errorLogsRepository { get; set; }


        #region  *******************  Assign RC school  ***********************

        [HttpGet]
        public IHttpActionResult get_rcList()
        {
            var _model = userRepository.GetRCList().Select(x => new
            {
                x.Id,
                x.UserName
            });

            return Ok(new { result = _model });
        }

        [HttpPost]
        public IHttpActionResult SearchSchool(SearchRC_ViewModel model)
        {
            // all filter is applied Here :)
            // if (model.CityId) value found null then all city (x.CityId) would be get
            var schools = schoolRepository.FindBy(x => x.CityId == (model.CityId ?? x.CityId)
                                                    && x.StateId == (model.StateId ?? x.StateId)
                                                    && x.DistrictId == (model.DistrictId ?? x.DistrictId))
                                          .ToList()
                                          .Select(school => SearchRC_ViewModel.parse(school));

            return Ok(new { result = schools });
        }

        [HttpGet]
        public IHttpActionResult get_rcSchoolsbyId(long rcId)
        {
            rcId = rcId == 0 ? Convert.ToInt64(User.Identity.Name) : rcId;
            var rcSchools = rcSchoolsRepository.FindBy(x => x.RCId == rcId).SingleOrDefault();

            if (rcSchools == null)
                return Ok(new { result = string.Empty });

            List<long> _modelSchId = new List<long>(new JavaScriptSerializer().Deserialize<long[]>(rcSchools.Schools));

            var schools = _modelSchId.Select(x => SearchRC_ViewModel
                                    .parse(schoolRepository.FindById(x)));

            return Ok(new { result = schools });
        }

        [HttpGet]
        public IHttpActionResult get_cityModel()
        {
            var _model = new
            {
                states = stateRepository.GetAll().Select(x => new { x.Id, x.StateName }),
                districts = districtRepository.GetAll().Select(x => new { x.Id, x.DistrictName }),
                cities = cityRepository.GetAll().Select(x => new { x.Id, x.CityName })
            };

            return Ok(new { result = _model });
        }

        [HttpPost]
        public IHttpActionResult save_RcSchools(RcSchools_ViewModel model)
        {
            long RcId = 0;

            var rcSchools = rcSchoolsRepository.FindBy(x => x.RCId == model.RcId).SingleOrDefault();

            if (rcSchools == null && !model.addRescords)     // if record will delete & not found
                return Ok(new { result = "notfound" });

            if (rcSchools != null && model.addRescords)     // for new item & already exist
            {
                string _matched = string.Empty;

                List<long> _modelSchId = new List<long>(new JavaScriptSerializer().Deserialize<long[]>(rcSchools.Schools));

                foreach (long schId in model.schIds)
                {
                    bool isMatched = false;
                    foreach (long modelschId in _modelSchId)
                    {
                        if (modelschId == schId)
                        {
                            isMatched = true;
                            _matched += schoolRepository.FindById(schId).SchCode + ", ";
                            break;
                        }
                    }
                    if (!isMatched)
                        _modelSchId.Add(schId);
                }
                rcSchools.Schools = new JavaScriptSerializer().Serialize(_modelSchId);
                RcId = rcSchoolsRepository.Update(rcSchools).Id;

                return Ok(new
                {
                    result = RcId,
                    matched = _matched
                });
            }
            else if (rcSchools != null && !model.addRescords)       // for delete Item
            {
                string _notFound = string.Empty;
                List<long> _modelSchId = new List<long>(new JavaScriptSerializer().Deserialize<long[]>(rcSchools.Schools));

                foreach (long schId in model.schIds)
                {
                    bool isMatched = false;
                    foreach (long modelschId in _modelSchId)
                    {
                        if (modelschId == schId)
                        {
                            isMatched = true;
                            break;
                        }
                    }
                    if (isMatched)
                        _modelSchId.Remove(schId);
                    else
                        _notFound += schoolRepository.FindById(schId).SchCode + ", ";
                }

                rcSchools.Schools = new JavaScriptSerializer().Serialize(_modelSchId);
                RcId = rcSchoolsRepository.Update(rcSchools).Id;

                return Ok(new
                {
                    result = RcId,
                    notFound = _notFound
                });
            }

            // for a new record
            RcId = rcSchoolsRepository.Create(new Entities.Models.RcSchools()
            {
                RCId = model.RcId,
                Schools = new JavaScriptSerializer().Serialize(model.schIds),
                Status = true
            }).Id;

            return Ok(new { result = RcId });
        }

        [HttpPost]
        public IHttpActionResult save_RcSchool_byschCode(RcSchools_ViewModel model)
        {
            string _notFound = string.Empty;
            string _matched = string.Empty;
            List<long> schIds = new List<long>();
            long RcId = 0;

            foreach (long schCode in model.schIds)
            {
                var school = schoolRepository.findBySchCode(schCode);
                if (school == null)
                    _notFound += schCode + ", ";
                else
                    schIds.Add(school.Id);
            }

            var rcSchools = rcSchoolsRepository.FindBy(x => x.RCId == model.RcId).SingleOrDefault();

            if (rcSchools == null && !model.addRescords)     // if record will delete & not found
                return Ok(new { result = "notfound" });

            if (rcSchools != null && model.addRescords)
            {
                List<long> _modelSchId = new List<long>(new JavaScriptSerializer().Deserialize<long[]>(rcSchools.Schools));

                foreach (long schId in schIds)
                {
                    bool isMatched = false;
                    foreach (long modelschId in _modelSchId)
                    {
                        if (modelschId == schId)
                        {
                            isMatched = true;
                            _matched += schoolRepository.FindById(schId).SchCode + ", ";
                            break;
                        }
                    }
                    if (!isMatched)
                        _modelSchId.Add(schId);
                }
                rcSchools.Schools = new JavaScriptSerializer().Serialize(_modelSchId);
                RcId = rcSchoolsRepository.Update(rcSchools).Id;
            }
            else if (rcSchools != null && !model.addRescords)
            {
                List<long> _modelSchId = new List<long>(new JavaScriptSerializer().Deserialize<long[]>(rcSchools.Schools));

                foreach (long schId in schIds)
                {
                    bool isMatched = false;
                    foreach (long modelschId in _modelSchId)
                    {
                        if (modelschId == schId)
                        {
                            isMatched = true;
                            break;
                        }
                    }
                    if (isMatched)
                        _modelSchId.Remove(schId);
                    else
                        _notFound += schoolRepository.FindById(schId).SchCode + ", ";
                }

                rcSchools.Schools = new JavaScriptSerializer().Serialize(_modelSchId);
                RcId = rcSchoolsRepository.Update(rcSchools).Id;
            }

            // for a new record
            if (schIds.Any() && rcSchools == null)
            {
                RcId = rcSchoolsRepository.Create(new Entities.Models.RcSchools()
                {
                    RCId = model.RcId,
                    Schools = new JavaScriptSerializer().Serialize(schIds),
                    Status = true
                }).Id;
            }

            return Ok(new
            {
                result = RcId,
                notFound = _notFound,
                matched = _matched
            });
        }

        #endregion


        #region  *******************  RC school's Visit  ***********************

        [HttpGet]
        public IHttpActionResult get_visitTypes()
        {
            var res = Enum.GetValues(typeof(VisitType))
                          .Cast<VisitType>()
                          .Select(v => new
                          {
                              Id = v.GetHashCode(),
                              Name = v.ToString()
                          }).ToList();

            return Ok(new { result = res });
        }

        [HttpGet]
        public IHttpActionResult get_VisitStatus()
        {
            var res = Enum.GetValues(typeof(VisitStatus))
                          .Cast<VisitStatus>()
                          .Select(v => new
                          {
                              Id = v.GetHashCode(),
                              Name = v.ToString()
                          }).ToList();

            return Ok(new { result = res });
        }

        [HttpGet]
        public IHttpActionResult get_events()
        {
            var events = eventRepository.GetAll()
                        .Select(x => new
                        {
                            x.Id,
                            x.EventCode
                        });

            return Ok(new { result = events });
        }

        [HttpPost]
        public IHttpActionResult save_rcVisits(RcSchool_Visits_ViewModel model)
        {
            //if (rcSchool_VisitsInfoRepository.isAny(model.rcId, model.schId, model.eventId))
            //    return Ok(new { result = "exist" });

            using (var transaction = rcSchool_VisitsRepository.BeginTransaction())
            {
                try
                {
                    model.rcId = Convert.ToInt64(User.Identity.Name);

                    long rcvisitId = rcSchool_VisitsRepository.Create(
                    RcSchool_Visits_ViewModel.parse(model)).Id;

                    foreach(var rcSchool_info in model.rcEventInfo)
                    {
                        rcSchool_VisitsInfoRepository
                        .Create(RcSchool_Visits_ViewModel
                               .parse(rcSchool_info, rcvisitId), false);
                    }
                    rcSchool_VisitsInfoRepository.Save();

                    transaction.Commit();
                    return Ok(new { result = "success" });
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    errorLogsRepository.logError(ex);
                    return Ok(new { result = "error" });
                }
            }
        }

        [HttpPost]
        public IHttpActionResult update_rcVisits(update_RcSchool_Visits_ViewModel model)
        {
            var rcSchool = rcSchool_VisitsRepository.FindById(model.rcVisitId);

            if(rcSchool == null)
                return Ok(new { result = "notFound" });

            var res = rcSchool_VisitsRepository.Update(
                update_RcSchool_Visits_ViewModel.parse(model, rcSchool)).Id;

            return Ok(new { result = res });
        }

        [HttpGet]
        public IHttpActionResult get_rcVisits()
        {
            long rcId = Convert.ToInt64(User.Identity.Name);

            var res = rcSchool_VisitsRepository.FindBy(x => x.RCId == rcId);

            if (!res.Any())
                return Ok(new { result = string.Empty });

            return Ok(new
            {
                result = res.Select(x => new
                {
                    x.Contact_Peron_Mobile,
                    x.Contact_Peron_Name,
                    x.SchoolInfo.SchName,
                    x.VisitDate,
                    VisitType = x.VisitType.ToString(),
                    x.Remarks,
                    eventInfo = x.RcSchool_VisitsInfo.Select(y => new
                    {
                        y.Event.EventName,
                        VisitStatus = y.VisitStatus.ToString(),
                    })
                })
            });

        }


        #endregion

        #region  *******************  Follow Up  ***********************

        [HttpGet]
        public IHttpActionResult get_followUps(long rcId)
        {
            var res = rcSchool_VisitsRepository
                     .FindBy(x => x.RCId == rcId
                               && x.FoolowUpDate != null)
                     .Select(x => new
                     {
                         x.Contact_Peron_Mobile,
                         x.Contact_Peron_Name,
                         //x.Event.EventName,
                         x.SchoolInfo.SchName,
                         x.FoolowUpDate,
                         x.Remarks
                     });

            return Ok(new { result = res });
        }

        #endregion


        public RegCordController(
            ISchoolRepository _schoolRepository,
            IStateRepository _stateRepository,
            IDistrictRepository _districtRepository,
            ICityRepository _cityRepository,
            IRcSchoolsRepository _rcSchoolsRepository,
            IRcSchool_VisitsRepository _rcSchool_VisitsRepository,
            IEventRepository _eventRepository,
            IUserRepository _userRepository,
            IRcSchool_VisitsInfoRepository _rcSchool_VisitsInfoRepository,
            IErrorLogsRepository _errorLogsRepository
            )
        {
            schoolRepository = _schoolRepository;
            stateRepository = _stateRepository;
            districtRepository = _districtRepository;
            cityRepository = _cityRepository;
            rcSchoolsRepository = _rcSchoolsRepository;
            rcSchool_VisitsRepository = _rcSchool_VisitsRepository;
            rcSchool_VisitsInfoRepository = _rcSchool_VisitsInfoRepository;
            eventRepository = _eventRepository;
            userRepository = _userRepository;
            errorLogsRepository = _errorLogsRepository;
        }

    }
}
