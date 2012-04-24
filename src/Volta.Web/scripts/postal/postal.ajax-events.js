//     postal.ajax-events.js 0.1.0
//     (c) 2012 Mike O'Brien
//     May be freely distributed under the MIT license.
//     https://github.com/mikeobrien/jsplugins

(function() {
    var plugin = function($, postal) {
        var options = { errors: {} };
        
        $(document).ajaxError(function(error, xhr, settings, thrownError) {
            var json = parseInt(xhr.getResponseHeader('content-length')) > 0 && 
                       !xhr.getResponseHeader('content-type').indexOf('application/json');
            var status = options.errors[xhr.status] || {};
            return postal.channel("ajax.error." + (status.alias || xhr.status)).publish({
                status: status.alias || xhr.status,
                message: status.message || thrownError,
                data: json ? $.parseJSON(xhr.responseText) : xhr.responseText,
                settings: settings
            });
        });
        
        $(document).ajaxStart(function() { postal.channel("ajax.start").publish(); });
        $(document).ajaxStop(function() { postal.channel("ajax.stop").publish(); });
        
        return options;
    }
    
    if (typeof define === 'function' && define.amd) {
        define(['jquery', 'postal'], plugin);
    } else { plugin(window.jQuery, window.postal) }
})();
