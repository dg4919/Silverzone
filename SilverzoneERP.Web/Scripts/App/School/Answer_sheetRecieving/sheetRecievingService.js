(function () {

    var fn = function ($http, $q, globalConfig, $filter) {

        var apiUrl = globalConfig.apiEndPoint + globalConfig.version.School,
            fac = {};

        fac.get_schoolInfo = function (_model) {

            var defer = $q.defer();

            $http({
                url: apiUrl + '/answer_sheetRecieving/get_schoolInfo',
                method: 'POST',
                data: _model
            })
                .success(function (d) {
                    defer.resolve(d);
                })
                .error(function (e) {
                    defer.reject('something went wrong..');
                });
            return defer.promise;
        }

        fac.save_ansSheet_Recieve = function (_model) {

            var defer = $q.defer();

            $http({
                url: apiUrl + '/answer_sheetRecieving/save_ansSheet_Recieve',
                method: 'POST',
                data: _model
            })
                .success(function (d) {
                    defer.resolve(d);
                })
                .error(function (e) {
                    defer.reject('something went wrong..');
                });
            return defer.promise;
        }

        fac.get_json = function (_model) {

            var defer = $q.defer();

            $http({
                url: apiUrl + '/answer_sheetRecieving/get_json',
                method: 'GET',
                cache: !0
            })
                .success(function (d) {
                    defer.resolve(d);
                })
                .error(function (e) {
                    defer.reject('something went wrong..');
                });
            return defer.promise;
        }


        return fac;
    }


        angular.module('SilverzoneERP_App')
            .factory('answerReciveService', ['$http', '$q', 'globalConfig', '$filter', fn]);

})();