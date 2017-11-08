using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SilverzoneERP.Entities.Models.Common;
using SilverzoneERP.Entities.Models;

namespace SilverzoneERP.Entities.ViewModel.School
{
    public class QuestionAnswer
    {
        public string Question { set; get; }
        public string Answer { set; get; }        
    }
    public class LoginViewModel
    {
        [Required]
        public string EmailID { set; get; }
        [Required]
        public string Password { get; set; }

        public long RoleId { get; set; }
    }

    public class ViewModel
    {
        public long Id { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string EmailID { set; get; }
        public string ProfilePic { get; set; }
        [Required]
        public int GenderType { get; set; }
        public string UserAddress { get; set; }
        public string MobileNumber { get; set; } 
        [Required] 
        public long RoleId { set; get; }

        [DataType(DataType.Date)]
        public DateTime DOB { set; get; }

        [Required]
        public string SrcFrom { set; get; }

        [MaxLength(50)]
        public string Qualification { set; get; }

        [MaxLength(50)]
        public string OtherQualification { set; get; }

        [MaxLength(50)]
        public string Profession { set; get; }

        [MaxLength(50)]
        public string HowDid { set; get; }
    } 
    
    public class RolePermissionViewModel
    {
        public long Id { set; get; }
        public string RoleName { set; get; }
        public string RoleDescription { set; get; }
        public List<Forms> Forms { set; get; }
    }

    public class Forms
    {
        public long FormId { set; get; }       
        public Permission Permission { set; get; }
    }

    public class UserRoleViewModel
    {
        public long Id { set; get; }
        public List<multiSelect> Users { set; get; }
        public int RoleId { set; get; }
    }

    public class UserPermissionViewModel
    {
        public long UserId { set; get; }
        public List<Forms> Forms { set; get; }                
    }  

    public class multiSelect
    {
        public long Id { set; get; }
        public bool ticked { set; get; }
    }

    public class SchMngtViewModel
    {

        public long SchId { set; get; }
        [Required]
        [MaxLength(50)]
        public string SchName { set; get; }
        
        [MaxLength(50)]
        public string SchEmail { set; get; }
        [Required]
        [MaxLength(150)]
        public string SchAddress { set; get; }
        [MaxLength(150)]
        public string SchAltAddress { set; get; }
        public long SchPinCode { set; get; }
        public long CityId { set; get; }
        //public string CityName { set; get; }
        public long DistrictId { set; get; }
        public long StateId { set; get; }
        public long ZoneId { set; get; }
        public long CountryId { set; get; }
        public Nullable<long> SchPhoneNo { set; get; }
        public Nullable<long> SchFaxNo { set; get; }
        public string SchWebSite { set; get; }
        [MaxLength(50)]
        public string SchBoard { set; get; }
        [MaxLength(50)]
        public string SchAffiliationNo { set; get; }
       // public City NewCity { set; get; }
        public BlackListedViewModel BlackListed { set; get; }      
        public bool IsOtherContact { set; get; }
        public ContactViewModel OtherContact { set; get; }
        public List<ContactViewModel> Contact_List { set; get; }
        //public List<EventCoOrdViewModel> Events { set; get; }

       
        public Nullable<long> SchCategoryId { set; get; }
        
        public Nullable<long> SchGroupId { set; get; }
    }

    public class ContactViewModel
    {
        public long ContactId { set; get; }
        public long DesgId { set; get; }
        public long TitleId { set; get; }
        [Required]
        public string ContactName { set; get; }
        
        public long ContactMobile { set; get; }
        public Nullable<long> ContactAltMobile1 { set; get; }
        public Nullable<long> ContactAltMobile2 { set; get; }

        
        [MaxLength(50)]
        public string ContactEmail { set; get; }

        [MaxLength(50)]
        public string ContactAltEmail1 { set; get; }

        [MaxLength(50)]
        public string ContactAltEmail2 { set; get; }
        public short ContactType { set; get; }                
        public bool AddressTo { set; get; }
    }
  
    public class BlackListedViewModel
    {
        public bool IsBlocked { set; get; }
        [MaxLength(200)]
        public string BlackListedRemarks { set; get; }     
    }

    public class EnrollmentOrderViewModel
    {                
        public long Id { set; get; }
        public long EventManagementId { set; get; }       
        public long TotlaEnrollment { set; get; }
        //public List<EnrollmentOrderSummary> EnrollmentOrderDetail { set; get; }
        public Nullable<long> ExaminationDateId { set; get; }
       
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<DateTime> ChangeExamDate { set; get; }

        [Required]
        public string SrcFrom { set; get; }

        [Required]
        public bool IsConfirm { set; get; }

        public virtual List<EnrollmentOrderDetail> EnrollmentOrderDetail { set; get; }
    }

    public class EnrollmentOrderSummary
    {
        public long ClassId{ set; get; }
        public string ClassName { set; get; }
        public long No_Of_Student { set; get; }
    }

    public class SchoolVerify
    {
        public Guid Guid { set; get; }=Guid.NewGuid();
        [Required]        
        public long SchCode { set; get; }
        [Required]
        [MaxLength(50)]
        public string SchName { set; get; }
        [MaxLength(50)]
        public string SchEmail { set; get; }
        [Required]
        [MaxLength(150)]
        public string SchAddress { set; get; }
        [MaxLength(150)]
        public string SchAltAddress { set; get; }
        public long SchPinCode { set; get; }        
        public string City { set; get; }
        public Nullable<long> SchPhoneNo { set; get; }
        public Nullable<long> SchFaxNo { set; get; }
        public string SchWebSite { set; get; }
        [MaxLength(50)]
        public string SchBoard { set; get; }
        [MaxLength(50)]
        public string SchAffiliationNo { set; get; }            
    }

    public class ZoneVerify
    {
        public Guid Guid { set; get; } = Guid.NewGuid();
        [Required]
        [MaxLength(100)]
        public string ZoneName { set; get; }

        [Required]
        [MaxLength(100)]
        public string Country { set; get; }
    }
    public class StateVerify
    {
        public Guid Guid { set; get; } = Guid.NewGuid();

        [Required]
        [MaxLength(100)]
        public string StateName { set; get; }

        [Required]
        [StringLength(3, MinimumLength = 2)]
        public string StateCode { get; set; }

        [Required]
        [MaxLength(100)]
        public string Zone { set; get; }

        [Required]
        [MaxLength(100)]
        public string Country { set; get; }
    }

    public class DistrictVerify
    {
        public Guid Guid { set; get; } = Guid.NewGuid();

        [Required]
        [MaxLength(100)]
        public string DistrictName { set; get; }

        [Required]
        [MaxLength(100)]
        public string State { set; get; }
        
        [Required]
        [MaxLength(100)]
        public string Zone { set; get; }

        [Required]
        [MaxLength(100)]
        public string Country { set; get; }
    }

    public class CityVerify
    {
        public Guid Guid { set; get; } = Guid.NewGuid();

        [Required]
        [MaxLength(100)]
        public string CityName { set; get; }

        [Required]
        [MaxLength(100)]
        public string District { set; get; }

        [Required]
        [MaxLength(100)]
        public string State { set; get; }

        [Required]
        [MaxLength(100)]
        public string Zone { set; get; }

        [Required]
        [MaxLength(100)]
        public string Country { set; get; }
    }

    public class CreateLot
    {
        public List<EnrollmentOrderInfo> EnrollmentOrders { set; get; }       
    }
    public class EnrollmentOrderInfo
    {
        [Required]
        public long Id { set; get; }
        [Required]
        public long EventId { set; get; }
    }

    public class StudentDetails 
    {
        
        public long SoNo { set; get; }        
        public long Id { set; get; }
        public string EnrollmentNo { get; set; }
        public long EnrollmentOrderId { set; get; }
        public long EnrollmentOrderDetailId { set; get; }               
        public string ClassName { get; set; }        
        public string Section { get; set; }        
        public long RollNo { get; set; }        
        public string StudentName { get; set; }
        public byte[] RowVersion { set; get; }
        public DateTime UpdationDate { set; get; }                
    }

    public class CoOrdinator_AutoFill
    {
        public long Id { get; set; }
        public long TitleId { get; set; }
        public string Name { get; set; }
        public Nullable<long> CoOrdMobile { get; set; }
        public Nullable<long> CoOrdAltMobile1 { get; set; }
        public Nullable<long> CoOrdAltMobile2 { get; set; }        
        public string CoOrdEmail { get; set; }
        public string CoOrdAltEmail1 { get; set; }
        public string CoOrdAltEmail2 { get; set; }
    }

    public class GetLotDetail
    {
        public long LotNo { set; get; }
        public string ProcessedBy { set; get; }
        public long ExaminationDateId { set; get; }
        public Level ExamLevel { set; get; }
        public LotType LotType { set; get; }
        public DateTime ExamDate { set; get; }
        public DateTime ProcessedOn { set; get; }
        public long EventId { set; get; }
        public long SchId { set; get; }
        public long SchCode { set; get; }
        public string SchName { set; get; }
        public long SchPinCode { set; get; }
        public string CityName { set; get; }
        public string StateName { set; get; }
        public string Event { set; get; }
        public int NoOfStudent { set; get; }
    }
    public class RegNoList
    {
        public long EventManagementId { set; get; }
        public long RegNo { set; get; }
    }
    public class QPDispatchDetail
    {
        public long EventManagementId { set; get; }
        public Nullable<int> Class1 { set; get; }
        public Nullable<int> Class2 { set; get; }
        public Nullable<int> Class3 { set; get; }
        public Nullable<int> Class4 { set; get; }
        public Nullable<int> Class5 { set; get; }
        public Nullable<int> Class6 { set; get; }
        public Nullable<int> Class7 { set; get; }
        public Nullable<int> Class8 { set; get; }
        public Nullable<int> Class9 { set; get; }
        public Nullable<int> Class10 { set; get; }
        public Nullable<int> Class11 { set; get; }
        public Nullable<int> Class12 { set; get; }

        public Nullable<int> OMRFrom { set; get; }
        public Nullable<int> OMRTo { set; get; }
        public int OMR { set; get; }
        public int TotalEO { set; get; }
        public Nullable<long> Class1_QPStartNo { set; get; }
        public Nullable<long> Class2_QPStartNo { set; get; }
        public Nullable<long> Class3_QPStartNo { set; get; }
        public Nullable<long> Class4_QPStartNo { set; get; }
        public Nullable<long> Class5_QPStartNo { set; get; }
        public Nullable<long> Class6_QPStartNo { set; get; }
        public Nullable<long> Class7_QPStartNo { set; get; }
        public Nullable<long> Class8_QPStartNo { set; get; }
        public Nullable<long> Class9_QPStartNo { set; get; }
        public Nullable<long> Class10_QPStartNo { set; get; }
        public Nullable<long> Class11_QPStartNo { set; get; }
        public Nullable<long> Class12_QPStartNo { set; get; }
                        
        public Nullable<long> Class1_QPLastNo { set; get; }
        public Nullable<long> Class2_QPLastNo { set; get; }
        public Nullable<long> Class3_QPLastNo { set; get; }
        public Nullable<long> Class4_QPLastNo { set; get; }
        public Nullable<long> Class5_QPLastNo { set; get; }
        public Nullable<long> Class6_QPLastNo { set; get; }
        public Nullable<long> Class7_QPLastNo { set; get; }
        public Nullable<long> Class8_QPLastNo { set; get; }
        public Nullable<long> Class9_QPLastNo { set; get; }
        public Nullable<long> Class10_QPLastNo { set; get; }
        public Nullable<long> Class11_QPLastNo { set; get; }
        public Nullable<long> Class12_QPLastNo { set; get; }

    }

    public class SaveQPDispatch {

        public long Id { set; get; }
        [Required]
        public long LotId { set; get; }
        [Required]
        public long EventManagementId { set; get; }

        public QPDispatchDetail JSONData { set; get; }
    }
}