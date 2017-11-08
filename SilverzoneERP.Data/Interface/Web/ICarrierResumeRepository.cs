using SilverzoneERP.Entities.Models;

namespace SilverzoneERP.Data
{
    public interface ICarrierMasterRepository : IRepository<CarrierMaster>
    {
    }

    public interface ICarrierResumeRepository : IRepository<CarrierResume>
    {
        string saveResume(string htmlTemplate);
    }
}
