using SilverzoneERP.Entities.Models;
using System.Collections.Generic;

namespace SilverzoneERP.Data
{
    public interface IEnquiryRepository:IRepository<Enquiry>
    {
        Enquiry GetById(int id);
        IEnumerable<Enquiry> GetByStatus(bool status);
        Enquiry GetByUserName(string name);
        Enquiry GetByMobile(string mobile);
        IEnumerable<Enquiry> GetBySubject(string subject);
        Enquiry GetByEmail(string email);

        bool send_enquiryEmail(string emailTemplate);
    }
}
