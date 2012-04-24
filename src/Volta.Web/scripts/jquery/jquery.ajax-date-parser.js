//     jquery.ajax-dateparser.js 0.1.0
//     (c) 2012 Mike O'Brien
//     May be freely distributed under the MIT license.
//     https://github.com/mikeobrien/jsplugins

(function() {
    var plugin = function($) {
        $.ajaxPrefilter(function(ajax) {
            if (!ajax.success) return;
            var success = ajax.success;
            var getType = function(obj) { return Object.prototype.toString.apply(obj) }
            var objectType = getType({}), arrayType = getType([]);
            
            var parseValue = function(value) {
                if (typeof value === 'string' && !value.search(/^\/Date\(-*\d+\)\/$/))
                    return new Date(parseInt(value.substr(6)));
                else return value;
            };

             var parseObject = function(obj) {
                if (getType(obj) != objectType && getType(obj) != arrayType) return;
                for (key in obj) {
                    var value = obj[key];
                    if (getType(value) === objectType || getType(value) === arrayType) parseObject(value);
                    else if (typeof value != 'function') obj[key] = parseValue(value);
                }
            };

            ajax.success = function(data) {
                parseObject(data);
                success(data);
            };
        });
    }
    
    if (typeof define === 'function' && define.amd) {
        define(['jquery'], plugin);
    } else { plugin(window.jQuery) }
})();