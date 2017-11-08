

(function (app) {
    var teacherSchool_fun = function ($scope, $rootScope, Service, $filter, modalInstance) {
        // $rootScope.HeaderAction = "Templates/Teacher/TeacherSchool/Header.html";
        $scope.TeacherSchool = {};
        function GetCity() {
            Service.Get('school/schManagement/GetCity').then(function (response) {
                $scope.City = response.result;
            });
        }

        GetCity();

        $scope.CityChange = function () {
            debugger;
            var City_Filter = $filter("filter")($scope.City, { CityId: $scope.TeacherSchool.CityId }, true);
            if (City_Filter.length != 0) {
                $scope.TeacherSchool.CityId = City_Filter[0].CityId;
                $scope.TeacherSchool.DistrictId = City_Filter[0].DistrictId;
                $scope.TeacherSchool.DistrictName = City_Filter[0].DistrictName;
                $scope.TeacherSchool.StateId = City_Filter[0].StateId;
                $scope.TeacherSchool.StateName = City_Filter[0].StateName;
                $scope.TeacherSchool.ZoneId = City_Filter[0].ZoneId;
                $scope.TeacherSchool.CountryId = City_Filter[0].CountryId;
                $scope.TeacherSchool.CountryName = City_Filter[0].CountryName;
            }
        }
        $scope.Logout = function () {
            modalInstance.dismiss();
            Service.Logout();
        }
        $scope.validationOptions = {
            rules: {
                SchPinCode: {
                    required: true,
                    maxlength: 6,
                    minlength: 6
                },
                SchName: {
                    required:true,
                    maxlength: 50
                },
                SchAddress: {
                    required: true,
                    maxlength: 150
                },
                CityId: {
                    required: true
                }
            }
        }

        $scope.Submit = function (form) {
            if (form.validate() == false)
                return false;
            debugger;
            Service.Create_Update($scope.TeacherSchool, 'school/schManagement/TeacherSchool')

              .then(function (response) {
                  Service.Notification($rootScope, response.message);
                  if (response.result == 'Success') {
                      $rootScope.UserInfo.SchId = response.SchId;
                      modalInstance.dismiss();
                  }
              });
        }
    }
    app.controller('teacherSchoolController', ['$scope', '$rootScope', 'Service', '$filter', '$uibModalInstance', teacherSchool_fun]);

})(angular.module('SilverZone_Teacher_App'));