(function () {

    var fn = function ($http, $q) {

        this.get = function (_url, _params, _isCache) {

            var defer = $q.defer();

            $http({
                url: _url,
                method: 'GET',
                params: _params,
                cache: _isCache || 0    // 0 = false, !0 = 1 = true
            })
                .success(function (d) {
                    defer.resolve(d);
                })
                .error(function (e) {
                    defer.reject('something went wrong..');
                });
            return defer.promise;
        }

        this.post = function (_url, _model, _params) {

            var defer = $q.defer();

            $http({
                url: _url,
                method: 'POST',
                data: _model,     // send null, if no value to provide
                params: _params,
            })
                .success(function (d) {
                    defer.resolve(d);
                })
                .error(function (e) {
                    defer.reject('something went wrong..');
                });
            return defer.promise;
        }
    }

    angular
        .module('Http.Shared', [])
        .service('httpService', ['$http', '$q', fn]);

})();

