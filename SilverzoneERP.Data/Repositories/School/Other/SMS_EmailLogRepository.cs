using SilverzoneERP.Context;
using SilverzoneERP.Entities.Models;
using System;
using System.Data.SqlClient;
using System.Linq;

namespace SilverzoneERP.Data
{
    public class SMS_EmailLogRepository : BaseRepository<SMS_EmailLog>, ISMS_EmailLogRepository
    {
        public SMS_EmailLogRepository(SilverzoneERPContext context) : base(context) { }

        public bool Log(Nullable<long> MobileNo,string EmailId,string Purpose,string Content,string FormName,Nullable<long> SchCode)
        {
            try
            {
                _dbset.Add(new SMS_EmailLog() {
                    MobileNo= MobileNo,
                    EmailId= EmailId,
                    Purpose= Purpose,
                    Content= Content,
                    FormName= FormName,
                    SchCode= SchCode,
                    Status =true
                });
                return true;
            }
            catch (Exception)
            {
                return false;
            }            
        }

        public dynamic Search(Nullable<long> MobileNo, string EmailId, string Content, Nullable<DateTime> From, Nullable<DateTime> To,Nullable<long>SchCode)
        {
            var P_MobileNo = new SqlParameter("@MobileNo", DBNull.Value);
            if(MobileNo!=null)
                P_MobileNo = new SqlParameter("@MobileNo", MobileNo);

            var P_EmailId = new SqlParameter("@EmailId", DBNull.Value);
            if (!string.IsNullOrWhiteSpace(EmailId))
                P_EmailId = new SqlParameter("@EmailId", EmailId);

            var P_Content = new SqlParameter("@Content", DBNull.Value);
            if (!string.IsNullOrWhiteSpace(Content))
                P_Content = new SqlParameter("@Content", Content);

          

            var P_From = new SqlParameter("@From", DBNull.Value);
            if (From != null)
                P_From = new SqlParameter("@From", From);

            var P_To = new SqlParameter("@To", DBNull.Value);
            if (To != null)
                P_To = new SqlParameter("@To", To);

            var P_SchCode = new SqlParameter("@SchCode", DBNull.Value);
            if (SchCode != null)
                P_SchCode = new SqlParameter("@SchCode", SchCode);

            return _dbContext.Database.SqlQuery<SMS_EmailLog>("Get_Search_SMS_Email_Log @MobileNo,@EmailId,@Content,@From,@To,@SchCode", P_MobileNo, P_EmailId, P_Content,P_From,P_To,P_SchCode).ToList<SMS_EmailLog>();
        }
    }
}
