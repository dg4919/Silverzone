
(function () {
    'use strict';

    function getParameterByName(name, url) {
        if (!url) url = window.location.href;
        name = name.replace(/[\[\]]/g, "\\$&");
        var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"),
            results = regex.exec(url);
        if (!results) return null;
        if (!results[2]) return '';
        return decodeURIComponent(results[2].replace(/\+/g, " "));
    }

    angular
        .module('Silverzone_app')

        .component('olympiad', {
            template: `
                <section id="work">
                <div class="container ">
        <div class="row">
            <div class="col-lg-12  col-md-12 col-xs-12 text-center">
                <div class="section-title">
                    <h1 class ="heading bold"><span> Scheduled </span>Olympiads</h1>
                    <hr class="style17">
                </div>
            <section class="regular">
            <slick init-onload=true data="SOP_List" dots=true autoplay=true infinite=false speed=300 slides-to-show=3 touch-move=false slides-to-scroll=1 responsive=responsiveSetting>
                    <div class ="thumbnail galbg"
                    ng-repeat="SOP in SOP_List">
                        <a href="{{SOP.Link}}" target="_blank">
                        <img ng-src="{{SOP.ImageName }}" style="height: 132px;width: 350px;"></a>
                        <div class="ecaption">
                            <h3 ng-bind="SOP.Caption"></h3>
                            <a class ="btn btn-default gray btn-lg btn-block"
                               href="{{SOP.Link}}" target="_blank">Know More...</a>
                        </div>
                    </div>
            </slick>
           </section>
            </div>
        </div>
    </div>
    </section> `,
            controller: ['$scope', 'siteMasterService', function ($sc, svc) {
                $sc.SOP_List = [];

                svc.get_Schedule_olympads()
                .then(function (d) {
                    $sc.SOP_List = d.result;
                });
            }]
        })
        .component('gallery', {
            template: `
                <section id="gallery">
                <div class="container">
        <div class="row">
            <div class="section-title">
                <h1 class ="heading bold"><span>PHOTO </span> GALLERY</h1>
                <hr class="style17">
            </div>
            <section class ="photo">
            <slick init-onload=true data="galleryList" dots=true autoplay=true infinite=false speed=300 slides-to-show=4 touch-move=false slides-to-scroll=1 responsive=responsiveSetting>
                <div class="thumbnail galbg"
                     ng-repeat="gallery in galleryList">
                     <p class="btn btn-lg btn-block"
                        style="background: #6a6a6a;color: #fff;height: 25px;padding: 0px !important;font-size: 16px;"
                        ng-bind="gallery.category + ' - ' + gallery.year">
                     </p>
                    <img ng-src="{{ gallery.ImageUrl }}" /*style="height: 220px;width: 290px;" */>
                    <div class="ecaption">
                        <a class="btn gray btn-lg btn-block"
                           ui-sref="{{ $last == true ? 'hof' : 'gallery_Detail({ galleryId: gallery.Id })' }}">
                           View More...
            </slick> </a></div> </div> </section> </div></div></section> `,
            controller: 'galleryController'
        })
        .component('partners', {
            template: ``,
            controller: ['$scope', function ($sc) {
            }]
        })
        .component('clients', {
            template: `
              <section id="testi">
    <div class="container">
        <div class="row">
            <div class="section-title">
                <h1 class="heading bold"><span>SCHOOL</span> TESTIMONiALS</h1>
                <hr class="style17">
            </div>
            <div class='row'>
                <div class='col-md-offset-2 col-md-8'>
                    <div class="carousel slide" data-ride="carousel" id="quote-carousel">
                        <!--Bottom Carousel Indicators-->
                        <ol class="carousel-indicators">
                            <li data-target="#quote-carousel" data-slide-to="0" class="active"></li>
                            <li data-target="#quote-carousel" data-slide-to="1"></li>
                            <li data-target="#quote-carousel" data-slide-to="2"></li>
                        </ol>
                        <!--Carousel Slides / Quotes-->
                        <div class="carousel-inner">
                            <!--Quote 1 -->
                            <div class="item active">
                                <blockquote>
                                    <div class="row">
                                        <div class="col-sm-3 text-center">
                                            <img class="img-circle" src="images/testi.png" style="width: 100px;height:100px;">
                                            <!--<img class="img-circle" src="https://s3.amazonaws.com/uifaces/faces/twitter/kolage/128.jpg" style="width: 100px;height:100px;">-->
                                        </div>
                                        <div class="col-sm-9">
                                            <p>iiO is the best exam organized by computer literacy foundation to improve the knowledge of the students in the field of computer science and IT!</p>
                                            <small>Arun K.Pathak: Holy Cross Sch, Kolkata</small>
                                        </div>
                                    </div>
                                </blockquote>
                            </div>
                            <!--Quote 2 -->
                            <div class="item">
                                <blockquote>
                                    <div class="row">
                                        <div class="col-sm-3 text-center">
                                            <img class="img-circle" src="images/testi.png" style="width: 100px;height:100px;">
                                        </div>
                                        <div class="col-sm-9">
                                            <p>Children are getting more interested in computers and looking forward to other such exams for more felicitation.</p>
                                            <small>Sonal Thakkar: Centre Point School, Nagpur</small>
                                        </div>
                                    </div>
                                </blockquote>
                            </div>
                            <!--Quote 3 -->
                            <div class="item">
                                <blockquote>
                                    <div class="row">
                                        <div class="col-sm-3 text-center">
                                            <img class="img-circle" src="images/testi.png" style="width: 100px;height:100px;">
                                        </div>
                                        <div class="col-sm-9">
                                            <p>Good management.Timely response.International level.Best one, I have ever come across.</p>
                                            <small>Padma Hiranandani: Gokuldham School, Mumbai</small>
                                        </div>
                                    </div>
                                </blockquote>
                            </div>
                            <!--Quote 4 -->
                            <div class="item">
                                <blockquote>
                                    <div class="row">
                                        <div class="col-sm-3 text-center">
                                            <img class="img-circle" src="images/testi.png" style="width: 100px;height:100px;">
                                        </div>
                                        <div class="col-sm-9">
                                            <p>Creating more interest in students for computer education.Good efforts for making computer subject as much important as other important subjects like Maths, Physics, etc.</p>
                                            <small>Rajeev K.Srivastava: Seedling School, Jaipur</small>
                                        </div>
                                    </div>
                                </blockquote>
                            </div>
                        </div>
                        <!--Carousel Buttons Next/Prev-->
                        <a data-slide="prev" href="#quote-carousel" class="left carousel-control"><i class="fa fa-chevron-left"></i></a>
                        <a data-slide="next" href="#quote-carousel" class="right carousel-control"><i class="fa fa-chevron-right"></i></a>
					 </div>
					 <div class="text-center"><a ui-sref="testimonail" class="btn gray">View All </a></div>
                    </div>
                </div>
            </div>
            <div class="clear"></div>
            <div class="clear"></div>
        </div>
    </div>
</section>

                `,
            controller: ['$scope', function ($sc) {
            }]
        })
        .component('aboutSubject', {
            template: `<div ng-include="templateUrl"></div>

                <script type="text/javascript">
                    $("ui-view").on('click', '.bhoechie-tab-menu>div.list-group>a', function (e) {
                        e.preventDefault();
                        $(this).siblings('a.active').removeClass("active");
                        $(this).addClass("active");
                        var index = $(this).index();
                        $("div.bhoechie-tab>div.bhoechie-tab-content").removeClass("active");
                        $("div.bhoechie-tab>div.bhoechie-tab-content").eq(index).addClass("active");
                        });
                </script>
               `,
            controller: ['$scope', '$stateParams', '$window', 'siteMasterService', function ($sc, $stateParams, $window, svc) {
                $sc.templateUrl = 'templates/subjects/' + $stateParams.subjectName + '.html';
                $sc.subject_ndSyllabus_list = [];
                $sc.subjectName = $stateParams.subjectName;

                svc.getSyllabus_SampleQP($stateParams.subjectName)
                .then(function (d) {
                    $sc.subject_ndSyllabus_list = d.result;
                });

                $sc.dndFile = function (fileName) {
                    $window.open($window.location.origin + fileName);
                }

                $sc.downloadFile = function (fileName) {
                    $window.open($window.location.origin + '/Files/SampleOMRAnswerSheet.pdf');
                }

            }]
        })
        .component('excersion', {
            template: `<div ng-include="templateUrl"></div>
                <gallery></gallery>
                <partners></partners>
                `,
            controller: ['$scope', '$stateParams', function ($sc, $stateParams) {
                $sc.templateUrl = 'templates/subjects/' + $stateParams.subjectName + '.html';
            }]
        })
        .component('newsUpdates', {
            template: `
                <div class="row">
                <div class="col-md-12 col-xs-12 hidden-xs">
                    <div id="layout1" class="active">
                        <div class="ticker1 modern-ticker ">
                            <div class ="mt-body">
                            <a ui-sref="updates" style="color: #fff; ">
                               <div class ="mt-label" style="z-index:1;">UPDATES</div>
                             </a>
                                <div class="mt-news">
                                    <ul>
                                        <li ng-repeat="news in newsList">
                                        <a href="{{news.NewsUrl}}">{{ news.Title}}</a></li>
                                    </ul>
                                </div>
                                <div class="mt-controls">
                                    <div class="mt-prev"></div>
                                    <div class="mt-play"></div>
                                    <div class="mt-next"></div>
                                </div> </div></div></div></div></div>
               `,
            controller: ['$scope', '$state', '$stateParams', '$timeout', 'siteMasterService', function ($sc, $state, $stateParams, $timeout, svc) {
                $sc.templateUrl = 'templates/subjects/' + $stateParams.subjectName + '.html';
                $sc.newsList = [];

                svc.get_newsUpdates($state.current.url)
                   .then(function (d) {
                       $sc.newsList = d.result;

                       // to enable and ensure ticker work with angular
                       $timeout(initTicker, 1000);
                   });

                function initTicker() {
                    $(".ticker1").modernTicker({
                        effect: "scroll",
                        scrollType: "continuous",
                        scrollStart: "inside",
                        scrollInterval: 20,
                        transitionTime: 500,
                        autoplay: true
                    });
                }
            }]
        })

        .component('career', {
            templateUrl: '/templates/views/career.html',
            controller: ['$scope', 'siteMasterService', function ($sc, svc) {
                $sc.resumeFile = '';
                $sc.jobList = [];

                svc.get_carrier()
                .then(function (d) {
                    $sc.jobList = d.result;
                });

                $sc.uploadResume = function (Id) {
                    if ($sc.resumeFile) {
                        svc.uploadFile($sc.resumeFile, Id);
                        $sc.resumeFile = '';
                        $sc.$root.notify_fx('Thank you for submitting your Application !', 'info');
                    }
                    else
                        $sc.$root.notify_fx('Please select a file !', 'danger');
                }
            }]
        })
        .component('download', {
            templateUrl: '/templates/views/downloads.html',
            controller: ['$scope', '$window', 'siteMasterService', '$stateParams', '$location', '$anchorScroll', function ($sc, $window, svc, $stateParams, $location, $anchorScroll) {
                $sc.schDnd_pdfInfo = {};

                svc.getSchool_dndPdf()
                   .then(function (d) {
                       $sc.schDnd_pdfInfo = d.result;

                       if ($stateParams.eventName)
                           scrollToDiv($stateParams.eventName);
                   });

                function scrollToDiv(divPoint) {
                    // set the location.hash to the id of
                    // the element you wish to scroll to.
                    $location.hash(divPoint);

                    // call $anchorScroll()
                    $anchorScroll();
                }

                $sc.downloadFile = function (fileName) {
                    $window.open($window.location.origin + fileName);
                }

            }]
        })
        .component('paymentSuccess', {
            template: ` <div class="row heig ng-scope"></div>
                <div ng-if="payment_response.payment_refType === 'Instant_Downloads'">
                    <span class="text-success"><h3>Congrats you order is placed successfully !</h3></span>
                    <p>Your order ID: <b> {{ payment_response.orderid }}</b> </p>
                    <p>Payment Status: <b> {{ payment_response.payment_status }}</b> </p>
                    <p>Tracking Id: <b> {{ payment_response.tracking_id }} </b> </p>
                    <p>Bank Reference: <b> {{ payment_response.bank_ref_no }} </b> </p>
                    <div class="text-left ">
                    To download PDF,
                    <a ui-sref="InstantDownloads_login" class="btn gray">
                    Click Here...
                    </a>
                </div>
                </div>
                <div ng-if="payment_response.payment_refType === 'Online_Books'">
                    <section id="media" class="parallax-section">
                    <div id="container_full">
                    <div class="ts-contact-support parallax-section">
                    <div class="container">
                    <div class="row">
                    <div class="section-title text-center">
                    <h1 class="heading bold"><span> Your Order </span> Summary.</h1>
                      <hr class="style17"></div>
                    <div class="col-sm-4">
                    <div class="ts-wrapper">
                    <div class="ts-contact-hotline text-center mheight">
                    <div class="table-cell">
                    <span class="ts-contact-icon"><i class="fa fa-map-marker"></i></span>
                    <div class="ts-contact-info">
                    <h4><span>Shipping Details</span></h4>
                    <h4>{{ orderInfo.shipingAdres.Username }}</h4>
                    <p> {{  orderInfo.shipingAdres.Username }} - {{orderInfo.shipingAdres.PinCode}}, {{ orderInfo.shipingAdres.City }} <br /> {{ orderInfo.shipingAdres.State }} </p>
                    <p>{{ orderInfo.shipingAdres.Email }}</p>
                    <p>(+91) {{ orderInfo.shipingAdres.Mobile }}</p>
                    </div></div></div></div></div>
                    <div class="col-sm-4">
                    <div class="ts-wrapper">
                    <div class="ts-contact-hotline text-center mheight">
                    <div class="table-cell">
                    <span class="ts-contact-icon"><i class="fa fa-info"></i></span>
                    <div class="ts-contact-info">
                    <h4><span>Payment Information</span></h4>
                      <p>Tracking Id: <b> {{ payment_response.tracking_id }} </b> </p>
                      <p>Payment Status: <b> {{ payment_response.payment_status }}</b> </p>
                      <p>Bank Reference: <b> {{ payment_response.bank_ref_no }} </b> </p>
                    </div></div></div></div></div>
                    <div class="col-sm-4">
                    <div class="ts-wrapper">
                    <div class="ts-contact-hotline text-center mheight">
                    <div class="table-cell">
                    <span class="ts-contact-icon"><i class="fa fa-file-text-o"></i></span>
                    <div class="ts-contact-info">
                    <h4><span>Order Detail</span></h4>
                    <h4>demo data</h4>
                    <p><span>Order Number: </span> {{ orderInfo.OrderNumber }}</p>
                    <p><span>Order Date: </span> {{ orderInfo.OrderDate | dateFormat: 'DD/MM/YYYY' }}</p>
                    <p><span>Order Status: </span> Order Placed</p>
                    </div></div></div></div></div>
                    </div></div></div></div>
                    <div class="text-center container-fluid">
                       <h4><b><span>Your order has been placed successfully.
                       Your order may take 7-10 working days to deliver.</span></b></h4>
                     <button type="button" class="btn btn-default" ng-click="printOrder()"> <i class="fa fa-print"></i> Print </button>
                    </div></section></div></div></div> `,
            controller: ['$scope', 'siteMasterService', 'cartService', function ($sc, svc, cartSvc) {

                $sc.payment_response = {
                    orderid: getParameterByName('orderId')
                };
                $sc.orderInfo = {};

                svc.get_paymentResponse($sc.payment_response.orderid)
                    .then(function (d) {
                        angular.extend($sc.payment_response, d.result);

                        if ($sc.payment_response.payment_refType === 'Online_Books')
                            getOrderInfo($sc.payment_response.orderid);
                    });

                function getOrderInfo(orderNumber) {
                    cartSvc.get_orerInfo(orderNumber)
                    .then(function (d) {
                        $sc.orderInfo = d.result;
                    });
                }

                $sc.printOrder = function () {
                    cartSvc.print_ConfirmOrder($sc.orderInfo);
                }

            }]
        })
        .component('paymentError', {
            template: `
                <div class="row heig ng-scope"></div>
                <div class="container">
                    <h3 class="text-danger"> Oops! payment is failed due to some unavoidable reasons: (</h3>
                    <div class="col-sm-12" style="margin: 50px;">
                <div class="col-sm-6">
                <form method="post" action="https://secure.ccavenue.com/transaction/transaction.do?command=initiateTransaction">
                    <input type="hidden" name="encRequest" value="{{ checkSum_value.strEncRequest }}" />
                    <input type="hidden" name="access_code" value="{{ checkSum_value.strAccessCode }}" />
                  <span style="font-weight: 700;float:right;">
                  <input type="submit" value="Repayment" class="btn btn-danger" />
                </span> </form></div>
                <div class="col-sm-6">
                    <a class="btn btn-info" ui-sref="silverzone_home"> Home </a>
                </div> </div> </div></div>`,
            controller: ['$scope', 'siteMasterService', '$window', function ($sc, svc, $window) {

                $sc.payment_response = {
                    orderid: getParameterByName('orderId')
                };
                $sc.checkSum_value = {};
                start_rePayment();

                function start_rePayment() {
                    var paymentModel = {
                        redirect_url: 'http://' + $window.location.host + '/paymentSuccess',
                        cancel_url: 'http://' + $window.location.host + '/paymentError',
                    };

                    svc.get_rePaymentModel($sc.payment_response.orderid)
                    .then(function (d) {
                        paymentModel.redirect_url += '?orderId=' + d.result.order_id;
                        paymentModel.cancel_url += '?orderId=' + d.result.order_id;

                        var _model = angular.extend(d.result, paymentModel);
                        svc
                            .get_checksum(_model)
                            .then(function (d) {
                                $sc.checkSum_value = d.result;
                            });
                    });
                };

                //svc.get_payment_Response($sc.payment_response.orderid)
                //    .then(function (d) {
                //        angular.extend($sc.payment_response, d.result);
                //    });

            }]
        })

    ;

})();
