using SilverzoneERP.Context;
using SilverzoneERP.Entities.Models;
using System.Linq;

namespace SilverzoneERP.Data
{
    public class EnrollmentOrderDetailRepository : BaseRepository<EnrollmentOrderDetail>, IEnrollmentOrderDetailRepository
    {
        public EnrollmentOrderDetailRepository(SilverzoneERPContext context) : base(context) { }
       
        public void Delete(long EnrollmentOrderId)
        {
            _dbContext.Database.ExecuteSqlCommand("delete from EnrollmentOrderDetail where EnrollmentOrderId="+ EnrollmentOrderId);           
        }
        public long GetSchoolCode(long EnrollmentOrderDetailId)
        {
            return _dbset.Where(x=>x.Id== EnrollmentOrderDetailId).FirstOrDefault().EnrollmentOrder.EventManagement.School.SchCode;
        }
        public string GetNIORollNo(long EnrollmentOrderDetailId,long RollNo,string Section)
        {
            string NIORollNo = "" + _dbset.Where(x => x.Id == EnrollmentOrderDetailId).ToList().Select(x => new
            {
                NIORollNo = x.EnrollmentOrder.EventManagement.Event.EventLater
                            + x.EnrollmentOrder.EventManagement.School.SchCode
                            + x.Class.className.Replace("Class ","").PadLeft(3,'0')
                            + Section
                            + RollNo.ToString().PadLeft(2, '0')
            }).FirstOrDefault().NIORollNo;
            return NIORollNo;
        }
    }
}
