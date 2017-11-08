using System.IO;

namespace SilverzoneERP.Entities.Constant
{
    public class image_urlResolver
    {
        public const string profilePic_temp = "Img/Web/User/profilePic/{0}/temp/";
        public const string profilePic_main = "Img/Web/User/profilePic/{0}/";
        public const string school_profilePic_main = "Img/School/User/profilePic/";

        public const string bookImage_main = "Img/Web/Admin/BookImage/";
        public const string quizImage_main = "Img/Web/Admin/QuizImage/";
        public const string resume_path = "Img/Web/Resume/";

        public const string question_image = "Img/App/QuestionImage";
        public const string galleryImg_main = "Img/Web/Admin/Gallery/";
        public const string galleryImg_temp = "Img/Web/Admin/Gallery/temp/";

        public const string SOL_main = "Img/Web/Admin/SOL/";    // SOL refferd to schedule olympiad
        public const string SOL_temp = "Img/Web/Admin/SOL/temp/";


        // bcoz this value is not fixed like above string -> return API URL on which Images will be save
        //public static string project_root = Path.GetFullPath(Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "..\\Silverzone.Web")) + "/";
        public static string project_root = Path.GetFullPath(System.AppDomain.CurrentDomain.BaseDirectory);

        //public const string image_baseUrl = "http://localhost:55615/";              // api base URL 
        public const string image_baseUrl = "http://www.api.silverzonehome.com/";              // api base URL 


        //*******************  Methods  ******************

        public static string getProfileImage(string imageName, long userId)
        {
            if (string.IsNullOrEmpty(imageName))
                return imageName;

            return image_baseUrl
                 + string.Format(profilePic_main, userId)
                 + imageName;
        }

        public static string getBookImage(string imageName)
        {
            if (string.IsNullOrEmpty(imageName))
                return imageName;

            return string.Format("{0}{1}{2}",
                                image_baseUrl,
                                bookImage_main,
                                imageName);
        }

        public static string getQuizImage(string imageName)
        {
            if (string.IsNullOrEmpty(imageName))
                return imageName;

            return string.Format("{0}{1}{2}",
                                image_baseUrl,
                                quizImage_main,
                                imageName);
        }

        public static string parseImage(string image)
        {
            return image.Substring(image.LastIndexOf('/') + 1);
        }

        public static string getGalleryImage(string imageName)
        {
            if (string.IsNullOrEmpty(imageName))
                return imageName;

            return string.Format("{0}{1}{2}",
                                image_baseUrl,
                                galleryImg_main,
                                imageName);
        }

        public static string getSOFImage(string imageName)
        {
            if (string.IsNullOrEmpty(imageName))
                return imageName;

            return string.Format("{0}{1}{2}",
                                image_baseUrl,
                                SOL_main,
                                imageName);
        }

    }

}
