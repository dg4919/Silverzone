
(function (app) {

    var studentEntry_fun = function ($scope, $rootScope, $filter, Service, SchoolService, $stateParams, $state, $uibModal, localStorageService, Order) {
        debugger;
        $rootScope.HeaderAction = "Templates/School/Olympiad/StudentEntry/Partial/Header.html";

        $rootScope.IsEventShow = true;
        $scope.itemsPerPage = 10;
        $scope.currentPage = 1;
        $scope.rangeSize = 5;
        $scope.startIndex = 0;
        $scope.IsAutoRollNo=false;
        $scope.SchMngt = {};
        $scope.StudentEntry = {};
        $scope.IsLotCreated = true;
        $scope.Reset_EnrollmentOrder = true;
        $scope.Section = ['-','A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'];
        $scope.Search_School = function () {
            debugger;
            SchoolService.Search($scope, $rootScope,true);
        }

        if ($stateParams.SchCode != null)        
            localStorageService.set('StudentParams', $stateParams);

        $scope.Parameter = localStorageService.get('StudentParams');       

        $scope.EnrollmentOrder_Change = function (EnrollmentOrderId,IsCheck) {
            debugger;
            $scope.ClassList = [];
            if (!angular.isUndefined(EnrollmentOrderId))
            {
                var _class_filter = $filter('filter')($scope.SchMngt.EventManagement.EnrollmentOrder, { Id: EnrollmentOrderId }, true);
                if (_class_filter.length != 0)
                    $scope.ClassList = _class_filter[0].EnrollmentOrderDetail;
            }
                        
            
            if (IsCheck)
            {
                $scope.IsLotCreated = true;
                Service.Get('School/Lot/IsLotCreated?EnrollmentOrderId=' + EnrollmentOrderId)
                   .then(function (response) {
                       debugger;
                       $scope.IsLotCreated = response.IsLotCreated;
                   }, function () {

                   });
            }            
        }

        $scope.Class_Change = function (EnrollmentOrderDetailId) {
            debugger;
            var _class_filter = $filter('filter')($scope.ClassList, { Id: EnrollmentOrderDetailId }, true);
            if (_class_filter.length != 0)
                $scope.No_Of_Student_Classwise = _class_filter[0].No_Of_Student;
        }

        $scope.Back = function () {

            if ($scope.Parameter != null) {
                localStorageService.remove('StudentParams');

                var params = {
                    SchCode: $scope.Parameter.SchCode,
                };
                $state.go('EventManagement', params);
            }
        }

        $scope.AutoRollNo = function (IsAutoRollNo) {
            debugger;
            if (angular.isUndefined($scope.StudentEntry.RollNo) || $scope.StudentEntry.RollNo == null)
            {
                $scope.IsAutoRollNo = false;
                return false;
            }
            else
                $scope.IsAutoRollNo = IsAutoRollNo;
        }
        
        $scope.Submit = function (form) {
            debugger;
        
            if (form.validate() == false)
                return false;
            $scope.StudentEntry.EventmanagementId = $scope.SchMngt.EventManagement.Id;
                      
            if (angular.isUndefined($scope.StudentEntry.EventmanagementId)) {
                var obj = {
                    SchId: $scope.SchMngt.SchId,
                    EventId: $rootScope.SelectedEvent.EventId
                };

                Service.Create_Update(obj, 'school/EventManagement/Create_Update')
                   .then(function (response) {
                       Service.Notification($rootScope, response.message);
                       if (angular.lowercase(response.result) == 'success') {
                           $scope.StudentEntry.EventmanagementId = response.EventManagementId;
                           SaveStudentEntry();
                       }
                   });
            }
            else
                SaveStudentEntry();            
        }

        function SaveStudentEntry() {
            Service.Create_Update($scope.StudentEntry, 'school/studentEntry/Create_Update')
                 .then(function (response) {
                     Service.Notification($rootScope, response.message);

                     if (response.result == 'Success') {
                         $scope.Reset(0, 10);
                         document.getElementsByName("RollNo")[0].focus();
                     }
                 });
        }
        $scope.sortType = 'UpdationDate';

        $scope.sortReverse = true;  

        $scope.OrderBy = function () {
            $scope.StudentEntryList
        }

        $scope.Edit = function (data)
        {
            debugger;                        
            $scope.StudentEntry = angular.copy(data);
            $scope.EnrollmentOrder_Change($scope.StudentEntry.EnrollmentOrderId);
            $scope.StudentEntry.ClassName =  $scope.StudentEntry.ClassName;
        }

        $scope.validationOptions = {
            rules: {
                ClassName: {
                    required: true                   
                },
                Section: {
                    required: true,
                    maxlength: 1,
                    minlength:1
                },
                RollNo: {
                    required: true,
                    maxlength: 3,
                    minlength: 1
                },
                StudentName: {
                    required: true,
                    maxlength: 50,                   
                }                
            }
        }
      
        $scope.$watch("currentPage", function (newValue, oldValue) {
            debugger;
            if (!angular.isUndefined($scope.SchMngt.EventManagement)) {
                $scope.Reset(newValue, oldValue);
            }
        });        

        $scope.Reset=function(newValue, oldValue)
        {
            debugger;
            if (!angular.isUndefined($scope.SchMngt.EventManagement)) {
                $scope.StudentEntryList = [];
                $scope.total = 0;
                if (angular.isUndefined($scope.SchMngt.EventManagement.Id))
                    return false;
                $scope.EnrollmentOrder_Change($scope.StudentEntry.EnrollmentOrderId);
                if ($scope.IsAutoRollNo == true)                
                    $scope.StudentEntry = { "EnrollmentOrderDetailId": $scope.StudentEntry.EnrollmentOrderDetailId, "RollNo": parseInt($scope.StudentEntry.RollNo) + 1, "Section": $scope.StudentEntry.Section, EnrollmentOrderId: $scope.StudentEntry.EnrollmentOrderId };
                else
                    $scope.StudentEntry = { "EnrollmentOrderDetailId": $scope.StudentEntry.EnrollmentOrderDetailId, "Section": $scope.StudentEntry.Section, EnrollmentOrderId: $scope.StudentEntry.EnrollmentOrderId };


                Service.Reset($scope);

                
                var startIndex = (newValue * $scope.itemsPerPage);
                
                var EnrollmentOrderDetailId = "", Section = "", EnrollmentOrderId = "";

                if (!angular.isUndefined($scope.StudentEntry.EnrollmentOrderDetailId))
                    EnrollmentOrderDetailId = $scope.StudentEntry.EnrollmentOrderDetailId;

                if (!angular.isUndefined($scope.StudentEntry.EnrollmentOrderId))
                    EnrollmentOrderId = $scope.StudentEntry.EnrollmentOrderId;

                if (!angular.isUndefined($scope.StudentEntry.Section))
                    Section = $scope.StudentEntry.Section
               
                Service.Get('School/studentEntry/GetStudent?EventManagementId=' + $scope.SchMngt.EventManagement.Id + '&EnrollmentOrderId=' + EnrollmentOrderId + '&EnrollmentOrderDetailId=' + EnrollmentOrderDetailId + '&Section=' + Section + '&StartIndex=' + startIndex + '&Limit=' + $scope.itemsPerPage)
                    .then(function (response) {

                        debugger;
                        $scope.StudentEntryList = response.result;
                        $scope.total = response.Count;

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
            else
            {
                Service.Notification($rootScope, 'Please Search school !');               
            }
        }

        $scope.ShowClassWise = function () {
            debugger;

            if (!angular.isUndefined($scope.SchMngt.EventManagement)) {
                if (angular.isUndefined($scope.SchMngt.EventManagement.Id))
                    return false;

                Service.Get('School/studentEntry/GetStudentClassWise?EventManagementId=' + $scope.SchMngt.EventManagement.Id)
                        .then(function (response) {
                            debugger
                            $scope.ClassWiseList = response.result;

                            var modalInstance = $uibModal.open({
                                controller: 'classWiseController',
                                templateUrl: 'Templates/School/StudentEntry/Partial/Dialog_ClassWise.html',
                                backdrop: 'static',
                                resolve: {
                                    ClassWiseList: function () {
                                        return angular.copy($scope.ClassWiseList);
                                    }                  
                                }
                            });

                            modalInstance.result.then(function (response) {
                                debugger;
                                //on ok button press 
                            }, function () {
                                //on cancel button press
                                console.log("Modal Closed");
                            });
                        });
            }
            else {
                $.alert({
                    title: $rootScope.Title,
                    content: 'Please Search school !'
                });
            }
        }

        $scope.SearchSchool_KeyPress = function (SchCode)
        {
            SchoolService.SearchBySchoolCode($scope, SchCode, $rootScope,true);
        }

        if ($scope.Parameter != null)
        {                       
            $scope.SearchSchool_KeyPress($scope.Parameter.SchCode);
            $scope.StudentEntry.EnrollmentOrderId = $scope.Parameter.EnrollmentOrderId;            
        }

        $scope.ShowAnsEntry = function (StudentEntryId) {
            var modalInstance = $uibModal.open({
                controller: 'AnsEntryController',
                templateUrl: 'Templates/School/StudentEntry/Partial/Dialog_AnsEntry.html',
                windowClass: 'app-modal-window',
                backdrop: 'static',
                resolve: {
                    StudentEntryId: function () {
                        return StudentEntryId;
                    }
                }
            });

            modalInstance.result.then(function (response) {
                debugger;
                //on ok button press 
            }, function () {
                //on cancel button press
                console.log("Modal Closed");
            });
        }

        $scope.EditEnrollmentOrder = function (EnrollmentOrderId) {
            debugger;
            var data = $filter('filter')($scope.SchMngt.EventManagement.EnrollmentOrder, { Id: EnrollmentOrderId }, true)[0];           
         
            Order.Enrollment($scope, $rootScope, data);
        }
    }

    var fun_ClassWise = function ($scope, $rootScope, modalInstance, ClassWiseList) {
        $scope.ClassWiseList = ClassWiseList;

        $scope.Back = function () {
            debugger;
            modalInstance.dismiss();
        }
    }

    var AnsEntry_fun = function ($scope, $rootScope, $filter, modalInstance, Service,StudentEntryId) {

        $scope.Edit = false;

        $scope.QuestionAnswer = { "Createdby": "", "CreatedDate": "", "AnswerJSON": [] };

        Service.Get('School/studentAttendance/Get?Id=' + StudentEntryId).then(function (response) {
            debugger
            $scope.QuestionAnswer = response.result;

            if ($scope.QuestionAnswer.AnswerJSON.length == 0) {
                $scope.Reset();
            }
        });

        $scope.Reset = function () {
            $scope.QuestionAnswer.AnswerJSON = [];
            for (var i = 1; i <= 50; i++) {
                $scope.QuestionAnswer.AnswerJSON.push({ "Question": lpad('' + i, 2, '0'), "Answer": "" });
            }
        }

        $scope.Back = function () {
            debugger;
            modalInstance.dismiss();
        }

        function lpad(value, length, prefix) {
            debugger;
            var result = '';
            if (typeof (prefix) === 'undefined') {
                prefix = '0';
            }

            if (typeof (length) === 'undefined') {
                length = 2;
            }

            if (value.length < length) {
                for (var r = 0; r < length - value.length; r++) {
                    result += prefix;
                }
                result += value;
            }
            else {
                result = value;
            }
            return result;
        }

        $scope.Submit = function (form) {
            debugger;
            var answer_filter = $filter('filter')($scope.QuestionAnswer.AnswerJSON, function (value) {
                return value.Answer == "";
            });

            if ($scope.QuestionAnswer.length != 0) {
                var msg = '';
                angular.forEach(answer_filter, function (value, index) {
                    if (msg == '')
                        msg = '<br/>' + value.Question;
                    else {
                        msg += ',';
                        if (index % 15 == 0)
                            msg += '<br/>';
                        msg += value.Question;
                    }

                });
                $.confirm({
                    title: $rootScope.Title,
                    content: 'Following Question are empty ' + msg + ' <br/ ><br/ ><strong>Are you want to save ?</strong>',
                    buttons: {
                        YES: function () {
                            Save();
                        },
                        NO: function () {
                        }
                    }
                });
            }
            else
                Save();           
        }

        function Save() {
            debugger;
            var _questionAnswer = [];
            angular.forEach($scope.QuestionAnswer.AnswerJSON, function (data) {
                _questionAnswer.push({ "Question": data.Question, "Answer": data.Answer });
            })
            var obj = { 'Id': StudentEntryId, 'AnswerJSON': JSON.stringify(_questionAnswer), 'Type': 1 };
           

            Service.Create_Update(obj, 'school/studentAttendance/Create_Update').then(function (response) {
                Service.Notification($rootScope, response.message);
                if (response.result == 'Success') {
                    modalInstance.dismiss();
                }
            });
        }
    }

    app.controller('studentEntryController', ['$scope', '$rootScope', '$filter', 'Service', 'SchoolService', '$stateParams', '$state', '$uibModal', 'localStorageService', 'OrderService', studentEntry_fun])
    .controller('classWiseController', ['$scope', '$rootScope', '$uibModalInstance', 'ClassWiseList', fun_ClassWise])
    .controller('AnsEntryController', ['$scope', '$rootScope', '$filter', '$uibModalInstance', 'Service','StudentEntryId', AnsEntry_fun])
    ;


})(angular.module('SilverzoneERP_App'))