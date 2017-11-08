
(function (app) {
  
    var Export_Import_Fun = function ($scope, $rootScope, $filter, Service, $state, $http, $window, globalConfig) {
        $rootScope.HeaderAction = "Templates/School/Master/ExportImport/Partial/Header.html";
        $scope.uploadSection = true;
        $scope.verifyDocSection = false;
        $scope.resultDocSection = false;
        var uploaded = false;
        var formdata;
        $scope.verifyDocSection = false;
        $scope.dt = { ImportType: '' };

        $scope.list = [
                'School',
                'Country',
                'Zone',
                'State',
                'District',
                'City'
        ];

        $scope.importStepsActive1 = function () {
            return $scope.uploadSection == true ? "importStepsActive" : "";
        }
        $scope.importStepsActive2 = function () {
            return $scope.verifyDocSection == true ? "importStepsActive" : "";
        }
        $scope.importStepsActive3 = function () {
            return $scope.resultDocSection == true ? "importStepsActive" : "";
        }

        $scope.getTheFiles = function ($files) {
            debugger;
           
            formdata = new FormData();
            angular.forEach($files, function (value, key) {
                formdata.append(key, value);
            });
            uploaded = true;           
        };

        $scope.uploadFiles = function () {
            debugger;
           
            if (!uploaded)
                return false;
            
            var request = {
                method: 'POST',
                url: globalConfig.apiEndPoint + 'school/BulkImport/UploadFile',
                data: formdata,
                headers: {
                    'Content-Type': undefined
                }
            };
            // SEND THE FILES.
            $http(request)
                .success(function (response) {
                    debugger;
                    $scope.uploadedData = response.result;                    
                })
                .error(function (error) {

                });
        }

        $scope.Verify = function (_type) {
            debugger;
            if (angular.isUndefined(_type) || $scope._type == '')
            {
                Service.Notification($rootScope,'Please select type !');
                return false;
            }
            
            Service.Create_Update($scope.uploadedData.Data, 'school/BulkImport/Verify' + _type)
            .then(function (response) {
                Service.Notification($rootScope, response.Result);
                if (response.Result != 'error') {
                    $scope.VerifyData = response;
                    $scope.verifyDocSection = true;
                    $scope.uploadSection = false;
                }
            });
        }

        $scope.Back = function () {
            if ($scope.resultDocSection)
                $state.reload();
            else
            {
                $scope.verifyDocSection = false;
                $scope.uploadSection = true;
            }                        
        }

        $scope.Submit = function () {
            debugger;
            Service.Create_Update($scope.VerifyData.Correct, 'school/BulkImport/Save' + $scope.dt.ImportType)
                        .then(function (response) {
                            Service.Notification($rootScope, response.Result);
                            if (response.Result == 'success')
                            {
                                $scope.FinalData = response;
                                $scope.resultDocSection = true;
                                $scope.verifyDocSection = false;
                                $scope.uploadSection = false;
                            }                            
                        });
        }

        $scope.exportData = function (id,FileName) {
            var blob = new Blob([document.getElementById(id).innerHTML], {
                type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;charset=utf-8"
            });
            saveAs(blob, id + "_Report.xls");
        };

        $scope.exportFormat = function (FileName) {
            debugger;           
            $window.open(globalConfig.apiEndPoint+'school/BulkImport/ExportFormat?FileName=' + FileName);            
        }

    }

    app.controller('exportImportController', ['$scope', '$rootScope', '$filter', 'Service', '$state', '$http', '$window', 'globalConfig', Export_Import_Fun]);

})(angular.module('SilverzoneERP_App'));