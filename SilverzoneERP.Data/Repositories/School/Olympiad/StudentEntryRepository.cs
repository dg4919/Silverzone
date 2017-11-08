using SilverzoneERP.Context;
using SilverzoneERP.Entities.Models;
using System.Linq;
using System;
using SilverzoneERP.Entities.ViewModel.School;
using System.Data.SqlClient;
using System.Data;

namespace SilverzoneERP.Data
{
    public class StudentEntryRepository : BaseRepository<StudentEntry>, IStudentEntryRepository
    {
        public StudentEntryRepository(SilverzoneERPContext context) : base(context) { }

        public bool Exists(long EnrollmentOrderDetailId, string Section, long RollNo, string StudentName)
        {
            return _dbset.Count(x=>x.EnrollmentOrderDetailId== EnrollmentOrderDetailId  && x.Section.Trim().ToLower()==Section.Trim().ToLower() && x.RollNo==RollNo && x.StudentName.Trim().ToLower()==StudentName.Trim().ToLower())==0?false:true;
        }

        public bool Exists(long StudentEntryId,long EnrollmentOrderDetailId, string Section, long RollNo, string StudentName)
        {
            return _dbset.Count(x => x.Id!=StudentEntryId && x.EnrollmentOrderDetailId == EnrollmentOrderDetailId  && x.Section.Trim().ToLower() == Section.Trim().ToLower() && x.RollNo == RollNo && x.StudentName.Trim().ToLower() == StudentName.Trim().ToLower()) == 0 ? false : true;
        }
        
        public StudentEntry Get(long StudentEntryId)
        {
            return _dbset.FirstOrDefault(x => x.Id == StudentEntryId);
        }

        public dynamic Get(long EventManagementId, Nullable<long> EnrollmentOrderId, Nullable<long> EnrollmentOrderDetailId, string Section, int StartIndex, int Limit, out long Count)
        {
            try
            {
                //Count = 0;
                //string Query = "select top "+Limit+" * from (select ROW_NUMBER() OVER(ORDER BY st.UpdationDate DESC) AS SoNo, "
                //    + "st.Id,"
                //    + "st.EnrollmentNo,"
                //    + "eod.EnrollmentOrderId,"
                //    + "st.EnrollmentOrderDetailId,"
                //    + "cl.ClassName,"
                //    + "st.Section,"
                //    + "st.RollNo,"
                //    + "st.StudentName,"
                //    + "st.[RowVersion],"
                //    + "st.UpdationDate"
                //    + " from StudentEntry st inner join EnrollmentOrderDetail eod on st.EnrollmentOrderDetailId = eod.Id"
                //    + " inner join EnrollmentOrder eo on eod.EnrollmentOrderId = eo.Id"
                //    + " inner join EventManagement evm on eo.EventManagementId = evm.Id"
                //    + " inner join Class cl on eod.ClassId = cl.id"
                //    + " where eo.EventManagementId="+ EventManagementId;

                //if (EnrollmentOrderId != null)
                //    Query += " and eod.EnrollmentOrderId=" + EnrollmentOrderId;

                //if (EnrollmentOrderDetailId!=null)
                //    Query += " and st.EnrollmentOrderDetailId=" + EnrollmentOrderDetailId;
                //if (!string.IsNullOrWhiteSpace(Section))
                //    Query += " and st.Section='"+Section+"'";
                //Query += " ) as tbl where SoNo>="+StartIndex;

                var Total = new SqlParameter
                {
                    ParameterName = "@Count",
                    SqlDbType = SqlDbType.BigInt,
                    Size = 30,
                    Direction = System.Data.ParameterDirection.Output
                };
                ////,@Count output



                var EventManagementIdParam = new SqlParameter("@EventManagementId", EventManagementId);
                var EnrollmentOrderIdParam = new SqlParameter("@EnrollmentOrderId", DBNull.Value);
                if (EnrollmentOrderId != null)
                {
                    EnrollmentOrderIdParam.SqlValue = EnrollmentOrderId;
                }
                var EnrollmentOrderDetailIdParam = new SqlParameter("@EnrollmentOrderDetailId", DBNull.Value);
                if (EnrollmentOrderDetailId != null)
                {
                    EnrollmentOrderDetailIdParam.SqlValue = EnrollmentOrderDetailId;
                }
                var SectionParam = new SqlParameter("@Section", DBNull.Value);
                if (Section != null)
                {
                    SectionParam.SqlValue = Section;
                }
                var StartIndexParam = new SqlParameter("@StartIndex", StartIndex);
                
                var LimitParam = new SqlParameter("@Limit", Limit);

                var data = _dbContext.Database.SqlQuery<StudentDetails>(
                    "EXEC [GET_Student] @EventManagementId,@EnrollmentOrderId,@EnrollmentOrderDetailId,@Section,@StartIndex,@Limit,@Count Output"
                    , EventManagementIdParam, EnrollmentOrderDetailIdParam, EnrollmentOrderIdParam, SectionParam, StartIndexParam, LimitParam,Total
                    ).ToList<StudentDetails>();
                Count =(long)Total.Value;
                return data;            
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public dynamic StudentClassWise(long EventManagementId)
        {
            try
            {
                var data = _dbContext.StudentEntries.Where(x => x.EnrollmentOrderDetail.EnrollmentOrder.EventManagementId == EventManagementId)
                    .GroupBy(x => x.EnrollmentOrderDetail.Class.className)
                    .Select(x => new
                    {
                        ClassName = x.Key,
                        Sections = x.GroupBy(g => g.Section).Select(se => new
                        {
                            Section = se.Key,
                            Count = se.Count(),
                            Students = se.Select(s => new
                            {
                                s.Id,
                                s.EnrollmentOrderDetail.EnrollmentOrder.EventManagementId,
                                s.EnrollmentOrderDetail.ClassId,
                                ClassName = s.EnrollmentOrderDetail.Class.className,
                                s.Section,
                                s.RollNo,
                                s.StudentName,
                                s.RowVersion,
                                s.UpdationDate
                            })
                        })
                    });
                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GenerateOTP(int length)
        {
            string alphabets = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            //string small_alphabets = "abcdefghijklmnopqrstuvwxyz";
            string numbers = "1234567890";

            string characters = numbers;

            characters += alphabets  + numbers;
           
            string otp = string.Empty;
            for (int i = 0; i < length; i++)
            {
                string character = string.Empty;
                do
                {
                    int index = new Random().Next(0, characters.Length);
                    character = characters.ToCharArray()[index].ToString();
                } while (otp.IndexOf(character) != -1);
                otp += character;
            }
            return otp;
        }
    }
}
