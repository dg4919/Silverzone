using System.Configuration;
using System.IO;
using System.Net;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Silverzone.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(FormCollection form)
        {
            if (Request.Form.Count != 0)
            {
                var data = new
                {
                    encResp = Request.Form["encResp"],
                    orderNo = Request.Form["orderNo"]
                };

                string url = "api/Site/Payment/paymentResponse";
                if (ConfigurationManager.AppSettings["isProduction"] == "True")
                    url = ConfigurationManager.AppSettings["prodUrl"] + url;
                else
                    url = ConfigurationManager.AppSettings["testUrl"] + url;

                var httpRequest = WebRequest.Create(url);
                httpRequest.Method = "POST";
                httpRequest.ContentType = "application/json";

                JavaScriptSerializer serializer = new JavaScriptSerializer();
                using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
                {
                    string json = serializer.Serialize(data);
                    streamWriter.Write(json);
                    streamWriter.Close();
                }

                // write below code to call ajax method
                Stream dataStream = httpRequest.GetRequestStream();
                WebResponse tResponse = httpRequest.GetResponse();
                dataStream = tResponse.GetResponseStream();
                StreamReader tReader = new StreamReader(dataStream);
                var sResponseFromServer = tReader.ReadToEnd();
            }
            return View();
        }

    }
}