
(function () {

    function getCategoriesId(book_category_listId) {
        var array = [];
        if (book_category_listId.length > 0) {
            angular.forEach(book_category_listId, function (data, key) {
                array.push(parseInt(data.id));
            });
        }
        return array;
    }

    var book_createfn = function ($sc, $rsc, svc, sharedSvc, $modal, $stateParams, $state, $timeout, $filter) {

        // form will not disable if user edit a record of book list
        $sc.disableForm = $stateParams.bookId !== undefined ? false : true;
        $sc.disableSelection = $stateParams.bookId !== undefined ? true : false;

        $sc.bookModel = {
            bookInfo: {
                publisher: 'Silverzone'
            },
            bookDetail: {},
            bookContent: []
        };
        $sc.book_isCreating = true;

        $sc.book_publisherList = ["Silverzone", "3gen"];

        var loadBook_titleModel = function () {
            sharedSvc.get_bookTitle().then(function (data) {
                $sc.itemTitleList = data.result;
            });
        }();
        //loadBook_titleModel();

        $sc.resetType = function () {
            if ($sc.book_isCreating)
                $sc.bookModel.bookInfo.itemTitle_Id = '';
        }

        $sc.uploadImage = function (element) {
            console.log("I hav Changed");

            if (element.files.length > 0) {
                svc.upload_bookImage(element.files).then(function (data) {
                    //console.log('file saved sucesfully..   ' + data.result);

                    $sc.bookModel.bookInfo.bookImage = data.result[0];
                },
                    function (e) {
                        console.log('in fail.. ' + e);
                    });
            }
        }

        $sc.check_isBookexist = function (titleId) {
            $sc.disableForm = true;

            svc.book_isExist(titleId).then(function (data) {
                if (data.result === 'exist')
                    show_modal(data.entity);
                else
                    $sc.disableForm = false;
            });

        }

        function show_modal(_model) {
            var template = ' <div class="modal-header">                                                       '
                + ' <h4 class="box-title">Book Exist</h4>                                                  '
                + ' <button type="button" class="close" style="margin-top: -30px !important;">             '
                + ' <span aria-hidden="true" ng-click="cancel()">×</span></button> </div>                  '
                + ' <div class="modal-body" style="padding: 30px !important;">                             '
                + ' <div class="row"> <h4> Books already Exist, Do you want to Edit ? </h4> </div> </div>  '
                + ' <div class="modal-footer">                                                             '
                + ' <div class="col-md-3">                                                                 '
                + ' <button class="btn btn-info pull-left" ng-click="ok()">OK</button></div>               '
                + ' <button class="btn btn-warning" ng-click="cancel(country_type)">Cancel</button> </div> '

            var modalInstance = $modal.open({
                template: template,
                controller: 'book_editModal',
                size: 'md',
                resolve: {
                    bookModal: function () {
                        return _model;
                    }
                }
            });

            modalInstance.result.then(function (entity) {     // like success fx
                $sc.bookModel = entity;
                $sc.disableForm = false;
                $sc.disableSelection = true;
            }, function () {            // like error fx
                console.log('modal-component dismissed at: ' + new Date());
            });
        }

        $sc.submitForm = function (form) {
            //alert('page is submitted.. !');
            var entity = angular.copy($sc.bookModel);       // changes in entity will not reflect to scope

            entity.bookContent = getBook_contentModel();

            var book_Id = entity.bookInfo.bookId;

            if (form.validate()) {
                if (book_Id === undefined || book_Id === '') {
                    svc.createBook(entity).then(function (d) {
                        //alert('book is created !')                            // create Book
                        if (d.result === 'exist')
                            $rsc.notify_fx('Book exist in selected category, try another !', 'error');
                        else {
                            $rsc.notify_fx('Book is created !', 'success');

                            $timeout(function () {
                                $state.go('book_create', {}, { reload: true });    // 2nd use if want to send params
                            }, 2000);
                        }
                    });
                } else {
                    svc.updateBook(entity).then(function (data) {        // update Book
                        $rsc.notify_fx('Book is updated !', 'success');

                        $timeout(function () {
                            $state.go('book_list');
                        }, 2000);
                    });
                }
            }
        }

        function getBook_contentModel() {
            var arr = new Array();

            var nameList = angular.element('#book_ContentDv')
                .find('.bookContent_name');              // find all record by class Name

            var description_List = angular.element('#book_ContentDv')
                .find('.bookContent_description');

            angular.forEach(nameList, function (elem, key) {
                var name = angular.element(elem).val();         // got element
                var description = angular.element(description_List[key]).val();           // value fetch from same cnt :)

                arr.push({ Name: name, Description: description });
            });

            return arr;
        }

        $sc.validationOptions = {
            rules: {
                bookTitle: {            // use with name attribute in control
                    required: true
                },
                bookISBN: {
                    required: true
                },
                publisher: {            // use with name attribute in control
                    required: true
                },
                bookPage: {            // use with name attribute in control
                    required: true
                },
                bookWheight: {            // use with name attribute in control
                    required: true
                },
                bookPrice: {            // use with name attribute in control
                    required: true
                },
                bookDescription: {            // use with name attribute in control
                    required: true
                },
                reorderLevel: {            // use with name attribute in control
                    required: true,
                    min: 1
                }
            }
        }


        // ************* Load Book to edit :) ******************

        if ($stateParams.bookId !== undefined) {
            svc.get_bookInfo($stateParams.bookId).then(function (d) {
                $sc.bookModel = d.result;
                $sc.book_isCreating = false;

                $timeout(function () {
                    $sc.subjectModel = $filter('filter')($sc.itemTitleList, function (entity) {
                        return entity.subject.Id === d.titleModel.SubjectId;
                    })[0].subject.Class;

                    $sc.categoryModel = $filter('filter')($sc.subjectModel, { Id: d.titleModel.ClassId })[0].category;
                }, 1000);       // it ensure that data has been come inside > $sc.itemTitleList

            });
        }


    }

    var book_editModalfn = function ($sc, $modalInstance, bookModal) {
        $sc.ok = function () {
            // close use to preform an operation or Share Data > go to success of modalInstance.result.then
            $modalInstance.close(bookModal);
        }

        $sc.cancel = function () {
            $modalInstance.dismiss('cancel');
        }
    }

    var book_listfn = function ($sc, $rsc, svc, sharedSvc, $modal, modalService) {
        $sc.bookList = [];
        $sc.book_searchModel = {};

        // contain array of list / a Modal
        $sc.book_categorys = [];
        
        sharedSvc.getBook_itemTitle().then(function (data) {
                $sc.itemTitleList = data.result;
            });

        // Book categories list load at runtime
        sharedSvc.get_bookCategorys().then(function (data) {
            $sc.book_categorys = data.result;
        });

        $sc.searchBook = function () {
            $sc.book_searchModel.subjectId = $sc.subjectModel.Id;
            $sc.book_searchModel.bookCategoryId = getCategoriesId($sc.book_searchModel.bookCategoryId);

            svc.get_books($sc.book_searchModel).then(function (data) {
                $sc.bookList = data.result;
            });
        }

        $sc.Show_deleteModal = function (id) {
            var bookId = id;    // to preserve value 

            modalService.deleteModal('Book').then(function () {
                //on ok button press 
                //console.log('perform Action ' + bookId);

                svc.deleteBook(bookId).then(function (data) {
                    $rsc.notify_fx('Book is deleted !', 'danger');
                    $sc.searchBook();
                });

            }, function () {
                //on cancel button press >  means dissmiss
                console.log("Modal Closed");
            });
        }

        $sc.changeCallback = function (entity) {
            svc.setBook_outOfStock(entity.bookId, entity.inStock).then(function (data) {
                $rsc.notify_fx('Status is changed !', 'info');
            });
        }

    }

    //************************   Book Discount Bundle  *******************************************

    var bundle_createfn = function ($sc, $rsc, svc, sharedSvc, $stateParams, $state, $timeout) {
        var that = this;

        that.book_bundle = {
            booksId: [],
            totalPrice: 0
        };
        that.Isbook_selected = [];

        that.callfun = function (book, isChecked) {
            var selectedBook = that.book_bundle.booksId;
            var totalAmt = that.book_bundle.totalPrice;

            if (isChecked) {
                selectedBook.push(book.bookId);
                totalAmt += book.bookPrice;
            }
            else {
                var index = selectedBook.indexOf(book.bookId);
                selectedBook.splice(index, 1);
                totalAmt -= book.bookPrice;
            }

            that.book_bundle.booksId = selectedBook;
            that.book_bundle.totalPrice = totalAmt;
        }

        that.submit_data = function (form) {
            if (form.validate()) {

                if (that.book_bundle.booksId != undefined && that.book_bundle.booksId.length >= 2) {

                    var bundleInfo = angular.copy(that.book_bundle);                    

                    var bundle_id = bundleInfo.BundleId;

                    if (bundle_id === undefined || bundle_id === '') {   // create bundle
                        svc.createBundle(bundleInfo).then(function (data) {
                            $rsc.notify_fx('Discount bundle created succesfully !', 'success');

                            $timeout(function () {
                                $state.go('bundle_list');
                            }, 2000);
                        });
                    }
                    else {           // udpate bundle
                        svc.updateBundle(bundleInfo).then(function (data) {
                            $rsc.notify_fx('Discount bundle created succesfully !', 'success');

                            $timeout(function () {
                                $state.go('bundle_list');
                            }, 2000);
                        });
                    }
                }
                else
                    $rsc.notify_fx('Please select atleast 2 books !', 'error');
            }
        }

        that.validationOptions = {
            rules: {
                bundle_name: {            // use with name attribute in control
                    required: true
                },
                bundle_description: {
                    required: true
                },
                bundle_price: {            // use with name attribute in control
                    required: true
                }
            }
        }

        // ************* Load Bundles to edit :) ******************
        that.is_Edit = false;

        if ($stateParams.bundleId !== undefined) {
            svc.get_Bundle_byId($stateParams.bundleId).then(function (d) {
                that.book_bundle = d.result.bundleInfo;

                that.bookInfo = d.result.bookInfo;

                // to pass value to child ctrolers
                $sc.$broadcast('onGet_Class', that.book_bundle.ClassId);


                // to show by default checked to book list
                that.book_bundle.booksId = that.bookInfo.map(function (book) {

                    // bcoz we r setting array index value by our own >
                    // So if we assing Isbook_selected[10] = "xyz" then 0 to 9 index value would be NULL
                    that.Isbook_selected[book.bookId] = true;
                    return book.bookId;         // return array after loop through items
                });

                that.is_Edit = true;
            });
        }

        that.searchBtn_isCliked = false;
        that.changeStatus = function () {
            that.searchBtn_isCliked = true;
        }
    }

    var bundle_listfn = function ($sc, $rsc, svc) {
        $sc.bundles_List = {};

        svc.get_Bundles().then(function (data) {
            $sc.bundles_List = data.result;
        });

        $sc.changeCallback = function (entity) {
            svc.deleteBundle(entity).then(function (data) {
                $rsc.notify_fx('Status is changed !', 'info');
            });
        }

    }

    var bundle_bookSearchfn = function ($sc, svc, sharedSvc) {

        $sc.bookList = [];
        $sc.book_searchModel = {};

        $sc.classList = [];
        $sc.subjectList = [];
        $sc.book_categorys = [];


        // Book categories list load at runtime
        sharedSvc.get_bookCategorys().then(function (data) {
            $sc.book_categorys = data.result;
        });

        sharedSvc.get_bookTitle_byClass().then(function (data) {
            $sc.itemTitleList = data.result;
            $sc.selectedClass = data.result[0].Class;
        });

        $sc.$watch('selectedClass', function (newVal, oldVal) {
            if (newVal)
                $sc.$parent.bundleInfo.book_bundle.ClassId = newVal.Id;
            });

        $sc.$on('onGet_Class', function ($event, classId) {
            $sc.selectedClass = _.find($sc.itemTitleList, function (d) {
                if (d.Class.Id === classId)
                    return d;
            }).Class;
        });


        $sc.searchBook = function () {
            $sc.book_searchModel.bookCategoryId = getCategoriesId($sc.book_searchModel.bookCategoryId);
            $sc.book_searchModel.classId = $sc.selectedClass.Id;

            svc.get_books($sc.book_searchModel).then(function (data) {
                $sc.bookList = data.result;
            });
        }

    }


    angular.module('Silverzone_admin_app')
        .controller('book_create', ['$scope', '$rootScope', 'admin_book_Service', 'sharedService', '$uibModal', '$stateParams', '$state', '$timeout', '$filter', book_createfn])
        .controller('book_editModal', ['$scope', '$uibModalInstance', 'bookModal', book_editModalfn])
        .controller('book_list', ['$scope', '$rootScope', 'admin_book_Service', 'sharedService', '$uibModal', 'admin_modalService', book_listfn])
        .controller('bundle_create', ['$scope', '$rootScope', 'admin_book_Service', 'sharedService', '$stateParams', '$state', '$timeout', bundle_createfn])
        .controller('bundle_list', ['$scope', '$rootScope', 'admin_book_Service', bundle_listfn])
        .controller('bundle_bookSearch', ['$scope', 'admin_book_Service', 'sharedService', bundle_bookSearchfn])

        ;

})();

