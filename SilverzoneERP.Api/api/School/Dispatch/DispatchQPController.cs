using SilverzoneERP.Data;
using SilverzoneERP.Entities;
using SilverzoneERP.Entities.ViewModel.School;
using System.Linq;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace SilverzoneERP.Api.api.School
{
    [Authorize]
    public class DispatchQPController : ApiController
    {
        IQPDispatchERPRepository _QPDispatchERPRepository;
        ILotRepository _lotRepository;
        
        public DispatchQPController(ILotRepository _lotRepository,IQPDispatchERPRepository _QPDispatchERPRepository)
        {
            this._lotRepository = _lotRepository;
            this._QPDispatchERPRepository = _QPDispatchERPRepository;
        }

        [HttpGet]
        public IHttpActionResult GetLotNoList()
        {
            var data = _lotRepository.FindBy(x => x.LotYear == ServerKey.Event_Current_YearCode).Select(x => new { x.Id, x.LotNo });
            return Ok(new { LotNoList = data });
        }

        [HttpGet]
        public IHttpActionResult GetRegNoList(long LotId)
        {
            return Ok(new { RegNoList = _lotRepository.RegNoList(LotId), RegNoListByLot = _QPDispatchERPRepository.RegNoListByLot(LotId) });
        }

        [HttpGet]
        public IHttpActionResult GetQPDispatchDetail(long EventManagementId)
        {
            return Ok(new { QPDispatchDetail = _lotRepository.GetQPDispatchDetail(EventManagementId) });
        }

        [HttpPost]
        public IHttpActionResult Create_Update(SaveQPDispatch model)
        {
            if (ModelState.IsValid)
            {
                string msg = "";
                if (model.Id == 0)
                {
                    _QPDispatchERPRepository.Create(new Entities.Models.QPDispatchERP()
                    {
                        LotId=model.LotId,
                        EventManagementId = model.EventManagementId,
                        JSONData = new JavaScriptSerializer().Serialize(model.JSONData),
                        Status = true
                    });
                    msg = "Successfully Question paper added  !";
                }
                else
                {
                    var _QpDispatch = _QPDispatchERPRepository.FindById(model.Id);
                    if (_QpDispatch != null)
                    {
                        _QpDispatch.LotId = model.LotId;
                        _QpDispatch.EventManagementId = model.EventManagementId;
                        _QpDispatch.JSONData = new JavaScriptSerializer().Serialize(model.JSONData);
                        _QPDispatchERPRepository.Update(_QpDispatch);
                    }
                    msg = "Successfully Question paper updated !";
                }
                return Ok(new { result = "success", message = msg, RegNoListByLot = _QPDispatchERPRepository.RegNoListByLot(model.LotId) });
            }
            return Ok(new { result = "error", message = "error" });
        }

        [HttpGet]
        public IHttpActionResult RegNoListByLot(long LotId)
        {
            
            return Ok(new { RegNoListByLot = _QPDispatchERPRepository.RegNoListByLot(LotId) });
        }

    }
}
