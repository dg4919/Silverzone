using SilverzoneERP.Entities.Models;
using System.Collections.Generic;

namespace SilverzoneERP.Data
{
    public interface ISchedule_OlympiadsRepository : IRepository<Schedule_Olympiads>
    {
        List<string> uploadImage_toTemp(string tempPath);
        void save_Image_fromTemp(IEnumerable<string> imageName, string tempPath, string finalPath);
    }
}
