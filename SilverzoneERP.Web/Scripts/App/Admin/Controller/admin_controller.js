(function () {

    // use to create Sub user by Admin to provide role
    var user_createfn = function ($sc, $rsc, svc) {
        $sc.user = {
            userName: []
        };

        $sc.roleList = [
            //{ Id: 2, Name: 'Admin' },
            { Id: 3, Name: 'Dispatch' },
            { Id: 4, Name: 'Support' }
        ];

        $sc.validationOptions = {
            rules: {
                user_password: {
                    required: true
                },
                user_role: {
                    required: true
                }
            },
            messages: {
                user_password: {            // use with name attribute in control
                    required: "User Password is required !",
                },
                user_role: {            // use with name attribute in control
                    required: "Select User Role !",
                }
            }
        }

        $sc.submit_data = function (form) {

            if (form.validate()) {

                if ($sc.user.userName.length === 0) {
                    $rsc.notify_fx('Enter atleast one user name to continue !', 'error');
                    return;
                }

                svc.create_user($sc.user).then(function (data) {
                    if (data.result === 'ok') {
                        $rsc.notify_fx('User is created successfully !', 'success');
                    }
                    else if (data.result === 'invalid_Role') {
                        $rsc.notify_fx('Role is not currently activated, Try Again :(', 'warning');
                    }
                    else if (data.result === 'exist') {
                        $rsc.notify_fx(data.msg, 'warning');
                    }
                });
            }
        }

    }

    var meta_listFn = function ($sc, $rsc, $modal, svc) {
        $sc.metaTagsList = [];

        function load_metaTags() {
            svc.get_metaTags()
                .then(function (d) {
                    $sc.metaTagsList = d.result;
                });
        }
        load_metaTags();

        $sc.show_metaTagModal = function () {
            showModal().then(function () {
                load_metaTags();
            });
        }

        $sc.Show_editModal = function (metaInfo) {
            showModal(metaInfo).then(function () {
                load_metaTags();
            });
        }

        $sc.changeCallback = function (metaInfo) {
            svc.delete_metaTag(metaInfo.Id, metaInfo.Status)
                .then(function (d) {
                    if (d.result === 'notfound')
                        $rsc.notify_fx('Meta Tag does not exist..', 'danger');
                    else
                        $rsc.notify_fx('Meta tag status changed !', 'success');
                });
        }

        function showModal(_model) {
            var Template = ` <div class="modal-header" style="padding: 10px !important;">
                  <h4 class="box-title">Meta Tag</h4></div>
                  <div class="modal-body">
                  <form class="form-horizontal" name="metaForm" ng-submit="ok(metaForm)" ng-validate="validationOptions" novalidate="novalidate">
                    <div class="form-group">
                        <div class="col-sm-12" style="margin: 15px 0px;"><input type="text" class="form-control" placeholder="Site url to Show Meta Tag.." ng-model="meta.Link" name="link"> </div>
                        <div class="col-sm-12" style="margin: 15px 0px;"><input type="text" class="form-control" placeholder="Meta Title" ng-model="meta.Title" name="title"></div>
                        <div class="col-sm-12" style="margin: 15px 0px;"><textarea rows="3" class="form-control" placeholder="Meta Description" ng-model="meta.Description" name="description"></textarea></div>
                        <div class="col-sm-12" style="margin: 15px 0px;"><textarea rows="3" class="form-control" placeholder="Meta Keyword" ng-model="meta.Keyword" name="keyword"></textarea></div> </div>
                    <div class="modal-footer">
                        <button type="submit" class="btn btn-primary">Save</button>
                        <button type="button" class="btn btn-warning" ng-click="cancel()">Cancel</button>
                 </div> </form>`;

            var modalInstance = $modal.open({
                template: Template,
                size: 'md',
                resolve: {
                    metaInfo: function () {
                        return _model;               // send value from here to controller as dependency
                    }
                },
                controller: ['$scope', '$uibModalInstance', 'metaInfo', 'admin_Service', function ($sc, $modalInstance, metaInfo, svc) {
                    $sc.meta = metaInfo || {};

                    $sc.ok = function (form) {
                        if (form.validate()) {
                            svc.save_metaTag($sc.meta)
                                .then(function (d) {
                                    if (d.result === 'exist')
                                        $rsc.notify_fx('Meta Tag already exist for given url..', 'warning');
                                    else if (d.result === 'notfound')
                                        $rsc.notify_fx('Meta Tag does not exist..', 'danger');
                                    else {
                                        $rsc.notify_fx('Meta Tag succesfully created !', 'info');
                                        $modalInstance.close();
                                    }
                                });
                        }
                    }

                    $sc.cancel = function () {
                        $modalInstance.dismiss();
                        console.log('cancel popup');
                    }

                    $sc.validationOptions = {
                        rules: {
                            link: {
                                required: true
                            },
                            title: {
                                required: true
                            },
                            description: {
                                required: true
                            },
                            keyword: {
                                required: true
                            }
                        }
                    };

                }]
            });

            return modalInstance.result;
        }


    }

    var galleryFn = function ($sc, $rsc, svc) {

        $sc.years = [2012, 2013, 2014, 2015, 2016, 2017];
        $sc.galleryList = [];

        $sc.gallery_model = {
            //Description: 'Olympiad Winner 2017',
            //categoryType: 1,
            //year: 2017,
            //ImageUrl: null
        };

        svc.get_galleryCategory()
         .then(function (d) {
             $sc.categorys = d.result;
         });

        $sc.get_galleryList = function () {
            svc.get_gallerys()
               .then(function (d) {
                   $sc.galleryList = d.result;
               });
        }
        $sc.get_galleryList();

        $sc.uploadImage = function (files) {
            if (files.length > 0) {
                svc.uploadImage(files).then(function (data) {
                    $sc.gallery_model.ImageUrl = data[0];
                },
                function (e) {
                    console.log('in fail.. ' + e);
                });
            }
        }

        $sc.save_galleryInfo = function () {
            if (!$sc.gallery_model.ImageUrl) {
                $rsc.notify_fx('Please Select an Image !', 'warning');
                return;
            }

            svc.create_gallery($sc.gallery_model)
                .then(function (d) {
                    $rsc.notify_fx('Gallery created successfully !', 'info');
                    $sc.gallery_model = {};
                    $sc.gallery_model.ImageUrl = '';

                    $sc.get_galleryList();
                });
        }

        $sc.$on('onEdit', function (e, gallery) {
            $sc.gallery_model = gallery;
        });

        $sc.orderList = [];
        $sc.saveGallery_Order = function () {
            svc.setGallery_Order($sc.orderList)
               .then(function (d) {
                   if (d.result === 'ok') {
                       $rsc.notify_fx('Gallery order has been set !', 'info');
                       $sc.get_galleryList();
                   }
               });
        }

    }

    var siteConfigFn = function ($sc, $rsc, svc) {

        $sc.model = {};

        function load_siteConfig() {
            svc.get_siteConfiguration()
            .then(function (d) {
                $sc.model = d.result;
            });
        }
        load_siteConfig();

        $sc.submit_data = function (form) {
            if (form.validate()) {
                svc.save_siteConfiguration($sc.model)
                .then(function (d) {
                    $rsc.notify_fx('Configuration has been saved :)', 'info');
                    load_siteConfig();
                });
            }
        }

        $sc.validationOptions = {
            rules: {
                India_bookShiping_Charges1: {
                    required: true
                },
                India_bookShiping_Charges2: {
                    required: true
                },
                OutsideIndia_bookShiping_Charges1: {
                    required: true
                },
                OutsideIndia_bookShiping_Charges2: {
                    required: true
                },
                InstantDnd_Price: {
                    required: true
                }
            }
        };
    }

    var newsUpdatesFn = function ($sc, $rsc, svc) {
        $sc.newsModel = {};
        $sc.newsList = [];

        function getNews_updates() {
            svc.get_newsUpdates()
               .then(function (d) {
                   $sc.newsList = d.result;
               });
        }
        getNews_updates();

        $sc.submit_data = function (form) {
            if (form.validate()) {
                svc.save_newsUpdates($sc.newsModel)
                   .then(function (d) {
                       if ($sc.newsModel.Id)
                           $rsc.notify_fx('News has been updated :)', 'warning');
                       else
                           $rsc.notify_fx('News has been created :)', 'info');

                       $sc.newsModel = {};
                       getNews_updates();
                   });
            }
        }

        $sc.changeCallback = function (Id) {
            svc.delete_newsUpdates(Id)
               .then(function () {
                   $rsc.notify_fx('News status is changed !', 'success');
                   getNews_updates();
               });
        }

        $sc.editNews = function (newsInfo) {
            $sc.newsModel = angular.copy(newsInfo);
        }

        $sc.validationOptions = {
            rules: {
                title: {
                    required: true
                },
                news_url: {
                    required: true
                },
                page_url: {
                    required: true
                }
            }
        };

    }

    var instantDndFn = function ($sc, $rsc, svc) {
        $sc.json = {};
        $sc.model = {};
        $sc.isEdit = true;
        $rsc.HeaderAction = "Templates/School/Answer_sheetRecieving/Header.html";
        $sc.formTitlle = "Instant Download";

        svc.get_InstantDnd_SubjectsMapping_json()
        .then(function (d) {
            $sc.json = d.result;
        });

        $sc.checkItem = function (clasItem, status, subjectId) {
            $sc;
            if (status)
                clasItem.subjectIds.push(subjectId);
            else {
                var index = clasItem.subjectIds.indexOf(subjectId);
                clasItem.subjectIds.splice(index, 1);
            }
        }

        $sc.submit_data = function () {
            if (!$sc.model.yearId) {
                $rsc.notify_fx('Please select a year !', 'info');
                return;
            }

            angular.extend($sc.model, { mappingInfo: $sc.json.classList });

            svc.save_InstantDnd_mapping($sc.model)
            .then(function (d) {
                if (d.result === 'ok')
                    $rsc.notify_fx('Data is saved..', 'info');
                else
                    $rsc.notify_fx('Data already exist for selected year !', 'danger');
            });
        }

    }

    var carrierFn = function ($sc, $rsc, svc) {
        $sc.model = {};
        $sc.carrierList = [];
        $sc.formTitlle = "Carrier Master";

        $sc.isEdit = false;
        $rsc.HeaderAction = "Templates/School/Answer_sheetRecieving/Header.html";

        $sc.Back = function () {
            $sc.isEdit = false;
        }

        $sc.Add = function () {
            $sc.isEdit = true;
        }

        function get_carrierList() {
            svc.get_carrierList()
               .then(function (d) {
                   $sc.carrierList = d.result;
               });
        }
        get_carrierList();

        $sc.submit_data = function (form) {
            if (form.validate()) {
                saveData($sc.model);
            }
        }

        $sc.editData = function (carrier) {
            $sc.model = carrier;
            $sc.isEdit = true;
        }

        function saveData(_model) {
            svc.save_carrier(_model)
                   .then(function (d) {
                       if ($sc.model.Id)
                           $rsc.notify_fx('Data has been updated :)', 'warning');
                       else
                           $rsc.notify_fx('Data has been created :)', 'info');

                       $sc.model = {};
                       get_carrierList();
                   });
        }

        $sc.validationOptions = {
            rules: {
                title: {
                    required: true
                },
                vacancies: {
                    required: true
                },
                qualification: {
                    required: true
                },
                skills: {
                    required: true
                },
                experience: {
                    required: true
                }
            }
        };

        $sc.changeCallback = function (carrier) {
            angular.extend(carrier, { isDelete: true });
            saveData(carrier);
        }

    }

    var Schedule_olympiadFn = function ($sc, $rsc, svc) {
        $sc.SOPList = [];
        $sc.SOP_model = {};     // SOP => alias of Silverzone Olympiad

        $sc.formTitlle = "Schedule Olympiad";
        $sc.isEdit = false;
        $rsc.HeaderAction = "Templates/School/Answer_sheetRecieving/Header.html";

        $sc.get_SOFList = function () {
            svc.get_Schedule_olympads()
               .then(function (d) {
                   $sc.SOPList = d.result;
               });
        }
        $sc.get_SOFList();


        $sc.Back = function () {
            $sc.isEdit = false;
        }

        $sc.Add = function () {
            $sc.isEdit = true;
        }

        $sc.uploadImage = function (files) {
            if (files.length > 0) {
                svc.upload_Schedule_olympadsImg(files).then(function (data) {
                    $sc.SOP_model.ImageName = data[0];
                });
            }
        }

        $sc.save_SOFdata = function (form) {
            if(form.validate()){
                if (!$sc.SOP_model.ImageName) {
                    $rsc.notify_fx('Please Select an Image !', 'warning');
                    return;
                }
                saveData($sc.SOP_model);
            }
        }

        $sc.editData = function (SOP) {
            $sc.SOP_model = SOP;
            $sc.isEdit = true;
        }

        $sc.changeCallback = function (SOP) {
            saveData(SOP);
        }

        function saveData(_model) {
            svc.save_Schedule_olympadsImg(_model)
                    .then(function (d) {
                        if ($sc.SOP_model.Id)
                            $rsc.notify_fx('Schedule Olympiad updated successfully :)', 'warning');
                        else
                            $rsc.notify_fx('Schedule Olympiad created successfully :)', 'warning');

                        $sc.SOP_model = {};
                        $sc.SOP_model.ImageName = '';

                        $sc.get_SOFList();
                    });
        }

        $sc.validationOptions = {
            rules: {
                Caption: {
                    required: true
                },
                Link: {
                    required: true
                },
            }
        }

    }


    angular.module('Silverzone_admin_app')
        .controller('user_create', ['$scope', '$rootScope', 'admin_Service', user_createfn])
        .controller('meta_list', ['$scope', '$rootScope', '$uibModal', 'admin_Service', meta_listFn])
        .controller('gallery', ['$scope', '$rootScope', 'admin_Service', galleryFn])
        .controller('newsUpdates', ['$scope', '$rootScope', 'admin_Service', newsUpdatesFn])
        .controller('siteConfig', ['$scope', '$rootScope', 'admin_Service', siteConfigFn])
        .controller('instantDnd', ['$scope', '$rootScope', 'admin_Service', instantDndFn])
        .controller('carrier', ['$scope', '$rootScope', 'admin_Service', carrierFn])
        .controller('Schedule_olympiad', ['$scope', '$rootScope', 'admin_Service', Schedule_olympiadFn])

    ;

})();