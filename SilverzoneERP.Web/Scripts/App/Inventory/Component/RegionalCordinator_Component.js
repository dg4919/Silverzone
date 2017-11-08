//https://tests4geeks.com/build-angular-1-5-component-angularjs-tutorial/

(function () {
    'use strict';

    var rcSchoolAssign_fn = function ($sc, $rsc, svc, modalSvc) {
        $sc.rcList = [];
        $sc.rcSchools = null;
        $sc.selected_rcId;

        svc.get_rcList()
           .then(function (d) {
               $sc.rcList = d.result;
           });

        $sc.save = function (_model, insertByCode) {
            if (!$sc.selected_rcId) {
                $rsc.notify_fx('Please Select RC !', 'danger');
                return;
            }

            var _model = {
                RcId: $sc.selected_rcId,
                schIds: _model.schIds,
                addRescords: _model.sts
            }
            if (insertByCode)
                svc.save_RcSchool_byschCode(_model)
            .then(responseFn);
            else
                svc.save_RcSchool(_model)
                .then(responseFn);
        }

        var responseFn = function (d) {
            if (d.result) {
                if (d.result === 'notfound')
                    $rsc.notify_fx('Record not found :(, Try another !', 'danger');
                else
                    $rsc.notify_fx('School is assigned to selected RC :)', 'success');
            }
            else
                $rsc.notify_fx('Something went wrong :(, Try Again', 'warning');

            if (d.notFound) {
                $rsc.notify_fx('Scool with Ids ' + d.notFound + ' not found :(', 'warning');
            }

            if (d.matched) {
                $rsc.notify_fx('Scool with Ids ' + d.matched + ' already exist !', 'warning');
            }

        }

        // 1st btn
        $sc.getCodeModal = function () {
            modalSvc.get_manageSch_byCodeModal()
                .then(function (d) {
                    $sc.save(d, true);
                });
        }

        // 2nd btn
        $sc.getbulkModal = function () {
            modalSvc.get_bulkSchoolModal()
                .then(function (d) {
                    $sc.save(d, false);
                });
        }

        // 3rd btn
        $sc.get_rcSchools = function () {
            if (!$sc.selected_rcId) {
                $rsc.notify_fx('Please Select RC !', 'danger');
                return;
            }

            svc.get_rcSchools($sc.selected_rcId)
                .then(function (d) {
                    $sc.rcSchools = d.result;
                });
        }

    }

    var rcSchoolVisit_fn = function ($sc, $rsc, svc, modalSvc) {
        $rsc.HeaderAction = "Templates/inventory/RegionalCordinator/schoolVisits_header.html";
        $sc.isEdit = false;

        $sc.events = {};
        $sc.visitTypes = {};
        $sc.VisitStatus = {};
        $sc.rcSchools = {};
        $sc.model = {};
        $sc.rcVisits = {};

        $sc.Back = function () {
            $sc.isEdit = false;
        }

        $sc.Add = function () {
            $sc.isEdit = true;
        }

        svc.get_events()
           .then(function (d) {
               $sc.events = d.result;
           });

        svc.get_visitTypes()
           .then(function (d) {
               $sc.visitTypes = d.result;
           });

        svc.get_VisitStatus()
           .then(function (d) {
               $sc.VisitStatus = d.result;
           });

        svc.get_rcSchools(0)       // get schools list to current login RC User
            .then(function (d) {
                //$sc.rcSchools = d.result.map(function (school) {
                //    return ({ schId: school.Id, schName: school.SchName });
                //});
                $sc.rcSchools = d.result;
            });

        $sc.submit_data = function (form) {
            if (form.validate()) {
                $sc.model.schId = $sc.model.school.Id;

                var arr = [];
                angular.forEach($sc.events, function (entity, k) {
                    if (entity.positive_status ||
                       entity.negative_status ||
                       entity.hold_status)
                        arr.push({
                            eventId: entity.Id,
                            visitStatus: entity.positive_status ? 1
                                       : entity.negative_status ? 2 : 3
                        });
                });
                angular.extend($sc.model, { rcEventInfo: arr });

                svc.save_RcVisits($sc.model)
                   .then(function (d) {
                       if (d.result === 'exist')
                           $rsc.notify_fx('Data already exist :(', 'warning');
                       else {
                           get_RcVisits();
                           $rsc.notify_fx('Data is saved :)', 'success');
                       }
                   });
            }
        }

        function get_RcVisits() {
            svc.get_RcVisits()
            .then(function (d) {
                $sc.rcVisits = d.result;
            });
        }
        get_RcVisits();

        $sc.validationOptions = {
            rules: {
                school: {
                    required: true,
                },
                event: {
                    required: true,
                },
                visitType: {
                    required: true,
                },
                visitDate: {
                    required: true,
                },
                visitStatus: {
                    required: true,
                },
                remarks: {
                    required: true,
                }
            }
        }
    }

    var rcFollowUps_fn = function ($sc, $rsc, svc, modalSvc) {
        $sc.floowUps = {};

        svc.get_followUps(1)
        .then(function (d) {
            $sc.floowUps = d.result;
        });
    }

    angular
        .module('SilverzoneERP_invenotry_component')  // create a new module Here
        .controller('rcSchoolAssign_controller', ['$scope', '$rootScope', 'RegCordService', 'inventory_modalService', rcSchoolAssign_fn])
        .controller('rcSchoolVisit_controller', ['$scope', '$rootScope', 'RegCordService', 'inventory_modalService', rcSchoolVisit_fn])
        .controller('rcFollowUps_controller', ['$scope', '$rootScope', 'RegCordService', 'inventory_modalService', rcFollowUps_fn])

    ;

})();