
(function (app) {

    var fn = function (globalConfig, httpSvc) {
        
        var apiUrl = globalConfig.apiEndPoint + globalConfig.version.Site,
            fac = {};

        // *********************  Service methods start from here  *********************

        fac.get_bookDetail = function (book_Id) {

            return httpSvc.get(
                apiUrl + '/Book/get_bookDetail_byId',
                { bookId: book_Id },
                true);
        }

        //*********************** Searcing of Books  *********************************
       
        fac.searchBooks = function (model) {
            var params = {
                classId: model.classId,
                subjectId: model.subjectId,
                book_categoriesId: model.cateogysId
            };

            return httpSvc.get(
                apiUrl + '/Book/searchBooks',
                params,
                true);
        }

        fac.get_booksuggestion = function (book_id) {
            return httpSvc.get(
             apiUrl + '/Book/getbook_suggestions',
             { bookId: book_id },
             true);
        }

        fac.getbook_recommends = function (book_id) {
            return httpSvc.get(
             apiUrl + '/Book/getbook_recommends',
             { bookId: book_id },
             true);
        }

        //*********************** Books Bundle  *********************************

        fac.get_bookBundleDetail = function (bundleId) {
            return httpSvc.get(
            apiUrl + '/Book/get_bookBundleDetail_byId',
            { bundleId: bundleId },
            true);
        }

        fac.getbook_bundles = function (model) {
            return httpSvc.post(
              apiUrl + '/Book/getbook_bundles',
              model);
        }


        return fac;
    }

    app.factory('bookService', ['globalConfig', 'httpService', fn]);

})(angular.module('Silverzone_app'));

