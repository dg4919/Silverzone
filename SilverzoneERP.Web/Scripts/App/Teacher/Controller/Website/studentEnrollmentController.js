

(function (app) {
    var studentEnrollment_fun = function ($scope,$rootScope, Service, $filter) {
        $rootScope.HeaderAction = "Templates/Teacher/StudentEnrollment/Partial/Header.html";
     
        $scope.isEdit = false;
        function Reset() {
            $scope.StudentEnrollment = {
                TotlaEnrollment: 0,
                "IsConfirm": false,
                "SrcFrom": "online"
            };
        }

        Reset();

        function getExamDate() {
            Service.Get('school/examDate/GetExamDate').then(function (response) {
                debugger;
                $scope.ExamDateList = response.ExamDateList;
            });
        }

        getExamDate();

        function GetEnrollmentOrder() {
            Service.Get('teacher/studentEnrollment/GetEnrollmentOrder?SchoolId=' + $rootScope.UserInfo.SchId).then(function (response) {
                debugger;
                $scope.EventList = angular.copy($rootScope.EventInfo);
                $scope.StudentEnrollmentOrderList = response.EnrollmentOrderList;

                angular.forEach($scope.StudentEnrollmentOrderList.notconfirm, function (item) {
                    var Event_Filter = $filter("filter")($rootScope.EventInfo, { EventId: item.EventId }, true);
                    if (Event_Filter.length != 0) {
                        $scope.EventList.splice(Event_Filter[0], 1);
                    }
                })               
            });
        }

        GetEnrollmentOrder();

        $scope.No_Of_Student_Change = function () {
            debugger;
            $scope.StudentEnrollment.TotlaEnrollment = 0
            angular.forEach($scope.ClassList, function (item) {
                if (item.No_Of_Student != null && item.No_Of_Student != '')
                    $scope.StudentEnrollment.TotlaEnrollment += parseInt(item.No_Of_Student);
            })
        }

        $scope.Add = function () {
            $scope.isEdit = true;
            Reset();
        }

        $scope.Back = function () {
            $scope.isEdit = false;
            if (_preserve_SelectedEvent_Class != null) {
                $rootScope.SelectedEvent.Class = _preserve_SelectedEvent_Class;
                _preserve_SelectedEvent_Class = null;
            }
        }

        $scope.ChangeEvent = function (EventId) {
            var Event_Filter = $filter("filter")($rootScope.EventInfo, { EventId: EventId }, true);
            if (Event_Filter.length != 0) {
                $scope.ClassList =angular.copy( Event_Filter[0].Class);
            }
        }

        $scope.validationOptions = {
            rules: {
                ExaminationDateId: {
                    required: true,
                }
            }
        }

        $scope.Submit = function (form) {
            if ($scope.StudentEnrollment.TotlaEnrollment == 0) {
                Service.Notification($rootScope, "Enrollment can't be 0(zero) !");
                return false;
            }
            debugger;
            if (form.validate() == false)
                return false;

            $scope.StudentEnrollment.EnrollmentOrderDetail = $scope.ClassList;


            Service.Create_Update($scope.StudentEnrollment, 'school/EventManagement/EnrollmentOrder?EventId=' + $scope.StudentEnrollment.EventId + '&SchoolId=' + $rootScope.UserInfo.SchId)
                 .then(function (response) {
                     Service.Notification($rootScope, response.message);

                     if (angular.lowercase(response.result) == 'success') {
                         debugger;
                         GetEnrollmentOrder();
                         $scope.Back();
                     }
                 });
          
        }
     
        $scope.Edit = function (StudentEnrollment, _Order) {
            debugger;
           
            Reset();
            $scope.Add();           
            $rootScope.ChangeEvent(StudentEnrollment.EventId, 'btn-danger');
         
            $scope.StudentEnrollment.TotlaEnrollment = StudentEnrollment.TotalEnrollmentSummary;
            $scope.StudentEnrollment.ExaminationDateId = _Order.ExaminationDateId;
            $scope.ClassList = _Order.EnrollmentOrderDetail;
            $scope.StudentEnrollment.Id = _Order.Id;
        }
    };

    app.controller('studentEnrollmentController', ['$scope', '$rootScope', 'Service', '$filter', studentEnrollment_fun]);
})(angular.module('SilverZone_Teacher_App'));