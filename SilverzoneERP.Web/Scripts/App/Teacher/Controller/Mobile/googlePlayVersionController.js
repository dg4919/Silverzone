
(function (app) {
    'use strict';

    var My_Fun = function ($scope,$state, $rootScope, $filter, Service) {
        $rootScope.HeaderAction = "Templates/Teacher/Mobile/GooglePlayVersion/Partial/Header.html";
        
        debugger;
        $scope.Obj = {};

        Service.Get('teacherApp/StudentRegistration/GetGooglePlayVersion').then(function (response) {
            debugger;
            $scope.Obj.GooglePlayVersion = response.GooglePlayVersion;            
        });

        $scope.Submit = function (form) {
            debugger;
            if (form.validate() == false)
                return false;

            Service.Create_Update($scope.Obj, 'TeacherApp/StudentRegistration/UpdateGooglePlayVersion')
                  .then(function (response) {
                      Service.Notification($rootScope, response.message);
                      if (angular.lowercase(response.result) == 'success') {
                          $state.reload();
                      }
                  });
        }
        $scope.validationOptions = {
            rules: {
                GooglePlayVersion: {
                    required: true
                }
            }
        }
    }

    app.controller('googlePlayVersionController', ['$scope', '$state', '$rootScope', '$filter', 'Service', My_Fun]);
})(angular.module('SilverZone_Teacher_App'));