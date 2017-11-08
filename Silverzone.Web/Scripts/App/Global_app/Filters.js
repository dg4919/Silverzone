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
       .filter('stringFilter', function () {   // use to make URL for book details

            return function (input) {
                // using jquery moment.js
                return (input.trim().replace(/ /g, '-'));       // replace space with '-' in a string
            }
       })
     .filter('hindliClass', function () {

         return function (input) {

             var str = parseInt(input.split(' ')[1]);
             switch (str) {
                 case 3:
                     return '३';
                     break;
                 case 4:
                     return '४';
                     break;
                 case 5:
                     return '५';
                     break;
                 case 6:
                     return '६';
                     break;
                 case 7:
                     return '७';
                     break;
                 case 8:
                     return '८';
                     break;
                 case 9:
                     return '९';
                     break;
                 case 10:
                     return '१०';
                     break;
             }
         }
     })

    ;


})(angular.module('Silverzone_app'));