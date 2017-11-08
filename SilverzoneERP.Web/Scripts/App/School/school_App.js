

/// <reference path="../../Lib/angular-1.5.8.js" />

(function () {
    var configFn = function ($stateProvider) {

        $stateProvider.state("ERP_Login", {
            url: '/ERP/Login',
            templateUrl: 'Templates/Common/Login/Index.html',
            controller: 'accountController',
            params: {
                RoleName: null
            }
        })
        .state("ERP_Home", {
            url: '/ERP/Home',
            templateUrl: 'Templates/Common/Home/Index.html',
            controller: 'homeController'
        })
        .state("Registration", {
            url: '/ERP/User',
            templateUrl: 'Templates/School/User/Create.html',
            controller: 'registrationController'
        })
        .state('Role', {
            url: '/ERP/Role',
            templateUrl: 'Templates/Common/Role/Create.html',
            controller: 'roleController'
        })
        .state('UserRole', {
            url: '/ERP/UserRole',
            templateUrl: 'Templates/Common/Role/UserRole.html',
            controller: 'userRoleController'
        })
        .state('UserSummary', {
            url: '/ERP/UserSummary',
            templateUrl: 'Templates/School/User/Summary.html',
            controller: 'userSummaryController'
        })
        .state('UserPermission', {
            url: '/ERP/UserPermission',
            templateUrl: 'Templates/School/User/UserPermission.html',
            controller: 'userPermissionController'
        })
        .state('Location', {
            url: '/ERP/Location',
            templateUrl: 'Templates/School/Master/Location/Index.html',
            controller: 'locationController'
        })
         .state('Designtion', {
             url: '/ERP/Designation',
             templateUrl: 'Templates/School/Master/Designation/Index.html',
             controller: 'designationController'
         })
      .state('Title', {
          url: '/ERP/Title',
          templateUrl: 'Templates/School/Master/Title/Index.html',
          controller: 'titleController'
      })
       .state('Event', {
           url: '/ERP/Event',
           templateUrl: 'Templates/School/Master/Event/Create.html',
           controller: 'eventController'
       })
      .state('EventYear', {
          url: '/ERP/EventYear',
          templateUrl: 'Templates/School/Master/Event/YearManage.html',
          controller: 'eventYearController'
      })
       .state('SchoolManagement', {
           url: '/ERP/SchoolManagement',
           templateUrl: 'Templates/School/Olympiad/SchoolManagement/Index.html',
           controller: 'schoolManagementController',
           params: {
               SchCode: null
           }
       })
       .state('Category', {
           url: '/ERP/Category',
           templateUrl: 'Templates/School/Master/Category/Create.html',
           controller: 'categoryController'
       })
        .state('Class', {
            url: '/ERP/Class',
            templateUrl: 'Templates/School/Master/Class/Index.html',
            controller: 'classController'
        })
       .state('SchoolGroup', {
           url: '/ERP/SchoolGroup',
           templateUrl: 'Templates/School/Master/SchoolGroup/Create.html',
           controller: 'schoolGroupController'
       })
       .state('ExamDate', {
           url: '/ERP/ExamDate',
           templateUrl: 'Templates/School/Master/ExamDate/Index.html',
           controller: 'examDateController'
       })
       .state('EventManagement', {
           url: '/ERP/EventManagement',
           templateUrl: 'Templates/School/Olympiad/EventManagement/Index.html',
           controller: 'eventManagementController',
           params: {
               SchCode: null
           }
       })
      .state('StudentEntry', {
          url: '/ERP/StudentEntry',
          templateUrl: 'Templates/School/Olympiad/StudentEntry/Index.html',
          controller: 'studentEntryController',
          params: {
              SchCode: null,
              EnrollmentOrderId:null
          }
      })
        .state('Courier', {
            url: '/ERP/Courier',
            templateUrl: 'Templates/School/Master/Courier/Create.html',
            controller: 'courierController'
        })
        .state('VerifyOrder', {
            url: '/ERP/VerifyOrder',
            templateUrl: 'Templates/School/Olympiad/VerifyOrder/Index.html',
            controller: 'verifyOrderController'
        })
         .state('WebCamra', {
             url: '/ERP/WebCamra',
             templateUrl: 'Templates/School/WebCamra.html',
             controller: 'mainController'
         })
         .state('ExportImport', {
             url: '/ERP/ExportImport',
             templateUrl: 'Templates/School/Master/ExportImport/Index.html',
             controller: 'exportImportController'
         })
        
        .state('Profile', {
            url: '/ERP/Profile',
            templateUrl: 'Templates/Common/Profile/Index.html',
            controller: 'profileController'
        })
        .state('EnrollmentSummary', {
            url: '/ERP/Summary/Enrollment',
            templateUrl: 'Templates/School/Summary/Enrollment/Index.html',
            controller: 'summaryController',
            params: {
                Title: 'Enrollment'
            }
        })
        .state('BookOrderSummary', {
            url: '/ERP/Summary/BookOrder',
            templateUrl: 'Templates/School/Summary/BookOrder/Index.html',
            controller: 'summaryController',
            params: {
                Title: 'Book Order'
            }
        })


        //************ New Routes  ***************

        .state('sheetRecievie', {
            url: '/ERP/sheetRecieving',
            templateUrl: 'Templates/School/Answer_sheetRecieving/sheetRecieving.html',
            controller: 'answerReciveController'
        })
        ;
    }

    angular.module('Silverzone_school_App', [])
  .config(['$stateProvider', configFn]);

})();