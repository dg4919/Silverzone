using SilverzoneERP.Entities.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace SilverzoneERP.Entities.ViewModel.Site
{
    public class ccaRequest
    {
        public string encResp { get; set; }
        public string orderNo { get; set; }
    }

    public class cccaAuth
    {
        public string workingKey { get; set; }
        public string strAccessCode { get; set; }

        public cccaAuth()
        {
            if (ConfigurationManager.AppSettings["isProduction"] == "True")
            {
                workingKey = ConfigurationManager.AppSettings["ccaLive_workingKey"];
                strAccessCode = ConfigurationManager.AppSettings["ccaLive_accessCode"];
            }
            else
            {
                workingKey = ConfigurationManager.AppSettings["ccaTesting_workingKey"];
                strAccessCode = ConfigurationManager.AppSettings["ccaTesting_accessCode"];
            }
        }
    }

    public class payment_paramViewModel
    {
        public string billing_name { get; set; }
        public string billing_email { get; set; }
        public string billing_tel { get; set; }
        public string billing_address { get; set; }
        public string billing_city { get; set; }
        public string billing_state { get; set; }
        public string billing_country { get; set; }
        public string billing_zip { get; set; }
        public decimal amount { get; set; }
        public string order_id { get; set; }

        public string merchant_id { get; } = ConfigurationManager.AppSettings["cca_mearchantId"];
        public string currency { get; } = "INR";
        public string redirect_url { get; set; }
        public string cancel_url { get; set; }

        public static string GetQueryString(object obj)
        {
            var result = new List<string>();
            var props = obj.GetType().GetProperties().Where(p => p.GetValue(obj, null) != null);
            foreach (var p in props)
            {
                var value = p.GetValue(obj, null);
                var enumerable = value as ICollection;
                if (enumerable != null)
                {
                    result.AddRange(from object v in enumerable select string.Format("{0}={1}", p.Name, HttpUtility.UrlEncode(v.ToString())));
                }
                else
                {
                    result.Add(string.Format("{0}={1}", p.Name, HttpUtility.UrlEncode(value.ToString())));
                }
            }
            return string.Join("&", result.ToArray());
        }

    }

    public class paymentViewModel
    {
        public string billing_name { get; set; }
        public string billing_email { get; set; }
        public long billing_tel { get; set; }
        public string billing_address { get; set; }
        public string billing_city { get; set; }
        public string billing_state { get; set; }
        public string billing_country { get; set; }
        public int billing_zip { get; set; }
        public decimal amount { get; set; }

        public int classId { get; set; }
        public int year { get; set; }
        public int[] L1_subjects { get; set; }
        public int[] L2_subjects { get; set; }

        public static InstantDownloads parse(paymentViewModel vm)
        {
            return new InstantDownloads()
            {
                UserName = vm.billing_name,
                Address = vm.billing_address,
                City = vm.billing_city,
                ClassId = vm.classId,
                Country = vm.billing_country,
                EmailId = vm.billing_email,
                Pincode = vm.billing_zip,
                State = vm.billing_state,
                YearId = vm.year,
                Mobile = vm.billing_tel,
            };
        }

        public static PaymentInfo parse(
            long RefrenceId,
            Payment_refrenceType type,
            decimal Amount
            )
        {
            return new PaymentInfo()
            {
                RefrenceId = RefrenceId,
                Payment_Status = "Pending",
                RefrenceType = type,
                Amount = Amount,
                //Status = true  status does not matter here
            };
        }

        public static void parse(
            string payment_status,
            string tracking_id,
            string bank_ref_no,
            ref PaymentInfo paymentInfo)
        {
            paymentInfo.Payment_Status = payment_status;
            paymentInfo.Tracking_Id = tracking_id;
            paymentInfo.Bank_Ref_No = bank_ref_no;
        }

        public static InstantDownload_Subjects parse(
            long Id,
            long levelId,
            int subjectId)
        {
            return new InstantDownload_Subjects()
            {
                InstantDownloadId = Id,
                LevelId = levelId,
                SubjectId = subjectId
            };
        }


        public static string get_fileDndPath(
            int previousYrQPId,
            long levelId,
            long classId,
            string subjectName)
        {
            string className = string.Empty;
            switch (classId)
            {
                case 1:
                    className = classId + "st";
                    break;
                case 2:
                    className = classId + "nd";
                    break;
                case 3:
                    className = classId + "rd";
                    break;
                default:
                    className = classId + "th";
                    break;
            }

            var str = string.Format("/Files/InstantDndPdf/{0}/Level-{1}/{2}/{3}.pdf",
                          previousYrQPId,
                          levelId,
                          subjectName,
                          className
                          );
            return str;
        }

        public static dynamic parse(
        IQueryable<PaymentInfo> paymentInfo,
        InstantDownloads x)
        {
            if (paymentInfo.Any(z => z.RefrenceId == x.Id
                                   && z.RefrenceType == Payment_refrenceType.Instant_Downloads
                                   && z.Payment_Status == "Success"))
            {
                return new
                {
                    instantDndId = x.Id,
                    x.Class.className,
                    x.OrderId,
                    YearId = x.PreviousYrQP.year,
                    x.CreationDate,
                    subjects = x.InstantDownload_Subjects
                     .Select(a => new
                     {
                         a.Id,
                         subjectName = a.Subject.SubjectName,
                         ExpireDate = x.CreationDate.AddDays(3),
                         isExpired = DateTime.Now > x.CreationDate.AddDays(3) ? true : false,
                     })
                };
            }
            return null;
        }
    }

    public class dndViewModel
    {
        public long InstantDnd_subjectId { get; set; }
    }


}