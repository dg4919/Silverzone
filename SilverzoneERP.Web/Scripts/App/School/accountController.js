

(function (app) {

    var user_fun = function ($scope, $state, $rootScope, Service, globalConfig, localStorageService, $stateParams,$filter,$window) {        
        //alert($stateParams);
        debugger;
        var paramsObject = {};
        $window.location
               .search
               .replace(/\?/, '')
               .split('&').map(function (o) {
                   paramsObject[o.split('=')[0]] = o.split('=')[1]
               });

        if (!angular.isUndefined(paramsObject.RoleName))
        $stateParams.RoleName = paramsObject.RoleName.replace('%20',' ');
        //alert($stateParams.RoleName);
        $rootScope.HeaderAction = "";
      
        $scope.Account = {};
        $scope.msg = '';
        $rootScope.UserInfo = {};
        $scope.Login = function (form) {
            debugger;
            if (form.validate() == false)
                return false;
           // prompt('', JSON.stringify($scope.Account));
           
            Service.Login($scope.Account).then(function (response) {
                if (response.result == 'Success') {
                    debugger;
                    var HideShowMenu = true;
                    if (angular.lowercase(response.user.Role.RoleName) == 'teacher' && (response.user.IsActive == null || response.user.IsActive == false))
                        HideShowMenu = false;
                    $rootScope.UserInfo = {
                        "UserId": response.user.Id,
                        "UserName": response.user.UserName,
                        "RoleName": response.user.Role.RoleName,                        
                        "SchId": response.user.SchId,
                        "SchName": response.school.SchName,
                        "SchCode": response.school.SchCode,
                        "IsActive":response.user.IsActive,
                        "ImgPrefix": globalConfig.apiEndPoint.replace("/api/", "/"),
                        "ProfilePic": globalConfig.apiEndPoint.replace("/api/", "/") + (response.user.ProfilePic == null ? 'ProfilePic/placeholderImage.png' : response.user.ProfilePic),
                        "menu": response.menu,
                        "ShowMenu": true,
                        "HideShowMenu": HideShowMenu
                    };
                    $rootScope.EventInfo = response.Event;
                    $rootScope.EventYearInfo = response.EventYearInfo;
                    localStorageService.set("EventInfo", $rootScope.EventInfo);
                    localStorageService.set("EventYearInfo", $rootScope.EventYearInfo);
                    localStorageService.set("UserInfo", $rootScope.UserInfo);
                    $state.go("ERP_Home");
                }
                else
                    Service.Notification($rootScope, response.message);
            });
        }
       
        $scope.validationOptions = {
            rules: {
                EmailID: {
                    required: true
                },
                Password: {
                    required: true
                }
            }
        }

        $scope.IsLogin = true;
        $scope.IsSignUp = false;
        $scope.IsShowReg = false;
        $scope.IsNextToOTP = false;

        var GeneragedOTP = "";
       
        $scope.Action = function (IsLogin, IsSignUp, IsNextToOTP) {
            debugger;          
            $scope.IsLogin = IsLogin;
            $scope.IsSignUp = IsSignUp;            
            $scope.IsNextToOTP = IsNextToOTP;
            GeneragedOTP = "";
            //window.open("Sample.htm", null, "height=200,width=400,status=yes,toolbar=no,menubar=no,location=no");
        }

        function GetRole() {
            Service.Get('School/role/Get').then(function (response) {
                $scope.Role = response.result;
                Reload();              
            });
        }

    
        var RoleId;
        function Reload() {
            debugger;
            var Role_Filter = $filter("filter")($scope.Role, function (data) {
                if (angular.lowercase(data.RoleName) == angular.lowercase($stateParams.RoleName))
                    return data;
            });
            RoleId = Role_Filter[0].RoleId;
            $scope.Account.RoleId = Role_Filter[0].RoleId;
            if (Role_Filter.length != 0) {
                $scope.IsShowReg = true;
                $scope.TeacherReg = {
                    "GenderType": "1",
                    "ProfilePic": globalConfig.apiEndPoint.replace("/api/", "/") + "ProfilePic/placeholderImage.png",
                    "RoleId": Role_Filter[0].RoleId,
                    "SrcFrom": "online"
                };
                $scope.OTPObj = {};
            }
            else
            {
                Service.Notification($rootScope, 'Invalid Role');
                return false;
            }
                
            
        }
        if ($stateParams.RoleName != null && (angular.lowercase($stateParams.RoleName) == 'teacher' || angular.lowercase($stateParams.RoleName) == 'regional coordinator'))
        {
            GetRole();
            $scope.RoleName = angular.lowercase($stateParams.RoleName);
        }
            

        $scope.UploadImage = function (file) {
            Service.readAsDataURL(file, $scope).then(function (result) {
                $scope.TeacherReg.ProfilePic = result;
            });
        };
        $scope.validationSignUp = {
            rules: {
                UserName: {           
                    required: true,
                    maxlength: 100
                },
                DOB: {
                    required: true
                },
                EmailID: {
                    required: true,
                    maxlength: 50
                },
                RoleId: {
                    required: true
                }
                ,
                Password: {
                    required: true
                },
                ConfirmPassword: {
                    required: true
                },
                UserAddress: {
                    maxlength: 200
                },
                MobileNumber: {
                    required:true,
                    maxlength: 10,
                    minlength: 10
                }
                ,
                Qualification: {
                    required: true,
                    maxlength: 50
                }
                ,
                OtherQualification: {                
                    maxlength: 50
                }
                ,
                Profession: {
                    required: true,
                    maxlength: 50
                }
                ,
                HowDid: {
                    required: true,
                    maxlength: 50
                }
            }
        }
              
        $scope.SignUp = function (form) {
            debugger;
            if (form.validate() == false)
                return false;
            GeneragedOTP = "";
            var Html = '<!DOCTYPE html>'
                        + '<html>'
                        + '<head></head>'
                        + '<body>'
                        + '<div  id="myId" style="padding: 0px !important; border:2px black solid">'
                        + '<div style="text-align:center;background-color:#317eac;color:white;margin-top:-21px;">'
                        + '<h1>Silverzone Foundation</h1>'
                        + '</div>'
                        + '<div>'
                        + 'OTP is <span> _OTP </span>'
                        + '</div>'
                        + '</div>'
                        + '</body>'
                        + '</html>';
            var obj = {
                HtmlTemplate: Html,
                emailId: $scope.TeacherReg.EmailID,
                RoleId: RoleId
            };
            Service.Create_Update(obj, 'school/account/SendOTP').then(function (response) {
                if (response.result == 'Success') {
                    debugger;
                    $scope.Action(false, false, true);
                    GeneragedOTP = response.OTP;
                }
                else
                    Service.Notification($rootScope, response.message);
            });
         
        }

        $scope.NextToOTP = function (form) {
            debugger;
            $scope.IsNextToOTP = true;
        }

        $scope.VerifyOTP = function (form) {
            debugger;
            if (form.validate() == false)
                return false;
            if (GeneragedOTP != $scope.OTPObj.OTP)
            {
                Service.Notification($rootScope,'Invalid OTP !');
                return false;
            }

            Service.Create_Update($scope.TeacherReg, 'school/account/registration').then(function (response) {
                if (response.result == 'Success') {
                    debugger;
                    $scope.Action(true, false, false);
                    Reload();
                }                
                Service.Notification($rootScope, response.message);
            });

        }
        
        $scope.validationVerifyOTP = {
            rules: {
                OTP: {
                    required: true
                }
            }
        };
    }
   
    app.controller('accountController', ['$scope', '$state', '$rootScope', 'Service', 'globalConfig', 'localStorageService', '$stateParams', '$filter', '$window', user_fun]);

})(angular.module('SilverzoneERP_App'));