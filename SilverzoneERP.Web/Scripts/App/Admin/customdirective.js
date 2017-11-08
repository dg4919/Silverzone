/// <reference path="C:\dg4919_tfs\silverzoneLatest\Silverzone.Web\Scripts/Lib/angularjs/angular-1.5.0.js" />

(function (app) {
    'use strict';

    angular.module('Silverzone_admin_app')
        .directive("addBookContentHtml", ['$compile', function ($compile) {       // if wee don't provide dependency as array will throw error after minification of JS

            var linker = function (scope, elem, attrs) {

                elem.bind("click", function () {

                    // just use as normal sing;le string & '\' use for a new line
                    var html = ' <div class="form-group col-sm-12" style="margin-top: 15px;"> <div class="col-sm-4"> \
                         <input type="text" placeholder="Book Content Name" class="form-control bookContent_name"> </div> \
                         <div class="col-sm-7"> \
                         <textarea placeholder="Enter Book Content Description" class="form-control bookContent_description" rows="2"></textarea>  </div> \
 	   		             <div class="col-sm-1" style="margin-top: 10px;"> \
				         <a href="#" class="btn btn-default btn-sm" ng-click="remove($event)"> Remove </a> </div> </div>'


                    angular.element(elem)
                        .parents('#book_ContentDv')
                        .append($compile(html)(scope));
                });
            }

            return {
                restrict: 'A',
                link: linker,
                controller: ['$scope', function ($sc) {
                    $sc.remove = function (elem) {

                        // use like jquery
                        angular.element(elem.currentTarget)
                            .parents('.form-group.col-sm-12')
                            .remove();
                    }
                }]
            }

        }])
        .directive("editBookContentHtml", ['$compile', function ($compile) {       // if wee don't provide dependency as array will throw error after minification of JS

            var linker = function (scope, elem, attrs) {

                var html = '';
                var contentModel = angular.copy(scope.bookContent_model);       // copy Data > so that don't change directly in main object

                angular.forEach(contentModel, function (entity, key) {
                    if (key != 0) {
                        html += ' <div class="form-group col-sm-12" style="margin-top: 15px;"> <div class="col-sm-4">'
                            + ' <input type="text" placeholder="Book Content Name" class="form-control bookContent_name" value="' + entity.Name + '"> </div>'
                            + ' <div class="col-sm-7">'
                            + ' <textarea placeholder="Enter Book Content Description" class="form-control bookContent_description" rows="2">' + entity.Description + '</textarea>  </div>'
                            + ' <div class="col-sm-1" style="margin-top: 10px;">'
                            + ' <a href="#" class="btn btn-default btn-sm" ng-click="remove($event)"> Remove </a> </div> </div>'
                    }
                });

                //angular.element(elem)
                //    .html($compile(html)(scope));

                angular.element(elem)
                    .replaceWith($compile(html)(scope));

            }

            return {
                restrict: 'E',
                scope: {
                    bookContent_model: '=model'
                },
                link: linker,
                controller: ['$scope', function ($sc) {
                    $sc.remove = function (elem) {
                        // use like jquery
                        angular.element(elem.currentTarget)
                            .parents('.form-group.col-sm-12')
                            .remove();
                    }
                }]
            }

        }])

        .directive("fileUploader", ['admin_quiz_Service', function (svc) {       // if wee don't provide dependency as array will throw error after minification of JS

            var linker = function (scope, elem, attrs, ctrl) {
                var aa = ctrl;

                // to open a file dialog on click of btn :)
                angular.element(elem)
                       .find('a')
                       .on('click', function () {
                           angular.element(this)
                                  .next()
                                  .click();
                       });

                // file upload event code
                angular.element(elem)           // find content inside the directive DIV body
                       .find('#img_dialog')
                       .on('change', function (event) {
                           var target = angular.element(this).parent();

                           // var type = parseInt(attrs.type);      // to get attribute value attached with directive
                           var files = event.target.files;

                           if (files.length > 0) {
                               svc.upload_quizImage(files).then(function (data) {
                                   //target.children().eq(0).attr('src', data.result[0]);

                                   // corrosponding model value would be chage  :)
                                   scope.quizImage = data.result[0];

                                   //var value = ctrl.$modelValue;   // if want value of ngModel > scope obj value

                                   // contain the ngModel value OR we can direct set value into it
                                   //ctrl.$setViewValue(data.result[0]);
                                   //ctrl.$render();      // save new value

                                   // if we r binding an html inside Link fx
                                   //scope.$apply(); // needed if other parts of the app need to be updated
                               });
                           }
                       });
            }

            return {
                //require: 'ngModel',
                restrict: 'A',
                scope: {
                    quizImage: '=',        // this will be added into the link > scope
                    //type: "=",       // 2 way binding will return value as integer (required)
                    //index: "@",     // 1 way binding > return index integer value as string
                },
                link: linker,
                templateUrl: 'view/quizImages.html'
            }

        }])

        // use for create email id UI like Gmail on '/Admin/User/Create/' Page
        .directive("textChecker", ['$compile', '$rootScope', function ($compile, $rsc) {       // if wee don't provide dependency as array will throw error after minification of JS

            var linker = function (scope, elem, attr) {

                var update = function (evt) {

                    //var parent_width = angular.elem(element.parent()).css('width');
                    //var width = elem.css('width');

                    //var a = angular.element(elem.parent())[0].offsetWidth;
                    //var b = elem[0].offsetWidth;

                    //var f = angular.element(elem.parent()).find('.cA');
                    //var d = angular.forEach(f, function (dt, key) {
                    //    var ac = 0;
                    //    ac += dt.offsetHeight
                    //    return ac;
                    //});

                    var text = elem.val().trim();

                    //space	=> 32, comma => 188, tab => 9
                    if ((evt.keyCode === 32 ||
                       evt.keyCode === 188 ||
                       evt.keyCode === 9) &&
                        text !== ',' &&
                        text !== '') {

                        var data_isExist = false;
                        if (scope.model.length !== 0) {
                            scope.model.map(function (value) {
                                if (value === text)
                                    data_isExist = true;
                            });
                        }

                        if (data_isExist) {
                            $rsc.notify_fx('User Name already exist, try another !', 'warning');
                        } else {

                            var html = $compile('<create-ui text="' + text + '"> </create-ui>')(scope);
                            angular.element(elem.parent())
                                .prepend(html);

                            scope.model.push(text);
                        }

                        // set empty after prepend
                        elem.val('');
                        evt.preventDefault();
                    }
                }
                elem.bind('keyup keydown keypress change', update);
            }

            return {
                restrict: 'A',
                scope: {
                    model: '='
                },
                link: linker
            }

        }])

        .directive("createUi", function () {       // if wee don't provide dependency as array will throw error after minification of JS
            return {
                replace: true,
                restrict: 'E',
                scope: {
                    text: '@'
                },
                controller: ['$scope', function ($sc) {

                    $sc.remove = function (event, value) {
                        var target = angular.element(event.currentTarget);
                        target.parents('.cA').remove();

                        // get > parent controller scope object value
                        var lst = $sc.$parent.model;

                        lst.map(function (data, key) {
                            if (data === value)
                                lst.splice(key, 1);
                        });
                    }
                }],
                template: '<div class="cA">             \
                           <span class="cB">            \
                           <div class="cC"              \
                                ng-bind="text">         \
                           </div>                       \
                           <i class="fa fa-close"       \
                              ng-click="remove($event, text)">  \
                           </i> </span> </div>          '
            }
        })

        .directive("restrict", ['$rootScope', function ($rsc) {       // if wee don't provide dependency as array will throw error after minification of JS

            var linker = function (scope, elem, attr) {

                // find all list to show/hide it on acording to user role
                var list = angular.element(elem).find('li.treeview');
                angular.forEach(list, function (elem, key) {

                    var accessDenied = true,
                        elem_attr = angular.element(elem).attr('access'),
                        Roles = new Array()     // for admin > role attribute not defined on UI
                    ;

                    if (elem_attr !== undefined)
                        Roles = elem_attr.split(",");

                    Roles.push('Admin');        // every time adding 'Admin' Role

                    angular.forEach(Roles, function (role) {
                        if ($rsc.user.currentUser.Role === role.trim())
                            accessDenied = false;
                    });

                    if (accessDenied)
                        elem.remove();

                });
            }

            return {
                restrict: 'A',
                link: linker
            }

        }])

        .directive("galleryBind", ['$compile', function ($compile) {       // if wee don't provide dependency as array will throw error after minification of JS

            return {
                restrict: 'E',
                replace: true,
                scope: {
                   galleryList: '=model',
                   isGallery: '='
                },
                templateUrl: 'Templates/Admin/views/gallery/galleryBox.html',
                controller: ['$scope', '$filter', '$rootScope', '$uibModal', 'admin_Service', function ($sc, $filter, $rsc, $modal, svc) {

                    $sc.setOrder = function (gallery, status) {
                        var _galleryId = gallery.Id;
                        var orderNumber = $sc.$parent.galleryList.max_orderNum;

                        if (status) {   // $sc.$parent use to get objects of parent controller
                            $sc.$parent.orderList.push({ galleryId: _galleryId, order: orderNumber });
                            gallery.order = orderNumber;
                            
                            $sc.$parent.galleryList.max_orderNum = orderNumber + 1;
                        }
                        else {
                            gallery.order = null;
                            var dt = $filter("filter")($sc.$parent.orderList, { galleryId: _galleryId }, true)[0];

                            $sc.$parent.orderList.splice($sc.$parent.orderList.indexOf(dt), 1);
                            $sc.$parent.galleryList.max_orderNum = orderNumber - 1;

                            angular.forEach($sc.$parent.orderList, function (val, key) {
                                if (val.order > dt.order) {
                                    val.order -= 1;

                                    // $sc.galleryList is current list scope of directive
                                    $filter("filter")($sc.galleryList, { Id: val.galleryId }, true)[0].order = val.order;
                                }
                            });
                        }
                    }

                    $sc.Edit = function (gallery) {
                        $sc.$emit('onEdit', angular.copy(gallery));
                    }

                    $sc.changeSts = function (gallery) {
                        svc.change_gallerySts(gallery.Id, !gallery.status)
                        .then(function (d) {
                            if (d.result === 'notfound')
                                $rsc.notify_fx('Oops! Gallery not found :(', 'warning');
                            else
                                $rsc.notify_fx('Gallery status changed !', 'success');

                            $sc.$parent.get_galleryList();
                        });
                    }

                    $sc.showGallery_detailModal = function (_galleryId) {
                        var Template = ` <div class="modal-header" style="padding: 10px !important;">
                  <h4 class="box-title">Gallery Detail</h4></div>
                  <div class="modal-body">
                  <div class="row"
                       style="margin: 15px;">
                            <button type="button"
                                    class="btn btn-success"
                                    style="margin-bottom: 22px;"
                                    onclick="$('#img_dialog').click();">
                                Upload Files
                           </button>
                            <input id="img_dialog" type="file" ng-files="uploadImg($files)" accept=".png, .jpg, .jpeg" style="display:none;" multiple>
                   </div>
                   <div class="row"
                        ng-if="gallery_dtlImgs.length"
                        style="height: 350px;overflow: hidden;overflow-y: scroll;padding-top: 8px;">
                   <div class="col-sm-3"
                        style="position: relative;"
                        ng-repeat="image in gallery_dtlImgs">
                          <i class="fa fa-rotate-right loading align_icon1 pull-left" ng-click="rotate_image(image)" title="rotate" ng-if="image"></i>
                          <i class="fa fa-close align_icon2 pull-right" ng-class="{ 'loading' : image}" ng-click="remove_image(image)" title="{{ !image ? 'cancel' : 'remove' }}"></i>
                          <div class="thumbnail" style="padding: 5px; overflow: visible !important;">
                          <img class="img-responsive" style="height: 130px;" ng-src="{{ image.ImageUrl + '?' + (curentDate | dateFormat: 'hhmmss' ) }}"></div></div>
                   </div>
                   <div class="modal-footer">
                        <button type="submit" class="btn btn-primary" ng-click="saveData()">Save</button>
                        <button type="button" class="btn btn-warning" ng-click="cancel()">Cancel</button>
                  </div> </div>`;

                        var modalInstance = $modal.open({
                            template: Template,
                            size: 'md',
                            resolve: {
                                galleryId: function () {
                                    return _galleryId;               // send value from here to controller as dependency
                                }
                            },
                            controller: ['$scope', '$uibModalInstance', 'galleryId', 'admin_Service', function ($sc, $modalInstance, galleryId, svc) {
                                $sc.gallery_dtlImgs = [];
                                $sc.curentDate = new Date();

                                $sc.rotate_image = function (image) {
                                    svc.rotate_gallaryImg(image.ImageUrl, image.isNew)
                                       .then(function (d) {
                                           $sc.curentDate = new Date();
                                       });
                                }

                                $sc.remove_image = function (image) {
                                    var Imgs = $sc.gallery_dtlImgs;

                                    $sc.gallery_dtlImgs = _.without(Imgs, _.findWhere(Imgs, {
                                        ImageUrl: image.ImageUrl
                                    }));

                                    $rsc.notify_fx('Image is deleted successfully !', 'warning');

                                    if (!image.isNew)
                                        svc.remove_gallaryImg(image.ImageUrl);
                                }

                                $sc.uploadImg = function (files) {
                                    svc.uploadImage(files).then(function (data) {
                                        angular.forEach(data, function (v, k) {
                                            $sc.gallery_dtlImgs.push({ ImageUrl: v, isNew: true });
                                        });
                                    });
                                }

                                $sc.saveData = function () {
                                    $sc.model = {
                                        Id: galleryId,
                                        imageGallerys: $sc.gallery_dtlImgs.map(function (v) {
                                            return ({ ImageUrls: v.ImageUrl, isNew: v.isNew });
                                        })
                                    };

                                    svc.create_galleryDetail($sc.model)
                                        .then(function (data) {
                                            $modalInstance.close();
                                            $rsc.notify_fx('Gallery Images is added', 'info');
                                        });
                                }

                                $sc.cancel = function () {
                                    $modalInstance.dismiss();
                                    console.log('cancel popup');
                                }

                                svc.get_galleryDetail(galleryId)
                                   .then(function (d) {
                                       $sc.gallery_dtlImgs = d.result;
                                   });

                            }]
                        });

                    }

                }]
            }

        }])

        .directive("dateFilter", function() {

            return {
                restrict: 'E',
                scope: {
                    model: '='
                },
                template: `<div class="col-md-3">
                            <label style="margin-left: -170px !important;">From Date</label>
                            <div class="input-group date" id="myDatepicker" data-provide="datepicker">
                                <input type="text" class ="form-control ng-pristine ng-valid ng-not-empty ng-touched" ng-model="model.dateFrom">
                                <span class="input-group-addon">
                                    <span class="fa fa-calendar"></span>
                            </span></div></div>
                           <div class="col-md-3">
                            <label style="margin-left: -170px !important;">To Date</label>
                            <div class="input-group date" id="myDatepicker" data-provide="datepicker">
                                <input type="text" class ="form-control ng-pristine ng-valid ng-not-empty ng-touched" ng-model="model.dateTo">
                                <span class="input-group-addon">
                                    <span class="fa fa-calendar"></span>
                           </span></div></div>`,
                controller: ['$scope', '$filter', function ($sc, $filter) {
                    $sc.model.dateFrom = $filter('dateFormat')(new Date(), 'MM/DD/YYYY');
                    $sc.model.dateTo = $filter('dateFormat')(new Date(), 'MM/DD/YYYY');
                }]
            }
        })

    ;

})();