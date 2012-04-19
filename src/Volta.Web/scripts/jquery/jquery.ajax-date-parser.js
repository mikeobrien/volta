(function() {
    var plugin = function($) {
        $.ajaxPrefilter(function(ajax) {
            if (!ajax.success) return;
            var success = ajax.success;

            var parseValue = function(name, value) {
                if (typeof value === 'string' && !value.search(/^\/Date\(\d+\)\/$/))
                    return new Date(parseInt(value.substr(6)));
                else return value;
            };

             var parseObject = function(obj) {
                for (key in obj) {
                    var value = obj[key];
                    if (typeof value === 'object') parseObject(value);
                    else if (typeof value != 'function') obj[key] = parseValue(key, value);
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