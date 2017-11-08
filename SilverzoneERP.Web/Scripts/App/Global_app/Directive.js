(function (app) {

    app
    .directive('greaterThan', [function () {
        return {
            restrict: 'A',
            require: 'ngModel',
            link: function (scope, elem, attrs, control) {
                debugger;
                // alert('Test');
                var checker = function () {
                    debugger;
                    //get the value of the first number
                    var num1 = scope.$eval(attrs.ngModel);
                    if (angular.isUndefined(num1))
                        num1 = 0;
                    else
                        num1 = parseInt(num1);
                    //get the value of the second number
                    var num2 = scope.$eval(attrs.greaterThan);
                    if (angular.isUndefined(num2))
                        num2 = 0;
                    else
                        num2 = parseInt(num2);
                    if (num1 > num2)
                        return true
                    else
                        return false

                };
                scope.$watch(checker, function (n) {

                    //set the form control to valid if both 
                    //passwords are the same, else invalid
                    control.$setValidity("greater", n);
                });
            }
        };
    }])
    .directive('loading', ['$http', function ($http) {
        return {
            restrict: 'A',
            link: function (scope, elm, attrs) {
                scope.isLoading = function () {
                    return $http.pendingRequests.length > 0;
                };

                scope.$watch(scope.isLoading, function (v) {
                    if (v) {
                        elm.show();
                    } else {
                        elm.hide();
                    }
                });
            }
        };

    }])
    .directive("ngFileSelect", function () {
        return {
            link: function ($scope, el) {

                el.bind("change", function (e) {

                    $scope.file = (e.srcElement || e.target).files[0];
                    $scope.UploadImage($scope.file);
                })
            }
        }
    })
    .directive('passwordMatch', [function () {
        return {
            restrict: 'A',
            scope: true,
            require: 'ngModel',
            link: function (scope, elem, attrs, control) {
                var checker = function () {

                    //get the value of the first password
                    var e1 = scope.$eval(attrs.ngModel);

                    //get the value of the other password  
                    var e2 = scope.$eval(attrs.passwordMatch);
                    return e1 == e2;
                };
                scope.$watch(checker, function (n) {

                    //set the form control to valid if both 
                    //passwords are the same, else invalid
                    control.$setValidity("unique", n);
                });
            }
        };
    }])
    .directive('onlyNumbers', function () {
        return {
            require: 'ngModel',
            link: function (scope, element, attr, ngModelCtrl) {
                debugger;
                function fromUser(text) {
                    if (text) {
                        debugger;
                        var transformedInput = text.replace(/[^0-9]/g, '');

                        if (transformedInput !== text) {
                            ngModelCtrl.$setViewValue(transformedInput);
                            ngModelCtrl.$render();
                        }
                        return transformedInput;
                    }
                    return undefined;
                }            
                ngModelCtrl.$parsers.push(fromUser);
            }
        };
    })    
    .directive('onlyCharacter', function () {
        return {
            require: 'ngModel',
            link: function(scope, element, attrs, modelCtrl) {
                modelCtrl.$parsers.push(function (inputValue) {
                    debugger;
                    if (inputValue == undefined) return '' 
                    var transformedInput = inputValue.replace(/^[a-zA-Z]+$/, ''); 
                    if (transformedInput!=inputValue) {
                        modelCtrl.$setViewValue(transformedInput);
                        modelCtrl.$render();
                    }         

                    return transformedInput;         
                });
            }
        };
    })      
    .directive('answerOption', function () {
        debugger;
        return {
            require: 'ngModel',
            restrict: 'A',
            link: function (scope, elm, attrs, ctrl) {
                debugger;
                elm.on('keydown', function (event) {
                    console.log(ctrl);

                    if ([8, 13, 27, 37, 38, 39, 40, 9, 123, 12, 18].indexOf(event.which) > -1) {
                        // backspace, enter, escape, arrows
                        return true;
                    } else if (event.which >= 65 && event.which <= 70) {
                        // Allow capital latter
                        var value = String.fromCharCode(event.which);
                        $(this).val(value);
                        ctrl.$setViewValue(value);

                        return false;
                    } else if (event.which >= 97 && event.which <= 102) {
                        // Allow small latter and convert into capital
                        var value = String.fromCharCode(event.which - 32);
                        $(this).val(value);
                        ctrl.$setViewValue(value);

                        return false;
                    }
                    else if (event.which >= 49 && event.which <= 53) {
                        var value = String.fromCharCode(16 + event.which);
                        $(this).val(value);
                        ctrl.$setViewValue(value);                        
                        return false;
                    }
                    else {
                        event.preventDefault();
                        return false;
                    }
                });
            }
        }
    })
    .directive('examDatePicker', function () {
        debugger;
        return {
            require: 'ngModel',
            restrict: 'A',
            link: function (scope, elm, attrs, ctrl) {
                // to apply bootstarp jqeury Datepicker
                var maxExamDate = new Date();
                maxExamDate.setYear(maxExamDate.getFullYear() + 1);
                elm.datetimepicker({
                    minDate: new Date(),
                    maxDate: maxExamDate,
                    format: 'DD-MMM-YYYY'
                });

                elm.on("dp.change", function () {
                    // contains selected date into txt box
                    //alert('Test');
                    ctrl.$setViewValue(angular.element(elm).val());
                    ctrl.$render();      // save new value
                });
            }
        }
    })
    .directive('paymentDatePicker', function () {
        debugger;
        return {
            require: 'ngModel',
            restrict: 'A',
            link: function (scope, elm, attrs, ctrl) {
                // to apply bootstarp jqeury Datepicker                
                elm.datetimepicker({                  
                    maxDate: 'now',
                    format: 'DD-MMM-YYYY'
                });

                elm.on("dp.change", function () {
                    // contains selected date into txt box
                    //alert('Test');
                    ctrl.$setViewValue(angular.element(elm).val());
                    ctrl.$render();      // save new value
                });
            }
        }
    })
    .directive('showPicker', function () {
        debugger;
        return {
            require: 'ngModel',
            restrict: 'A',
            link: function (scope, elm, attrs, ctrl) {
                // to apply bootstarp jqeury Datepicker
                elm.datetimepicker({
                    maxDate: 'now',
                    format: 'DD-MMM-YYYY'
                });

                elm.on("dp.change", function () {
                    // contains selected date into txt box
                    ctrl.$setViewValue(angular.element(elm).val());
                    ctrl.$render();      // save new value
                });
            }
        }
    })
    .directive("digitalClock", function ($timeout, dateFilter) {
        return function (scope, element, attrs) {

            //element.addClass('alert alert-info text-center clock');

            scope.updateClock = function () {
                $timeout(function () {
                    element.text(dateFilter(new Date(), 'dd-MMM-yyyy   hh:mm:ss a'));
                    scope.updateClock();
                }, 1000);
            };
            scope.updateClock();
        };
    })
    .directive('decimalOnly', function () {

         function validate_Number(scope, element, attrs, modelCtrl) {
             modelCtrl.$parsers.push(function (val) {

                 if (angular.isUndefined(val)) {
                     var val = '';
                 }

                 var clean = val.replace(/[^-0-9\.]/g, '');
                 var negativeCheck = clean.split('-');
                 var decimalCheck = clean.split('.');
                 if (!angular.isUndefined(negativeCheck[1])) {
                     negativeCheck[1] = negativeCheck[1].slice(0, negativeCheck[1].length);
                     clean = negativeCheck[0] + '-' + negativeCheck[1];
                     if (negativeCheck[0].length > 0)
                         clean = negativeCheck[0];
                 }

                 if (!angular.isUndefined(decimalCheck[1])) {
                     decimalCheck[1] = decimalCheck[1].slice(0, 2);
                     clean = decimalCheck[0] + '.' + decimalCheck[1];
                 }

                 if (val !== clean) {
                     modelCtrl.$setViewValue(clean);
                     modelCtrl.$render();
                 }
                 return clean;
             });
         }

         return {
             restrict: 'A',
             require: 'ngModel',
             link: validate_Number
         };
     })
    .directive('ngFiles', ['$parse', function ($parse) {
        debugger;
        function fn_link(scope, element, attrs) {
            var onChange = $parse(attrs.ngFiles);
            element.on('change', function (event) {
                onChange(scope, { $files: event.target.files });
            });
        };

        return {
            link: fn_link
        }
    }])
    .directive('clientAutoComplete', ['$filter', function ($filter) {
        debugger;
        return {
            restrict: 'A',
            link: function (scope, elem, attrs) {
                elem.autocomplete({
                    source: function (request, response) {

                        //term has the data typed by the user
                        var params = request.term;

                        //simulates api call with odata $filter
                        var data = scope.dataSource;
                        if (data) {
                            var result = $filter('filter')(data, { name: params });
                            angular.forEach(result, function (item) {
                                item['value'] = item['name'];
                            });
                        }

                        response(result);

                    },
                    minLength: 1,
                    select: function (event, ui) {
                        //force a digest cycle to update the views
                        scope.$apply(function () {
                            scope.setClientData(ui.item);
                        });
                    },

                });
            }

        };
    }])
    .directive('customAutoComplete', function ($document) {
        return {
            restrict: 'E',
            scope: {
                srchTxt: '=',
                displayMember: '=',
                list: '=',
                onChange: '&',
                onSelect: '&'
            },
            link: function (scope, elem, attrs) {
                debugger;
                scope.selectedIndex = 0;
                scope._KeyDown = function (event) {
                    var keyCode = event.charCode || event.keyCode || 0;
                    if (keyCode == 38)
                    {
                        scope.selectedIndex--;
                       
                        //alert('UPARROW');
                    }
                    else if (keyCode == 40)
                    {
                        debugger;
                        scope.selectedIndex++;                       
                       // alert('DOWNARROW');
                    }
                    else if (keyCode == 13) {
                        if (scope.selectedIndex > 0) {
                            scope.onSelect({ param: scope.list[scope.selectedIndex] });
                            scope.selectedIndex = 0;
                        }
                        event.stopPropagation();
                        event.preventDefault();
                        return;
                        // alert('Enter');
                    }
                    if (scope.list.length == 0)
                        scope.selectedIndex = 0;
                }
                scope.getSelectedCssClass = function (index) {
                    if (index === scope.selectedIndex) {
                        return 'auto-complete-item-selected';
                    }
                    return '';
                }
            },
            template: `
                 <input class ="form-control"
                       ng-model="srchTxt"
                       ng-change="onChange({param: srchTxt})"
                       placeholder="Search"
                       ng-keydown="_KeyDown($event)"
                       type="text">

               <div class ="auto-complete-container unselectable auto-complete-absolute-container"
                    ng-if="srchTxt && list.length"
                     style="min-width:100%;">
                    
                    <ul class ="auto-complete-results">
                        <li ng-repeat="item in list | filter: srchTxt"
                            ng-click="onSelect({param: item})"
                            class ="auto-complete-item"
                           ng-class ="getSelectedCssClass($index)"
                            data-index="{{ $index }}">
                            <div ng-bind="item.Name"></div>
                        </li>
                    </ul>                 
                </div>
               `            
        };
    })
    ;

})(angular.module('SilverzoneERP_App'));







