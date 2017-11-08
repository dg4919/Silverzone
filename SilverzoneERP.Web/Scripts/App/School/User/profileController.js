(function (app) {

    var user_fun = function ($scope, $location, $rootScope, Service, $filter, $state, globalConfig) {

        $rootScope.HeaderAction = "Templates/Common/Profile/Header.html";

    }


    app.controller('profileController', ['$scope', '$location', '$rootScope', 'Service', '$filter', '$state', 'globalConfig', user_fun]);

})(angular.module('SilverzoneERP_App'));