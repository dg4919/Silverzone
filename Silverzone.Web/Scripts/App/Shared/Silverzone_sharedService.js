(function () {

    var fn = function (globalConfig, httpSvc) {
        
        var apiUrl = globalConfig.apiEndPoint + globalConfig.version.Site,
            fac = {};

        fac.getAll_class = function () {
            return httpSvc.get(apiUrl + '/Book/getAllClass');
        }

        fac.get_subject_byClassId = function (_classId) {
            return httpSvc.get(
                apiUrl + '/Subject/Get_subjects_ByclassId',
                { classId: _classId },
                true);
        }

        fac.get_bookCategorys = function () {
            return httpSvc.get(apiUrl + '/Category/category_List');
        }

        fac.getAll_subject = function () {
            return httpSvc.get(apiUrl + '/Subject/Get_allSubjects');
        }

        fac.get_class_bySubjctId = function (_subjectId) {
            return httpSvc.get(
                apiUrl + '/Subject/Get_class_BysubjectId',
                { subjectId: _subjectId },
                true);
        }

        fac.get_bookTitle_byClass = function () {
            return httpSvc.get(apiUrl + '/Category/get_bookTitle_byClass');
        }


        return fac;
    }

    angular.module('Silverzone_service.Shared', [])
    .factory('sharedService', ['globalConfig', 'httpService', fn]);

})();

