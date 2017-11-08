
(function () {
    var configFn = function ($stateProvider) {
        $stateProvider.state("Lot", {
            url: '/ERP/Report/Lot',
            templateUrl: 'Templates/School/Report/Lot/Index.html',
            controller: 'lotController'
        })
    };

    angular.module('SilverZone_Report_App', [])
    .config(['$stateProvider', configFn]);
})();