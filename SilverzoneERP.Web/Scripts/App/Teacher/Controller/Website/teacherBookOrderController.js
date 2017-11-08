(function (app) {

    var BookOrder_fun = function ($scope, $rootScope, $filter, Service) {
        debugger;
        $rootScope.HeaderAction = "Templates/Teacher/BookOrder/Partial/Header.html";
        $scope.EventList = angular.copy($rootScope.EventInfo);
        var resetCopy = { "From": 6, "To": 7 };
        $scope.TeacherBookOrder = { EventId :''};

        $scope.IsStep1 = true;

        $scope.Step = function (_isStep) {
            debugger;
            $scope.IsStep1 = _isStep;
            if (_isStep == false)
            {
                Get_Book();
                GetPurchaseOrder(true);                
            }
                
        }
        function Get_Book() {
            Service.Get('school/EventManagement/GetAllBook?SubjectId=' + $scope.TeacherBookOrder.EventId).then(function (response) {
                debugger;
                $scope.AllBook = response.result;                
            });
        }
        
        $scope.ChangeEvent = function (EventId) {
            var Event_Filter = $filter("filter")($rootScope.EventInfo, { EventId: EventId }, true);
            if (Event_Filter.length != 0) {
                $scope.ClassList = angular.copy(Event_Filter[0].Class);
                
            }                        
        }

        function GetPurchaseOrder(IsReset) {

            Service.Get('school/EventManagement/AllOrderBySchool?SchoolId=' + $rootScope.UserInfo.SchId + '&EventId=' +$scope.TeacherBookOrder.EventId).then(function (response) {
                $scope.BookOrderlist = response.result;
               // $scope.AllBook = response.Book;
                resetCopy.srcTo = response.CompanyId;
                resetCopy.srcFrom = response.EventManagementId;
                if (IsReset)
                    $scope.BookOrder = angular.copy(resetCopy);

                var Book_filter = $filter("filter")($scope.BookOrderlist, { IsConfirm: false }, true);
                if (Book_filter.length != 0)
                    $scope.BookOrder.PO_masterId = Book_filter[0].Id;
            });
        }
                       
        $scope.Back = function () {
            debugger;
            modalInstance.dismiss();
        }

        $scope.Submit = function (form) {
            debugger;           
            if ($scope.BookOrder.srcTo == null)
            {
                Service.Notification($rootScope, 'Inventory scource not exists !');
                return false;
            }

            if (form.validate() == false)
                return false;
            if (angular.isUndefined($scope.BookOrder.srcFrom) || $scope.BookOrder.srcFrom==0) {
                var obj = {
                    SchId:$rootScope.UserInfo.SchId,
                    EventId: $scope.TeacherBookOrder.EventId
                };

                Service.Create_Update(obj, 'school/EventManagement/Create_Update')
                   .then(function (response) {
                       //Service.Notification($rootScope, response.message);

                       if (angular.lowercase(response.result) == 'success') {
                           $scope.BookOrder.srcFrom = response.EventManagementId;                          
                           resetCopy.srcFrom = response.EventManagementId;
                           SaveBookOrder();
                       }
                   });
            }
            else
                SaveBookOrder();
            
           // return false;
            
        }

        function SaveBookOrder() {
            Service.Create_Update($scope.BookOrder, 'Inventory/Stock/save_purchaseOrder').then(function (response) {
                debugger;
                if (angular.lowercase(response.result) == 'success') {
                    var BookId = $scope.BookOrder.BookId;
                    $scope.BookOrder = angular.copy(resetCopy);
                    $scope.BookOrder.PO_masterId = response.PO_masterId;
                    $scope.BookOrder.BookId = BookId;
                   
                    //Change class
                    var index = $scope.ClassList.map(function (item) {
                        return item.ClassId;
                    }).indexOf($scope.SelectedClassId);

                    $scope.SelectedClassId = $scope.ClassList[index + 1].ClassId;
                    $scope.Class_Selected_IndexChanged($scope.SelectedClassId);
                    Service.Notification($rootScope, 'Successfully added !');
                    GetPurchaseOrder(false);
                    
                }
                else if (angular.lowercase(response.result) == 'exist')
                    Service.Notification($rootScope, 'Already exists !');

            });
        }

        $scope.validationOptions = {
            rules: {
                SelectedClassId: {
                    required: true
                },
                BookId: {
                    required: true
                },
                Quantity: {
                    required: true
                }
            }
        }

        $scope.Class_Selected_IndexChanged = function (ClassId) {
            debugger;
            var CategoryName = "";
            if (!angular.isUndefined($scope.Book))
            {
                var Book_filter = $filter("filter")($scope.Book, { BookId: $scope.BookOrder.BookId }, true);
                if (Book_filter.length != 0)
                    CategoryName = Book_filter[0].CategoryName;
            }
                

            $scope.Book = $filter("filter")($scope.AllBook, { 'ClassId': ClassId == '' ? 0 : ClassId }, true);
            //Change class
            if (CategoryName != "")
            {
                var Book_filter = $filter("filter")($scope.Book, { CategoryName: CategoryName }, true);
                if (Book_filter.length != 0)
                    $scope.BookOrder.BookId = Book_filter[0].BookId;
            }                       
        }

        $scope.Edit = function (data,_PO_masterId) {
            debugger;
            $scope.BookOrder = angular.copy(resetCopy);
            $scope.SelectedClassId = data.ClassId;
            $scope.Class_Selected_IndexChanged($scope.SelectedClassId);
            $scope.BookOrder.BookId = data.BookId;
            $scope.BookOrder.Quantity = data.Quantity;
            $scope.BookOrder.POId = data.Id;
            $scope.BookOrder.PO_masterId = _PO_masterId;
        }
    }
  
    app.controller('teacherBookOrderController', ['$scope', '$rootScope', '$filter', 'Service', BookOrder_fun]);

})(angular.module('SilverzoneERP_App'));