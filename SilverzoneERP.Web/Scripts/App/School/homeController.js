

(function (app) {
    var Home_fun = function ($scope, $rootScope, Service, $filter, $state, $uibModal) {
        $scope.welcome = 'Welcome To Silverzone';

       

         function TeacherSchool() {
            var modalInstance = $uibModal.open({
                controller: 'teacherSchoolController',
                templateUrl: 'Templates/Teacher/Website/TeacherSchool/Index.html',
                windowClass: 'model-large',
                backdrop: 'static',
                keyboard: false
            });

            modalInstance.result.then(function (response) {
                debugger;

                //on ok button press 
            }, function () {
                //on cancel button press
                console.log("Modal Closed");
            });
         }

         //prompt('', JSON.stringify($rootScope.UserInfo));
         if($rootScope.UserInfo.SchId ==null && angular.lowercase($rootScope.UserInfo.RoleName) == 'teacher')
            TeacherSchool();
    }
    
    app.controller('homeController', ['$scope', '$rootScope', 'Service', '$filter', '$state', '$uibModal', Home_fun]);

})(angular.module('SilverzoneERP_App'));