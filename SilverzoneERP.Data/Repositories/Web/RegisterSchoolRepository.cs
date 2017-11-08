using SilverzoneERP.Context;
using SilverzoneERP.Entities.Models;

namespace SilverzoneERP.Data
{
    public class RegisterSchoolRepository : BaseRepository<RegisterSchool>, IRegisterSchoolRepository
    {
        public RegisterSchoolRepository(SilverzoneERPContext dbcontext) : base(dbcontext) { }

        public bool send_schoolRegister_Email(string emailTemplate)
        {
            ClassUtility.sendMail(emailSender.emailInfo, "New School Registration", emailTemplate, emailSender.emailNoreply);
            return true;
        }

    }
}
