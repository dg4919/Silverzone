using SilverzoneERP.Context;
using SilverzoneERP.Entities.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace SilverzoneERP.Data.Repositories
{
    public class StudentRegistrationRepository:BaseRepository<StudentRegistration>,IStudentRegistrationRepository
    {
        public StudentRegistrationRepository(SilverzoneERPContext dbcontext) : base(dbcontext) { }
        public StudentRegistration GetById(int id)
        {
            return _dbset.Where(x => x.Id == id).FirstOrDefault();
        }
       public  IEnumerable<StudentRegistration> GetByEventCode(string ecode)
        {
            return _dbset.Where(x => x.EventCode == ecode).AsEnumerable();
        }
        
        public IEnumerable<StudentRegistration> GetBySchoolCode(string schcode)
        {
            return FindBy(x => x.SchCode == schcode).AsEnumerable();
        }
        public IEnumerable<StudentRegistration> GetByRegSrlNo(int srno)
        {
            return _dbset.Where(x => x.RegSrlNo == srno).AsEnumerable();
        }
        public IEnumerable<StudentRegistration> GetByEventCodeAndSchCode(string ecode, string schcode)
        {
            return _dbset.Where(x => x.EventCode == ecode && x.SchCode == schcode).AsEnumerable();
        }
        public IEnumerable<StudentRegistration> GetByExamDateOpted(int edate)
        {
            return _dbset.Where(x => x.ExamDateOpted == edate).AsEnumerable();
        }
        public decimal BookPrice(long CategoryId)
        {            
            decimal Price=0;
            var BookDetail = (from itemTitle in _dbContext.ItemTitle_Masters.Where(x => x.CategoryId == CategoryId && x.Status == true)
                          join book in _dbContext.Books.Where(x => x.Status == true) on itemTitle.Id equals book.Title_Mid
                          select new
                          {
                              book.Price
                          }).FirstOrDefault();
            if (BookDetail != null)
                Price = BookDetail.Price;
            return Price;
        }

        public dynamic GetEventList(bool IsBook)
        {
            if(IsBook)
                return _dbContext.BookOrders.Select(x => x.EventCode).Distinct();
            else
                return _dbset.Select(x => x.EventCode).Distinct();
        }
        public string GetGooglePlayVersion()
        {
            return _dbContext.Database.SqlQuery<string>("select distinct GooglePlayVersion from [user] where GooglePlayVersion is not null").FirstOrDefault<string>();         
        }
        public bool UpdateGooglePlayVersion(string Version)
        {
            var RoleId = _dbContext.Roles.Where(x => x.RoleName.Trim().ToLower().Equals("teacher")).FirstOrDefault().Id;

           var res= _dbContext.Database.ExecuteSqlCommand("update [user] set GooglePlayVersion=@Version where RoleId="+ RoleId, new SqlParameter("@Version", Version) );
            return res>0?true:false;
        }
       
        public string GenerateOrderNo(string SchoolCode)
        {
            if(_dbset.Any(x => x.OrderNo != null))
            {
                string _orderNo = _dbset.Where(x => x.OrderNo != null).OrderByDescending(x => x.Id).Take(1).Select(x => x.OrderNo).ToList().FirstOrDefault();
                _orderNo = "On/" + SchoolCode +"/"+ (Convert.ToInt32(Convert.ToInt32(_orderNo.Split('/')[2]) + 1));
                return _orderNo;                
            }
            else
                return "On/" + SchoolCode + "/" + 100000;
        }
        public int GenerateRegNo(string SchoolCode,string EventCode)
        {

            var stuDetails = _dbset.Where(x => x.EventCode == EventCode).ToList();
            if (stuDetails.Count() != 0)
            {
                var _sch_stuSNo = stuDetails.Where(x => x.SchCode == SchoolCode);
                if (_sch_stuSNo.Count() != 0)
                    return _sch_stuSNo.FirstOrDefault().RegSrlNo;
                else
                    return stuDetails.FirstOrDefault().RegSrlNo;                
            }
            else
                return 1000;            
        }
    }
}
