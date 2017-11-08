(function (app) {
    'use strict';

    app
       .filter('split', function () {
           return function (input, splitChar, splitIndex) {
               // do some bounds checking here to ensure it has that index
               return input.split(splitChar)[splitIndex];
           }
       })
       .filter('dateFormat', function () {

           return function (input, formatType) {
               // using jquery moment.js
               return (moment(input).format(formatType));
           }
       })
        .filter('moment', function () {
            return function (input, momentFn /*, param1, param2, ...param n */) {
                debugger;
                var args = Array.prototype.slice.call(arguments, 2),
                    momentObj = moment(input);
                return momentObj[momentFn].apply(momentObj, args);
            };
        })
     .filter('utcdate', ['$filter','$locale', function($filter, $locale){

         return function (input, format) {
            
             if (!angular.isDefined(format)) {
                 format = $locale['DATETIME_FORMATS']['medium'];
             }

             var date = new Date(input);
             var d = new Date()
             var _utc = new Date(date.getUTCFullYear(), date.getUTCMonth(), date.getUTCDate(),  date.getUTCHours(), date.getUTCMinutes(), date.getUTCSeconds());
             return $filter('date')(_utc, format)
         };

     }])
       .filter('stringFilter', function () {   // use to make URL for book details

           return function (input) {
               // using jquery moment.js
               return (input.trim().replace(/ /g, '-'));       // replace space with '-' in a string
           }
       }).
    filter('groupBy', function () {
        return _.memoize(function (items, field) {
            return _.groupBy(items, field);
        }
            );
    })
    .filter('unique', function () {
         return function (arr, field) {
             return _.uniq(arr, function (a) { return a[field]; });
         };
    })
   .filter('sumOfValue', function () {
       return function (data, key) {
           debugger;
           if (angular.isUndefined(data) || angular.isUndefined(key))
               return 0;
           var sum = 0;
           angular.forEach(data, function (value) {
               sum = sum + parseInt(value[key], 10);
           });
           return sum;
       }
   })
    ;


})(angular.module('SilverzoneERP_App'));