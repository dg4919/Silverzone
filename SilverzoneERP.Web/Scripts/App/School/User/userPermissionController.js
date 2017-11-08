
(function(app) {    

    var userPermission_fun = function ($scope, $rootScope, Service, userPermissionService, globalConfig, $filter, roleService, $q, $timeout, $http, $location, localStorageService, $modal) {
        
        $rootScope.HeaderAction = "Templates/School/User/User_Permission_Header.html";

        $scope.UserPermission = { "UserId": 0, "Forms": [], "RoleId": 0 };
        $scope.resetCopy = angular.copy($scope.UserPermission);       
        var RoleList_All;
        $scope.Changed = false;        
        $scope.isEdit = false;
        $scope.UserPermissionList = [];

        $rootScope.UserInfo = localStorageService.get('UserInfo');

        function GetUserList() {
            Service.Get('school/account/GetAllUser').then(function (response) {
                $scope.UserList = response.result;
            });
        }

        GetUserList();

        function Reset() {            
            $scope.UserPermission = angular.copy($scope.resetCopy);

            $scope.UserPermission = { "UserId": 0, "Forms": [], "RoleId": 0 };
            $scope.resetCopy = angular.copy($scope.UserPermission);
            var RoleList_All;
            $scope.Changed = false;
            $scope.isEdit = false;
            $scope.UserPermissionList = [];
        }
        $scope.Back = function () {
            Reset()
        }
        
          

        $scope.newState = function (state) {
            alert("Sorry! You'll need to create a Constitution for " + state + " first!");
        }

      
        $scope.searchTextChange = function (UserName) {            
            $scope.UserList = GetUserList(UserName);      
        }

        $scope.selectedItemChange = function (UserId) {
            debugger;

            $scope.UserPermissionList = [];
            $scope.selectedItem = $filter("filter")($scope.UserList, { Id: UserId }, true);
            if ($scope.selectedItem.lenght != 0)
            {
                $scope.UserPermission.UserId = UserId;
                $scope.UserPermission.RoleId = $scope.selectedItem[0].RoleId;
                
                GetUserPermissionList($scope.UserPermission.UserId, $scope.UserPermission.RoleId);
                $scope.selectedItem = $scope.selectedItem[0];                
                
            }
            else
            {
                $scope.UserPermission.UserId = 0;
                $scope.UserPermission.RoleId = 0;
            }                       
        }
        
        var Preserve_UserPermissionList;

        function GetUserPermissionList(UserId, RoleId) {
            debugger;
            if (UserId == 0 && RoleId == 0)
            {
                Preserve_UserPermissionList = [];
                $scope.UserPermissionList = [];
            }
            else {
                userPermissionService.GetUserPermission(UserId, RoleId).then(function (response) {
                    debugger;
                    Preserve_UserPermissionList = response.Preserve_UserPermission;
                    $scope.UserPermissionList = response.UserPermission;
                    if ($scope.UserPermissionList.lenght != 0)
                        $scope.isEdit = true;
                    else
                        alert('Permission not assigned ');
                });
            }            
        }
       
        $scope.Checked_Unchecked_GroupWise = function (data, index, isDirect) {
            roleService.Checked_Unchecked_GroupWise(data, index, isDirect);           
        }

        $scope.Checked_Unchecked = function (data, index) {
            roleService.Checked_Unchecked(data, index);            
        }

        $scope.All = function (data) {            
            debugger;
            roleService.All(data);            
        }
        
        $scope.Submit = function () {

            debugger;
            if ($scope.UserPermission.UserId == 0)
                return false;

            userPermissionService.Submit($scope.UserPermission.UserId, $scope.UserPermissionList, Preserve_UserPermissionList)
            .then(function (response) {
                alert(response.message);
                Reset();
            });
          
        }
                                
    }

    
    app.controller('userPermissionController', ['$scope', '$rootScope', 'Service', 'userPermissionService', 'globalConfig', '$filter', 'roleService', '$q', '$timeout', '$http', '$location', 'localStorageService', userPermission_fun]);
})(angular.module('SilverzoneERP_App'));
