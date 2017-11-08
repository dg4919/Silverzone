using SilverzoneERP.Entities.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Web;

namespace SilverzoneERP.Entities.ViewModel.Site
{

    public class loginModel
    {
        [Required]
        public string userName { get; set; }
        public string html_template { get; set; }

        // input type use to check > user enter emailId/mobile at client side
        // "email" for EmailId OR  "mobile" for mobile No
        [Required]
        public verificationMode verificationMode { get; set; }
    }

    public class LoginViewModel : loginModel
    {
        [Required]
        public string Password { get; set; }

        public string CurrentVersion { get; set; }
        public string FireBaseToken { get; set; }

        public Nullable<long> RoleId { set; get; }
    }

    public class OTPViewModel
    {
        [Required]
        public string mobileNumber { get; set; }
        public int sms_OTP { get; set; }
    }

    public class RegisterViewModel : OTPViewModel
    {
        public int RoleId { get; set; }
        public string Password { get; set; }
    }

    public class Register_newUser_ViewModel
    {
        public int RoleId { get; set; }
        [Required]
        public string userName { get; set; }
        [Required]
        public string emailId { get; set; }
        [Required]
        public string mobileNo { get; set; }
        [Required]
        public string password { get; set; }

        public static void parse(Register_newUser_ViewModel vm, User model)
        {
            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());

            var ipAddress = host
                .AddressList
                .FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork);

            model.UserName = vm.userName;
            model.EmailID = vm.emailId;
            model.MobileNumber = vm.mobileNo;
            model.Password = vm.password;
            model.Browser = HttpContext.Current.Request.Browser.Browser;
            model.IPAddress = ipAddress.ToString();
            model.OperatingSystem = Environment.OSVersion.ToString();
            model.Location = RegionInfo.CurrentRegion.DisplayName;
            model.CreationDate = DateTime.UtcNow;
            model.UpdationDate = DateTime.UtcNow;
            model.RoleId = vm.RoleId;
            model.Status = true;
        }
    }

    public class passwordRecoveryViewModel : loginModel
    {
        public long userId { get; set; }
        public int OTP_code { get; set; }
        public verficationType verficationType { get; set; }

        public string emailSubject { get; set; }
    }
}
