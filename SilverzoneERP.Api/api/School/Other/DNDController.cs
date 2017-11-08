using SilverzoneERP.Data;
using SilverzoneERP.Entities;
using SilverzoneERP.Entities.Models;
using SilverzoneERP.Entities.ViewModel.School;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace SilverzoneERP.Api.api.School
{
    public class DNDController : ApiController
    {
        IDND_MobileNoRepository _DND_MobileNoRepository;
        IDND_EmailIdRepository _DND_EmailIdRepository;
        public DNDController(IDND_MobileNoRepository _DND_MobileNoRepository, IDND_EmailIdRepository _DND_EmailIdRepository)
        {
            this._DND_MobileNoRepository = _DND_MobileNoRepository;
            this._DND_EmailIdRepository = _DND_EmailIdRepository;
        }

        [HttpGet]
        public IHttpActionResult GetDNDType()
        {
            var DNDType = Enum.GetValues(typeof(DNDType))
                        .Cast<DNDType>()
                        .Select(v => new
                        {
                            Id = v.GetHashCode(),
                            Name = v.ToString().Replace("_", " ")
                        }).ToList();
            return Ok(new { DNDType= DNDType });
        }

        [HttpPost]
        public IHttpActionResult Create_Update_Mobile(DND_MobileNo model)
        {            
            if (ModelState.IsValid)
            {
                string msg = "";
                if (model.Id == 0)
                {
                    _DND_MobileNoRepository.Create(new DND_MobileNo()
                    {
                        MobileNo = model.MobileNo,
                        Remarks = model.Remarks,
                        Status = true
                    });
                    msg = "Successfully Mobile added  !";
                }
                else
                {
                    var _DND_Mobile = _DND_MobileNoRepository.FindById(model.Id);
                    if (_DND_Mobile != null)
                    {
                        _DND_Mobile.MobileNo = model.MobileNo;
                        _DND_Mobile.Remarks = model.Remarks;
                        _DND_MobileNoRepository.Update(_DND_Mobile);
                    }
                    msg = "Successfully Question paper updated !";
                }
                return Ok(new { result = "success", message = msg });
            }
            return Ok(new { result = "error", message = "error" });
        }

        [HttpPost]
        public IHttpActionResult Create_Update_Email(DND_EmailId model)
        {            
            if (ModelState.IsValid)
            {
                string msg = "";
                if (model.Id == 0)
                {
                    _DND_EmailIdRepository.Create(new DND_EmailId()
                    {
                        EmailId = model.EmailId,
                        Remarks = model.Remarks,
                        Status = true
                    });
                    msg = "Successfully Email-Id added  !";
                }
                else
                {
                    var _DND_Email = _DND_EmailIdRepository.FindById(model.Id);
                    if (_DND_Email != null)
                    {
                        _DND_Email.EmailId = model.EmailId;
                        _DND_Email.Remarks = model.Remarks;
                        _DND_EmailIdRepository.Update(_DND_Email);
                    }
                    msg = "Successfully Email-Id updated !";
                }
                return Ok(new { result = "success", message = msg });
            }
            return Ok(new { result = "error", message = "error" });
        }

        [HttpGet]
        public IHttpActionResult Get_DND(string Search,long StartIndex,long Limit, DNDType DNDType)
        {
            long Count = 0;

            var DND_MobileNo = _DND_MobileNoRepository.Get(null, StartIndex, Limit, DNDType.MobileNo, out Count);
            var DND_EmailId = _DND_MobileNoRepository.Get(null, StartIndex, Limit, DNDType.EmailId, out Count);
            return Ok(new { DND_MobileNo, DND_EmailId });
        }

        [HttpPost]
        public IHttpActionResult Active_Deactive_MobileNo(List<multiSelect> model)
        {
            try
            {
                foreach (var item in model)
                {
                    var _DND_MobileNo = _DND_MobileNoRepository.FindById(item.Id);
                    if (_DND_MobileNo != null)
                    {
                        _DND_MobileNo.Status = !_DND_MobileNo.Status;
                        _DND_MobileNoRepository.Update(_DND_MobileNo);
                    }
                }

                return Ok(new { result = "Success", message = "Successfully Save Changed !" });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public IHttpActionResult Active_Deactive_EmailId(List<multiSelect> model)
        {
            try
            {
                foreach (var item in model)
                {
                    var _DND_EmailId = _DND_EmailIdRepository.FindById(item.Id);
                    if (_DND_EmailId != null)
                    {
                        _DND_EmailId.Status = !_DND_EmailId.Status;
                        _DND_EmailIdRepository.Update(_DND_EmailId);
                    }
                }

                return Ok(new { result = "Success", message = "Successfully Save Changed !" });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
