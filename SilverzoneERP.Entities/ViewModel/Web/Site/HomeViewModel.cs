using SilverzoneERP.Entities.Models;
using System;

namespace SilverzoneERP.Entities.ViewModel.Site
{
    public class DownloadViewModel
    {
        public static string parse(
            downloadType type,
            string fileName,
            string folderName)
        {
            string path = "/Files/Downloads/";
            if (type == downloadType.Excel)
            {
                path += string.Format("Excel/{0}/{1}.xlsx",
                          folderName.ToUpper(),
                          fileName);
            }
            else if (type == downloadType.Pdf)
            {
                path += string.Format("Pdf/{0}/{1}.pdf",
                          folderName.ToUpper(),
                          fileName);
            }
            return path;
        }
    }

    public class resultViewModel
    {
        public levelType levelType { get; set; }
        public long result_eventId { get; set; }
        public string enrolmentNo { get; set; }

        public static dynamic parse(Result_L1 model, levelType levelTyp)
        {
            return new
            {
                model.StudName,
                eventName = string.Format("{0} - {1}",
                            model.ResultEvent.EventInfo.EventCode,
                            model.ResultEvent.ResultEventInfo.shortCode),
                model.Class,
                rollNo = model.NIORollNo,
                RawScore = levelTyp == levelType.Level_2 ? model.Result_L2.RawScore : model.RawScore,
                TotMarks = levelTyp == levelType.Level_2 ? model.Result_L2.TotMarks : model.TotMarks,
                olympiadRank = levelTyp == levelType.Level_2 ? model.Result_L2.AllIndiaRank : model.AllIndiaRank,
                ClassRank = levelTyp == levelType.Level_2 ? model.Result_L2.ClassRank : model.ClassRank
            };
        }

    }

    public class subscribe_updatesViewModel
    {
        public string UserName { get; set; }
        public long classId { get; set; }
        public string Address { get; set; }
        public string EmailId { get; set; }
        public long Mobile { get; set; }
        public string City { get; set; }
        public long stateId { get; set; }
        public long countryId { get; set; }

        public static void parse(subscribe_updatesViewModel vm, Subscribe_Updates model)
        {
            model.UserName = vm.UserName;
            model.Address = vm.Address;
            model.classId = vm.classId;
            model.EmailId = vm.EmailId;
            model.Mobile = vm.Mobile;
            model.StateId = vm.stateId;
            model.City = vm.City;
            model.CountryId = vm.countryId;
            model.Status = true;
        }
    }

    public class school_registrationViewModel
    {
        public string UserName { get; set; }
        public string Address { get; set; }
        public string EmailId { get; set; }
        public long Mobile { get; set; }
        public string City { get; set; }
        public long stateId { get; set; }
        public long countryId { get; set; }
        public genderType GenderId { get; set; }
        public int PinCode { get; set; }
        public string AuthCode { get; set; }
        public school_ProfileType ProfileId { get; set; }
        public string schName { get; set; }
        public string HtmlTemplate { get; set; }

        public static void parse(school_registrationViewModel vm, RegisterSchool model)
        {
            model.UserName = vm.UserName;
            model.Address = vm.Address;
            model.EmailId = vm.EmailId;
            model.Mobile = vm.Mobile;
            model.StateId = vm.stateId;
            model.City = vm.City;
            model.CountryId = vm.countryId;
            model.Gender = vm.GenderId;
            model.PinCode = vm.PinCode;
            model.AuthCode = vm.AuthCode;
            model.ProfileType = vm.ProfileId;
            model.SchoolName = vm.schName;
            model.Status = true;
        }
    }


    public class enquiryViewModel
    {
        public string UserName { get; set; }
        public string EmailId { get; set; }
        public string Mobile { get; set; }
        public string Country { get; set; }
        public string Description { get; set; }
        public string HtmlTemplate { get; set; }

        public static Enquiry parse(enquiryViewModel vm)
        {
            return new Enquiry()
            {
                UserName = vm.UserName,
                EmailId = vm.EmailId,
                Description = vm.Description,
                Country = vm.Country,
                Mobile = vm.Mobile,
                QueryDate = DateTime.Now,
                Status = true
            };
        }
    }

    public class rcViewModel
    {
        public string UserName { get; set; }
        public string EmailId { get; set; }
        public string Mobile { get; set; }
        public string Qualification { set; get; }
        public int Age { get; set; }          // 2 ways making value nullable
        public genderType GenderId { get; set; }       // get from Enum

        public string Address { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public int Pincode { get; set; }

        public static ERPuser parse(rcViewModel vm)
        {
            return new ERPuser
            {                
                UserName = vm.UserName,
                EmailID = vm.EmailId,
                MobileNumber = vm.Mobile,
                Qualification = vm.Qualification,
                Age = vm.Age,
                GenderType = vm.GenderId,
                UserAddress = vm.Address,
                Country = vm.Country,
                City = vm.City,
                State = vm.State,
                Pincode = vm.Pincode,
                RoleId = 10,
                SrcFrom = "online",
                Status = true
            };
        }
            
    }

}