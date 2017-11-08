(function (app) {
    var lot_fun = function ($scope, $rootScope, Service, $filter, $state, $uibModal, SchoolService) {
        $rootScope.HeaderAction = "Templates/School/Report/Lot/Header.html";

        $scope.isEdit = false;
        $rootScope.IsEventShow = true;
        $scope.SchMngt = {};
        $scope.Search = {
            IsZeroEnroll: false,
            IsZeroStuReg: false,
            IsDiff: false,
            IsNotInSelectedDate: false,
            
        };

        function Get_Option() {
            Service.Get('school/Lot/Get_Option').then(function (response) {
                $scope.Lot_Option = response;
            });
        }

        Get_Option();
              
        var preserveExaminationDateId = '';

        $scope.SearchSchool = function (form) {
            debugger
            if (form.validate() == false)
                return false;
            var URL = 'school/Lot/SearchSchool?EventId=' + $scope.Search.EventId;
            URL += '&ExamDateId=' + $scope.Search.ExaminationDateId;
            URL += '&ChangeDate=';
            if ($scope.Search.ExaminationDateId ==-1)
                URL += $scope.Search.ChangeExamDate;
            URL += '&IsZeroEnroll=' + $scope.Search.IsZeroEnroll;
            URL += '&IsZeroStuReg=' + $scope.Search.IsZeroStuReg;
            URL += '&IsDiff=' + $scope.Search.IsDiff;
            URL += '&IsNotInSelectedDate=' + $scope.Search.IsNotInSelectedDate;
            
            
            preserveExaminationDateId = $scope.Search.ExaminationDateId
            Service.Get(URL).then(function (response) {
                $scope.SchoolList = response.SchoolList;
            });
        }

        $scope.validationOptions = {
            rules: {
                EventId: {            // field will be come from component
                    required: true
                },
                ExaminationDateId: {
                    required: true
                },
                ChangeExamDate: {
                    required: true
                }
            }
        }

        $scope.exportData = function () {
            var blob = new Blob([document.getElementById('SchoolList').innerHTML], {
                type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;charset=utf-8"
            });
            saveAs(blob, 'SchoolList' + "_Report.xls");
        };

        $scope.Reset = function () {
            $state.reload();
        }

        $scope.CreateLot = function () {
            debugger;
            if (angular.isUndefined($scope.Search.Level) || $scope.Search.Level == '')
            {
                Service.Notification($rootScope,'Please select level !');
                return false;
            }
            if (angular.isUndefined($scope.Search.LotType) || $scope.Search.LotType == '') {
                Service.Notification($rootScope, 'Please select LotType !');
                return false;
            }
            $.confirm({
                title: $rootScope.Title,
                content: 'Do you want to create lot ?',
                buttons: {
                    YES: function () {
                        var data = angular.copy($scope.SchoolList);
                        
                        Service.Create_Update(data, 'school/Lot/CreateLot?ExaminationDateId=' + preserveExaminationDateId + '&_level=' + $scope.Search.Level + '&_lotType=' + $scope.Search.LotType)
                              .then(function (response) {
                                  Service.Notification($rootScope, response.message);
                                  if (angular.lowercase(response.result) == 'success')
                                    $scope.Reset();
                              });
                    },
                    NO: function () {

                    }
                }
            });
            
        }

        function Get_Lot(EventId, SchCode, LotNo) {
            
            Service.Get('school/Lot/GetQuestionPaperLot?EventId=' + EventId + '&LotNo=' + LotNo + '&SchoolCode=' + SchCode + '&ExamDate=').then(function (response) {
                $scope.LotList = response.result;
                //prompt('', JSON.stringify($scope.LotList));
            });
        }
        $scope.Search_School = function () {
            debugger;
            if (angular.isUndefined($rootScope.SelectedEvent) || $rootScope.SelectedEvent == null) {
                Service.Notification($rootScope, 'Please select event !');
                return false;
            }

            SchoolService.Search($scope, $rootScope, false);
        }
       
        $scope.$watch('SelectedEvent', function (newVal, oldVal) {
            if (newVal)
            {
                debugger;
                //prompt('', JSON.stringify(newVal));
                Get_Lot(newVal.EventId, $scope.SchMngt.SchCode, $scope.Search.LotNo);
            }
                
            //    $sc.$parent.bundleInfo.book_bundle.ClassId = newVal.Id;
        });

        $scope.Lot_Search = function () {
            Get_Lot($rootScope.SelectedEvent.EventId, $scope.SchMngt.SchCode, $scope.Search.LotNo);
        }

        $scope.Add = function () {
            $scope.isEdit = true;
            $rootScope.IsEventShow = false;
        }

        $scope.Back = function () {
            $scope.isEdit = false;
            $rootScope.IsEventShow = true;
        }

        $scope.GenerateReport = function (ev,_Lot) {
            debugger;
         
            var Param = {
                "LotNo":null,
                "LotFilter": $scope.Lot_Option.LotFilter,
                "Level": $scope.Lot_Option.Level,
                "LotType": $scope.Lot_Option.LotType,
                "ExamDateList": $scope.Lot_Option.ExamDateList
            };
            if (_Lot != null)
            {
                Param.LotNo = _Lot.LotNo;                
                Param.ExaminationDateId = _Lot.ExaminationDateId;
                Param.EventId = _Lot.EventId;
                Param.SelectedLevel = _Lot.ExamLevel;
                Param.SelectedLotType = _Lot.LotType;
                Param.ExaminationDateId = ""+_Lot.ExaminationDateId;
            }
            else
                Param.EventId = $rootScope.SelectedEvent.EventId;

          //  prompt('', JSON.stringify(Param));
            var modalInstance = $uibModal.open({
                controller: 'GenerateReportController',
                templateUrl: 'Templates/School/Report/Lot/Dialog_Generate_Report.html',
                windowClass: 'app-modal-window',
                backdrop: 'static',
                resolve: {
                    Param: function () {
                        return angular.copy(Param);
                    }
                    //,
                    //Lot_Option: function () {
                    //    return angular.copy($scope.Lot_Option);
                    //}
                }
                
            });

            modalInstance.result.then(function (response) {
                debugger;

                //on ok button press 
            }, function () {
                //on cancel button press
                
            });
            ev.stopPropagation();           
            ev.preventDefault();
        }
        
    };

    var GenerateReport_fun = function ($scope, $rootScope, Service, $filter, modalInstance, $window, globalConfig, Param) {
        $scope.Param = Param;
        $scope.Action = {ExamLevel:1};
       // $scope.Lot_Option = Lot_Option;

        $scope.Back = function () {
            modalInstance.dismiss();
        }

        $scope.GenerateReport = function (form) {            
            if (form.validate() == false)            
                return false;
            $window.open(globalConfig.apiEndPoint + 'school/Lot/GenerateReport?LotNo=' + $scope.Param.LotNo + '&EventId=' + $scope.Param.EventId + '&ExaminationDateId=' + $scope.Param.ExaminationDateId + "&ExamLevel=" + $scope.Param.SelectedLevel + '&ReportName=' + $scope.Action.ReportName + '&LotFilter=' + $scope.Action.LotFilter + '&ExamDate=' + $scope.Action.ExamDate, '_blank');
        }

        $scope.validationOptions = {
            rules: {
                EventId: {
                    required:true
                },
                LotType: {
                    required: true
                },
                Level: {
                    required: true
                },
                ExaminationDateId: {
                    required: true
                },              
                ReportName: {
                    required: true
                }
            }
        };

    };

    app.controller('lotController', ['$scope', '$rootScope', 'Service', '$filter', '$state', '$uibModal', 'SchoolService', lot_fun])
    .controller('GenerateReportController', ['$scope', '$rootScope', 'Service', '$filter', '$uibModalInstance', '$window', 'globalConfig', 'Param', GenerateReport_fun]);

})(angular.module('SilverZone_Report_App'));