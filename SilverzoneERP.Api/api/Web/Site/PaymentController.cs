using CCA.Util;
using SilverzoneERP.Data;
using SilverzoneERP.Entities;
using SilverzoneERP.Entities.Models;
using SilverzoneERP.Entities.ViewModel.Site;
using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace SilverzoneERP.Api.api.Site
{
    public class PaymentController : ApiController
    {
        private IAccountRepository accountRepository;
        private IInstantDownloadsRepository instantDownloadsRepository { get; set; }
        private IInstantDownload_SubjectsRepository instantDownload_SubjectsRepository { get; set; }
        private IInstantDownload_SubjectsMappingPRepository instantDnd_mapingPRepository { get; set; }

        private IPaymentInfoRepository paymentInfoRepository { get; set; }
        private IOrderRepository orderRepository { get; set; }
        private IStateRepository stateRepository { get; set; }

        private IPreviousYrQPRepository previousYrQPRepository { get; set; }
        private IClassRepository classRepository { get; set; }
        private IEventRepository eventRepository { get; set; }
        private IResult_levelClassRepository result_levelClassRepository { get; set; }

        [HttpPost]
        public IHttpActionResult processPayment(paymentViewModel model)
        {
            using (var transaction = instantDownloadsRepository.BeginTransaction())
            {
                try
                {
                    var instantDnd = instantDownloadsRepository
                               .Create(
                                paymentViewModel.parse(model)
                                );

                    instantDnd.OrderId = instantDownloadsRepository.get_orderNo(instantDnd.Id);
                    instantDownloadsRepository.Update(instantDnd);

                    var paymentInfo = paymentInfoRepository.FindBy_refrenceId(instantDnd.Id, Payment_refrenceType.Instant_Downloads);

                    // save payment details for online/offline
                    if (paymentInfo == null)
                        paymentInfoRepository.Create(paymentViewModel.parse(
                        instantDnd.Id,
                        Entities.Payment_refrenceType.Instant_Downloads,
                        model.amount));

                    for (int i = 0; i < model.L1_subjects.Length; i++)
                    {
                        instantDownload_SubjectsRepository
                        .Create(
                            paymentViewModel.parse(instantDnd.Id, 1, model.L1_subjects[i]),
                            false);
                    }

                    for (int i = 0; i < model.L2_subjects.Length; i++)
                    {
                        instantDownload_SubjectsRepository
                        .Create(
                            paymentViewModel.parse(instantDnd.Id, 2, model.L2_subjects[i]),
                            false);
                    }

                    instantDownload_SubjectsRepository.Save();
                    transaction.Commit();       // it must be there if want to save record :)

                    return Ok(new
                    {
                        result = "success",
                        orderId = instantDnd.OrderId
                    });
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    return Ok(new { result = "error" });
                }
            }
        }

        [HttpPost]
        public IHttpActionResult getPayment_checksum(payment_paramViewModel model)
        {
            CCACrypto ccaCrypto = new CCACrypto();

            var ccaAuth_model = new cccaAuth();

            var req = new
            {
                strEncRequest = ccaCrypto.Encrypt(payment_paramViewModel.GetQueryString(model),
                                                  ccaAuth_model.workingKey),
                strAccessCode = ccaAuth_model.strAccessCode
            };

            return Ok(new { result = req });
        }
        
        // when CCAavenue gateway response after success/fail
        [HttpPost]
        public IHttpActionResult paymentResponse(ccaRequest model)
        {
            string workingKey = new cccaAuth().workingKey;

            CCACrypto ccaCrypto = new CCACrypto();
            string encResponse = ccaCrypto.Decrypt(model.encResp, workingKey);
            string[] segments = encResponse.Split('&');

            string order_id = segments.FirstOrDefault(x => x.Contains("order_id")).Split('=')[1];
            string order_status = segments.FirstOrDefault(x => x.Contains("order_status")).Split('=')[1];
            string tracking_id = segments.FirstOrDefault(x => x.Contains("tracking_id")).Split('=')[1];
            string bank_ref_no = segments.FirstOrDefault(x => x.Contains("bank_ref_no")).Split('=')[1];

            PaymentInfo paymentInfo = null;
            if (order_id.Contains("ORD/InstantDnd"))
            {
                paymentInfo = paymentInfoRepository.FindBy_refrenceId(
                   instantDownloadsRepository.findBy_OrderId(order_id).Id,
                   Payment_refrenceType.Instant_Downloads);
            }
            else if (order_id.Contains("ORD/SZ"))
            {
                paymentInfo = paymentInfoRepository.FindBy_refrenceId(
                       orderRepository.GetByOrderNumber(order_id).Id,
                       Payment_refrenceType.Online_Books);
            }

            // updating records
            paymentViewModel.parse(
                       order_status,
                       tracking_id,
                       bank_ref_no,
                       ref paymentInfo);        // change by refrence

            paymentInfoRepository.Update(paymentInfo);
            return Ok();
        }

        [HttpGet]
        public IHttpActionResult get_paymentResponse(string orderId)
        {
            string Payment_refType = string.Empty;
            PaymentInfo paymentInfo = null;

            if (orderId.Contains("ORD/InstantDnd"))
            {
                paymentInfo = paymentInfoRepository
                        .FindBy_refrenceId(
                    instantDownloadsRepository.findBy_OrderId(orderId).Id,
                    Payment_refrenceType.Instant_Downloads);

                Payment_refType = Payment_refrenceType.Instant_Downloads.ToString();
            }
            else if (orderId.Contains("ORD/SZ"))
            {
                paymentInfo = paymentInfoRepository
                        .FindBy_refrenceId(
                        orderRepository.GetByOrderNumber(orderId).Id,
                        Payment_refrenceType.Online_Books);

                Payment_refType = Payment_refrenceType.Online_Books.ToString();
            }

            var _data = new
            {
                payment_status = paymentInfo.Payment_Status,
                tracking_id = paymentInfo.Tracking_Id,
                bank_ref_no = paymentInfo.Bank_Ref_No,
                payment_refType = Payment_refType
            };

            return Ok(new { result = _data });
        }

        public IHttpActionResult get_rePaymentModel(string orderId)
        {
            dynamic model = null;
            if (orderId.Contains("ORD/InstantDnd"))
            {
                var userInfo = instantDownloadsRepository.findBy_OrderId(orderId);

                var paymentInfo = paymentInfoRepository
                             .FindBy_refrenceId(
                                    userInfo.Id,
                                    Payment_refrenceType.Instant_Downloads);

                model = new payment_paramViewModel()
                {
                    order_id = userInfo.OrderId,
                    amount = paymentInfo.Amount,
                    billing_address = userInfo.Address,
                    billing_city = userInfo.City,
                    billing_country = userInfo.Country,
                    billing_email = userInfo.EmailId,
                    billing_name = userInfo.UserName,
                    billing_state = userInfo.State,
                    billing_tel = userInfo.Mobile.ToString(),
                    billing_zip = userInfo.Pincode.ToString(),
                };
            }
            else if (orderId.Contains("ORD/SZ"))
            {
                var orderInfo = orderRepository.GetByOrderNumber(orderId);

                var paymentInfo = paymentInfoRepository
                             .FindBy_refrenceId(
                                    orderInfo.Id,
                                    Payment_refrenceType.Online_Books);

                model = new payment_paramViewModel()
                {
                    order_id = orderInfo.OrderNumber,
                    amount = paymentInfo.Amount,
                    billing_address = orderInfo.UserShippingAddress.Address,
                    billing_city = orderInfo.UserShippingAddress.City,
                    billing_country = orderInfo.UserShippingAddress.CountryType.ToString(),
                    billing_email = orderInfo.UserShippingAddress.Email,
                    billing_name = orderInfo.UserShippingAddress.Username,
                    billing_state = orderInfo.UserShippingAddress.State,
                    billing_tel = orderInfo.UserShippingAddress.Mobile,
                    billing_zip = orderInfo.UserShippingAddress.PinCode,
                };
            }
            return Ok(new { result = model });
        }

        public IHttpActionResult getJson()
        {
            var _classList = classRepository.GetAll().Select(x => new
            {
                x.Id,
                x.className
            });

            var _subjectList = eventRepository
                              .GetAll()
                              .OrderBy(x => x.Id)
                              .Select(x => new
                              {
                                  x.Id,
                                  Name = x.SubjectName
                              });

           
            var _result2 = instantDnd_mapingPRepository.FindBy(y => y.ClassId == null);
            var _availableSubjects = instantDnd_mapingPRepository
                                    .FindBy(x => x.ClassId != null)
                                    .OrderBy(x => x.SubjectId)
                                    .GroupBy(x => new { x.ClassId, x.YearId })
                                    .Select(x => new
                                    {
                                        classId = x.Key.ClassId,
                                        yearId = x.Key.YearId,
                                        eventInfo = (from subject in _subjectList
                                                     join result in x
                                                     on subject.Id equals result.SubjectId into a
                                                     from level1 in a.DefaultIfEmpty()
                                                     join resultt in _result2
                                                     on subject.Id equals resultt.SubjectId into b
                                                     from level2 in b.DefaultIfEmpty()
                                                     select new
                                                     {
                                                         subjectId = subject.Id,
                                                         Level1 = level1 != null ? true : false,
                                                         Level2 = level2 != null ? true : false
                                                     }),
                                    });


            var _previousYrQp = previousYrQPRepository
                               .FindBy(x => x.Status == true)
                               .Select(x => new
                               {
                                   x.Id,
                                   x.year
                               });
            var _stateList = stateRepository.GetAll()
                            .OrderBy(x => x.StateName)
                            .Select(x => new
                            {
                                x.Id,
                                x.StateName
                            });

            var _data = new
            {
                classList = _classList,
                events = _availableSubjects,
                subjectList = _subjectList,
                previousYrQp = _previousYrQp,
                stateList = _stateList
            };

            return Ok(new { result = _data });
        }

        public IHttpActionResult get_userInstant_dndPdf(
            string emailId,
            long mobile
            )
        {
            var res = instantDownloadsRepository
                     .FindBy(x => x.EmailId == emailId
                               && x.Mobile == mobile);

            if (!res.Any())
                return Ok(new { result = "notfound" });

            var paymentInfo = paymentInfoRepository.GetAll();

            return Ok(new
            {
                result = res.ToList().Select(x => paymentViewModel.parse(paymentInfo, x))
            });
        }

        [HttpPost]
        public IHttpActionResult dnd_instantFile(dndViewModel model)
        {
            var res = instantDownload_SubjectsRepository.FindById(model.InstantDnd_subjectId);

            if (!(DateTime.Now < res.InstantDownloads.CreationDate.AddDays(3)))
                return Ok();

            string FileName = paymentViewModel
                             .get_fileDndPath
                             (
                                    res.InstantDownloads.PreviousYrQP.year,
                                    res.LevelId,
                                    res.InstantDownloads.ClassId,
                                    res.Subject.EventCode.ToLower()
                             );

            MemoryStream ms = new MemoryStream();
            string path = System.Web.Hosting.HostingEnvironment.MapPath("~" + FileName);
            using (FileStream source = File.Open(path, FileMode.Open))
            {
                // Copy source to destination.
                source.CopyTo(ms);
            }

            HttpContext.Current.Response.BinaryWrite(ms.ToArray());
            HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + FileName);
            HttpContext.Current.Response.StatusCode = 200;
            HttpContext.Current.Response.End();

            return Ok();
        }

        public PaymentController(
            IAccountRepository _accountRepository,
            IInstantDownloadsRepository _instantDownloadsRepository,
            IInstantDownload_SubjectsRepository _instantDownload_SubjectsRepository,
            IPaymentInfoRepository _paymentInfoRepository,
            IOrderRepository _orderRepository,
            IPreviousYrQPRepository _previousYrQPRepository,
            IClassRepository _classRepository,
            IEventRepository _eventRepository,
            IStateRepository _stateRepository,
            IResult_levelClassRepository _result_levelClassRepository,
            IInstantDownload_SubjectsMappingPRepository _instantDnd_mapingPRepository
            )
        {
            instantDnd_mapingPRepository = _instantDnd_mapingPRepository;
            accountRepository = _accountRepository;
            instantDownloadsRepository = _instantDownloadsRepository;
            instantDownload_SubjectsRepository = _instantDownload_SubjectsRepository;
            paymentInfoRepository = _paymentInfoRepository;
            orderRepository = _orderRepository;
            previousYrQPRepository = _previousYrQPRepository;
            classRepository = _classRepository;
            eventRepository = _eventRepository;
            stateRepository = _stateRepository;
            result_levelClassRepository = _result_levelClassRepository;
        }

    }
}
