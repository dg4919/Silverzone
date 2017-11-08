


(function (app) {

    var teacher_fun = function ($scope, $location, $rootScope, Service, $filter, $state, $uibModal, globalConfig) {        
        $rootScope.HeaderAction = "Templates/Teacher/Mobile/User/Partial/Header.html";
        $scope.itemsPerPage = 10;
        $scope.currentPage = 1;
        $scope.rangeSize = 5;
        $scope.startIndex = 0;
        $scope.Filter = { selectedIndex: "2", From: "", To: "", AnySearch: "", IsTeacherApp :"1"};
        var TeacherList_Preserve;
        $scope.IsActiveList = [];
      //  alert($location.host());
        $scope.IsActive = function (Id)
        {           
            Service.Get('School/user/ActivateTeacher?Id=' + Id + '&IsTeacherApp=' + ($scope.Filter.IsTeacherApp == 1 ? true : false)).then(function (response) {
                debugger;
                Service.Notification($rootScope, response.message);
                GetTeacherList(preserve_NewValue, preserve_OldValue);
            });
        }
      
        var preserve_NewValue, preserve_OldValue;

        $scope.$watch("currentPage", function (newValue, oldValue) {
            debugger;
            GetTeacherList(newValue, oldValue);
            preserve_NewValue = newValue;
            preserve_OldValue = oldValue;
        });

        function GetTeacherList(newValue, oldValue) {
            debugger;
            var startIndex = ((newValue - 1) * $scope.itemsPerPage);
            var IsAcive;
            
            if ($scope.Filter.selectedIndex == "1")
                IsAcive = true;
            else if ($scope.Filter.selectedIndex == "2")
                IsAcive = false;
            Service.Get('School/user/GetTeacherlist?StartIndex=' + startIndex + '&Limit=' + $scope.itemsPerPage + '&IsTeacherApp=' + ($scope.Filter.IsTeacherApp == 1 ? true : false) + '&From=' + $scope.Filter.From + '&To=' + $scope.Filter.To + '&AnySearch=' + $scope.Filter.AnySearch + '&IsActive=' + IsAcive)
                .then(function (response) {

                    debugger;
                    $scope.TeacherList = angular.copy(response.TeacherList);
                    $scope.total = response.Count;
                    $scope.AllCount = response.AllCount;

                    angular.forEach($scope.IsActiveList, function (item) {
                        var data = $filter('filter')($scope.TeacherList, function (entity) {
                            return entity.Id == item.Id;
                        });

                        if (data.length != 0)
                            data[0].IsChecked = true;
                    });
                    
                    //$scope.SelectedIndexChanged($scope.Filter.selectedIndex);

                    $scope.range = function () {
                        var ret = [];
                        var start = ((Math.ceil($scope.currentPage / $scope.rangeSize) - 1) * $scope.rangeSize) + 1;
                        for (var i = start; i < (start + $scope.rangeSize) && i <= $scope.pageCount() ; i++)
                            ret.push(i);
                        return ret;
                    };


                    $scope.prevPage = function () {
                        if ($scope.currentPage > 1) {
                            $scope.currentPage--;
                        }
                    };

                    $scope.prevPageDisabled = function () {
                        return $scope.currentPage === 1 ? "disabled" : "";
                    };

                    $scope.firstPage = function () {
                        $scope.currentPage = 1;
                    }
                    $scope.firstPageDisabled = function () {
                        return $scope.currentPage === 1 ? "disabled" : "";
                    };

                    $scope.lastPage = function () {
                        $scope.currentPage = $scope.pageCount();
                    }
                    $scope.lastPageDisabled = function () {
                        return $scope.currentPage === $scope.pageCount() ? "disabled" : "";
                    };

                    $scope.nextPage = function () {
                        if ($scope.currentPage <= $scope.pageCount()) {
                            $scope.currentPage++;
                        }
                    };

                    $scope.nextPageDisabled = function () {
                        return $scope.currentPage === $scope.pageCount() ? "disabled" : "";
                    };

                    $scope.pageCount = function () {
                        return Math.ceil($scope.total / $scope.itemsPerPage);
                    };

                    $scope.setPage = function (n) {
                        if (n > 0 && n <= $scope.pageCount()) {
                            $scope.currentPage = n;
                        }
                    };

                }, function () {

                })
        }   

        $scope.View = function (UserId) {
            debugger;
            var Parameter = { UserId: UserId, Filter: $scope.Filter };
            var modalInstance = $uibModal.open({
                controller: 'ViewController',
                templateUrl: 'Templates/Teacher/Mobile/User/Partial/Dialog_View.html',
                backdrop: 'static',
                windowClass: 'app-modal-window',
                resolve: {
                    Parameter: function () {
                        return angular.copy(Parameter);
                    }
                }
            });

            modalInstance.result.then(function (response) {
                debugger;
                GetTeacherList(preserve_NewValue, preserve_OldValue);
                //on ok button press 
            }, function () {
                //on cancel button press
                console.log("Modal Closed");
            });
        }

        $scope.SelectedIndexChanged = function (selectedIndex) {
            debugger;
            GetTeacherList(1, 1);
            //$scope.TeacherList = Service.SelectedIndexChanged(selectedIndex, TeacherList_Preserve);
        }

        $scope.Search = function () {
            $scope.currentPage = 1;
            GetTeacherList(1, 1);
        }

        $scope.AddToDelete = function (UserId) {
            $scope.IsActiveList = Service.IsActive(UserId, $scope.IsActiveList, $scope);
            //prompt('', JSON.stringify(IsActiveList));
        }
        $scope.Delete = function () {
            debugger;
            $.confirm({
                title: $rootScope.Title,
                content: 'Do you want to delete ?',
                buttons: {
                    YES: function () {
                        Service.Create_Update($scope.IsActiveList, 'School/user/DeleteTeacher').then(function (response) {
                            debugger;
                            Service.Notification($rootScope, response.message);
                            if (angular.lowercase(response.result) == 'success')
                            {
                                $scope.currentPage = 1;
                                $scope.IsActiveList = [];
                                GetTeacherList(1, 1);
                            }
                                
                        });
                    },
                    NO: function () {

                    }
                }
            });            
        }
    }
   
    var View_Fun = function ($scope, $rootScope, modalInstance, $filter, Service, Parameter) {

        debugger;

        function Reset() {            
            Service.Get('School/user/GetTeacherDetail?UserId=' + Parameter.UserId).then(function (response) {
                debugger;
                $scope.TeacherDetail = response.TeacherDetail;
            });

            $scope.Remarks = { UserId: Parameter.UserId };
            Service.Get('School/remarks/RemarksList?UserId=' + $scope.Remarks.UserId + '&OrderNo=').then(function (response) {
                debugger;
                $scope.RemarksList = response.RemarksList;
            });
        }
        Reset();

        $scope.Back = function () {
            debugger;
            modalInstance.dismiss();
        }

        $scope.IsActive = function (form) {
            debugger;
            if (form.validate() == false)
                return false;

            Service.Create_Update($scope.Remarks, 'School/user/ActivateTeacher?SchoolCode=' + $scope.TeacherDetail.SchoolCode + '&IsTeacherApp=' + (Parameter.Filter.IsTeacherApp == 1 ? true : false) + '&IsConfirm=' + $scope.TeacherDetail.User.IsActive).then(function (response) {
                debugger;
                Service.Notification($rootScope, response.message);
                if (angular.lowercase(response.result) == 'success')
                    modalInstance.close();
                
            });
        }
       
        $scope.validationOptions = {
            rules: {
                SchoolCode: {
                   required: true
                },
                Description: {
                    required: true
                }
            }
        }
    }

    app.controller('teacherController', ['$scope', '$location', '$rootScope', 'Service', '$filter', '$state', '$uibModal', 'globalConfig', teacher_fun])   
    .controller('ViewController', ['$scope', '$rootScope', '$uibModalInstance', '$filter', 'Service', 'Parameter', View_Fun]);

})(angular.module('SilverZone_Teacher_App'));