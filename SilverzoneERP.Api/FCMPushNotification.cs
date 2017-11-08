
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

using System.Net;
using System.Text;
using System.Web.Script.Serialization;

namespace SilverzoneERP.Api
{
    public class FCMPushNotification
    {
        public FCMPushNotification()
        {
            //
            // TODO: Add constructor logic here
            //
        }


        public static void SendPushNotification(string token, string serverkeyApi, string senderId, string Role, string title, string message,string imgUrl)
        {
            try
            {
                WebRequest tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
                tRequest.Method = "post";
                tRequest.ContentType = "application/json";
                string dmessage = "{ \"payload\": {\"team\":\"India\", " +
                 "\"Score\":\"5.6\"}}";

                Dictionary<string, string> values = new Dictionary<string, string>();
                values.Add("team", "india");
                values.Add("score", "5.6");

                string json = JsonConvert.SerializeObject(values);
                // {
                //   "key1": "value1",
                //   "key2": "value2"
                // }

                var sdata = new
                {

                    to = token,
                    notification = new
                    {
                        body = message,
                        title = title,
                        sound = "Enabled",
                        icon = "http://www.silverzone.org/images/Events/iio_banner.png"
                    },
                    data = new Dictionary<string, string>
                    {
                        {"Title",title },
                        {"Message",message },
                        {"isBackground","false"},
                        {"image",imgUrl },
                        {"timestamp",DateTime.Now.ToString("yyyyMMddHHmmssfff") },
                        { "payload",  json }
                    }
                   

                };

                var serializer = new JavaScriptSerializer();
                var jsondata = serializer.Serialize(sdata);
                Byte[] byteArray = Encoding.UTF8.GetBytes(jsondata);
                tRequest.Headers.Add(string.Format("Authorization: key={0}", serverkeyApi));
                tRequest.Headers.Add(string.Format("Sender: id={0}", senderId));
                tRequest.ContentLength = byteArray.Length;
                using (Stream dataStream = tRequest.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                    using (WebResponse tResponse = tRequest.GetResponse())
                    {
                        using (Stream dataStreamResponse = tResponse.GetResponseStream())
                        {
                            using (StreamReader tReader = new StreamReader(dataStreamResponse))
                            {
                                String sResponseFromServer = tReader.ReadToEnd();
                                string str = sResponseFromServer;
                            }
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }

        }


        //public string PushNotification(string deviceId, string serverapikey,string senderId, string message)
        //{

        //    var value = message;
        //    WebRequest tRequest;
        //    tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
        //    tRequest.Method = "post";
        //    tRequest.ContentType = " application/json";
        //    tRequest.Headers.Add(string.Format("Authorization: key={0}", serverapikey));

        //    tRequest.Headers.Add(string.Format("Sender: id={0}", senderId));

        //    string postData = "collapse_key=score_update&time_to_live=108&delay_while_idle=1&data.message=" + value + "&data.time=" + System.DateTime.Now.ToString() + "&registration_id=" + deviceId + "";
        //    Console.WriteLine(postData);
        //    Byte[] byteArray = Encoding.UTF8.GetBytes(postData);
        //    tRequest.ContentLength = byteArray.Length;

        //    Stream dataStream = tRequest.GetRequestStream();
        //    dataStream.Write(byteArray, 0, byteArray.Length);
        //    dataStream.Close();

        //    WebResponse tResponse = tRequest.GetResponse();

        //    dataStream = tResponse.GetResponseStream();

        //    StreamReader tReader = new StreamReader(dataStream);

        //    String sResponseFromServer = tReader.ReadToEnd();


        //    tReader.Close();
        //    dataStream.Close();
        //    tResponse.Close();
        //    return sResponseFromServer;
        //}
    }
   
}