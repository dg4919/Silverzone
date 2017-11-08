/// <reference path="../../Lib/jquery/jquery-1.10.2.js" />

(function (app) {
    'use strict';

    // global variable use to share value b/w 2 ctrler > these var r use to maintain sessions 
    var country_code = 0;
    var user_Addres_array = [];
    var orderId;

    // we can use controller with as keyword when there is ctrler inside another/multiple ctrler
    // we can use as keyword either by setting in URL config (.state) OR on UI page with ng-controller Tag
    var cartDetailsControllerfn = function ($rsc, $state, $stateParams, svc, $filter, $window, paymentSvc, modalSvc) {
        var that = this;            // this object reffers to object of current controller fx like scope      

        //   **************  START > Code to select Country poup  *********************

        that.country_type = country_code;

        // show popup to select country
        that.cart_proceedModal = function () {
            modalSvc.show_cart_proceedModal()
            .then(function (d) {
                country_code = d;
                $state.go('cart_detail');
            });
        }

        //   **************  End  *********************


        // this ctrler use on UI with as keyword
        // this points to property/method of current ctrler
        that.removeItem = function (index) {
            // at which position of array, how much element want to remove
            $rsc.buyBook_isDisable[$rsc.cart.Items[index].bookId] = false;

            $rsc.cart.Items.splice(index, 1);
            $rsc.notify_fx('Item is removed from your cart !', 'danger');
        }

        // array > contain new list in rootscope > cartItemsList
        // $watch use to track changes items into Cart List > and update values in total purchase amt
        $rsc.$watch('cart.Items', function (array) {
            //alert('add items to cart');
            var cart_Amount = 0;
            var item_qty = 0;

            angular.forEach($rsc.cart.Items, function (array, key) {
                cart_Amount += parseFloat(array.bookTotalPrice);
                item_qty += parseInt(array.bookQty);
            });

            $rsc.cart.shipping_Amount = cart_Amount;
            $rsc.cart.shipping_Charges = calculate_shipping_charges(item_qty);

            $rsc.cart.total_Amount = $rsc.cart.shipping_Amount + $rsc.cart.shipping_Charges;

        }, true);

        function calculate_shipping_charges(item_qty) {
            if (item_qty !== 0) {
                if (country_code === 1) {
                    return ($rsc.siteConfig.India_bookShiping_Charges1 + (item_qty - 1) * $rsc.siteConfig.India_bookShiping_Charges2);
                }
                else {
                    return ($rsc.siteConfig.OutsideIndia_bookShiping_Charges1 + (item_qty - 1) * $rsc.siteConfig.OutsideIndia_bookShiping_Charges2);
                }
            }
            else {
                return 0;
            }
        }

        //   **************  Cart Summary Code start from Here  *********************

        if ($stateParams.shipping_adress) {
            that.selected_shipping_Adress = $stateParams.shipping_adress;       // reserve previous value while redirects

            var model = {           // carete a model same as our view model > pass in ajax
                Shipping_addressId: $stateParams.shipping_adress.Id,
                Total_Shipping_Amount: $rsc.cart.shipping_Amount,
                Total_Shipping_Charges: $rsc.cart.shipping_Charges,
                countryId: country_code,
                bookList: getBooksInfo()           // return array
            }

            svc.insert_orderInfo(model).then(function (data) {

                if (data.result === 'success') {
                    orderId = data.orderId;
                    $rsc.cart.quizPoints = data.quizPoints;

                    // code to disable back btn
                    history.pushState(null, null, $(location).attr('href'));
                    window.addEventListener('popstate', function () {
                        history.pushState(null, null, $(location).attr('href'));
                    });
                    confirmOrder();
                }
                else
                    $rsc.notify_fx('Something went wrong.. :(', 'danger');
            });
        }

        that.addCartqty = function (isAdd_qty, index) {
            var book = $rsc.cart.Items[index];

            var qty = parseInt(book.bookQty);

            // true for Add Qty 
            if (isAdd_qty && qty < 99)
                updateCart(book, qty + 1);
            else if (!isAdd_qty && qty > 1)
                updateCart(book, qty - 1);
            else if (isAdd_qty)
                $rsc.notify_fx('You can add only 99 quantity of a product in your cart !', 'warning');
        }

        that.updateQty = function (index) {
            var book = $rsc.cart.Items[index];
            var item_qty = parseInt(book.bookQty);

            if (item_qty < 1 || isNaN(item_qty)) {
                updateCart(book, 1);
            }
            else {
                updateCart(book, item_qty);
            }
        }


        function updateCart(bookInfo, itemQty) {
            bookInfo.bookTotalPrice = bookInfo.bookPrice * itemQty;
            bookInfo.bookQty = itemQty;
        }

        // *********************   add order info to the DB > At payment method page  ******************

        if ($state.is('cart_detail') && $rsc.cart.Items.length === 0)
            $state.go('empty_cart');

        function getBooksInfo() {
            var books = [];

            angular.forEach($rsc.cart.Items, function (data, key) {
                books.push({ BookId: data.bookId, Quantity: data.bookQty, unitPrice: data.bookPrice, bookType: data.bookType });
            });

            return books;
        }

        function confirmOrder() {
            var deduction = $rsc.cart.quizPoint_isChecked ? parseInt($rsc.cart.quizPoints / 10) : null;

            svc.confirmOrder(orderId, deduction).then(function (data) {

                if (data.status === 'success') {
                    that.order_confirmData = data.result;

                    var shipingAdres = that.selected_shipping_Adress;
                    var userInfo = {
                        amount: $rsc.cart.shipping_Amount + $rsc.cart.shipping_Charges,
                        billing_address: shipingAdres.Address,
                        billing_city: shipingAdres.City,
                        billing_country: shipingAdres.Country === 1 ? 'India' : 'Outside India',
                        billing_email: shipingAdres.Email,
                        billing_name: shipingAdres.Username,
                        billing_state: shipingAdres.State,
                        billing_tel: shipingAdres.Mobile,
                        billing_zip: shipingAdres.PinCode
                    };

                    userInfo.order_id = data.result.OrderNumber;
                    var paymentModel = {
                        redirect_url: `http://${$window.location.host}/paymentSuccess?orderId=${data.result.OrderNumber}`,
                        cancel_url: `http://${$window.location.host}/paymentError?orderId=${data.result.OrderNumber}`,
                    };
                    
                    paymentSvc
                   .get_checksum(angular.extend(userInfo, paymentModel))
                   .then(function (d) {
                       $rsc.checkSum_value = d.result;
                   });
                }
                else if (data.status === 'notfound' || data.status === 'error') {      // if orderId not found
                    $state.go('shoppingError');       // page will be refresh
                    $rsc.notify_fx('Sorry, we are unable to process this requet :(', 'error');
                }
                else if (data.status === 'notmatched') {
                    $state.go('shoppingError');       // page will be refresh
                    $rsc.notify_fx('Book Prices has changed in the mean time.', 'error');
                }
            });
        }

    }

    var user_addressControllerfn = function ($sc, $rsc, svc, $filter, $state, modalSvc) {

        $sc.user_adressList = [];  // contains array

        // use to show select button for user adress list while checkOut cart
        $sc.show_selectAdressBtn = $state.is('user_profileAdress') ? false : true;

        $sc.get_all_Usershipping_Address = function () {
            svc.get_shipping_Address().then(function (data) {
                // order by records in descending
                user_Addres_array = data.result;

                var result = $filter('orderBy')(user_Addres_array, '-create_date');
                $sc.user_adressList = chunk(result, 3);

                if (!data.result.length) {
                    modalSvc.get_userAdress_modal(country_code)
                            .then(function (data) {
                                $state.go('cart_summary', { shipping_adress: data });
                            });
                }
            });
        }

        $sc.get_all_Usershipping_Address();

        $sc.show_userAdress_modal = function (modal) {
            modalSvc.get_userAdress_modal(country_code, modal)
                    .then(function () {
                        if (modal)
                            $rsc.notify_fx('Shipping adress is updated !', 'info');
                        else
                            $rsc.notify_fx('Shipping adress is saved !', 'info');

                        $sc.get_all_Usershipping_Address();
                    });
        }

        $sc.remove_adress = function (adresId) {
            svc.remove_Shipping_adress(adresId).then(function (data) {

                $sc.get_all_Usershipping_Address();
                $rsc.notify_fx('Shipping adress is removed !', 'warning');

            }, function () {
                console.log('in error');
            });
        }

    }

    // use to divide a list in specified size
    function chunk(arr, size) {
        var newArr = [];
        for (var i = 0; i < arr.length; i += size) {
            newArr.push(arr.slice(i, i + size));
        }
        return newArr;
    }


    app.controller('cartDetailsController', ['$rootScope', '$state', '$stateParams', 'cartService', '$filter', '$window', 'siteMasterService', 'modalService', cartDetailsControllerfn])
       .controller('user_addressController', ['$scope', '$rootScope', 'cartService', '$filter', '$state', 'modalService', user_addressControllerfn])
    ;

})(angular.module('Silverzone_app'));