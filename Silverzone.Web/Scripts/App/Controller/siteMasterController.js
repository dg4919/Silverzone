/// <reference path="../angular-1.5.8.js" />

// Use to work with Website Layout functionality
(function (app) {

    // when redirect to other URL > then there respective ctrler is called & 'site_MasterController' called once
    // children controller inside this ctrler > can access scope variable of this ctrl
    var site_MasterControllerfn = function ($sc, $rsc, $modal, $state, loginModal, acount_svc, localStorageService, siteSvc, $window) {

        $rsc.Show_signInModal = loginModal;         // auto open Login Modal > when we call service

        $rsc.user_logOut = function () {
            acount_svc.Logout();
            $state.go('book_list');
        }

        $sc.show_cartDetail_popUp = function () {
            // return true when URL is matched
            // but i dont want to show popup on these URL so will use (!is_show_cartDetail_popUp)

            return ($state.is("cart_detail") ||
                    $state.is("cart_address_detail") ||
                    $state.is("cart_summary") ||
                    $state.is("payment_summary") ||
                    $state.is("payment_method")
                );
        }

        $sc.open_EnquiryForm = function () {
            modalSvc.show_EnquiryForm();
        }

        $rsc.metaInfo = {};
        $rsc.$on('$stateChangeStart', function (event, toState, toParams, fromState, fromParams) {
            var url = toState.url;

            // to replace parameter based url's
            angular.forEach(toParams, function (v, k) {
                url = url.replace('{' + k + '}', v);
            });

            siteSvc.getMeta_tagInfo(url)
                   .then(function (d) {
                       $rsc.metaInfo = d.result;
                   });
        });

        $sc.freelancClik = function () {
            $window.location = "http://localhost:55604/ERP/Login?RoleName=Teacher"
        }

        $sc.goto_bookDetail = function (_bookId) {
            $state.go('book_details', { bookId: _bookId });
        }

        // hold detail of book shiping xharger, instant DND price etc.
        siteSvc.get_siteConfiguration()
        .then(function (d) {
            $rsc.siteConfig = d.result;
        });

    }

    var galleryDetailfn = function ($sc, $rsc, $stateParams, $modal, svc, modalSvc) {
        $sc.galleryInfo = {};

        svc.get_galleryDetail($stateParams.galleryId)
           .then(function (d) {
               $sc.galleryInfo = d.result;
           });

        $sc.showGallery_detailModal = function (index) {
            modalSvc.get_imgModal(index, $sc.galleryInfo.galleryDetail);
        }

    }

    var result_ControllerFn = function ($sc, $rsc, $modal, svc) {
        $sc.eventModel = {
            levelType: 1
        };
        $sc.dataIs_loaded = false;

        svc.getEvents()
           .then(function (d) {
               $sc.eventList = d.result;
               assignEvent(1);
           });

        $sc.assignLevel = function (val) {
            $sc.eventModel.levelType = val;
            assignEvent(val);
        }

        function assignEvent(levelType) {
            $sc.ResultEvents = _.find($sc.eventList, function (entity) {
                return entity.Key === levelType;
            }).eventInfo;
        }

        $sc.findResult = function () {
            if (!$sc.eventModel.enrolmentNo) {
                $rsc.notify_fx('Enter Your Enrollment Number !', 'error');
                return;
            }
            if (!$sc.eventModel.result_eventId) {
                $rsc.notify_fx('Please select an Event !', 'error');
                return;
            }

            svc.search_result($sc.eventModel)
               .then(function (d) {
                   $sc.dataIs_loaded = true;
                   $sc.srchResult = d.result;

                   $sc.ResultMsg = {
                       Level: 'Level ' + $sc.eventModel.levelType,
                       EventName: _.find($sc.eventList.ResultEvents, function (item) {   // return only single matched record
                           return item.Id === $sc.eventModel.result_eventId
                       }).EventCode,
                       currentYear: (new Date()).getFullYear()
                   }
               });
        }
    }

    var subscribe_updatesControllerFn = function ($sc, $rsc, svc) {
        $sc.model = {};

        svc.getCountry()
        .then(function (d) {
            $sc.countrys = d.result;
        });

        svc.getJson()
           .then(function (d) {
               $sc.jsonModel = d.result;
           });

        $sc.submit = function (form) {
            if (form.validate()) {
                svc.register_subscribeUpdates($sc.model)
                   .then(function () {
                       $sc.model = {};
                       $rsc.notify_fx('Congrats! you have subscribed for new updates :)', 'info');
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
                Confirm: {
                    required: true
                },
            }
        };
    }

    var register_schoolControllerFn = function ($sc, $rsc, svc) {
        $sc.model = {};

        svc.getCountry()
        .then(function (d) {
            $sc.countrys = d.result;
        });

        svc.getJson()
           .then(function (d) {
               $sc.jsonModel = d.result;
           });

        svc.get_registerSchool_json()
        .then(function (d) {
            $sc.school_registerJson = d.result;
        });

        $sc.submit = function (form) {
            if (form.validate()) {

                var _model = {
                    genderName: _.find($sc.school_registerJson.genders, function (entity) {
                        return entity.Id === $sc.model.GenderId;
                    }).Name,
                    stateName: _.find($sc.jsonModel.stateList, function (entity) {
                        return entity.Id === $sc.model.stateId;
                    }).StateName,
                    countryName: _.find($sc.countrys, function (entity) {
                        return entity.code === $sc.model.countryId;
                    }).name,
                    profileName: _.find($sc.school_registerJson.profiles, function (entity) {
                        return entity.Id === $sc.model.ProfileId;
                    }).Name
                };

                svc.register_school(angular.extend($sc.model, _model))
                   .then(function () {
                       $sc.model = {};
                       $rsc.notify_fx('Congrats! you have Register you School :)', 'info');
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
                country: {
                    required: true
                },
                address: {
                    required: true
                },
                pinCode: {
                    required: true
                },
                year: {
                    required: true
                },
                Confirm: {
                    required: true
                },
                gender: {
                    required: true
                },
                profile: {
                    required: true
                },
                schName: {
                    required: true
                },
            }
        };
    }

    var galleryfn = function ($sc, svc) {
        $sc.galleryList = [];
        $sc.responsiveSetting = [{
            breakpoint: 1024,
            settings: {
                slidesToShow: 2,
                slidesToScroll: 3,
                infinite: true,
                dots: true
            }
        },
        {
            breakpoint: 600,
            settings: {
                slidesToShow: 2,
                slidesToScroll: 2
            }
        }, {
            breakpoint: 480,
            settings: {
                slidesToShow: 1,
                slidesToScroll: 1
            }
        }];

        svc.get_gallery()
        .then(function (d) {
            $sc.galleryList = d.result;
        });
    }

    var mediafn = function ($sc, svc) {
        $sc.mediaList = [];

        svc.get_MediaImg()
       .then(function (d) {
           $sc.mediaList = d.result;
       });
    }

    var hof_fn = function ($sc, svc) {
        $sc.hofList = [];

        svc.get_hallofFame()
           .then(function (d) {
               $sc.hofList = d.result;
           });
    }

    var freelance_fn = function ($sc, $rsc, svc) {
        $sc.model = {};

         svc.getCountry()
        .then(function (d) {
            $sc.countrys = d.result;
         });

        svc.getJson()
           .then(function (d) {
               $sc.jsonModel = d.result;
           });

        svc.get_registerSchool_json()
        .then(function (d) {
            $sc.school_registerJson = d.result;
        });


        $sc.submit = function (form) {
            if (form.validate()) {
                $sc.model.UserName = $sc.model.fName + ' ' + $sc.model.lName;

                svc.save_freelance($sc.model)
                .then(function (d) {
                    $sc.model = {};
                    $rsc.notify_fx('Your details has been submitted with us :)', 'info');
                });
            }
        }

        $sc.validationOptions = {
            rules: {
                fname: {
                    required: true
                },
                lname: {
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
                country: {
                    required: true
                },
                address: {
                    required: true
                },
                confirm: {
                    required: true                
                },
                pinCode: {
                    required: true
                },
                dob: {
                    required: true,
                    min: 21,
                    max: 60
                },
                gender: {
                    required: true
                },
                qualification: {
                    required: true
                },
            }
        };
    }

    var associates_fn = function ($sc, svc) {
        var that = this;

        $sc.myval = "Divya Gupta";
        $sc.myvalu = "DivyaG4919";
    }

    var resultRequest_ControllerFn = function ($sc, $rsc, svc) {
        $sc.model = {};

        svc.getCountry()
        .then(function (d) {
            $sc.countrys = d.result;
        });

        svc.getJson()
           .then(function (d) {
               $sc.jsonModel = d.result;
           });

        $sc.submit = function (form) {
            if (form.validate()) {
                svc.save_resultRequest($sc.model)
                   .then(function () {
                       $sc.model = {};
                       $rsc.notify_fx('We have submit your details, we will get back to you !', 'info');
                   });
            }
        }

        $sc.validationOptions = {
            rules: {
                uname: {
                    required: true
                },
                enrollNo: {
                    required: true
                },
                schName: {
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
                Confirm: {
                    required: true
                },
            }
        };
    }


    app
    .controller('site_MasterController', ['$scope', '$rootScope', '$uibModal', '$state', 'login_modalService', 'user_Account_Service', 'localStorageService', 'siteMasterService', '$window', site_MasterControllerfn])
    .controller('gallery_DetailController', ['$scope', '$rootScope', '$stateParams', '$uibModal', 'siteMasterService', 'modalService', galleryDetailfn])
    .controller('resultController', ['$scope', '$rootScope', '$uibModal', 'siteMasterService', result_ControllerFn])
    .controller('subscribe_updatesController', ['$scope', '$rootScope', 'siteMasterService', subscribe_updatesControllerFn])
    .controller('register_schoolController', ['$scope', '$rootScope', 'siteMasterService', register_schoolControllerFn])
    .controller('galleryController', ['$scope', 'siteMasterService', galleryfn])
    .controller('MediaController', ['$scope', 'siteMasterService', mediafn])
    .controller('hall_of_fameController', ['$scope', 'siteMasterService', hof_fn])
    .controller('freelanceController', ['$scope', '$rootScope', 'siteMasterService', freelance_fn])
    .controller('associatesController', ['$scope', 'siteMasterService', associates_fn])
    .controller('resultRequest_Controller', ['$scope', '$rootScope', 'siteMasterService', resultRequest_ControllerFn])

    .component('associatesPage', {
        template: 'Hello Associate {{ astCtrl.name }} {{ $parent.myval }}',
        bindings:{
            fname: '@'
        },
        controller: ['$scope', function ($sc) {
            this.name = "Yooo";
        }],
        controllerAs: 'astCtrl',
    })
    .directive('associatesPagee', function () {

        return {
            restrict: 'E',
            template: 'Hello Associate {{ name }} {{ $parent.myval }}',
            scope: {
                fname: '@'
            },
            controller: ['$scope', function ($sc) {
                $sc.name = "Yooo";
            }],
            //controllerAs: 'astCtrl'
        };
    })
    ;

})(angular.module('Silverzone_app'));