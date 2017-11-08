using SilverzoneERP.Context;
using SilverzoneERP.Entities.Constant;
using SilverzoneERP.Entities.Models;
using System.IO;
using System.Web;

namespace SilverzoneERP.Data
{
    public class CarrierMasterRepository : BaseRepository<CarrierMaster>, ICarrierMasterRepository
    {
        public CarrierMasterRepository(SilverzoneERPContext dbcontext) : base(dbcontext) { }
    }

    public class CarrierResumeRepository : BaseRepository<CarrierResume>, ICarrierResumeRepository
    {
        public CarrierResumeRepository(SilverzoneERPContext dbcontext) : base(dbcontext) { }

        public string saveResume(string htmlTemplate)
        {
            string file_newName = string.Empty;
            var files = HttpContext.Current.Request.Files;

            string UploadFolderPath = image_urlResolver.project_root + image_urlResolver.resume_path;

            if (!Directory.Exists(UploadFolderPath))
                Directory.CreateDirectory(UploadFolderPath);

            for (var i = 0; i < files.Count; i++)
            {
                var file = files[i];
                file_newName = ClassUtility.genrate_uid() + Path.GetExtension(file.FileName);
                var path = UploadFolderPath + file_newName;

                // to save file into the speicified path
                file.SaveAs(path);

                // send mail with attachment
                ClassUtility.sendMail(emailSender.emailVacancy, "Candidate Resume", htmlTemplate, emailSender.emailNoreply, path);
            }
            return file_newName;
        }

    }
    
}
