/// <reference path="/Lib/angularjs/angular-1.5.0.js" />
// use app variable as local scope, i.e can access inside this JS only

(function (app) {
    'use strict';

    // show the book details in 2nd column on > /Book/Info
    var bookDetail_Controllerfn = function ($rsc, svc, $state, $stateParams, cartSvc) {
        var that = this;

        //alert('Selected book id is   ' + $stateParams.bookId);
        that.bookInfo = {};
        that.combo = {
            combo_status: {},
            comboNames: {},
            comboInfo: []
        };

        that.book_cart_count = 1;

        that.addCartqty = function (isAdd_qty) {
            that.book_cart_count = cartSvc.addqty(that.book_cart_count, isAdd_qty);
        }

        that.check_cart = function () {
            that.book_cart_count = cartSvc.checkCart_Value(that.book_cart_count);
        }

        svc.get_bookDetail($stateParams.bookId).then(function (data) {
            that.bookInfo = data.result.book_info;
            that.combo.combo_status = data.result.bookCombo_status;  // add a new property

            // making a new Entity for combo :)
            if (that.combo.combo_status) {
                that.combo.comboNames = data.result.comboInfo.map(function (entity) {
                    that.combo.comboInfo.push(
                        angular.extend(entity.bundleInfo, { bookInfo: entity.bookInfo })
                        );

                    // return statement must be at the end bcoz line below it won't be execute
                    return entity.bundleInfo.Name;
                });

            }

            var bookId = data.result.book_info.BookId;
            bookSuggestion(bookId);
            bookRecommend(bookId);
        });

        that.addtoCart = function (bookInfo, itemQty) {
            // use variable to initialise value of it rather than manipulation to direct $scope/$rootScope variable
            $rsc.cart.Items = cartSvc.add_itemInCart(bookInfo, itemQty);
        }

        that.suggestionList = [];
        function bookSuggestion(bookId) {

            svc.get_booksuggestion(bookId).then(function (data) {
                that.suggestionList = data.result;
            });
        }

        that.recommendList = [];
        function bookRecommend(bookId) {
            svc.getbook_recommends(bookId).then(function (data) {
                that.recommendList = data.result;
            });
        }
    }

    var search_model = {
        classId: 1,
        subjectId: '',
        cateogysId: [],
        searchBundle: false     // when user clik btn of bundle
    };

    // Book serarch panel 
    var book_search_Controllerfn = function ($sc, svc, sharedSvc, $state, $stateParams) {

        // contain array of list / a Modal
        $sc.book_categorys = [];

        // contains a single model >  initialise default values
        $sc.search_model = {
            selected_class: search_model.classId,
            selected_subject: search_model.subjectId,
            book_category_listId: search_model.cateogysId
        };

        sharedSvc.get_bookTitle_byClass().then(function (data) {
            $sc.itemTitleList = data.result;
            $sc.selectedClass = data.result[0].Class;
        });

        $sc.searchBooks = function () {

            search_model.classId = $sc.selectedClass.Id,
            search_model.subjectId = $sc.search_model.selected_subject || null,
            search_model.cateogysId = getCategoriesId($sc.search_model.book_category_listId)

            if (!$state.is('book_list'))
                $state.go('book_list');         // search_model is global var > use to serach book so no need to pass as param
            else
                $sc.$emit('searchBooks');       // send data to parent ctroler
        }
        
        // Book categories list load at runtime
        sharedSvc.get_bookCategorys()
            .then(function (data) {
                $sc.book_categorys = data.result;

                angular.forEach($sc.book_categorys, function (value, key) {
                    value.ticked = true;                // adding new property to each item from List
                });

            },
            function () {
                console.log('in error');
            });

        function getCategoriesId(book_category_listId) {
            var array = [];
            if (book_category_listId.length > 0) {
                angular.forEach(book_category_listId, function (data, key) {
                    array.push(parseInt(data.id));
                });
            }
            return array;
        }

        $sc.$watch('searchBundle', function (n, o) {
            search_model.searchBundle = n;
        });

    }

    // global variable > ajax wont call if its called whil redirect to other pages :)
    var bookList = [],
        bundleList = {
            comboNames: {},
            comboInfo: []
        };

    var book_searchResult_Controllerfn = function ($sc, svc, $stateParams) {
        $sc.bookList = bookList;
        $sc.bundleList = bundleList;

        if ($stateParams.classId) {
            search_model.classId = $stateParams.classId;
            search_model.subjectId = $stateParams.subjectId;
            search_model.searchBundle = true;
        }


        $sc.$on('searchBooks', function (event, param) {
            $sc.get_books(search_model);
        });

        $sc.get_books = function (model) {
            if (model.searchBundle)
                get_Bundles(model);
            else
                get_books(model);
        }

        $sc.get_books(search_model);

        function get_books(model) {
            $sc.bundleList = [];
            svc.searchBooks(model).then(function (d) {
                bookList = d.result;
                $sc.classId = model.classId;
                $sc.bookList = bookList;
            });
        }

        // TO DO : change in fx to call bundle list
        function get_Bundles(model) {
            $sc.bookList = [];

            svc.getbook_bundles(model)
               .then(function (data) {
                   $sc.bundleList = data.result;
               });
        }

    }

    app.controller('book_Detail_Controller', ['$rootScope', 'bookService', '$state', '$stateParams', 'cartFunction', bookDetail_Controllerfn])
        .controller('book_search_Controller', ['$scope', 'bookService', 'sharedService', '$state', '$stateParams', book_search_Controllerfn])
        .controller('book_searchResult_Controller', ['$scope', 'bookService', '$stateParams', book_searchResult_Controllerfn])

    ;

})(angular.module('Silverzone_app'));