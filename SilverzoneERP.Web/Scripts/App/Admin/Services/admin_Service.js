
(function (app) {

    var fn = function ($http, $q, globalConfig) {

        var apiUrl = globalConfig.apiEndPoint + globalConfig.version.Admin,
             fac = {};

        fac.create_user = function (_model) {
            debugger;

            // we r going to return a httprespone + data of list -> (here angular will automatically serialise of list into the Json format)
            var defer = $q.defer();

            $http({
                url: apiUrl + '/User/Register_user',
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

        //******************  For Meta Tags  ********************

        fac.save_metaTag = function (_model) {
            debugger;

            // we r going to return a httprespone + data of list -> (here angular will automatically serialise of list into the Json format)
            var defer = $q.defer();

            $http({
                url: apiUrl + '/Home/save_metaTag',
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

        fac.delete_metaTag = function (_Id, _status) {
            debugger;

            // we r going to return a httprespone + data of list -> (here angular will automatically serialise of list into the Json format)
            var defer = $q.defer();

            $http({
                url: apiUrl + '/Home/delete_metaTag',
                method: 'POST',
                params: {
                    Id: _Id,
                    status: _status
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

        fac.get_metaTags = function () {
            var defer = $q.defer();

            $http({
                url: apiUrl + '/Home/get_allMetaTagList',
                method: 'GET'
            })
                .success(function (d) {
                    defer.resolve(d);
                })
                .error(function (e) {
                    defer.reject('something went wrong..');
                });
            return defer.promise;
        }

        //******************  For Gallery  ********************

        fac.get_galleryCategory = function () {
            var defer = $q.defer();

            $http({
                url: apiUrl + '/Home/get_galleryCategory',
                method: 'GET'
            })
                .success(function (d) {
                    defer.resolve(d);
                })
                .error(function (e) {
                    defer.reject('something went wrong..');
                });
            return defer.promise;
        }

        fac.get_gallerys = function () {
            var defer = $q.defer();

            $http({
                url: apiUrl + '/Home/get_gallerys',
                method: 'GET'
            })
                .success(function (d) {
                    defer.resolve(d);
                })
                .error(function (e) {
                    defer.reject('something went wrong..');
                });
            return defer.promise;
        }

        fac.rotate_gallaryImg = function (_imagName, _isNew) {
            var defer = $q.defer();

            $http({
                url: apiUrl + '/Home/rotateGallaryImg',
                method: 'GET',
                params: {
                    imagName: _imagName,
                    isNew: _isNew
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

        fac.change_gallerySts = function (_Id, _status) {
            var defer = $q.defer();

            $http({
                url: apiUrl + '/Home/changeSts_Gallary',
                method: 'GET',
                params: {
                    Id: _Id,
                    status: _status
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

        fac.setGallery_Order = function (_model) {
            var defer = $q.defer();

            $http({
                url: apiUrl + '/Home/setGallery_Order',
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

        fac.remove_gallaryImg = function (_imagName) {
            var defer = $q.defer();

            $http({
                url: apiUrl + '/Home/removeGallaryImg',
                method: 'GET',
                params: {
                    imagName: _imagName,
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

        fac.uploadImage = function (files) {
            var form_Data = new FormData();

            for (var i = 0; i < files.length; i++) {
                form_Data.append("file", files[i]);
            }

            var defer = $q.defer();

            $http({
                method: 'POST',
                url: apiUrl + '/Home/uploadGallaryImg',
                data: form_Data,
                headers: {
                    'Content-Type': undefined
                }
            })
                .success(function (d) {
                    defer.resolve(d);
                })
                .error(function (e) {
                    defer.reject('file upload failed');
                });
            return defer.promise;
        }

        fac.upload_Schedule_olympadsImg = function (files) {
            var form_Data = new FormData();

            for (var i = 0; i < files.length; i++) {
                form_Data.append("file", files[i]);
            }

            var defer = $q.defer();

            $http({
                method: 'POST',
                url: apiUrl + '/Home/upload_Schedule_olympadsImg',
                data: form_Data,
                headers: {
                    'Content-Type': undefined
                }
            })
                .success(function (d) {
                    defer.resolve(d);
                })
                .error(function (e) {
                    defer.reject('file upload failed');
                });
            return defer.promise;
        }

        fac.save_Schedule_olympadsImg = function (_model) {
            debugger;

            // we r going to return a httprespone + data of list -> (here angular will automatically serialise of list into the Json format)
            var defer = $q.defer();

            $http({
                url: apiUrl + '/Home/save_Schedule_olympadsImg',
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

        fac.get_Schedule_olympads = function () {
            var defer = $q.defer();

            $http({
                url: apiUrl + '/Home/get_Schedule_olympads',
                method: 'GET',
            })
                .success(function (d) {
                    defer.resolve(d);
                })
                .error(function (e) {
                    defer.reject('something went wrong..');
                });
            return defer.promise;
        }

        fac.create_gallery = function (_model) {
            debugger;

            // we r going to return a httprespone + data of list -> (here angular will automatically serialise of list into the Json format)
            var defer = $q.defer();

            $http({
                url: apiUrl + '/Home/saveGallery',
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

        fac.create_galleryDetail = function (_model) {
            debugger;

            // we r going to return a httprespone + data of list -> (here angular will automatically serialise of list into the Json format)
            var defer = $q.defer();

            $http({
                url: apiUrl + '/Home/save_galleryDetail',
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

        fac.get_galleryDetail = function (_gallaryId) {
            var defer = $q.defer();

            $http({
                url: apiUrl + '/Home/get_galleryDetail',
                method: 'GET',
                params: {
                    gallaryId: _gallaryId
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

        fac.get_newsUpdates = function () {
            var defer = $q.defer();

            $http({
                url: apiUrl + '/Home/get_newsUpdates',
                method: 'GET',
            })
                .success(function (d) {
                    defer.resolve(d);
                })
                .error(function (e) {
                    defer.reject('something went wrong..');
                });
            return defer.promise;
        }

        fac.get_siteConfiguration = function () {
            var defer = $q.defer();

            $http({
                url: apiUrl + '/Home/get_siteConfiguration',
                method: 'GET',
            })
                .success(function (d) {
                    defer.resolve(d);
                })
                .error(function (e) {
                    defer.reject('something went wrong..');
                });
            return defer.promise;
        }

        fac.save_siteConfiguration = function (_model) {
            debugger;

            // we r going to return a httprespone + data of list -> (here angular will automatically serialise of list into the Json format)
            var defer = $q.defer();

            $http({
                url: apiUrl + '/Home/save_siteConfiguration',
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

        fac.delete_newsUpdates = function (_newsId) {
            var defer = $q.defer();

            $http({
                url: apiUrl + '/Home/delete_newsUpdates',
                method: 'GET',
                params: {
                    newsId: _newsId
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

        fac.save_newsUpdates = function (_model) {
            debugger;

            // we r going to return a httprespone + data of list -> (here angular will automatically serialise of list into the Json format)
            var defer = $q.defer();

            $http({
                url: apiUrl + '/Home/save_newsUpdates',
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

        fac.get_InstantDnd_SubjectsMapping_json = function (_newsId) {
            var defer = $q.defer();

            $http({
                url: apiUrl + '/Home/get_InstantDnd_SubjectsMapping_json',
                method: 'GET',
            })
                .success(function (d) {
                    defer.resolve(d);
                })
                .error(function (e) {
                    defer.reject('something went wrong..');
                });
            return defer.promise;
        }

        fac.save_InstantDnd_mapping = function (_model) {
            debugger;

            // we r going to return a httprespone + data of list -> (here angular will automatically serialise of list into the Json format)
            var defer = $q.defer();

            $http({
                url: apiUrl + '/Home/save_InstantDnd_mapping',
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

        fac.save_carrier = function (_model) {
            debugger;

            // we r going to return a httprespone + data of list -> (here angular will automatically serialise of list into the Json format)
            var defer = $q.defer();

            $http({
                url: apiUrl + '/Home/save_carrier',
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

        fac.get_carrierList = function (_newsId) {
            var defer = $q.defer();

            $http({
                url: apiUrl + '/Home/get_carrierList',
                method: 'GET',
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

    app.factory('admin_Service', ['$http', '$q', 'globalConfig', fn]);

})(angular.module('Silverzone_admin_app'));

