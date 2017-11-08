using Newtonsoft.Json;
using SilverzoneERP.Entities.ViewModel.Site;
using SilverzoneERP.Data;
using SilverzoneERP.Entities;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Script.Serialization;
using SilverzoneERP.Entities.Models;

namespace SilverzoneERP.Api.api.Site
{
    [Authorize]          // to check user is authenticate or not
    public class OrderController : ApiController
    {
        IUserShippingAddressRepository userShippingAddressRepository;
        IOrderDetailRepository orderDetailRepository;
        IOrderRepository orderRepository;
        IAccountRepository accountRepository;
        IPaymentInfoRepository paymentInfoRepository;
        IErrorLogsRepository errorLogsRepository;
        ISiteConfigurationRepository siteConfigurationRepository;

        //  *********************   For User shipping address ************

        public IHttpActionResult get_shipping_Address_byUserId()
        {
            var userId = Convert.ToInt32(User.Identity.Name);           // to get forma authrntication current user login Info

            if (userId == 0)
                return NotFound();      // user not found > throw 404 error

            var shipping_address = shippingAdresModel.Parse(
                userShippingAddressRepository.GetByUserId(userId)
                );

            return Ok(new { result = shipping_address });
        }

        [HttpPost]
        public IHttpActionResult save_shipping_Address(shippingAdresModel _model)
        {
            string _result = string.Empty;
            int userId = Convert.ToInt32(User.Identity.Name);

            if (_model.Id == 0)
            {
                var model = shippingAdresModel.Parse(_model);       // return new 
                model.UserId = userId;
                model.create_date = userShippingAddressRepository.get_DateTime();

                _model.Id = userShippingAddressRepository.Create(model).Id;
            }
            else
            {
                var entity = userShippingAddressRepository.FindBy(
                x => x.Id == _model.Id &&
                x.UserId == userId              // dont write line > Convert.ToInt32(User.Identity.Name); // will throw error
                ).SingleOrDefault();            // we have unique adress id, so will get always 1 or 0 record

                shippingAdresModel.Parse(_model, entity);
                userShippingAddressRepository.Update(entity);
            }

            return Ok(new { result = _model.Id });
        }

        [HttpPost]
        public void remove_uesr_shipping_Address(int adresId)
        {
            var entity = userShippingAddressRepository.GetById(adresId);

            if (entity != null)
            {
                //_userShippingAddressRepository.Delete(entity);
                entity.Status = false;

                userShippingAddressRepository.Update(entity);
            }
        }

        //  *********************   For orders ************

        [HttpPost]
        public IHttpActionResult create_order(OrderViewModel model)
        {
            if (!ModelState.IsValid)
                return Ok(new { result = "error" });

            using (var transaction = orderRepository.BeginTransaction())
            {
                try
                {
                    var order = new Order();

                    OrderViewModel
                   .parse(model,
                          Convert.ToInt32(User.Identity.Name),
                          order,
                          siteConfigurationRepository.GetRecord());

                    order = orderRepository.Create(order);

                    order.OrderNumber = orderRepository.get_Order_Number(order.Id);
                    orderRepository.Update(order);

                    foreach (var entity in model.bookList)     // insert data in order detail table
                    {
                        var _model = new OrderDetail();
                        OrderViewModel.parse(entity, order.Id, _model);
                        orderDetailRepository.Create(_model, false);
                    }

                    orderDetailRepository.Save();
                    transaction.Commit();       // it must be there if want to save record :)

                    int userid = Convert.ToInt32(User.Identity.Name);
                    var points = accountRepository.GetById(userid).TotalPoint;
                    return Ok(new { result = "success", orderId = order.Id, quizPoints = points });
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    return Ok(new { result = "error" });
                }
            }
        }

        [HttpGet]
        public IHttpActionResult confirm_order(
            int oderId,
            int? Quiz_Points_Deduction = null
            )
        {
            var order = orderRepository.GetById(oderId);

            if (order == null)
                return Ok(new { status = "notfound" });

            var data = orderPriceViewModel.Parse(orderDetailRepository.GetByOrderId(oderId));

            var siteConfig = siteConfigurationRepository.GetRecord();
            if (!orderPriceViewModel.validate_orderAmount(data, siteConfig))     // validate amount of order
                return Ok(new { status = "notmatched" });            // if amount is not matched/validated

            var user = accountRepository.FindById(Convert.ToInt32(User.Identity.Name));
            if (Quiz_Points_Deduction > user.TotalPoint)
                return Ok(new { status = "error" });    // Redeem point is less than total points > so payment could not process

            user.TotalPoint -= Convert.ToInt32(Quiz_Points_Deduction) * 10;
            if (Quiz_Points_Deduction != null)
                accountRepository.Update(user);

            // ********************  Proceed to order confirmation  **********

            order.Quiz_Points_Deduction = Quiz_Points_Deduction;

            orderRepository.Update(order);

            var paymentInfo = paymentInfoRepository.FindBy_refrenceId(order.Id, Payment_refrenceType.Online_Books);

            // save payment details for online/offline
            if (paymentInfo == null)
                paymentInfoRepository.Create(paymentViewModel.parse(
                    order.Id,
                    Payment_refrenceType.Online_Books,
                    order.Total_Shipping_Amount + order.Total_Shipping_Charges));

            // new anonymoustype object  data
            var _result = new { OrderNumber = order.OrderNumber, OrderDate = order.OrderDate };
            return Ok(new
            {
                status = "success",
                result = _result
            });

        }

        [HttpPost]
        public void sendEmail(emailViewModel model)
        {
            //string path = HostingEnvironment.MapPath("~/templates/EmailTemplates/orderConfirmation_offline.html");
            orderRepository.sendEmail_Payment_Confirmation(model.HtmlTemplate, model.emailId);
        }

        [HttpGet]
        public IHttpActionResult get_location(string pincode)
        {
            var result = string.Empty;
            try
            {
                WebRequest myWebRequest = WebRequest.Create(string.Format("http://maps.googleapis.com/maps/api/geocode/json?address={0}&sensor=true", Uri.EscapeDataString(pincode)));

                using (var response = myWebRequest.GetResponse())
                {
                    using (var reader = new StreamReader(response.GetResponseStream()))
                    {
                        JavaScriptSerializer js = new JavaScriptSerializer();
                        var data = js.DeserializeObject(reader.ReadToEnd());
                        result = JsonConvert.SerializeObject(data);
                        //return Ok(1);
                    }
                }
            }

            catch (Exception ex)
            {
                errorLogsRepository.logError(ex);
            }
            return Ok(result);
        }


        [HttpGet]
        public IHttpActionResult get_orerInfo(string orderNumber)
        {
            var orders = orderDetailViewModel.Parse(
                orderRepository.GetByOrderNumber(orderNumber)
                );

            return Ok(new { result = orders });
        }

        // *****************  Constructors  ********************************

        public OrderController(
            IUserShippingAddressRepository _userShippingAddressRepository,
            IOrderDetailRepository _orderDetailRepository,
            IOrderRepository _orderRepository,
            IAccountRepository _accountRepository,
            IErrorLogsRepository _errorLogsRepository,
            IPaymentInfoRepository _paymentInfoRepository,
            ISiteConfigurationRepository _siteConfigurationRepository
            )
        {
            userShippingAddressRepository = _userShippingAddressRepository;
            orderDetailRepository = _orderDetailRepository;
            orderRepository = _orderRepository;
            accountRepository = _accountRepository;
            errorLogsRepository = _errorLogsRepository;
            paymentInfoRepository = _paymentInfoRepository;
            siteConfigurationRepository = _siteConfigurationRepository;
        }

    }
}
