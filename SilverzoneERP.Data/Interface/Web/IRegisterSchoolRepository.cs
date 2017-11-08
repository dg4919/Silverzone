using SilverzoneERP.Entities.Models;

namespace SilverzoneERP.Data
{
    public interface IRegisterSchoolRepository : IRepository<RegisterSchool>
    {
        bool send_schoolRegister_Email(string emailTemplate);
    }
}
