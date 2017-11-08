using SilverzoneERP.Data;
using SilverzoneERP.Entities.Models;
using System.Linq;
using System.Web.Http;

namespace SilverzoneERP.Api.api.School
{
    public class ExcellenceAwardController : ApiController
    {
        IExcellenceAwardRepository _excellenceAwardRepository;
        IAwardRepository _awardRepository;
        public ExcellenceAwardController(IExcellenceAwardRepository _excellenceAwardRepository, IAwardRepository _awardRepository)
        {
            this._excellenceAwardRepository = _excellenceAwardRepository;
            this._awardRepository = _awardRepository;
        }

        [HttpGet]
        public IHttpActionResult GetAwardList()
        {
            var Awardlist = _awardRepository.FindBy(x=>x.Status==true).Select(x=>new {
                x.Id,
                Name=x.Name+" ("+x.Year+")"
            });
            return Ok(new { Awardlist });
        }

        [HttpPost]
        public IHttpActionResult Create_Update(ExcellenceAward model)
        {
            if (ModelState.IsValid)
            {
                string msg = "";
                if (model.Id == 0)
                {
                    _excellenceAwardRepository.Create(new ExcellenceAward()
                    {
                        SchoolId = model.SchoolId,
                        AwardId = model.AwardId,
                        NomineeName = model.NomineeName,
                        Remarks = model.Remarks,
                        EventId=model.EventId,
                        Status = true
                    });
                    msg = "Successfully Excellence Award added  !";
                }
                else
                {
                    var _excellenceAward = _excellenceAwardRepository.FindById(model.Id);
                    if (_excellenceAward != null)
                    {
                        _excellenceAward.SchoolId = model.SchoolId;
                        _excellenceAward.AwardId = model.AwardId;
                        _excellenceAward.NomineeName = model.NomineeName;
                        _excellenceAward.Remarks = model.Remarks;
                        _excellenceAward.EventId = model.EventId;
                        _excellenceAwardRepository.Update(_excellenceAward);
                        msg = "Successfully Excellence Award updated !";
                    }
                }
                return Ok(new { result = "success", message = msg });
            }
            return Ok(new { result = "error", message = "error" });
        }
    }
}
