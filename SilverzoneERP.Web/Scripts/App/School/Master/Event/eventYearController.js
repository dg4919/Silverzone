
(function (app) {
    var EventYear_fun = function ($scope, $location, $rootScope, Service, $state, $filter,localStorageService) {
        debugger;
        $rootScope.HeaderAction = "Templates/School/Master/Event/EventYear_Header.html";
        //Start Declare Global Variable
        $scope.YearList = [];

        var EventYearInfo = localStorageService.get('EventYearInfo');
        $scope.YearList.push(EventYearInfo.Current);
        $scope.YearList.push(EventYearInfo.Next);
     

        $scope.EventYear = {
            "selectedEventYear": EventYearInfo.Current.Code,
            "EventYearCode": EventYearInfo.Current.Code,
            "Event_Year": EventYearInfo.Current.Year
        };
        $scope.resetCopy = angular.copy($scope.EventYear);
        var IsActiveList = [];
        var EventYearList_All;
        $scope.Changed = false;
        $scope.selectedIndex = "1";
        $scope.isEdit = false;
        //$scope.selectedEventYear = EventYearInfo.Current.Code;
        //End Declare Global Variable
                                          
        function GetEvent() {
            Service.Get('school/event/Get?IsClass=true').then(function (response) {
                debugger;
                $scope.Event_Class_List = response.result;                
            });
        }

        function GetEventYear() {
            Service.Get('school/eventYear/Get').then(function (response) {
                EventYearList_All = response.result;
                $rootScope.EventInfo = response.Event;
                $scope.EventYearList_Year = response.result;
                $scope.SelectedIndexChanged($scope.selectedIndex, $scope.EventYear.selectedEventYear);
            });
        }

        GetEvent();
        GetEventYear();

        $scope.Back = function () {
            debugger;
            $scope.EventYear = angular.copy($scope.resetCopy);
            $scope.isEdit = false;           
            Service.Reset($scope);
            $scope.EventYear.selectedEventYear = EventYearInfo.Current.Code;
        }

        $scope.Event_Year_Change = function (_eventYear) {
            debugger;            
            var _eventYear_filter = $filter('filter')($scope.YearList, { Year: _eventYear }, true);
            if (_eventYear_filter.length != 0)
            {
                $scope.EventYear.EventYearCode = _eventYear_filter[0].Code;               
            }
                
        }

        $scope.Add = function () {
            debugger;
            if ($scope.Changed) {
                var result = confirm('Do you want to save change ?');
                if (result == true) {
                    $scope.Active_Deactive();
                    $scope.Changed = false;
                } else {
                    $scope.SelectedIndexChanged($scope.selectedIndex, $scope.EventYear.selectedEventYear);
                    $scope.Changed = false;
                }
            }            

            $scope.EventYear = angular.copy($scope.resetCopy);
            $scope.isEdit = true;
        }

        $scope.Edit = function (data) {
            debugger;
            if ($scope.Changed) {
                var result = confirm('Do you want to save change ?');
                if (result == true) {
                    $scope.Active_Deactive();
                    $scope.Changed = false;
                    IsActiveList = [];
                } else {
                    $scope.Changed = false;
                    IsActiveList = [];
                    $scope.SelectedIndexChanged($scope.selectedIndex,null);
                }
            }
            $scope.EventYear.Id = data.EventYearId;
            $scope.EventYear.EventId = data.EventId;
            $scope.EventYear.Event_Year = data.EventYear;
            $scope.EventYear.EventFee = data.EventFee;
            $scope.EventYear.RetainFee = data.RetainFee;
            $scope.EventYear.RowVersion = data.RowVersion;           
            $scope.EventYear.EventYearCode = data.EventYearCode;
            $scope.EventYear.Event_Year = data.EventYear;

            //$scope.EventYear = {
            //    "Id": data.EventYearId,
            //    "EventId": data.EventId,
            //    "Event_Year": data.EventYear,
            //    "EventFee": data.EventFee,
            //    "RetainFee":data.RetainFee,
            //    "RowVersion": data.RowVersion,
            //    "selectedEventYear": EventYearInfo.Current.Code,
            //    "EventYearCode": EventYearInfo.Current.Code,
            //    "Event_Year": EventYearInfo.Current.Year
            //};
            angular.forEach($scope.Event_Class_List.Class, function (item) {
                var _class_filter = $filter('filter')(data.Class, { ClassId: item.ClassId }, true);
                if (_class_filter.length != 0)
                    item.IsChecked = true;
                else
                    item.IsChecked = false;
            })
          
            $scope.isEdit = true;
        }

        $scope.Submit = function (form) {
            
            debugger;
            if (form.validate() == false)
                return false;
            
            $scope.EventYear.EventYearClass = $filter('filter')($scope.Event_Class_List.Class, { IsChecked: true }, true);

            if ($scope.EventYear.EventYearClass.length == 0)
            {
                Service.Alert($rootScope, 'Please select atleat one class !');
                return false;
            }
                
            

            Service.Create_Update($scope.EventYear, 'school/eventYear/Create_Update').then(function (response) {
                Service.Alert($rootScope, response.message);
                if (response.result == 'Success') {                  
                    $state.reload();
                }                
            });
        }

        $scope.validationOptions = {
            rules: {
                EventId: {            
                    required: true                  
                },
                Event_Year: {
                    required: true
                },
                EventFee: {
                    maxlength: 5,
                    required: true
                },
                RetainFee: {
                    maxlength: 5,
                    required: true
                },
                FromClass: {
                    maxlength: 5,
                    required: true
                },
                ToClass: {
                    maxlength: 5,
                    required: true
                }
            }
        }

        $scope.SelectedIndexChanged = function (value, EventYear) {

            debugger;            
            var IsActive;           
                
            $scope.EventYearList = angular.copy(EventYearList_All);
            if (value == "1")
                IsActive = true;
            else if (value == "2")
                IsActive = false;
            else
            {
                $scope.EventYearList = $filter("filter")($scope.EventYearList, {EventYearCode: EventYear }, true);
                return false;
            }
                

            $scope.EventYearList = $filter("filter")($scope.EventYearList, { Status: IsActive, EventYearCode: EventYear }, true);
        }

        $scope.IsActive = function (Id) {
            debugger;           
            IsActiveList = Service.IsActive(Id, IsActiveList, $scope);            
        }


        $scope.Active_Deactive = function () {
            Service.Create_Update(IsActiveList, 'school/eventYear/Active_Deactive')
             .then(function (response) {
                 Service.Alert($rootScope, response.message);
                 
                 if (response.result == 'Success') {
                     GetEventYear();
                     IsActiveList = [];
                     $scope.Changed = false;
                 }
             });
        }

    }
    app.controller('eventYearController', ['$scope', '$location', '$rootScope', 'Service', '$state', '$filter', 'localStorageService', EventYear_fun]);

})(angular.module('SilverzoneERP_App'));