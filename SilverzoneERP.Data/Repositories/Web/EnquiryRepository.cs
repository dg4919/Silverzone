using System.Linq;
using System.Collections.Generic;
using SilverzoneERP.Context;
using SilverzoneERP.Entities.Models;

namespace SilverzoneERP.Data
{
    public class EnquiryRepository : BaseRepository<Enquiry>, IEnquiryRepository
    {
        public EnquiryRepository(SilverzoneERPContext dbcontext) : base(dbcontext) { }

        public Enquiry GetById(int id)
        {
            return _dbset.Where(x => x.Id == id).FirstOrDefault();
        }
        public IEnumerable<Enquiry> GetByStatus(bool status)
        {
            return FindBy(x => x.Status == status).AsEnumerable();
        }
        public Enquiry GetByUserName(string name)
        {
            return FindBy(x => x.UserName == name).FirstOrDefault();
        }
        public Enquiry GetByMobile(string mobile)
        {
            return FindBy(x => x.Mobile == mobile).FirstOrDefault();
        }
        public IEnumerable<Enquiry> GetBySubject(string subject)
        {
            return FindBy(x => x.Country == subject).AsEnumerable();
        }
        public Enquiry GetByEmail(string email)
        {
            return FindBy(x => x.EmailId == email).FirstOrDefault();
        }

        public bool send_enquiryEmail(string emailTemplate)
        {
            ClassUtility.sendMail(emailSender.emailInfo, "New Enquiry Recieved", emailTemplate, emailSender.emailNoreply);
            return true;
        }

    }
}
