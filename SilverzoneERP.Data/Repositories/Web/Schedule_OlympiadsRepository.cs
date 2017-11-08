using SilverzoneERP.Entities.Models;
using SilverzoneERP.Context;
using System.Collections.Generic;

namespace SilverzoneERP.Data
{
    public class Schedule_OlympiadsRepository : BaseRepository<Schedule_Olympiads>, ISchedule_OlympiadsRepository
    {
        public Schedule_OlympiadsRepository(SilverzoneERPContext dbcontext):base(dbcontext){ }

        public List<string> uploadImage_toTemp(string tempPath)
        {
            return ClassUtility.upload_orignal_Images_toTemp(tempPath);
        }

        public void save_Image_fromTemp(IEnumerable<string> imageName, string tempPath, string finalPath)
        {
            ClassUtility.save_Images_toPhysical(imageName, tempPath, finalPath);
        }
    }
}
