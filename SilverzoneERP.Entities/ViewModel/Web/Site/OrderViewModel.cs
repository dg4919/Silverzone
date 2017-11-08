using SilverzoneERP.Entities.Models;
using System.Linq;
using System.Collections.Generic;
using System;

namespace SilverzoneERP.Entities.ViewModel.Site
{
    public class OrderViewModel
    {
        public int Shipping_addressId { get; set; }
        public decimal Total_Shipping_Amount { get; set; }
        public decimal Total_Shipping_Charges { get; set; }
        public countryType countryId { get; set; }

        public IEnumerable<bookViewModel> bookList { get; set; }

        public static void parse(
            OrderViewModel model, 
            long userId, 
            Order order,
            SiteConfiguration siteConfig)
        {
            order.UserId = userId;
            order.Shipping_addressId = model.Shipping_addressId;
            order.Total_Shipping_Amount = model.Total_Shipping_Amount;
            order.Total_Shipping_Charges = model.Total_Shipping_Charges;
            order.OrderDate = DateTime.UtcNow;
            order.Status = true;

            if (model.countryId == countryType.India)
            {
                order.First_Shipping_Charge = siteConfig.India_bookShiping_Charges1;
                order.Other_Shipping_Charge = siteConfig.India_bookShiping_Charges2;
            }
            else
            {
                order.First_Shipping_Charge = siteConfig.OutsideIndia_bookShiping_Charges1;
                order.Other_Shipping_Charge = siteConfig.OutsideIndia_bookShiping_Charges2;
            }
        }

        public static void parse(bookViewModel entity, long orderId, OrderDetail model)
        {
            model.Quantity = entity.Quantity;
            model.OrderId = orderId;
            model.bookType = entity.bookType;
            model.UnitPrice = entity.unitPrice;

            if (entity.bookType == bookType.Book)
                model.BookId = entity.BookId;
            else
                model.BundleId = entity.BookId;
        }

    }

    public class bookViewModel
    {
        public int BookId { get; set; }
        public int Quantity { get; set; }
        public decimal unitPrice { get; set; }
        public bookType bookType { get; set; }
    }

    public class orderPriceViewModel
    {
        public decimal Total_Shipping_Amount { get; set; }
        public decimal Total_Shipping_Charges { get; set; }

        public long First_Shipping_Charge { get; set; }
        public long Other_Shipping_Charge { get; set; }

        public IEnumerable<bookPriceViewModel> bookPrice { get; set; }


        public static orderPriceViewModel Parse(IEnumerable<OrderDetail> modelList)
        {
            var order = modelList.FirstOrDefault().Order;

            return new orderPriceViewModel()
            {
                Total_Shipping_Amount = order.Total_Shipping_Amount,
                Total_Shipping_Charges = order.Total_Shipping_Charges,
                First_Shipping_Charge = order.First_Shipping_Charge,
                Other_Shipping_Charge = order.Other_Shipping_Charge,
                //shipping_country = order.UserShippingAddress.CountryType,        // get values by navigation proprties
                bookPrice = modelList.Select(x => new bookPriceViewModel()
                {
                    BookPrice = x.bookType == bookType.Book ? x.Book.Price : x.Bundle.bundle_totalPrice,
                    //BookPrice = x.Book.Price,
                    Quantity = x.Quantity,
                    book_newPrice = x.Book != null && x.Book.ItemTitle_Master.BookCategory.CouponId.HasValue && x.bookType == bookType.Book
                                  ? getBook_newPrice(x.Book.Price, x.Book.ItemTitle_Master.BookCategory.Coupons)
                                  : 0
                })
            };
        }

        private static decimal getBook_newPrice(decimal bookPrice, Coupon coupon)
        {
            var sys_date = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")).Date;

            if (coupon.Status &&
                sys_date >= coupon.Start_time &&
                sys_date <= coupon.End_time)
            {
                if (coupon.DiscountType == CouponType.FlatDiscount)
                    return bookPrice - coupon.Coupon_amount;
                else
                    return bookPrice - (bookPrice * coupon.Coupon_amount) / 100;
            }

            return 0;       // if coupon is not valid return > 0  
        }


        public static bool validate_orderAmount(orderPriceViewModel modelList, SiteConfiguration config)
        {
            var shipping_amount = modelList.bookPrice.Sum(x => (x.book_newPrice != 0 ? x.book_newPrice : x.BookPrice) * x.Quantity);  // sum of list of each(sum * quantity)
            var item_quantity = modelList.bookPrice.Sum(x => x.Quantity);         // return sum of quantity in the list

            var shipping_charge = modelList.First_Shipping_Charge + (item_quantity - 1) * modelList.Other_Shipping_Charge;

            if ((modelList.Total_Shipping_Amount + modelList.Total_Shipping_Charges).Equals
                (shipping_amount + shipping_charge))
                return true;
            else
                return false;
        }
    }

    public class bookPriceViewModel
    {
        public decimal BookPrice { get; set; }
        public int Quantity { get; set; }
        public decimal book_newPrice { get; set; }
    }

    public class bundleWheight_ViewModel
    {
        public long dispatch_mId { get; set; }
        public IEnumerable<bundleWheight_Model> wheightModel { get; set; }

        public static Packet_BundleInfo parse(long dispatchId, bundleWheight_Model model)
        {
            return new Packet_BundleInfo()
            {
                dispatch_mId = dispatchId,
                PM_Id = model.PM_Id,
                Netwheight = model.Netwheight
            };
        }

    }

    public class bundleWheight_Model
    {
        public long PM_Id { get; set; }
        public decimal Netwheight { get; set; }
    }

}