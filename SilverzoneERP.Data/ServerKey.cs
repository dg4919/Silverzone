using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilverzoneERP.Data
{
    public class ServerKey
    {
        public const string serverapikey="AAAAB51RaO0:APA91bE3zIvtxrZ5YbMcEHaPZfP4Mu6bfwmPd4FD95MVQIqgn5ah1VTkkqzQdciX-Vq5wJvHsJfCggLKxOnD6JKhVjbwLef7LWGz5-dwc6HnfkzWczlK2opIuisCYZjFQ3iRAky003DO";
        //public const string serverapikey = "AIzaSyAq1lp2P9f_Kscv9H1IGIIs-Ez8qk1YzfM";
        public const string senderId = "32704129261";
        public static string EventYear = (DateTime.Now.Month < 4 ? ((DateTime.Now.Year - 1) + "-" + DateTime.Now.Year.ToString().Substring(2, 2)) : (DateTime.Now.Year + "-" + (DateTime.Now.Year + 1).ToString().Substring(2, 2)));
        public static int Event_Year = (DateTime.Now.Month < 4 ? ((DateTime.Now.Year - 1) + DateTime.Now.Year) : (DateTime.Now.Year +  (DateTime.Now.Year + 1));
    }
}
