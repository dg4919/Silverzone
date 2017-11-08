(function (app) {

    var fn = function ($sc, $rsc, $window, svc, modalSvc) {
        $sc.model = {
            L1_subjects: [],
            L2_subjects: []
        };
        $sc.userInfo = {
            amount: 0
        };

        svc.getJson()
            .then(function (d) {
                $sc.jsonModel = d.result;
            });

        $sc.$watch('[model.classId, model.year]', function (arr_new, arr_old) {
            $sc.model.L1_subjects = [];
            $sc.model.L2_subjects = [];
            $sc.eventInfo = null;

            if (arr_new[1]) {
                if (!arr_new[0]) {
                    $rsc.notify_fx('Please select a class !', 'danger');
                    return;
                }

                $sc.disableCheckBox = false;
                $sc.userInfo.amount = 0;

                var eventList = _.find($sc.jsonModel.events, function (item) {
                    return item.classId === arr_new[0] && item.yearId === arr_new[1];
                }).eventInfo;

                $sc.eventInfo = eventList;
                angular.forEach($sc.jsonModel.subjectList, function (entity, key) {
                    entity.L1 = [];
                    entity.L2 = [];

                    entity.LevelSts = _.find(eventList, function (item) {
                        return item.subjectId === entity.Id;
                    });
                });
            }
            else
                $sc.disableCheckBox = true;                
           
        });

        $sc.onChange = function (status, id, type) {
            if (type === 1) {
                if (status)
                    $sc.model.L1_subjects.push(id);
                else
                    $sc.model.L1_subjects = _.reject($sc.model.L1_subjects, function (d) { return d === id; });
            }
            else if (type === 2) {
                if (status)
                    $sc.model.L2_subjects.push(id);
                else
                    $sc.model.L2_subjects = _.reject($sc.model.L2_subjects, function (d) { return d === id; });
            }

            $sc.userInfo.amount = $rsc.siteConfig.InstantDnd_Price * ($sc.model.L1_subjects.length + $sc.model.L2_subjects.length);
        }

        $sc.submit = function (form) {
            if (form.validate()) {
                
                if (!$sc.model.L1_subjects.length &&
                    !$sc.model.L2_subjects.length) {
                    $rsc.notify_fx('Please select atleast one subject', 'danger');
                    return;
                }

                svc.processPayment(
               angular.extend($sc.model, $sc.userInfo)
               ).then(function (d) {
                   if (d.result === 'success') {
                       $sc.userInfo.order_id = d.orderId;
                       instantDnd_orderId = d.orderId;
                       var paymentModel = {
                           redirect_url: `http://${$window.location.host}/paymentSuccess?orderId=${d.orderId}`,
                           cancel_url: `http://${$window.location.host}/paymentError?orderId=${d.orderId}`,
                       };

                       var model = {
                           clasName: _.find($sc.jsonModel.classList, function (clas) { return clas.Id === $sc.model.classId }).className,
                           yearName: _.find($sc.jsonModel.previousYrQp, function (year) { return year.Id === $sc.model.year }).year,
                           L1_subjects: $sc.model.L1_subjects,
                           L2_subjects: $sc.model.L2_subjects
                       };

                       modalSvc.show_instantDnd_payment_confirmModal(
                           angular.extend($sc.userInfo, paymentModel),
                           $sc.jsonModel.subjectList,
                           model);
                   }
                   else
                       alert('error ocured');
               });
            }
        }

        $sc.validationOptions = {
            rules: {
                uname: {
                    required: true
                },
                email: {
                    required: true,
                    email: true
                },
                Mobile: {
                    required: true,
                    minlength: 10,
                    maxlength: 10
                },
                city: {
                    required: true
                },
                state: {
                    required: true
                },
                postcode: {
                    required: true
                },
                country: {
                    required: true
                },
                address: {
                    required: true
                },
                clas: {
                    required: true
                },
                year: {
                    required: true
                },
            }
        };

        $sc.paymentModel = {
            redirect_url: 'http://' + $window.location.host + '/paymentSuccess',
            cancel_url: 'http://' + $window.location.host + '/paymentError',
        };

        svc.getCountry()
        .then(function (d) {
            $sc.countrys = d.result;
        });
    }

    var instantDnd_loginFn = function ($sc, $rsc, $window, svc) {
        $sc.userInfo = {};
        $sc.pdfList = [];

        $sc.submit = function (form) {
            if (form.validate()) {

                svc.get_userInstant_dndPdf($sc.userInfo)
                   .then(function (d) {
                       if (d.result === 'notfound')
                           $rsc.notify_fx('EmailId and Mobile number not matched !', 'danger');
                       else
                           $sc.pdfList = d.result;
                   });
            }
        }

        $sc.validationOptions = {
            rules: {
                email: {
                    required: true,
                    email: true
                },
                mobile: {
                    required: true,
                    minlength: 10,
                    maxlength: 10
                },
            }
        };

        $sc.downloadPdf = function (Id) {
            svc.dnd_instantFile({ InstantDnd_subjectId: Id });
        }

    }

    app
    .controller('instant_downloadsController', ['$scope', '$rootScope', '$window', 'siteMasterService', 'modalService', fn])
    .controller('instantDnd_loginController', ['$scope', '$rootScope', '$window', 'siteMasterService', instantDnd_loginFn])

    ;

})(angular.module('Silverzone_app'));