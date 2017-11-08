(function () {
    var configFn = function ($stateProvider) {
        //==================Website========================
        $stateProvider.state("BookOrder", {
            url: '/Teacher/BookOrder',
            templateUrl: 'Templates/Teacher/BookOrder/Index.html',
            controller: 'teacherBookOrderController'
        })
        .state("StudentEnrollment", {
            url: '/Teacher/StudentEnrollment',
            templateUrl: 'Templates/Teacher/StudentEnrollment/Index.html',
            controller: 'studentEnrollmentController'
        })
        //=================Mobile=========================
        .state('Teacher', {
            url: '/Teacher/User',
            templateUrl: 'Templates/Teacher/Mobile/User/Index.html',
            controller: 'teacherController'
        })
        .state('ConfirmEnrollmentOrder', {
            url: '/Teacher/ConfirmEnrollmentOrder',
            templateUrl: 'Templates/Teacher/Mobile/confEnrollOrder/Index.html',
            controller: 'confirmEnrollmentOrderController'
        })
        .state('ConfirmBookOrder', {
            url: '/Teacher/ConfirmBookOrder',
            templateUrl: 'Templates/Teacher/Mobile/BookOrder/Index.html',
            controller: 'bookOrderController'
        })
          .state('GooglePlayVersion', {
              url: '/Teacher/GooglePlayVersion',
              templateUrl: 'Templates/Teacher/Mobile/GooglePlayVersion/Index.html',
            controller: 'googlePlayVersionController'
          })
        .state('Import', {
            url: '/Teacher/Import',
            templateUrl: 'Templates/Teacher/Mobile/Import/Index.html',
            controller: 'googlePlayVersionController'
        });
    };

    angular.module('SilverZone_Teacher_App', [])
    .config(['$stateProvider', configFn]);

})();