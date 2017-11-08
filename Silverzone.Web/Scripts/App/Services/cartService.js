
(function (app) {

    var fn = function (globalConfig, $filter, httpSvc) {

        var apiUrl = globalConfig.apiEndPoint + globalConfig.version.Site,
            fac = {};


        //***************** User Shipping Address  **********************

        // we will not pass UserId here, it will be done at server side code
        fac.get_shipping_Address = function () {
            return httpSvc
                   .get(apiUrl + '/Order/get_shipping_Address_byUserId');
        }

        fac.save_sippingAdress = function (_model) {
            return httpSvc.post(
              apiUrl + '/Order/save_shipping_Address',
                _model);
        }

        fac.remove_Shipping_adress = function (Id) {
            return httpSvc.post(
              apiUrl + '/Order/remove_uesr_shipping_Address',
              null,
              { adresId: Id });
        }

        fac.get_location = function (pincode) {
            return httpSvc.get(
             apiUrl + '/Order/get_location',
             { pincode: pincode })
            .then(function (res) {  // when response would be recievce, (then is a sucess fx call when data would recived)

                // parseJSON convert json string to json object, so that can access data by its property
                var data = jQuery.parseJSON(res),   // res contain json string in string format
                userLocation = {};

                if (data.status === "OK") {

                    var list = data.results[0].address_components;

                    for (var i = 0; i < list.length; i++) {

                        angular.forEach(list[i].types, function (data, key) {
                            if (data === "locality" || data === "administrative_area_level_2")
                                userLocation.city = list[i].long_name;
                            else if (data === "administrative_area_level_1")
                                userLocation.state = list[i].long_name;
                        });
                    }
                }
                return userLocation;
            });
        }

        //******************  for Orders  ********************

        fac.insert_orderInfo = function (_model) {
            return httpSvc.post(
             apiUrl + '/Order/create_order',
              _model);
        }

        fac.confirmOrder = function (Id, deduction) {
            var params = {
                oderId: Id,
                Quiz_Points_Deduction: deduction
            };

            return httpSvc.get(
               apiUrl + '/Order/confirm_order',
               params);
        }

        fac.get_orerInfo = function (_orderNumber) {
            return httpSvc.get(
             apiUrl + '/Order/get_orerInfo',
             { orderNumber: _orderNumber });
        }

        fac.print_ConfirmOrder = function (model) {
            return httpSvc
                   .get('templates/EmailTemplates/orderLabel.html')
                   .then(function (data) {
                       var htmlResult = bind_Print_ConfirmOrder_Html(data, model);

                       var mywindow = window.open('', 'my div', 'height=800,width=1400');
                       mywindow.document.write(htmlResult);

                       setTimeout(function () {
                           mywindow.document.close();

                           mywindow.focus();
                           mywindow.print();
                           mywindow.close();
                       }, 200);

                   });
        }

        function bind_Print_ConfirmOrder_Html(myHtml, model) {

            var html = $("<div>" + myHtml + "</div>"),
                itemList_html = '',
                userAdress = model.shipingAdres,
                userFull_adres = userAdress.Address + '-' + userAdress.PinCode + ', ' + userAdress.City + '<br />' + userAdress.State;

            $(html).find('#AdressInfo').html('<h4 style="margin: 0px !important;">' + userAdress.Username + '</h4>                                                                                                     '
                                + ' <p style="margin-bottom: 10px;" class="ng-binding">' + userFull_adres + '                                                                                         '
                                + ' <p style="color: #565656;margin: 0px !important;border-top: #000 solid 1px;">                            '
                                + ' <i class="fa fa-envelope"></i><span style="margin-left: 5px;">' + userAdress.Email + '</span> <br> '
                                + ' <i class="fa fa-phone"></i><span style="margin-left: 5px;"><strong>' + userAdress.Mobile + '</strong></span> </p>                                   ');

            $(html).find('#packetDetail').html('<p> Order ID - <b>' + model.OrderNumber + '</b> </p>'
                                           + '<p>Order Date - <b>' + $filter('dateFormat')(model.OrderDate, 'DD/MM/YYYY hh:mm a') + '</b> </p>');

            angular.forEach(model.books, function (item, key) {
                itemList_html += '<tr> <td>' + (key + 1) + '</td>'
                               + '<td> ' + item.subject + ' </td>      '
                               + '<td> ' + item.className + ' </td>    '
                               + '<td> ' + item.bookCategory + ' </td> '
                               + '<td> ' + item.bookQuantity + ' </td> '
                               + '<td> ' + item.bookPrice + '  </td>   '
                               + '<td> ₹ ' + parseFloat(item.bookPrice) * parseFloat(item.bookQuantity) + ' </td> </tr>'

                if (item.bookType === 2) {          // for bundle get name of books inside bundle
                    var chars = ['A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J'];

                    angular.forEach(item.bundle_booksInfo, function (item, key) {
                        itemList_html += '<tr> <td>' + $filter('lowercase')(chars[key]) + '. </td> '
                                       + '<td> ' + item.SubjectName + ' </td>  <td></td>'
                                       + '<td> ' + item.CategoryName + ' </td>  '
                    });
                }
            });

            var cntry = model.shipingAdres.Country;
            var shipcharge = '<td colspan="6" style="text-align: right;"><strong>Shipping Charges</strong> <div> <strong style="font-size: 75% !important;">'
                            + (model.First_Shipping_Charge === 0 ? '' : '(for first unit @' + model.First_Shipping_Charge + ' and rest @' + model.Other_Shipping_Charge + ' each)') + '</strong></div> </td>'

            var totl_amt = parseFloat(model.Total_Shipping_Amount) + parseFloat(model.Total_Shipping_Charges);
            itemList_html += '<tr> '
                            + '<td colspan="6" style="text-align: right;"><strong>Shipping Amount</strong></td>'
                            + '<td> ₹ ' + parseFloat(model.Total_Shipping_Amount) + '</td> </tr>'
                            + '<tr>' + shipcharge
                            + '<td> ₹ ' + parseFloat(model.Total_Shipping_Charges) + '</td> </tr>'
                            + '<tr> <td colspan="3"> Counted By ___________ </t> <td colspan="3" style="text-align: right;"><strong>Total Amount</strong></td>'
                            + '<td> ₹ ' + totl_amt + '</td> </tr>'


            $(html).find('#customerCopy').hide();
            $(html).find('#oficCopy').hide();

            $(html).find('#item_container').append(itemList_html);

            return html.html().trim();
        }

        return fac;
    }

    app.factory('cartService', ['globalConfig', '$filter', 'httpService', fn]);

})(angular.module('Silverzone_app'));

