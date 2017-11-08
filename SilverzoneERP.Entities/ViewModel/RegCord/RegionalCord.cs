using SilverzoneERP.Entities.Models;
using System;
using System.Collections.Generic;

namespace SilverzoneERP.Entities.ViewModel.RegCord
{
    public class SearchRC_ViewModel
    {
        public long? StateId { get; set; }
        public long? DistrictId { get; set; }
        public long? CityId { get; set; }

        public static dynamic parse(Models.School school)
        {
            return new
            {
                school.Id,
                school.SchName,
                school.SchCode,
                school.SchAddress,
                school.City.CityName,
                school.State.StateName,
                school.District.DistrictName
            };
        }
    }

    public class RcSchools_ViewModel
    {
        public long RcId { get; set; }
        public ICollection<long> schIds { get; set; }
        public bool addRescords { get; set; }
    }

    public class RcSchool_Visits_ViewModel
    {
        public long rcId { get; set; }
        public long schId { get; set; }
        public VisitType visitType { get; set; }
        public DateTime visitDate { get; set; }
        public DateTime? followupDate { get; set; }

        public string contactPerson { get; set; }
        public string contactMobile { get; set; }

        public string remarks { get; set; }
        public IEnumerable<RcSchool_VisitEvents_ViewModel> rcEventInfo { get; set; }

        public static RcSchool_Visits parse(RcSchool_Visits_ViewModel vm)
        {
            return new RcSchool_Visits()
            {
                RCId = vm.rcId,
                SchoolId = vm.schId,
                VisitType = vm.visitType,
                VisitDate = vm.visitDate,
                FoolowUpDate = vm.followupDate,
                Contact_Peron_Mobile = vm.contactMobile,
                Contact_Peron_Name = vm.contactPerson,
                Remarks = vm.remarks,
                Status = true
            };
        }

        public static RcSchool_VisitsInfo parse(RcSchool_VisitEvents_ViewModel vm, long rcVisitId)
        {
            return new RcSchool_VisitsInfo()
            {
                RcSchool_VisitId = rcVisitId,
                EventId = vm.eventId,
                VisitStatus = vm.visitStatus,
                Status = true
            };
        }
    }

    public class RcSchool_VisitEvents_ViewModel
    {
        public long eventId { get; set; }
        public VisitStatus visitStatus { get; set; }
    }

    public class update_RcSchool_Visits_ViewModel
    {
        public long rcVisitId { get; set; }
        public VisitType visitType { get; set; }
        public VisitStatus visitStatus { get; set; }
        public string remarks { get; set; }

        public static RcSchool_Visits parse(
            update_RcSchool_Visits_ViewModel vm,
            RcSchool_Visits model)
        {
            model.VisitType = vm.visitType;
            //model.VisitStatus = vm.visitStatus;
            model.Remarks = vm.remarks;

            return model;
        }

    }

}
