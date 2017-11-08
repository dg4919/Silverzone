(function () {
    'use strict';

    var fun = function ($http, $q, $uibModal,Service) {
        var fac = {}

        fac.Confirm = function (OrderNo, IsConfirm, IsBook, scope, ActionName) {
            debugger;
            var modalInstance = $uibModal.open({
                controller: 'confirmOrderController',
                templateUrl: 'Templates/Teacher/Mobile/confEnrollOrder/Partial/Dialog_Remarks.html',
                backdrop: 'static',
                windowClass: 'app-modal-window',
                resolve: {
                    OrderNo: function () {
                        return angular.copy(OrderNo);
                    },
                    IsConfirm: function () {
                        return angular.copy(IsConfirm);
                    }
                    ,
                    IsBook: function () {
                        return angular.copy(IsBook);
                    }
                }
            });

            modalInstance.result.then(function (response) {
                debugger;                
                fac.GetOrderList(scope, ActionName);
                scope.Actionfilter.IsChange = true;
                //on ok button press 
            }, function () {
                //on cancel button press
                console.log("Modal Closed");
            });
        }

        fac.GetOrderList = function (scope, ActionName) {
            debugger;
            var IsConfirm = false
            
            if (angular.lowercase(scope.Select.Tab) == 'confirm')
                IsConfirm = true;
            if (!angular.isUndefined(scope.Actionfilter.IsChange))
                scope.Preserve_EventCode = scope.Actionfilter.EventCode;

            scope.Actionfilter.IsChange = false;
            Service.Get('TeacherApp/StudentRegistration/' + ActionName + '?EventCode=' + scope.Actionfilter.EventCode + '&IsConfirm=' + IsConfirm + '&From=' + scope.Actionfilter.From + '&To=' + scope.Actionfilter.To).then(function (response) {
                debugger;
                if (angular.lowercase(scope.Select.Tab) == 'confirm')
                    scope.Confirm_OrderList = response.StudentEnrollList;
                else
                    scope.Un_Confirm_OrderList = response.StudentEnrollList;
            });
        }

        return fac;
    }

    angular.module('SilverzoneERP_App')
    .factory('ConfirmOrderService', ['$http', '$q', '$uibModal', 'Service', fun]);
})();



(function (app) {
    'use strict';

    var Confirm_Fun = function ($scope, $rootScope, modalInstance, $filter, Service, OrderNo, IsConfirm, IsBook) {

        debugger;
        $scope.Remarks = { OrderNo: OrderNo, IsConfirm: IsConfirm };

        Service.Get('School/remarks/RemarksList?UserId=' + $scope.Remarks.UserId + '&OrderNo=' + $scope.Remarks.OrderNo).then(function (response) {
            debugger;
            $scope.RemarksList = response.RemarksList;
        });

        $scope.Back = function () {
            debugger;
            modalInstance.dismiss();
        }

        $scope.Submit = function (form) {
            debugger;
            if (form.validate() == false)
                return false;

            Service.Create_Update($scope.Remarks, 'school/remarks/save?IsConfirm=' + $scope.Remarks.IsConfirm + '&IsBook=' + IsBook)
                  .then(function (response) {
                      Service.Notification($rootScope, response.message);
                      if (angular.lowercase(response.result) == 'success') {
                          modalInstance.close();
                      }
                  });
        }
        $scope.validationOptions = {
            rules: {
                Description: {
                    required: true
                }
            }
        }
    }

    app.controller('confirmOrderController', ['$scope', '$rootScope', '$uibModalInstance', '$filter', 'Service', 'OrderNo', 'IsConfirm', 'IsBook', Confirm_Fun]);

})(angular.module('SilverZone_Teacher_App'));






