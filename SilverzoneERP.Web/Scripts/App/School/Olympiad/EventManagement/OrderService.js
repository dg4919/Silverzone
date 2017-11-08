(function (app) {
    'use strict'

    var fun = function ($http, $q, globalConfig, $filter, $uibModal, Service, $rootScope, SchoolService) {

        var fact = {}

        fact.Enrollment = function (scope, rootScope, data) {
            debugger;

            var modalInstance = $uibModal.open({
                controller: 'EnrollmentOrderController',
                templateUrl: 'Templates/School/Olympiad/EventManagement/Partial/Dialog_Enrollment_Order.html',
                backdrop: 'static',
                resolve: {
                    ExamDateList: function () {
                        return angular.copy(scope.ExamDateList);
                    },
                    EnrollmentOrder: function () {
                        return angular.copy(data);
                    },
                    ExaminationDetails: function () {
                        return angular.copy(scope.ExaminationDetails);
                    },
                    SchMngt: function () {
                        return angular.copy(scope.SchMngt);
                    }
                }
            });

            modalInstance.result.then(function (response) {
                debugger;
                SchoolService.SearchBySchoolCode(scope, scope.SchMngt.SchCode, rootScope, false);
               
                //on ok button press 
            }, function () {
                //on cancel button press
                console.log("Modal Closed");
            });
        }

        fact.Book = function (scope,data) {
            var modalInstance = $uibModal.open({
                controller: 'BookOrderController',
                templateUrl: 'Templates/School/EventManagement/Partial/Dialog_Book_Order.html',
                windowClass: 'model-large',
                backdrop: 'static',
                resolve: {
                    data: function () {
                        return data;
                    }
                }
            });

            modalInstance.result.then(function (response) {
                debugger;
               
                //on ok button press 
            }, function () {
                //on cancel button press  
                debugger;
                fact.Get_BookOrder(scope);
                console.log("Modal Closed");
            });
        }

        fact.BookVerify = function (scope, data) {
            var modalInstance = $uibModal.open({
                controller: 'verifyBookOrderController',
                templateUrl: 'Templates/School/VerifyOrder/Partial/Dialog_Verify_Book_Order.html',
                windowClass: 'model-large',
                backdrop: 'static',
                resolve: {
                    data: function () {
                        return data;
                    }
                }
            });

            modalInstance.result.then(function (response) {
                debugger;
                fact.GetOrder(scope);
                //on ok button press 
            }, function () {
                //on cancel button press  
                debugger;
              
                console.log("Modal Closed");
            });
        }

        fact.Get_BookOrder = function (scope) {
            debugger;
            if (angular.isUndefined(scope.SchMngt.EventManagement.Id))
                scope.BookOrderlist = [];
            else
            {
                Service.Get('school/EventManagement/AllOrderBySchool?EventManagementId=' + scope.SchMngt.EventManagement.Id).then(function (response) {
                    scope.BookOrderlist = response.result;
                });
            }
            
        }

        function GetExamDate(ExaminationDateId, scope) {
            debugger;

            var ExamDate_filter = $filter("filter")(scope.ExamDateList, { ExaminationDateId: ExaminationDateId }, true);
            if (ExamDate_filter.length != 0) {
                return ExamDate_filter[0].ExamDate;
            }
            return null;
        }
        fact.GetOrder = function (scope) {
            Service.Get('/school/verifyOrder/GetOrder').then(function (response) {
                scope.BookOrderlist = response.result;
            });
        }
        return fact;
    }

    app.factory('OrderService', ['$http', '$q', 'globalConfig', '$filter', '$uibModal', 'Service', '$rootScope', 'SchoolService', fun]);

})(angular.module('SilverzoneERP_App'));