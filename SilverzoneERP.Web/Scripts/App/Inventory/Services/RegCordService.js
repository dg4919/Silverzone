(function (app) {

    var fn = function ($http, $q, globalConfig, $filter) {

        var apiUrl = globalConfig.apiEndPoint + globalConfig.version.Inventory,
            fac = {};

        //***********************  Assign School  ***********************

        fac.get_rcList = function () {

            var defer = $q.defer();

            $http({
                url: apiUrl + '/RegCord/get_rcList',
                method: 'Get',
            })
                .success(function (d) {
                    defer.resolve(d);
                })
                .error(function (e) {
                    defer.reject('something went wrong..');
                });
            return defer.promise;
        }

        fac.srchSchool = function (_model) {

            var defer = $q.defer();

            $http({
                url: apiUrl + '/RegCord/SearchSchool',
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

        fac.get_rcSchools = function (Id) {

            var defer = $q.defer();

            $http({
                url: apiUrl + '/RegCord/get_rcSchoolsbyId',
                method: 'Get',
                params: {
                    rcId: Id
                }
            })
                .success(function (d) {
                    defer.resolve(d);
                })
                .error(function (e) {
                    defer.reject('something went wrong..');
                });
            return defer.promise;
        }

        fac.save_RcSchool = function (_model) {

            var defer = $q.defer();

            $http({
                url: apiUrl + '/RegCord/save_RcSchools',
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

        fac.save_RcSchool_byschCode = function (_model) {

            var defer = $q.defer();

            $http({
                url: apiUrl + '/RegCord/save_RcSchool_byschCode',
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

        fac.get_cityJson = function () {

            var defer = $q.defer();

            $http({
                url: apiUrl + '/RegCord/get_cityModel',
                method: 'GET',
                cache: true
            })
                .success(function (d) {
                    defer.resolve(d);
                })
                .error(function (e) {
                    defer.reject('something went wrong..');
                });
            return defer.promise;
        }

        //***********************  School's Visit  ***********************

        fac.get_visitTypes = function () {

            var defer = $q.defer();

            $http({
                url: apiUrl + '/RegCord/get_visitTypes',
                method: 'GET',
                cache: true
            })
                .success(function (d) {
                    defer.resolve(d);
                })
                .error(function (e) {
                    defer.reject('something went wrong..');
                });
            return defer.promise;
        }

        fac.get_VisitStatus = function () {

            var defer = $q.defer();

            $http({
                url: apiUrl + '/RegCord/get_VisitStatus',
                method: 'GET',
                cache: true
            })
                .success(function (d) {
                    defer.resolve(d);
                })
                .error(function (e) {
                    defer.reject('something went wrong..');
                });
            return defer.promise;
        }

        fac.get_events = function () {

            var defer = $q.defer();

            $http({
                url: apiUrl + '/RegCord/get_events',
                method: 'GET',
                cache: true
            })
                .success(function (d) {
                    defer.resolve(d);
                })
                .error(function (e) {
                    defer.reject('something went wrong..');
                });
            return defer.promise;
        }

        fac.save_RcVisits = function (_model) {

            var defer = $q.defer();

            $http({
                url: apiUrl + '/RegCord/save_rcVisits',
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

        fac.get_RcVisits = function () {

            var defer = $q.defer();

            $http({
                url: apiUrl + '/RegCord/get_rcVisits',
                method: 'Get'
            })
                .success(function (d) {
                    defer.resolve(d);
                })
                .error(function (e) {
                    defer.reject('something went wrong..');
                });
            return defer.promise;
        }

        //***********************  Follow Up  ***********************

        fac.get_followUps = function (Id) {

            var defer = $q.defer();

            $http({
                url: apiUrl + '/RegCord/get_followUps',
                method: 'Get',
                params: {
                    rcId: Id
                }
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

    angular
       .module('SilverzoneERP_invenotry_service')       // creating a new module here
       .factory('RegCordService', ['$http', '$q', 'globalConfig', '$filter', fn]);

})();