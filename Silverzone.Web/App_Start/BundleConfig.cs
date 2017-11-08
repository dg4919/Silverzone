using System.Web.Optimization;

namespace Silverzone.Web
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            #region  Required Css for Project 
          
            bundles.Add(new StyleBundle("~/Content/Site/css").Include(
                   "~/Content/bootstrap.min.css",
                   "~/Content/font-awesome.css",
                  
                   // new css
                   "~/Content/slick/slick.css",
                   "~/Content/slick/slick-theme.css",
                   "~/Content/modern-ticker/modern-ticker.css",
                   "~/Content/modern-ticker/theme.css",
                   "~/Content/animate.min.css",
                   "~/Content/new.css",
                   
                   "~/Content/pnotify.css",
                   "~/Content/angular-block-ui.css",
                   "~/Content/isteven-multi-select.css",
                   "~/Content/jquery.mCustomScrollbar.css"
           ));
           
            #endregion


            #region  Main bundle files > for Site > if use before <body> than js of particular page won't run

            bundles.Add(new ScriptBundle("~/bundles/Site/jqueryBundle").Include(
                
                 "~/Scripts/Lib/jquery/jquery.js",
                 "~/Scripts/Lib/jquery/bootstrap.min.js",
                 
                 "~/Scripts/Lib/jquery/custom.js",
                 "~/Scripts/Lib/jquery/slick.js",
                 "~/Scripts/Lib/jquery/jquery.modern-ticker.min.js",

                 "~/Scripts/Lib/jquery/jquery-validate.js",

                 "~/Scripts/Lib/jquery/pnotify.js",

                 // Use in angular to format date time
                 "~/Scripts/Lib/jquery/moment.js",

                 // jquery ScrollBar
                 "~/Scripts/Lib/jquery/jquery.mCustomScrollbar.js",

                 // user control panel jss start from here
                 "~/Scripts/Lib/jquery/userTheme/bootstrap-datepicker.js",
                 "~/Scripts/Lib/jquery/userTheme/ace-elements.min.js",
                 "~/Scripts/Lib/jquery/userTheme/ace.min.js",

                 "~/Scripts/Lib/jquery/underscore.js"

                 ));

            bundles.Add(new ScriptBundle("~/bundles/Site/AngularBundle").Include(
                    "~/Scripts/Lib/angularjs/angular-1.5.0.js",
                    "~/Scripts/Lib/angularjs/ui-bootstrap-tpls-2.3.0.js",

                    "~/Scripts/Lib/angularjs/angular-ui-router.js",
                    "~/Scripts/Lib/angularjs/angular-pnotify.js",

                     // for Angular validation
                     "~/Scripts/Lib/angularjs/angular-validate.js",

                     "~/Scripts/Lib/angularjs/angular-block-ui.js",
                     "~/Scripts/Lib/angularjs/angular-local-storage.js",
                     "~/Scripts/Lib/angularjs/isteven-multi-select.js",

                     // Angularjs ScrollBar
                     "~/Scripts/Lib/angularjs/scrollbars.js",
                     "~/Scripts/Lib/angularjs/slick.js"
                 ));

            #endregion

            #region  ************  Application JS created by Developer > for Site  ************

            bundles.Add(new ScriptBundle("~/bundles/Site/AppBundle").Include(

                    // Shared Service in a new modeule use as dependncy injection
                    "~/Scripts/App/Shared/silverzone_app.Common.js",
                    "~/Scripts/App/Shared/lazyLoader.js",
                    "~/Scripts/App/Shared/shared_customDirective.js",
                    "~/Scripts/App/Shared/httpService.js",

                    "~/Scripts/App/Shared/Silverzone_sharedService.js",
                    "~/Scripts/App/Shared/user_Account_sharedService.js",

                    "~/Scripts/App/Global_app/Silverzone_app.js",
                    "~/Scripts/App/Services/httpInterceptorService.js",
                    "~/Scripts/App/Global_app/customDirective.js",
                    "~/Scripts/App/Global_app/Filters.js",
                    "~/Scripts/App/Services/modalService.js",

                    "~/Scripts/App/component.js",
                    "~/Scripts/App/Shared/cartFunction.js",

                    "~/Scripts/App/Controller/siteMasterController.js",
                    "~/Scripts/App/Services/siteMasterService.js",

                    "~/Scripts/App/Controller/cartController.js",
                    "~/Scripts/App/Services/cartService.js",

                    "~/Scripts/App/Controller/bookController.js",
                    "~/Scripts/App/Services/bookService.js",

                    "~/Scripts/App/Controller/user_Account_Controller.js",

                    "~/Scripts/App/Services/loginModal_service.js",
                    "~/Scripts/App/Controller/instantDownloads.js",

                     "~/Areas/User/Scripts/Controller/userProfileController.js",
                    "~/Areas/User/Scripts/Services/userProfileService.js",
                    "~/Areas/User/Scripts/Controller/userOrderController.js"

                 ));

            #endregion
            
        }
    }
}
