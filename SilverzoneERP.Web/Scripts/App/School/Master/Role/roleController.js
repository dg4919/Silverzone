(function (app) {
    
    var role_fun = function ($scope, $rootScope, Service, $filter, roleService) {
        $rootScope.HeaderAction = "Templates/Common/Role/Header.html";
        
        $scope.Role = {};
        $scope.resetCopy = angular.copy($scope.Role);
        var IsActiveList = [];
        var RoleList_All;
        $scope.Changed = false;
        $scope.selectedIndex = "1";
        $scope.isEdit = false;

        function Reset() {
            $scope.Role = angular.copy($scope.resetCopy);
            $scope.Role.FormDetails = JSON.parse(forms);
            $scope.isEdit = false;
            $scope.selectedIndex = "1";
            $scope.Changed = false;
            IsActiveList = [];
            RoleList_All = null;           
        }
        $scope.Add = function () {
            debugger;
            $scope.isEdit = true;            
        }
        $scope.Back = function () {
            debugger;
            Reset();            
        }

        $scope.Submit = function () {                    
            //return false;
            roleService.Submit($scope).then(function (response) {
                if (response.result == 'Success') {
                    alert(response.message);
                    $scope.Cancel();
                    GetRolePermission();
                    $scope.Role = { "RoleId": 0, "FormDetails": JSON.parse(forms) };
                    $scope.isAdd = false;
                }
                else                    
                    Service.Notification($rootScope, response.message);                    
            });
        }

        function GetRolePermission() {
            Service.Get('school/role/GetRolePermission').then(function (response) {
                debugger;
                $scope.RolePermissionList = response.result;
            });
        }
        var forms;
        function GetFormGroupWise() {
            Service.Get('school/formManagement/GetFormGroupWise').then(function (response) {
                $scope.Role.FormDetails = response.result;
                forms = JSON.stringify(response.result);
            });
        }
        GetFormGroupWise();
        GetRolePermission();

        $scope.Edit = function (index) {
            $scope.Role = { "RoleId": 0, "FormDetails": JSON.parse(forms) };
            roleService.Edit(index, $scope);
            $scope.isAdd = true;
            $scope.isCheckedPermission = true;
        }
            
        var firstTime = true;         
        $scope.Cancel = function () {
            debugger;
            $scope.isEdit = !$scope.isEdit;
            $scope.Role = { "RoleId": 0 };            
            //Clear selected Permission
            $scope.Role.FormDetails =JSON.parse(forms);           
        }
        
        $scope.Checked_Unchecked_GroupWise = function (data, index, isDirect) {
            debugger;
            isPermission();
            roleService.Checked_Unchecked_GroupWise(data, index, isDirect);
        }

        $scope.Checked_Unchecked = function (data, index)
        {
            isPermission();
            roleService.Checked_Unchecked(data, index);                  
        }

        $scope.All = function (data) {
            isPermission();
            roleService.All(data)           
        }
        $scope.All_Add = function (data) {
            debugger;
            isPermission();
            roleService.All_Add(data);
        }

        $scope.All_Edit = function (data) {
            debugger;
            isPermission();
            roleService.All_Edit(data);
        }

        $scope.All_Delete = function (data) {
            debugger;
            isPermission();
            roleService.All_Delete(data);
        }

        $scope.All_Read = function (data) {
            debugger;
            isPermission();
            roleService.All_Read(data);
        }

        $scope.All_Print = function (data) {
            debugger;
            isPermission();
            roleService.All_Print(data);
        }
        $scope.isCheckedPermission = false;
        function isPermission() {//Check here atleast one permission is checked or not
           // debugger;
            $scope.isCheckedPermission = false;
           // debugger;
            if (angular.isUndefined($scope.Role.FormDetails)) {
                $scope.isCheckedPermission = false;
                return false;
            }
            $.each($scope.Role.FormDetails, function (i, item) {
                $.each(item.Forms, function (i, frm) {
                    //debugger;
                    if (frm.Permission.Add == true || frm.Permission.Edit == true || frm.Permission.Read == true || frm.Permission.Print == true || frm.Permission.Delete == true) {
                        {
                            $scope.isCheckedPermission = true;
                            return false;
                        }
                    }
                });
            });
        }
    }

  
    app.controller('roleController', ['$scope', '$rootScope', 'Service', '$filter', 'roleService', role_fun]);
   
})(angular.module('SilverzoneERP_App'));