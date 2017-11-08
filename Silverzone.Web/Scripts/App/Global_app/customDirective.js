(function () {
    'use strict';

    angular.module('Silverzone_app')
          .directive('bindBooks', function () {

              bookList_Controllerfn.$inject = ['$scope', '$rootScope', 'cartFunction'];
              function bookList_Controllerfn($sc, $rsc, cartSvc) {


                  $sc.addtoCart = function (bookInfo) {
                      var qty = parseInt(cartSvc.find_qty_byBookId(bookInfo));

                      if (qty < 99)
                          $rsc.cart.Items = cartSvc.add_itemInCart(bookInfo, qty + 1);           // fx avilable in bookController.js
                      else
                          $rsc.notify_fx('You can add only 99 quantity of a product in your cart !', 'warning');
                  }

              }

              return {
                  restrict: 'E',
                  replace: true,        // to remove definition of directive from page
                  scope: {
                      book: '=',
                      classId: '@'
                  },
                  controller: bookList_Controllerfn,
                  templateUrl: 'templates/customDirective_template/bookList.html'    // work according to parent controller   
              }
          })

          .directive('bindBundles', function () {

              bookbundle_Controllerfn.$inject = ['$scope', '$rootScope', '$uibModal', 'cartFunction'];
              function bookbundle_Controllerfn($sc, $rsc, $modal, cartSvc) {
                  var that = this;
                  that.bookBundle_cart_count = [];
                  $sc.comboDetail = null;

                  that.addCartqty = function (isAdd_qty, id) {
                      // this = that will help us to given updated model value
                      that.bookBundle_cart_count[id] = cartSvc.addqty(that.bookBundle_cart_count[id], isAdd_qty);
                  }

                  that.check_cart = function (id) { // calling global fx from bookController.js
                      that.bookBundle_cart_count[id] = cartSvc.checkCart_Value(that.bookBundle_cart_count[id]);
                  }

                  that.intializeQty = function () {
                      if ($sc.comboDetail)
                          that.bookBundle_cart_count[$sc.comboDetail.bundleInfo.Id] = 1;
                  }

                  that.addCombo_toCart = function (bookBundleInfo, Id) {
                      var itemQty = that.bookBundle_cart_count[Id];

                      var cartItems = {
                          bookId: bookBundleInfo.Id,
                          bookImage: 'Images/book_bundle.jpg',
                          bookTitle: bookBundleInfo.Name + ' - ' + bookBundleInfo.className,
                          bookCategory: 'Bundle',
                          bookClass: bookBundleInfo.className,
                          bookTotalPrice: bookBundleInfo.bundle_totalPrice * itemQty,
                          bookQty: itemQty,
                          bookPrice: bookBundleInfo.bundle_totalPrice,
                          bookPublisher: 'Silverzone',
                          Subject: 'Books Bundle',
                          bookType: 2     // for book bundle
                      }
                      $rsc.cart.Items = cartSvc.add_update_Cart(cartItems);
                  }

                  that.viewBundle_detail = function (bookInfo) {

                      var template = ` <div class="modal-header">
                          <h4 class="box-title">Bundle Items</h4>
                          <button type="button" class="close" style="margin-top: -30px !important;">
                          <span aria-hidden="true" ng-click="cancel()">×</span></button> </div>
                          <div class ="modal-body" style="padding: 10px !important;border: 2px solid #ddd;">
                        <div>
                          <h3>Combo Contains Following Items: - </h3>
                          <div class ="row top-buffer">
                              <div ng-repeat="book in combo_bookInfo">
                                  <div class ="col-xs-12 col-sm-6 col-md-3">
                                  <div class ="col-item">
                                  <div class ="post-img-content" style="height: 238px;">
                                       <p style="position: absolute; z-index: 10; font-size: 18px; font-weight: 600; right: 18px; margin-top: 20px; width: 30px;"
                                          ng-bind="book.classId">
                                       </p>
                                       <img class ="img-responsive"
                                            ng-src="{{ book.BookImage }}" />
                                       <span class ="post-title"
                                             style="text-align: left;">
                                           <b>{{ book.subject }}</b><br>
                                       </span></div>
                                   <div class ="info">
                                       <div class ="row">
                                           <div class ="price col-md-12">
                                               <h5 class ="price-text-color">
                                                       {{ book.title }}
                                               </h5></div></div>
                                       <div class ="separator clear-left">
                                           <p class ="btn-add">
                                               <i class ="fa fa-info-circle"></i>
                                               <a ui-sref="book_details({bookId : book.BookId, bookTitle : (book.title | stringFilter) })">
                                                   Detail
                                               </a> </p>
                                           <p class ="btn-details">
                                               <i class ="fa fa-inr" aria-hidden="true"></i>
                                               <span ng-bind="::book.price">
                                               </span> </p> </div>
                                       <div class ="clearfix">
                                       </div> </div> </div> </div>
                                  <div class ="col-sm-1"
                                       ng-if="!$last"
                                       style="width: 25px !important; padding: 0px !important; margin-top: 10%;">
                                      <i class ="fa fa-plus fa-2x" style="color: burlywood;"></i>
                                  </div> </div> </div></div> </div>
                                    <div class="modal-footer">
                                    <button class="btn btn-info" ng-click="submit_data(country_type)">OK</button>  </div>   `

                      var modalInstance = $modal.open({
                          template: template,
                          size: 'lg',
                          resolve: {
                              bookInfo: function () {
                                  return bookInfo;               // send value from here to controller as dependency
                              }
                          },
                          controller: ['$scope', '$uibModalInstance', 'bookInfo', function ($sc, $modalInstance, bookInfo) {
                              $sc.combo_bookInfo = bookInfo;

                              $sc.cancel = function () {
                                  $modalInstance.dismiss();
                              }
                          }]
                      });
                  }

              }

              return {
                  restrict: 'E',
                  replace: true,        // to remove definition of directive from page
                  scope: {
                      combo: '='
                  },
                  controller: bookbundle_Controllerfn,
                  controllerAs: 'bookbundle',           // use with page > bookbundle.scope_objectName
                  templateUrl: 'templates/customDirective_template/bundleList.html'    // work according to parent controller   
              }
          })

          .directive('scrollTo', ['$location', '$anchorScroll', function ($location, $anchorScroll) {
              return function (scope, element, attrs) {       // link fx of directive
                  element.bind('click', function (event) {
                      event.stopPropagation();
                      event.preventDefault();

                      var old = $location.hash(location);

                      var location = attrs.scrollTo;
                      $location.hash(location);
                      $anchorScroll();

                      //reset to old to keep any additional routing logic from kicking in
                      $location.hash(old);
                  });
              };
          }])

        // prevent page go up on <a> tag click
          .directive('a', function () {
              return {
                  restrict: 'E',
                  link: function (scope, elem, attrs) {
                      if (attrs.ngClick || attrs.href === '' || attrs.href === '#') {
                          elem.on('click', function (e) {
                              e.preventDefault();
                          });
                      }
                  }
              };
          })

          .directive('browseResume', [function () {
              return {
                  restrict: 'E',
                  link: function (scope, elem, attrs) {
                      angular
                          .element(elem)
                          .find('input')
                          .bind("change", function (e) {
                              var files = (e.srcElement || e.target).files;
                              if (files.length > 0) {
                                  var file = files[0];

                                  var fileExt = file.name.substr(file.name.lastIndexOf('.') + 1);
                                  var arrExt = ['doc', 'docx', 'pdf'];
                                  if (arrExt.indexOf(fileExt) > -1)
                                      scope.$parent.resumeFile = file;
                                  else
                                      // scope contain current ctrl scope & scope.$root has $rootscope objects
                                      scope.$root.notify_fx('Please select file with extension ' + arrExt.toString());
                              }
                          });
                  },
                  template: ` <label class="btn oran btn-file">
                               Browse
                              <input type="file" style="display: none;">
                            </label> `
              };
          }])

          .directive('ddTextCollapse', ['$compile', function ($compile) {

              var linkfn = function (scope, element, attrs) {

                  // start collapsed
                  scope.collapsed = false;

                  // create the function to toggle the collapse
                  scope.toggle = function () {
                      scope.collapsed = !scope.collapsed;
                  };

                  // wait for changes on the text
                  attrs.$observe('ddTextCollapseText', function (text) {

                      // get the length from the attributes
                      var maxLength = scope.$eval(attrs.ddTextCollapseMaxLength);

                      if (text.length > maxLength) {
                          // split the text in two parts, the first always showing
                          var firstPart = String(text).substring(0, maxLength);
                          var secondPart = String(text).substring(maxLength, text.length);

                          // create some new html elements to hold the separate info
                          var firstSpan = $compile('<span>' + firstPart + '</span>')(scope);
                          var secondSpan = $compile('<span ng-if="collapsed">' + secondPart + '</span>')(scope);
                          var moreIndicatorSpan = $compile('<span ng-if="!collapsed">... </span>')(scope);
                          var lineBreak = $compile('<br ng-if="collapsed">')(scope);
                          var toggleButton = $compile('<span class="collapse-text-toggle" ng-click="toggle()">{{collapsed ? "less" : "more"}}</span>')(scope);

                          // remove the current contents of the element
                          // and add the new ones we created
                          element.empty();
                          element.append(firstSpan);
                          element.append(secondSpan);
                          element.append(moreIndicatorSpan);
                          element.append(lineBreak);
                          element.append(toggleButton);
                      }
                      else {
                          element.empty();
                          element.append(text);
                      }
                  });
              };

              return {
                  restrict: 'A',
                  scope: true,
                  link: linkfn,
              }
          }])


          .directive('showBundleModal', ['$uibModal', 'bookService', '$filter', function ($modal, svc, $filter) {
              var linkfn = function (scope, element, attrs) {       // link fx of directive
                  element.bind('click', function (event) {
                      svc.get_bookBundleDetail(attrs.bundleid).then(function (data) {
                          var entity = data.result[0];
                          var bookList = '';
                          var lenth = entity.bookInfo.length;

                          angular.forEach(entity.bookInfo, function (book, key) {
                              //var myUrl = '/Site/Book/Info/' + book.BookId + '/' + $filter('stringFilter')(book.title);
                              //scope.$root.imgBase

                              bookList += ' <div class="col-sm-3" style="padding: 5px;">'
                             + ' <img src="' + book.BookImage + '" class="img-responsive" style="margin: 0 auto; height:170px;">'
                             + ' <div class="text-center"> <div class="name-container"><a href="#">' + book.title + '</a></div>'
                             + ' <div class="price" style="margin: 6px 0px 6px 0px;">'
                             + ' <strong><i class="fa fa-inr"></i> </strong><span>' + book.price + '</span></div>'
                             + ' </div></div>'
                             + (key === lenth - 1 ? '' : ' <div class="col-sm-1" style="width: 25px !important; padding: 0px !important; margin-top: 10%;">'
                             + ' <i class="fa fa-plus fa-2x" style="color: burlywood;"></i></div>')
                          });

                          var template = '<div class="modal-header" style="padding: 8px !important;">                 '
                          + ' <h4 class="box-title">' + entity.bundleInfo.Name + '</h4>                               '
                          + '<button type="button" class="close" style="margin-top: -30px !important;">               '
                          + '<span aria-hidden="true" ng-click="cancel()">×</span></button> </div>                    '
                          + ' <div class="modal-body">                                                                '
                          + ' <div class="row">'
                          + ' <div class="col-sm-12"> <h4> This Combo includes following Items :-</h4> </div> </div>  '
                          + '<div class="col-sm-6 text-danger" style="font-size: larger;"><span class="text-info">'
                          + 'Total Price : </span> <strong><i class="fa fa-inr"></i> </strong>'
                          + '<span style="text-decoration: line-through;">' + entity.bundleInfo.books_totalPrice + '</span> </div>'
                          + ' <div class="col-sm-6 text-warning text-right" style="font-size: larger;">'
                          + ' <span class="text-info">Combo Price :</span> <strong><i class="fa fa-inr"></i> </strong>'
                          + ' <span>' + entity.bundleInfo.bundle_totalPrice + '</span></div> <div class="row" style="margin-top: 50px;">'
                          + bookList + '</div>'
                          + ' <div class="modal-footer">                                                              '
                          + ' <button class="btn btn-info" ng-click="ok()">OK</button>  </div>   '

                          var modalInstance = $modal.open({
                              template: template,
                              size: 'lg',
                              controller: 'modal_Controller',
                              windowClass: 'bundleModal',     // point to current ctrler
                          });

                      });
                  });
              };

              return {
                  restrict: 'A',
                  link: linkfn,
              }
          }])

        // shared modal controller > if popup has only close & cancel fxnality
         .controller('modal_Controller', ['$scope', '$uibModalInstance', function ($sc, $modalInstance) {

             $sc.ok = function () {
                 $modalInstance.close();
             }

             $sc.cancel = function () {
                 $modalInstance.dismiss();
             }
         }])

    ;

})();