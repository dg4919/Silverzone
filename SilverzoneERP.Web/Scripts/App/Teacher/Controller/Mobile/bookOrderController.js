

(function (app) {
    'use strict';

    var myFn = function ($scope, $location, $rootScope, Service, $filter, $state, $uibModal, globalConfig, ConfirmOrderService) {        
        $rootScope.HeaderAction = "Templates/Teacher/Mobile/BookOrder/Partial/Header.html";

        $scope.Preserve_EventCode;

        Service.Get('TeacherApp/StudentRegistration/GetEventCodeList?IsBook=true').then(function (response) {
            debugger;
            $scope.EventCodeList = response.EventCodeList;
        });
        var ActionName = 'GetBookOrderList';
        $scope.Select = { Tab: 'Un-Confirm' };
        $scope.Search = function (form) {
            debugger;
            if (form.validate() == false)
                return false;
                                   
            ConfirmOrderService.GetOrderList($scope, ActionName);
        }
        
        $scope.Tab_Change = function (TabName) {
            debugger;
            if (angular.isUndefined($scope.Actionfilter) || $scope.Actionfilter.EventCode == null)
                return false;
            $scope.Select.Tab = TabName;
            if ($scope.Preserve_EventCode != $scope.Actionfilter.EventCode || $scope.Actionfilter.IsChange == true)
            {                              
                ConfirmOrderService.GetOrderList($scope, ActionName);
            }            
        }

        $scope.Reset = function () {
            $state.reload();
        }
        

        $scope.validationOptions = {
            rules: {
                EventCode: {
                    required: true
                }
            }
        }

        $scope.Confirm = function (OrderNo, IsConfirm) {
            debugger;
            ConfirmOrderService.Confirm(OrderNo, IsConfirm, true, $scope, ActionName);
        }

    }  

    app.controller('bookOrderController', ['$scope', '$location', '$rootScope', 'Service', '$filter', '$state', '$uibModal', 'globalConfig', 'ConfirmOrderService', myFn])
    ;

})(angular.module('SilverZone_Teacher_App'));