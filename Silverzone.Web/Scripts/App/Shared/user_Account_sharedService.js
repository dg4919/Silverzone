// contains all ajax related to user A/c > like regiter,login, forget/reset passowrd etc

(function () {

    var fn = function (globalConfig, localStorageService, $filter, $rsc, httpSvc) {

        // define multiple variables at a time
        var apiUrl = globalConfig.apiEndPoint + globalConfig.version.Site,
            fac = {};

        fac.sendSms_onPhone = function (_mobileNo) {
            return httpSvc.get(
                apiUrl + '/Account/send_sms_onPhone',
                { mobileNo: _mobileNo });
        }

        fac.verify_OTP = function (_model) {
            return httpSvc.post(
             apiUrl + '/Account/verify_OTP',
             _model);
        }

        fac.Register = function (_model) {
            return httpSvc.post(
               apiUrl + '/Account/Register_user',
               _model);
        }

        fac.Register_newUser = function (_model) {
            return httpSvc.post(
             apiUrl + '/Account/Register_newUser',
             _model);
        }

        fac.Login = function (_model) {
            return httpSvc
                  .post(apiUrl + '/Account/Login', _model)
                  .then(function (d) {
                      if (d.result === 'ok')
                          fac.saveUserInfo(d.user, d.token);

                      return {
                          msg: d.result,
                          role: d.user !== undefined ? d.user.Role : ''   // role use in admin login section
                      };                                                  // only will pass status of login
                  });
        }

        fac.Logout = function () {

            localStorageService.remove('authorizeData');

            $rsc.user.currentUser = '';
            $rsc.user.is_login = false;
        }

        fac.saveUserInfo = function (user, token) {

            if (user.DOB !== null)
                user.DOB = $filter('dateFormat')(user.DOB, 'MM/DD/YYYY');

            // to refresh image add a unique datetime string
            user.ProfilePic = user.ProfilePic !== null ? (user.ProfilePic + '?' + $filter('date')(new Date(), 'HHmmss')) : null;

            var entity = {
                userInfo: user,
                tokenInfo: token
            }

            // local storage store user info in browser & when ever user clear history then it will also removed
            localStorageService.set('authorizeData', entity);

            $rsc.user.currentUser = user;
            $rsc.user.is_login = true;
        }


        //*********  Forget Login ******************

        fac.forgetPassword = function (_model) {
            return httpSvc
                .get('templates/EmailTemplates/forget_password.html')
                .then(function (response) {
                    angular.extend(_model, {
                        html_template: response
                    });

                    return httpSvc.post(
                                 apiUrl + '/Account/forget_password',
                                 _model);
                });
        }

        fac.verify_forgetLogin_OTP = function (_model) {
            return httpSvc.post(
              apiUrl + '/Account/verify_forgetLogin_OTP',
              _model);
        }

        fac.save_newPassword = function (_model) {
            return httpSvc.post(
             apiUrl + '/Account/saved_newforget_password',
             _model);
        }


        return fac;
    }

    angular.module('Silverzone_service.Shared')
        .factory('user_Account_Service', ['globalConfig', 'localStorageService', '$filter', '$rootScope', 'httpService', fn]);

})();

