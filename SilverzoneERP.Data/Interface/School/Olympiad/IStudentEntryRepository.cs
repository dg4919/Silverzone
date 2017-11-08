using SilverzoneERP.Entities.Models;
using System;

namespace SilverzoneERP.Data
{
    public interface IStudentEntryRepository : IRepository<StudentEntry>
    {
        bool Exists(long EnrollmentOrderDetailId, string Section,long RollNo,string StudentName);
        bool Exists(long StudentEntryId, long EnrollmentOrderDetailId, string Section, long RollNo, string StudentName);
        StudentEntry Get(long StudentEntryId);
        dynamic Get(long EventManagementId, Nullable<long> EnrollmentOrderId, Nullable<long> EnrollmentOrderDetailId, string Section, int StartIndex, int Limit, out long Count);
        dynamic StudentClassWise(long EventManagementId);

        string GenerateOTP(int length);
    }
}
