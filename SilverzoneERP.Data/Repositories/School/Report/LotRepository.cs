using SilverzoneERP.Context;
using SilverzoneERP.Entities.Models;
using SilverzoneERP.Entities.ViewModel.School;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace SilverzoneERP.Data
{
    public class LotRepository : BaseRepository<Lot>, ILotRepository
    {
        public LotRepository(SilverzoneERPContext context) : base(context) { }

        public long GetLotNo()
        {
            if (_dbset.Count() == 0)
                return 1000;
            else
            return _dbset.Max(x => x.LotNo)+1;            
        }

        public dynamic GetLotDetail(long EventId,Nullable<long> LotNo,Nullable<long> SchoolCode,Nullable<DateTime> ExamDate)
        {
            var EventIdParam = new SqlParameter("@EventId", EventId);
            var LotNoParam = new SqlParameter("@LotNo", DBNull.Value);
            if (LotNo != null)
            {
                LotNoParam.SqlValue = LotNo;
            }
            var SchoolCodeParam = new SqlParameter("@SchoolCode", DBNull.Value);
            if (SchoolCode != null)
            {
                SchoolCodeParam.SqlValue = SchoolCode;
            }
            var ExamDateParam = new SqlParameter("@ExamDate", DBNull.Value);
            if (ExamDate != null)
            {
                ExamDateParam.SqlValue = ExamDate;
            }
            
            var data = _dbContext.Database.SqlQuery<GetLotDetail>("Exec GET_Lot @EventId,@LotNo,@SchoolCode,@ExamDate",EventIdParam,LotNoParam,SchoolCodeParam,ExamDateParam).ToList<GetLotDetail>()
                       .GroupBy(x => x.LotNo)
                      .Select(x => new
                      {
                          LotNo = x.Key,
                          ProcessedBy = x.FirstOrDefault().ProcessedBy,
                          x.FirstOrDefault().ExamDate,
                          x.FirstOrDefault().ExaminationDateId,
                          x.FirstOrDefault().EventId,
                          x.FirstOrDefault().Event,
                          x.FirstOrDefault().ProcessedOn,
                          x.FirstOrDefault().ExamLevel,
                          x.FirstOrDefault().LotType,
                          TotalEnrollmentOrder = x.Sum(st => st.NoOfStudent),
                          SchoolDetails = x.GroupBy(g => g.SchCode).Select(s => new {
                              s.FirstOrDefault().SchId,
                              SchoolName = s.FirstOrDefault().SchName,
                              SchCode = s.Key,
                              s.FirstOrDefault().SchPinCode,
                              s.FirstOrDefault().CityName,
                              s.FirstOrDefault().StateName,
                              NoOfStudent = s.Sum(st => st.NoOfStudent)
                          })
                      });
            return data;
        }

        public List<RegNoList> RegNoList(long LotId)
        {
            var data = _dbContext.Database.SqlQuery<RegNoList>("select Distinct EO.EventManagementId,RegNo from lotDetail LTD inner join EnrollmentOrder EO on LTD.ObjectId = EO.Id inner join EventManagement EVM on EO.EventManagementId = EVM.Id where LTD.LotId =" + LotId).ToList<RegNoList>();
            return data;
        }
        public QPDispatchDetail GetQPDispatchDetail(long EventManagementId)
        {            
            var data = _dbContext.Database.SqlQuery<QPDispatchDetail>("Exec GET_QPDispatchDetail @EventManagementId", new SqlParameter("@EventManagementId", EventManagementId)).ToList<QPDispatchDetail>();            
            return data.FirstOrDefault();
        }
    }
}
