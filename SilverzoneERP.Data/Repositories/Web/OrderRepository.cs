using System;
using System.Collections.Generic;
using System.Linq;
using SilverzoneERP.Entities;
using SilverzoneERP.Context;
using SilverzoneERP.Entities.Models;

namespace SilverzoneERP.Data
{
    public class OrderRepository : BaseRepository<Order>, IOrderRepository
    {
        public OrderRepository(SilverzoneERPContext dbcontext) : base(dbcontext) { }

        public Order GetById(long id)
        {
            return FindBy(x => x.Id == id).FirstOrDefault();
        }
        public Order GetByOrderNumber(string id)
        {
            return FindBy(x => x.OrderNumber == id).FirstOrDefault();
        }
        public IEnumerable<Order> GetByOrderDate(DateTime orderDate)
        {
            return FindBy(x => x.OrderDate >= orderDate).AsEnumerable();
        }

        public string get_Order_Number(long orderId)
        {
            int order_number = 1000000;

            var order_date = string.Format("{0: yyyyMMdd}", get_DateTime()).Trim();

            return (string.Format("ORD/SZ{0}/{1}", order_date, order_number + orderId));
        }

        public bool sendEmail_Payment_Confirmation(string emailTemplate, string emailId)
        {
            ClassUtility.sendMail(emailId, "Order Confirmation - Silverzone", emailTemplate, emailSender.emailInfo);
            return true;
        }

        public IEnumerable<Order> GetByuserId(int userId)
        {
            return FindBy(x => x.UserId == userId);
        }

        public Order GetByuser_andOrderId(int orderId, int userId)
        {
            return _dbset.SingleOrDefault(x => x.Id == orderId && x.UserId == userId);
        }

        public bool send_sms_orderConfirmation(string mobileNo, string orderNumber, decimal orderAmount, string orderTrackLink)
        {
            try
            {
                var msg = string.Format(
                    smsTemplates.order_confirmation,
                    orderNumber,
                    orderAmount,
                    orderTrackLink);

                return ClassUtility.send_message(mobileNo, msg);
            }
            catch (Exception)
            {
                return false;
            }
        }

    }

    public class OrderDetailRepository : BaseRepository<OrderDetail>, IOrderDetailRepository
    {
        public OrderDetailRepository(SilverzoneERPContext dbcontext) : base(dbcontext) { }

        public OrderDetail GetById(int id)
        {
            return FindBy(x => x.Id == id).FirstOrDefault();
        }
        public IEnumerable<OrderDetail> GetByOrderId(int id)
        {
            return _dbset.Where(x => x.OrderId == id).AsEnumerable();
        }


    }

    public class OrderStatusReasonRepository : BaseRepository<OrderStatusReason>, IOrderStatusReasonRepository
    {
        public OrderStatusReasonRepository(SilverzoneERPContext dbcontext) : base(dbcontext) { }
    }

    public class PaymentInfoRepository : BaseRepository<PaymentInfo>, IPaymentInfoRepository
    {
        public PaymentInfoRepository(SilverzoneERPContext dbcontext) : base(dbcontext) { }

        public PaymentInfo FindBy_refrenceId(long refrenceId, Payment_refrenceType type)
        {
            return _dbset.SingleOrDefault(x => x.RefrenceId == refrenceId
                                            && x.RefrenceType == type);
        }
    }

}
