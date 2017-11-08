
(function (app) {

    var enrollmentLog_fun = function ($scope, $rootScope, Service, $filter, roleService, $stateParams) {
        $rootScope.HeaderAction = "Templates/School/Summary/Header.html";
        $scope.Title = $stateParams.Title;
        function GetUserList() {
            Service.Get('school/summary/GetUserList').then(function (response) {
                $scope.UserList = response.UserList;
            });
        }
        GetUserList();
        $scope.Filter = {};
        function GenerateQuery() {            
            var From = '';
            var To = '';
            var EventId = '';
            var UserId = '';
            if (!angular.isUndefined($scope.Filter.DateFrom))
                From = $scope.Filter.DateFrom;
            if (!angular.isUndefined($scope.Filter.DateTo))
                To = $scope.Filter.DateTo;
            if (!angular.isUndefined($scope.Filter.EventId))
                EventId = $scope.Filter.EventId;
            if (!angular.isUndefined($scope.Filter.UserId))
                UserId = $scope.Filter.UserId;
            var Query = 'From=' + From + '&To=' + To + '&EventId=' + EventId + '&UserId=' + UserId;
            return Query;
        }        
        $scope.GetSummary = function (ActionName) {
            
            var URL = 'school/summary/' + ActionName + '?' + GenerateQuery();

            Service.Get(URL).then(function (response) {
                debugger;
                $scope.Result = response;
                
                $scope.Summary = angular.copy(response.BookCategory);
            });
        }

        $scope.GetQty = function (PurchaseOrder, bookCategory) {
           // debugger;
            var data = $filter('filter')(PurchaseOrder, function (entity) {
                return entity.Title === bookCategory.Title;
            });
            if (data.length != 0)
            {               
                return data[0].sumQty;
            }
                
            return 0;
        }
    }


    app.controller('summaryController', ['$scope', '$rootScope', 'Service', '$filter', 'roleService', '$stateParams', enrollmentLog_fun]);

})(angular.module('SilverzoneERP_App'));