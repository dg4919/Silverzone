using SilverzoneERP.Data;
using SilverzoneERP.Entities;
using SilverzoneERP.Entities.Models;
using System;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace SilverzoneERP.Api.api.School
{
    [Authorize]
    public class ChequeTransactionController : ApiController
    {
        IChequeTransactionRepository _chequeTransactionRepository;
        public ChequeTransactionController(IChequeTransactionRepository _chequeTransactionRepository)
        {
            this._chequeTransactionRepository = _chequeTransactionRepository;
        }


        [HttpGet]
        public IHttpActionResult GetBankType()
        {
            var BankType = Enum.GetValues(typeof(BankType))
                        .Cast<BankType>()
                        .Select(v => new
                        {
                            Id = v.GetHashCode(),
                            Name = v.ToString()
                        }).ToList();
            return Ok(new { BankType });
        }

        [HttpPost]
        public IHttpActionResult Create_Update(ChequeTransaction model)
        {
            if (ModelState.IsValid)
            {
                string msg = "";
                if (model.Id == 0)
                {
                    _chequeTransactionRepository.Create(new ChequeTransaction()
                    {
                        PartyName = model.PartyName,
                        Amount=model.Amount,
                        Remarks = model.Remarks,
                        BankType=model.BankType,
                        Status = true
                    });
                    msg = "Successfully Cheque Transaction added  !";
                }
                else
                {
                    var _chequeTransaction = _chequeTransactionRepository.FindById(model.Id);
                    if (_chequeTransaction != null)
                    {
                        if(!_chequeTransaction.IsVerified)
                        {
                            _chequeTransaction.PartyName = model.PartyName;
                            _chequeTransaction.Amount = model.Amount;
                            _chequeTransaction.BankType = model.BankType;
                            _chequeTransaction.Remarks = model.Remarks;
                            _chequeTransactionRepository.Update(_chequeTransaction);
                            msg = "Successfully Cheque Transaction updated !";
                        }
                        else
                            msg = "Can't be update Cheque Transaction !";
                    }                    
                }
                return Ok(new { result = "success", message = msg });
            }
            return Ok(new { result = "error", message = "error" });
        }

        [HttpGet]
        public IHttpActionResult ChequeTransactionList(BankType BankType, int StartIndex, int Limit)
        {
            long Count = _chequeTransactionRepository.FindBy(x => x.Status == true && x.BankType == BankType).Count();

            var ChequeTransactionList = _chequeTransactionRepository.FindBy(x => x.Status == true && x.BankType==BankType).OrderByDescending(x=>x.UpdationDate).Skip(StartIndex).Take(Limit).Select(x => new
            {
                x.Id,
                x.PartyName,
                x.Amount,
                x.Remarks,
                x.RowVersion,
                x.IsVerified,
                x.BankType,
                x.Status
            });
            return Ok(new { ChequeTransactionList, Count });
        }

        [AllowAnonymous]
        [HttpGet]
        public IHttpActionResult Report(BankType BankType,long Id)
        {
            HttpContext.Current.Session["BankType"] = BankType;            

            var domain = HttpContext.Current.Request.Url.Host;
            string url = "http://" + domain + "/FinalReport/ChequeTransaction.aspx";
            if (domain == "localhost")
                url = "http://localhost:55615/FinalReport/ChequeTransaction.aspx";

            System.Uri uri = new System.Uri(url);

            return Redirect(uri);
        }

        [HttpPost]
        public IHttpActionResult Verify(long Id)
        {
            try
            {
                var data = _chequeTransactionRepository.FindById(Id);
                if (data != null)
                {
                    data.IsVerified = !data.IsVerified;
                    _chequeTransactionRepository.Update(data);
                    return Ok(new { result = "success", message = "Successfully verified !" });
                }
                else
                    return Ok(new { result = "Error", message = "Failed verification !" });

            }
            catch (Exception)
            {
                return Ok(new { result = "Error", message="Failed verification !" });
            }
            
        }
    }
}
