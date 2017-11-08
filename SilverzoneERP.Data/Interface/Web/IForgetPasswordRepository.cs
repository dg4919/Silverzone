using SilverzoneERP.Entities;
using SilverzoneERP.Entities.Models;

namespace SilverzoneERP.Data
{
    public interface IForgetPasswordRepository : IRepository<ForgetPassword>
    {
        bool sendEmail_forgetPassword(string htmlTemplate, int verfiy_code, string emailId, string emailSubject);
        ForgetPassword getRecords(long userId, verificationMode type);

    }
}
