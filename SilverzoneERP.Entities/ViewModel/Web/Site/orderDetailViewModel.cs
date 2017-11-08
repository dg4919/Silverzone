using SilverzoneERP.Entities.Constant;
using SilverzoneERP.Entities.Models;
using System.Collections.Generic;
using System.Linq;

namespace SilverzoneERP.Entities.ViewModel.Site
{
    public class orderDetailViewModel
    {
        public long OrderId { get; set; }
        public string OrderNumber { get; set; }
        public decimal Total_Shipping_Amount { get; set; }
        public decimal Total_Shipping_Charges { get; set; }
        public System.DateTime OrderDate { get; set; }
        public int? Quiz_Points_Deduction { get; set; }

        public long First_Shipping_Charge { get; set; }
        public long Other_Shipping_Charge { get; set; }

        public shippingAdresModel shipingAdres { get; set; }
        public IEnumerable<bookModel> books { get; set; }

        // method to parse data :)
        public static orderDetailViewModel Parse(Order model)
        {
            return new orderDetailViewModel()
            {
                OrderId = model.Id,
                OrderNumber = model.OrderNumber,
                Total_Shipping_Amount = model.Total_Shipping_Amount,
                Total_Shipping_Charges = model.Total_Shipping_Charges,
                Quiz_Points_Deduction = model.Quiz_Points_Deduction,
                OrderDate = model.OrderDate,
                First_Shipping_Charge = model.First_Shipping_Charge,
                Other_Shipping_Charge = model.Other_Shipping_Charge,
                shipingAdres = shippingAdresModel.Parse(model.UserShippingAddress),
                books = bookModel.Parse(model.OrderDetails)
            };
        }
    }

    public class bookModel
    {
        public string bookImage { get; set; }
        public string bookTitle { get; set; }
        public string bookPublisher { get; set; }
        public string bookCategory { get; set; }
        public string className { get; set; }
        public string subject { get; set; }
        public decimal bookPrice { get; set; }
        public int bookQuantity { get; set; }
        public bookType bookType { get; set; }
        public IEnumerable<dynamic> bundle_booksInfo { get; set; }       // contaoin anonymous list of parmas

        public static IEnumerable<bookModel> Parse(ICollection<OrderDetail> model)
        {
            return model.Select(x => getModel(x));
        }

        private static bookModel getModel(OrderDetail model)
        {
            var bookModel = new bookModel();
            bookModel.bookPrice = model.UnitPrice;
            bookModel.bookQuantity = model.Quantity;
            bookModel.bookType = model.bookType;

            if (model.Book != null)
            {
                bookModel.bookImage = image_urlResolver.getBookImage(model.Book.BookImage);
                bookModel.bookTitle = model.Book.Title;
                bookModel.bookPublisher = model.Book.Publisher;
                bookModel.bookCategory = model.Book.ItemTitle_Master.BookCategory.Name;    // navigation property to fetch data
                bookModel.className = model.Book.ItemTitle_Master.Class.className;
                bookModel.subject = model.Book.ItemTitle_Master.Subject.SubjectName;
                bookModel.bundle_booksInfo = null;
            }
            else
            {
                bookModel.bookImage = "Images/bundle.jpg";
                bookModel.bookTitle = model.Bundle.Name;
                bookModel.bookPublisher = "Silverzone";
                bookModel.bookCategory = "Bundle";
                bookModel.className = model.Bundle.Class.className;
                bookModel.subject = "Books Bundle";
                // contains the name of books inside a bundle
                bookModel.bundle_booksInfo = model.Bundle.bundle_details.Select(x => new
                {
                    SubjectName = x.book.ItemTitle_Master.Subject.SubjectName,
                    CategoryName = x.book.ItemTitle_Master.BookCategory.Name
                });
            }
            return bookModel;
        }

    }

    public class shippingAdresModel
    {
        public long Id { get; set; }
        public string Username { get; set; }
        public string Address { get; set; }
        public string PinCode { get; set; }
        public countryType Country { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string City { get; set; }
        public string State { get; set; }

        public static shippingAdresModel Parse(UserShippingAddress model)
        {
            return new shippingAdresModel()
            {
                Username = model.Username,
                Address = model.Address,
                PinCode = model.PinCode,
                City = model.City,
                Country = model.CountryType,
                Email = model.Email,
                Mobile = model.Mobile,
                State = model.State
            };
        }

        // method overloading
        public static IEnumerable<shippingAdresModel> Parse(IEnumerable<UserShippingAddress> modelList)
        {
            return modelList.Select(x => new shippingAdresModel()
            {
                Id = x.Id,
                Username = x.Username,
                Address = x.Address,
                PinCode = x.PinCode,
                City = x.City,
                Country = x.CountryType,
                Email = x.Email,
                Mobile = x.Mobile,
                State = x.State
            });
        }

        public static UserShippingAddress Parse(shippingAdresModel model)
        {
            return new UserShippingAddress()
            {
                Username = model.Username,
                Address = model.Address,
                PinCode = model.PinCode,
                City = model.City,
                CountryType = model.Country,
                Email = model.Email,
                Mobile = model.Mobile,
                State = model.State,
                Status = true
            };
        }

        public static void Parse(
            shippingAdresModel vm,
            UserShippingAddress model   // (UserShippingAddress) class is refrence type so change made to directly model
            )
        {
            model.Username = vm.Username;
            model.Address = vm.Address;
            model.PinCode = vm.PinCode;
            model.City = vm.City;
            model.CountryType = vm.Country;
            model.Email = vm.Email;
            model.Mobile = vm.Mobile;
            model.State = vm.State;
        }

    }


}