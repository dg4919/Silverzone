(function (app) {

    var answerReciveFn = function ($sc, $rsc, $state, svc, globalConfig, $filter, globalConfig, $window) {
        $sc.levelTypes = [
            { Id: 1, type: 'Level 1' },
            { Id: 2, type: 'Level 2' },
            { Id: 3, type: 'Level 3' },
        ];

        $rsc.HeaderAction = "Templates/School/Answer_sheetRecieving/Header.html";
        $sc.formTitlle = "Answer Sheet Recieve Form";

        $rsc.IsEventShow = true;
        $sc.model = {};
        $sc.paymentType;
        var bundleNo = 0;

        $sc.isEdit = false;
        $sc.schInfo = '';

        $sc.Back = function () {
            $sc.isEdit = false;
        }

        $sc.Add = function () {
            $sc.isEdit = true;
        }

        $sc.Submit = function () {
            if (!$rsc.SelectedEvent) {
                $rsc.notify_fx('Please select an event !', 'warning');
                return;
            }
            if (!$sc.model.EventMgtId) {
                $rsc.notify_fx('Enter a valid School !', 'warning');
                return;
            }

            if (!$sc.model.levelId) {
                $rsc.notify_fx('Select level of recieving answer sheet !', 'warning');
                return;
            }

            if (!$sc.model.OMR_Sheets
            && !$sc.model.QP_Class1
            && !$sc.model.QP_Class2) {
                $rsc.notify_fx('Please enter some input !', 'warning');
                return;
            }

            if ($sc.model.OMR_Scanned
             || $sc.model.ScannedNo_From
             || $sc.model.ScannedNo_To
             || $sc.model.Rejected_Sheet)
                $sc.model.isEnter_ansSheet_scanInfo = true;

            svc.save_ansSheet_Recieve($sc.model)
               .then(function (d) {
                   if (d.result === 'error')
                       $rsc.notify_fx('Oops data is not saved :(', 'danger');
                   else {
                       $rsc.notify_fx('Data is saved :)', 'success');
                       $sc.model = {};
                       $sc.srchSchool();
                   }
               });
        }

        svc.get_json()
        .then(function (d) {
            $sc.paymentType = d.result;
        });

        $sc.srchSchool = function () {
            if (!$sc.schoolCode)
            {
                $rsc.notify_fx('Enter a School Code or Registration Code. !', 'danger');
                return;
            }

            var codeLenth = $sc.schoolCode.length;
            if (!(codeLenth >= 4 && codeLenth <=5)) {
                $rsc.notify_fx('Enter a valid School Code or Registration Code. !', 'warning');
                return;
            }
            if (!$rsc.SelectedEvent) {
                $rsc.notify_fx('Please select an event !', 'warning');
                return;
            }

            var model = {
                Code: $sc.schoolCode,
                codeType: codeLenth === 5 ? 1 : 2,
                eventId: $rsc.SelectedEvent.EventId
            };

            svc.get_schoolInfo(model)
            .then(function (d) {
                if (d.result === '') {
                    $rsc.notify_fx('School record is not found :(', 'info');
                    return;
                }

                $sc.schInfo = d.schInfo;
                $sc.ansSheet_reciveList = d.ansSheet_reciveInfo;
                $sc.model.EventMgtId = d.schInfo.eventMgtId;
                $sc.model.budleNo = d.bundleNo;
                bundleNo = d.bundleNo;
            });           
        }

        $sc.get_Report = function (_bundleNo) {
            var apiUrl = globalConfig.apiEndPoint + globalConfig.version.School;
            var url = apiUrl + `/answer_sheetRecieving/GenerateReport?bundleNo=${_bundleNo}&eventId=${$rsc.SelectedEvent.EventId}`;
            $window.open(url, '_blank');
        }

        $sc.reset_bunleNo = function () {
            $sc.model.budleNo = bundleNo;
        }
    }

    app.controller('answerReciveController', ['$scope', '$rootScope', '$state', 'answerReciveService', 'globalConfig', '$filter', 'globalConfig', '$window', answerReciveFn]);

})(angular.module('SilverzoneERP_App'));