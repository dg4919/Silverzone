using SilverzoneERP.Context;
using SilverzoneERP.Entities;
using SilverzoneERP.Entities.Models;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace SilverzoneERP.Data
{
    public class DND_MobileNoRepository : BaseRepository<DND_MobileNo>, IDND_MobileNoRepository
    {
        public DND_MobileNoRepository(SilverzoneERPContext context) : base(context) { }

        public dynamic Get(string Search, long StartIndex, long Limit,DNDType DNDType, out long Count)
        {
            var P_Search = new SqlParameter("@Search",DBNull.Value);
            if(!string.IsNullOrWhiteSpace(Search))
                P_Search = new SqlParameter("@Search", Search);
            var P_StartIndex = new SqlParameter("@StartIndex", StartIndex);
            var P_Limit = new SqlParameter("@Limit", Limit);

            var Total = new SqlParameter
            {
                ParameterName = "@Count",
                SqlDbType = SqlDbType.BigInt,
                Size = 30,
                Direction = System.Data.ParameterDirection.Output
            };
            var P_TableName = new SqlParameter("@TableName", DNDType.ToString());
            if (DNDType== DNDType.MobileNo)
            {               
                var data = _dbContext.Database.SqlQuery<DND_MobileNo>("GET_Search_DND_Mobile @Search,@StartIndex,@Limit,@TableName,@Count output", P_Search, P_StartIndex, P_Limit, P_TableName, Total).ToList<DND_MobileNo>();
                Count = (long)Total.Value;
                return data;
            }                
            else if (DNDType == DNDType.EmailId)
            {             
                var data = _dbContext.Database.SqlQuery<DND_EmailId>("GET_Search_DND_Mobile @Search,@StartIndex,@Limit,@TableName,@Count output", P_Search, P_StartIndex, P_Limit, P_TableName, Total).ToList<DND_EmailId>();
                Count = (long)Total.Value;
                return data;
            }
            Count = 0;
            return null;       
        }
    }
}
