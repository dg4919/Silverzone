
(function (app) {
    var lot_fun = function ($scope, $rootScope, Service, $filter, $state, $uibModal, SchoolService) {
        $rootScope.HeaderAction = "Templates/School/Dispatch/QuestionPaper/Header.html";
               
        $scope.Search = {};

        Service.Get('school/DispatchQP/GetLotNoList').then(function (response) {
            $scope.LotNoList = response.LotNoList;
        });

        $scope.LotNo_Changed = function (LotId) {
            Service.Get('school/DispatchQP/GetRegNoList?LotId=' + LotId).then(function (response) {
                $scope.RegNoList = response.RegNoList;
            });
        }
        $scope.RegNo_Changed = function (EventManagementId) {
            Service.Get('school/DispatchQP/GetQPDispatchDetail?EventManagementId=' + EventManagementId).then(function (response) {
                $scope.QPDispatchDetail = response.QPDispatchDetail;
                $scope.QPDispatchDetail_Copy = angular.copy($scope.QPDispatchDetail);
            });
        }
        $scope.QPFrom_Changed = function (From) {
            debugger;
            if (From == "")
                $scope.QPDispatchDetail.To = "";
            else
                $scope.QPDispatchDetail.To = parseInt(From) + $scope.QPDispatchDetail.OMR;
        }

        $scope.Save = function (form) {
            prompt('', JSON.stringify($scope.QPDispatchDetail));
        }
    };

   

    app.controller('dispatchQPController', ['$scope', '$rootScope', 'Service', '$filter', '$state', '$uibModal', 'SchoolService', lot_fun]);

})(angular.module('SilverZone_Report_App'));