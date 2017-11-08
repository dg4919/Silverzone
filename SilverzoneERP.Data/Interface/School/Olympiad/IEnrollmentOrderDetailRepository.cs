using SilverzoneERP.Entities.Models;

namespace SilverzoneERP.Data
{
    public interface IEnrollmentOrderDetailRepository : IRepository<EnrollmentOrderDetail>
    {
        void Delete(long EnrollmentOrderId);
        long GetSchoolCode(long EnrollmentOrderDetailId);
        string GetNIORollNo(long EnrollmentOrderDetailId, long RollNo, string Section);
    }    
}
