(function (app) {

    var fn = function ($rsc) {
        var cartItems_array = new Array();

        //  ------- global fxs ----------
        this.add_itemInCart = function(bookInfo, itemQty) {

            // when coupon Info is not null & book_newPrice have some value then it will assign otherwise > bookInfo.Price
            var bookPrice = parseFloat(bookInfo.CouponInfo && bookInfo.CouponInfo.book_newPrice) || parseFloat(bookInfo.Price);

            var cartItems = {
                bookId: bookInfo.BookId,
                bookImage: bookInfo.BookImage,
                bookTitle: bookInfo.book_title,
                bookCategory: bookInfo.Category,
                bookClass: bookInfo.Class,
                bookTotalPrice: bookPrice * itemQty,
                bookQty: itemQty,
                bookPrice: bookPrice,
                bookPublisher: bookInfo.Publisher,
                Subject: bookInfo.Subject,
                bookType: 1     // for books
            }

            // use to show text > Added' OR 'Buy Now' option with Book list
            $rsc.buyBook_isDisable[bookInfo.BookId] = true;

            return this.add_update_Cart(cartItems);
        }

        // $rsc is rootScope which we can't pass in above self executable fx, its a services which only inject in controller
        this.add_update_Cart = function(bookInfo) {
            var item_notFound = true;

            angular.forEach(cartItems_array, function (data, key) {

                if (data.bookId === bookInfo.bookId && data.bookType === bookInfo.bookType) {   //  bookType is > type of book or bundle
                    // necessary fields will update
                    data.bookTotalPrice = bookInfo.bookTotalPrice;
                    data.bookQty = bookInfo.bookQty;

                    item_notFound = false;
                    $rsc.notify_fx('Item already exist in your cart !', 'info');
                }
            });

            if (item_notFound) {
                cartItems_array.push(bookInfo);
                $rsc.notify_fx('Item is added in your cart !', 'info');
            }

            return cartItems_array;
        }

        // to check value is integer or not > while keypress in cart quantity
        this.checkCart_Value = function(Qty) {

            // convert a string value too int > like "7f" = 7
            var item_qty = parseInt(Qty);

            // NaN become true if > Qty doesn't have any value or have a string
            return (isNaN(item_qty) || item_qty === 0 ? 1 : item_qty);
        }

        this.addqty = function (Quantity, isAdd_qty) {
            var itemQty = parseInt(Quantity);

            // true for Add Qty 
            if (isAdd_qty) {
                itemQty < 99 ? itemQty++ : 99;
            }
            else {
                itemQty > 1 ? itemQty-- : 1
            }

            return itemQty;
        }

        this.find_qty_byBookId = function(bookInfo) {
            var _returnVal = 0;

            // loop each element & if found then return value
            angular.forEach(cartItems_array, function (data, key) {
                if (data.bookId === bookInfo.BookId) {       // means element is found
                    _returnVal = data.bookQty
                    return;         // exit from this loop
                }
            });

            return _returnVal;
        }

    }

    app.service('cartFunction', ['$rootScope', fn]);

})(angular.module('Silverzone_app'));