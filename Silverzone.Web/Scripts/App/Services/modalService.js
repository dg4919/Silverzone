(function (app) {

    app.service('modalService', ['$uibModal', function ($modal) {

        this.show_cart_proceedModal = function () {

            var template = ' <div class="modal-header">                                                              '
                         + ' <h4 class="box-title">Select Your Country</h4>                                          '
                         + '<button type="button" class="close" style="margin-top: -30px !important;" ng-click="cancel()">  '
                         + '<span aria-hidden="true">×</span></button> </div>                                        '
                         + ' <div class="modal-body" style="padding: 30px !important;">                              '
                         + ' <label class="radio-inline col-sm-6"> <input type="radio" name="optradio" ng-model="country_type" value="1">India</label> '
                         + ' <label class="radio-inline"><input type="radio" name="optradio" ng-model="country_type" value="2">Outside India</label> </div>'
                         + ' <div class="modal-footer">                                                              '
                         + ' <button class="btn gray" ng-click="submit_data(country_type)">OK</button>  </div>   '

            var modalInstance = $modal.open({
                template: template,
                size: 'sm',
                controller: ['$scope', '$rootScope', '$uibModalInstance', '$state', function ($sc, $rsc, $modalInstance, $state) {
                    $sc.submit_data = function (country_type) {
                        if (country_type !== undefined)
                            $modalInstance.close(parseInt(country_type));
                        else
                            $rsc.notify_fx('Select your country name !', 'warning');
                    }

                    $sc.cancel = function () {
                        $modalInstance.dismiss();
                    }
                }]
            });

            return modalInstance.result;
        }

        this.get_userAdress_modal = function (country_code, model) {
            var _model = model ? angular.copy(model) : '';

            var modalInstance = $modal.open({
                templateUrl: 'templates/modal/user_shipping_Addressform.html',
                windowClass: 'shippingAdressModal',
                resolve: {
                    user_addressModel: function () {
                        return _model;               // send value from here to controller as dependency
                    }
                },
                controller: ['$scope', '$rootScope', '$uibModalInstance', 'cartService', '$filter', 'user_addressModel', function ($sc, $rsc, $modalInstance, svc, $filter, user_addressModel) {
                    $sc.shippingInfo = user_addressModel ? user_addressModel : {};      // check value exist or not for add or Edit
                    var Is_Add = user_addressModel ? false : true;

                    $sc.shippingInfo.is_disableEmail = false;
                    $sc.shippingInfo.is_disableMobile = false;

                    // NUll coleasce fx > if $sc.shippingInfo.Country value = 0, null, undefine, '' > then country_code will assign otherwise $sc.shippingInfo.Country
                    $sc.shippingInfo.Country = parseInt($sc.shippingInfo.Country || country_code);

                    // use to show City & State txtbox
                    $sc.is_show = true;

                    // for only add adress > check if user is login then fetch record
                    if ($rsc.user.currentUser !== '' && Is_Add) {
                        $sc.is_show = false;

                        // if data is present
                        if ($rsc.user.currentUser.EmailID) {
                            $sc.shippingInfo.Email = $rsc.user.currentUser.EmailID;
                            $sc.shippingInfo.is_disableEmail = true;
                        }

                        if ($rsc.user.currentUser.MobileNumber) {
                            $sc.shippingInfo.Mobile = $rsc.user.currentUser.MobileNumber;
                            $sc.shippingInfo.is_disableMobile = true;
                        }
                    }

                    $sc.close = function () {
                        $modalInstance.dismiss();
                    }

                    $sc.save_user_shipping_Address = function (form) {
                        if (form.validate()) {

                            if (!Is_Add)
                                $sc.shippingInfo.Id = user_addressModel.Id;

                            svc.save_sippingAdress($sc.shippingInfo)
                               .then(function (data) {
                                   $sc.shippingInfo.Id = data.result;
                                   $modalInstance.close($sc.shippingInfo);
                               });
                        }
                    }

                    $sc.get_userLocation_by_pinCode = function (pincode) {
                        if (pincode) {      // when txtbox is not empty
                            svc.get_location($sc.shippingInfo.PinCode).then(function (response) {
                                $sc.shippingInfo.City = response.city;      // wrk when ajax call sucesfuly return data
                                $sc.shippingInfo.State = response.state;
                                $sc.is_show = true;
                            }, function () {
                                console.log('in error');
                            });
                        }
                    }

                    $sc.change_sts = function (Type) {
                        if (Type === 'email')
                            $sc.shippingInfo.is_disableEmail = false;
                        else if (Type === 'mobile')
                            $sc.shippingInfo.is_disableMobile = false;
                    }

                    // validating rules for registration mmodel
                    $sc.validationOptions = {
                        rules: {
                            userName: {            // use with name attribute in control
                                required: true
                            },
                            email: {
                                required: true,
                                email: true
                            },
                            mobile: {
                                required: true,
                                minlength: 10
                            },
                            shipping_adress: {
                                required: true
                            },
                            shipping_country: {
                                required: true
                            },
                            shipping_pincode: {
                                required: true
                            },
                            shipping_city: {
                                required: true
                            },
                            shipping_state: {
                                required: true
                            }
                        }
                    }
                }]
            });

            return modalInstance.result;
        }

        this.show_instantDnd_payment_confirmModal = function (user, subjects, selected_Subjects) {
            var _template = `
                 <div class="modal-header">
                 <span class="text-warning" ng-click="cancel()" style="float: right;font-size: 20px;cursor: pointer;">
                 <i class="fa fa-edit" title="Edit"></i></span>
                 <h4 class="box-title">Confirm Detail</h4> </div>
                 <div class="modal-body">
                 <p> Name: <b> {{ userInfo.billing_name }} </b></p>
                 <p> <i class="fa fa-envelope-o"></i> Email Id: {{ userInfo.billing_email }}</p>
                 <p> <i class="fa fa-phone"></i> Mobile: {{ userInfo.billing_tel }}</p>
                 <p> Address: {{ userInfo.billing_address }}, {{ userInfo.billing_city }}, {{ userInfo.billing_state }} - {{ userInfo.billing_zip }}, {{ userInfo.billing_country }}  </p>
                 <p class ="col-sm-4" style="padding: 0px;"> Class: <b> {{ scopeModel.clasName }} </b></p>
                 <p class="col-sm-4"> Year: <b> {{  scopeModel.yearName }} </b></p>
                 <p class="col-sm-4"> Total Amount: <b> {{ userInfo.amount }} </b></p>

                 <div class="row" style="padding: 10px;">
                 <table class="table table-hover text-center">
                     <thead>
                         <tr class="text-center">
                             <th>Levels</th>
                             <th class="text-center col-sm-1"
                                 ng-repeat="subject in subjects"
                                 ng-bind="subject.Name"></th>
                         </tr></thead><tbody>
                         <tr>
                             <td><label value="LevelI">Level-I</label></td>
                             <td class="text-center col-sm-1"
                                 ng-repeat="subject in subjects">
                                 <i class="fa fa-check text-success"
                                    ng-if="subject.L1_subject"></i>
                                 <i class="fa fa-close text-danger"
                                    ng-if="!subject.L1_subject"></i>
                             </td></tr>
                         <tr>
                             <td><label value="LevelI">Level-2</label></td>
                             <td class="text-center col-sm-1"
                                 ng-repeat="subject in subjects">
                                 <i class="fa fa-check text-success"
                                    ng-if="subject.L2_subject"></i>
                                 <i class="fa fa-close text-danger"
                                    ng-if="!subject.L2_subject"></i>
                 </td></tr></tbody></table></div></div>

                 <div class="modal-footer">
                     <input type="button" value="Cancel" class="btn btn-danger pull-left" ng-click="cancel()">

                 <form method="post" action="https://secure.ccavenue.com/transaction/transaction.do?command=initiateTransaction">
                    <input type="hidden" name="encRequest" value="{{ checkSum_value.strEncRequest }}" />
                    <input type="hidden" name="access_code" value="{{ checkSum_value.strAccessCode }}" />
                  <span style="font-weight: 700;float:right;">
                  <input type="submit" value="Pay and Download" class="btn btn-primary pull-right" />
                 </span> </form></div> `;

            var modalInstance = $modal.open({
                template: _template,
                resolve: {
                    model: function () {
                        return ({
                            userInfo: user,
                            subjects: subjects,
                            selected_Subjects: selected_Subjects
                        });
                    }
                },
                size: 'lg',
                windowClass: 'model-large',
                controller: ['$scope', '$uibModalInstance', 'model', 'siteMasterService', function ($sc, $modalInstance, model, svc) {
                    $sc.userInfo = model.userInfo;
                    $sc.scopeModel = model.selected_Subjects;

                    var subjects = [];
                    angular.forEach(model.subjects, function (subject, k) {
                        var isMatched = false;
                        angular.forEach(model.selected_Subjects.L1_subjects, function (v, k) {
                            if (subject.Id === v)
                                isMatched = true;
                        });
                        subjects.push(angular.extend(subject, { L1_subject: isMatched }));
                    });

                    $sc.subjects = subjects.map(function (subject, k) {
                        var isMatched = false;
                        angular.forEach(model.selected_Subjects.L2_subjects, function (v, k) {
                            if (subject.Id === v)
                                isMatched = true;
                        });
                        return angular.extend(subject, { L2_subject: isMatched });
                    });

                    svc.get_checksum($sc.userInfo)      // $.param($sc.userInfo)
                    .then(function (d) {
                        $sc.checkSum_value = d.result;
                    });

                    $sc.cancel = function () {
                        $modalInstance.dismiss();
                    }
                }],

            });
        }

        // ************* Site Master Modal's  *************

        this.show_EnquiryForm = function () {
            var instance = $modal.open({
                templateUrl: 'templates/modal/enquireyForm.html',
                size: 'md',
                controller: ['$scope', '$rootScope', 'siteMasterService', '$uibModalInstance', function ($sc, $rsc, svc, $modalInstance) {
                    $sc.model = {};

                    $sc.submitForm = function (form) {
                        if (form.validate()) {

                            svc.submitEnquiry($sc.model)
                            .then(function (d) {
                                if (d.result) {
                                    $rsc.notify_fx('Enquiry Has been submitted', 'info');
                                    $modalInstance.close();
                                }
                            });
                        }
                    }

                    $sc.cancel = function () {
                        $modalInstance.dismiss();
                    }

                    $sc.validationOptions = {
                        rules: {
                            name: {            // use with name attribute in control
                                required: true
                            },
                            email: {
                                required: true,
                                email: true
                            },
                            phone: {            // use with name attribute in control
                                required: true,
                                minlength: 10
                            },
                            state: {            // use with name attribute in control
                                required: true
                            },
                            comment: {            // use with name attribute in control
                                required: true
                            }
                        }
                    };

                }],
            });
        }

        this.get_imgModal = function (index, imgList) {
            var Template = ` <div class="modal-header" style="padding: 10px !important;">
                  <h4 class="box-title">Gallery Detail</h4></div>
                  <div class="modal-body">
                  <img class="img-responsive"
                       ng-src="{{ currentImg }}"
                       style="margin: 0 auto;">
                   </div>
                   <div class="modal-footer">
                        <button type="submit" class="btn btn-primary pull-left" ng-click="showNextImg(false)" ng-disabled="disable_prevBtn">Previous</button>
                        <button type="button" class="btn btn-warning" ng-click="showNextImg(true)" ng-disabled="disable_nextBtn">Next</button>
                  </div> </div>`;

            var modalInstance = $modal.open({
                template: Template,
                size: 'md',
                controller: ['$scope', '$uibModalInstance', function ($sc, $modalInstance) {
                    $sc.currentImg = imgList[index].ImageUrl;
                    var ImgLen = imgList.length - 1;

                    function checkBtnState() {
                        $sc.disable_prevBtn = index === 0 ? true : false;
                        $sc.disable_nextBtn = index === ImgLen ? true : false;
                    }
                    checkBtnState();

                    $sc.showNextImg = function (isNext) {
                        if (isNext) {
                            ++index;
                            $sc.currentImg = imgList[index].ImageUrl;
                            checkBtnState();
                        }
                        else {
                            --index;
                            $sc.currentImg = imgList[index].ImageUrl;
                            checkBtnState();
                        }
                    }
                }]
            });

        }

    }]);

})(angular.module('Silverzone_app'));