
(function (app) {

    var fn = function (globalConfig, httpSvc) {
        var apiUrl = globalConfig.apiEndPoint + globalConfig.version.Site,
            fac = {};

        fac.submitEnquiry = function (model) {
            return httpSvc.post(
             apiUrl + '/Home/save_enquiryForm',
             model);
        }

        get_enquiryHtml = function (model) {
            return httpSvc.get('templates/EmailTemplates/enquiryForm.html');
        }

        function bind_itemList_Html(myHtml, customerInfo) {
            var html = $("<div>" + myHtml + "</div>");

            $(html).find('#customerInfo').html(`
                 Customer Name: <strong> ${customerInfo.UserName} </strong> <br>
                 Email Id: <strong> ${customerInfo.EmailId} </strong> <br>
                 Mobile: <strong> ${customerInfo.Mobile}</strong> <br>
                 Country: <strong>${customerInfo.Country}</strong> <br>
                 Query: <strong> ${customerInfo.Description}</strong>`);

            return html.html().trim();
        }

        //**************** For Payments  ************************

        fac.getJson = function () {
            return httpSvc.get(apiUrl + '/Payment/getJson');
        };

        fac.get_checksum = function (model) {
            return httpSvc.post(
             apiUrl + '/Payment/getPayment_checksum',
             model);
        };

        fac.processPayment = function (model) {
            return httpSvc.post(
             apiUrl + '/Payment/processPayment',
             model);
        };

        fac.get_paymentResponse = function (_orderId) {
            return httpSvc.get(
              apiUrl + '/Payment/get_paymentResponse',
              { orderId: _orderId });
        };

        fac.get_rePaymentModel = function (_orderId) {
            return httpSvc.get(
              apiUrl + '/Payment/get_rePaymentModel',
               { orderId: _orderId });
        };

        //**************** For Download PDF  ************************

        fac.get_userInstant_dndPdf = function (model) {
            var params = {
                emailId: model.email,
                mobile: model.mobileNo
            };

            return httpSvc.get(
               apiUrl + '/Payment/get_userInstant_dndPdf',
               params);
        };

        fac.dnd_instantFile = function (_model) {
            return httpSvc.post(
             apiUrl + '/Payment/dnd_instantFile',
             _model);
        }

        fac.getSchool_dndPdf = function () {
            return httpSvc.get(apiUrl + '/Home/getSchool_dndFiles');
        };

        fac.getMeta_tagInfo = function (_url) {
            return httpSvc.get(
               apiUrl + '/Home/getMeta_tagbyUrl',
               { url: _url },
               true);
        };

        fac.getSyllabus_SampleQP = function (_eventName) {
            return httpSvc.get(
               apiUrl + '/Home/getSyllabus_SampleQP',
               { eventName: _eventName },
               true);
        };

        fac.get_gallery = function () {
            return httpSvc.get(apiUrl + '/Home/get_gallerys');
        };

        fac.get_MediaImg = function () {
            return httpSvc.get(apiUrl + '/Home/get_MediaImg');
        };

        fac.get_hallofFame = function () {
            return httpSvc.get(apiUrl + '/Home/get_hallofFame');
        };

        fac.get_galleryDetail = function (_gallaryId) {
            return httpSvc.get(
               apiUrl + '/Home/get_galleryDetail',
               { gallaryId: _gallaryId },
               true);
        };

        fac.saveCountry = function (_model) {
            return httpSvc.post(
             apiUrl + '/Home/createCountry',
             _model);
        }

        fac.getCountry = function () {
            return httpSvc.get(apiUrl + '/Home/getAll_country');
        }

        fac.getEvents = function () {
            return httpSvc.get(apiUrl + '/Home/getAll_events');
        }

        fac.search_result = function (model) {
            return httpSvc.post(
             apiUrl + '/Home/searchResult',
             model);
        }

        fac.register_subscribeUpdates = function (model) {
            return httpSvc.post(
             apiUrl + '/Home/register_subscribeUpdates',
             model);
        }

        fac.get_registerSchool_json = function () {
            return httpSvc.get(apiUrl + '/Home/get_registerSchool_json');
        }

        fac.get_newsUpdates = function (_curentUrl) {
            return httpSvc.get(
                apiUrl + '/Home/get_newsUpdates',
                { curentUrl: _curentUrl });
        };

        fac.register_school = function (model) {
            return httpSvc
                .get('templates/EmailTemplates/registerSchool.html')
                .then(function (data) {
                    angular.extend(model, {
                        HtmlTemplate: bind_registerSchool(data, model)
                    });

                    return httpSvc.post(
                        apiUrl + '/Home/register_school',
                        model);
                });
        }

        fac.save_freelance = function (model) {
            return httpSvc.post(
             apiUrl + '/Home/save_freelance',
             model);
        }

        function bind_registerSchool(myHtml, schoolInfo) {
            var html = $("<div>" + myHtml + "</div>");

            $(html).find('#schoolInfo').html(`
                <p><strong> User Name: </strong> ${schoolInfo.UserName} </p>
                <p><strong> Gender: </strong> ${schoolInfo.genderName} </p>
                <p><strong> Profile: </strong> ${schoolInfo.profileName} </p>
                <p><strong> Email Address: </strong> ${schoolInfo.EmailId} </p>
                <p><strong> Mobile Number: </strong> ${schoolInfo.Mobile} </p>
                <p><strong> School Name: </strong> ${schoolInfo.schName} </p>
                <p><strong> School Address: </strong> ${schoolInfo.Address} </p>
                <p><strong> City: </strong> ${schoolInfo.City} </p>
                <p><strong> State: </strong> ${schoolInfo.stateName} </p>
                <p><strong> Pin Code: </strong> ${schoolInfo.PinCode} </p>
                <p><strong> Country: </strong> ${schoolInfo.countryName} </p>`);

            return html.html().trim();
        }

        fac.get_siteConfiguration = function () {
            return httpSvc.get(apiUrl + '/Home/get_siteConfiguration');
        };

        fac.uploadFile = function (file, _jobId) {
            return httpSvc
                  .get('templates/EmailTemplates/careeier_resume.html')
                  .then(function (d) {
                      var params = {
                          htmlTemplate: d,
                          jobId: _jobId
                      };

                      var form_Data = new FormData();
                      form_Data.append("file", file);

                      return httpSvc.post(
                         apiUrl + '/Home/uploadResume',
                         form_Data,
                         params);
                  });
        }

        fac.get_carrier = function () {
            return httpSvc.get(apiUrl + '/Home/get_carrier');
        };

        fac.save_resultRequest = function (model) {
            return httpSvc.post(
             apiUrl + '/Home/save_resultRequest',
             model);
        }

        fac.get_Schedule_olympads = function () {
            return httpSvc.get(apiUrl + '/Home/get_Schedule_olympads');
        };


        return fac;
    }

    app.factory('siteMasterService', ['globalConfig', 'httpService', fn]);

})(angular.module('Silverzone_app'));

