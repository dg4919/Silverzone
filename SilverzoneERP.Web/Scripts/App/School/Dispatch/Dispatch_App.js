
(function () {
    var configFn = function ($stateProvider) {
        $stateProvider.state("DispatchQP", {
            url: '/ERP/dispatch/DispatchQP',
            templateUrl: 'Templates/School/Dispatch/QuestionPaper/Index.html',
            controller: 'dispatchQPController'
        })
    };

    angular.module('SilverZone_Dispatch_App', [])
    .config(['$stateProvider', configFn]);
})();