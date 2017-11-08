using System.Collections.Generic;
using System;
using System.Linq;
using SilverzoneERP.Entities.Models;
using SilverzoneERP.Context;
using SilverzoneERP.Entities.ViewModel.School;

namespace SilverzoneERP.Data
{
    public class CoOrdinatorRepository : BaseRepository<CoOrdinator>, ICoOrdinatorRepository
    {
        public CoOrdinatorRepository(SilverzoneERPContext context) : base(context) { }

        public List<CoOrdinator> GetByEventCoOrdId(long EventManagementId)
        {
            try
            {
                return _dbContext.CoOrdinators.Where(x=>x.EventManagementId== EventManagementId).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }                       
        }

        public List<CoOrdinator_AutoFill> Get_CoOrdinator_AutoFill(string CoOrdName)
        {
            string Query= "select CO.Id,Co.TitleId,CO.CoOrdName as Name,CO.CoOrdName,CO.CoOrdMobile,CO.CoOrdAltMobile1,CO.CoOrdAltMobile2,CO.CoOrdEmail,CO.CoOrdAltEmail1,CO.CoOrdAltEmail2 from EventManagement EVM inner join CoOrdinator CO on EVM.Id = CO.EventManagementId where CONTAINS(CO.CoOrdName,'\""+ "*" + CoOrdName.Replace("'", "''") + "*" + "\"') and EVM.EventManagement_YearCode < 4045";
            return _dbContext.Database.SqlQuery<CoOrdinator_AutoFill>(Query).ToList< CoOrdinator_AutoFill>();
        }
    }
}
