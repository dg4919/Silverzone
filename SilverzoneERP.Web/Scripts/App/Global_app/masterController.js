
(function (app) {
    
    var masterCtrl_fun = function ($scope, $rootScope, Service, $location, localStorageService, $filter) {
        debugger;        
        $rootScope.UserInfo = localStorageService.get('UserInfo');
        $scope.Color = localStorageService.get('BackColor');
        $rootScope.EventInfo = localStorageService.get('EventInfo');
        $rootScope.Permission = localStorageService.get('Permission');
        $rootScope.Title = 'Silverzone';
       
        $rootScope.IsEventShow = false;
        $rootScope.IsEventDisable = true;

        $rootScope.SelectedEvent = angular.copy(localStorageService.get('SelectedEvent'));        
            
        $scope.Logout = function () {           
            Service.Logout();
        }
        $rootScope.StatusInfo = '';
        //if ($rootScope.UserInfo == null)
        //    $scope.Logout();
       
        $rootScope.IsLeftMenu = true;
        $rootScope.LeftMenu_Clic = function () {
            $rootScope.IsLeftMenu = !$rootScope.IsLeftMenu;
        }


        
        $rootScope.ChangeEvent = function (EventId, EventColor) {
            debugger;            
            var Event_filter = $filter("filter")($rootScope.EventInfo, { EventId: EventId }, true);
            if (Event_filter.length != 0)
            {
                $rootScope.SelectedEvent = Event_filter[0];
                $rootScope.SelectedEvent.EventColor = EventColor;
            }
 
            localStorageService.set('SelectedEvent', $rootScope.SelectedEvent);                        
        }

        $scope.ChangeBackColor = function () {          
            if (!$rootScope.UserInfo.show_menu)
                return 'login';
            else if (!angular.isUndefined($scope.Color))
                return $scope.Color;
            else
                return '';
        }        
        $scope.MainMenu = function (data) {
            debugger;
            data.Active = !data.Active;
        }

        $scope.SetPermission = function (data) {            
            $rootScope.Permission = data;
            $rootScope.StatusInfo = "";
            localStorageService.set('Permission', data);
        }
    }

    app.controller('masterController', ['$scope', '$rootScope', 'Service', '$location', 'localStorageService', '$filter', masterCtrl_fun]);

})(angular.module('SilverzoneERP_App'));