(function() {
    var plugin = function($) {
        $.ajaxPrefilter(function(options, originalOptions, jqXHR) {
            var deferred = options.deferred = 
                           options.deferred || { done: [], fail: [], always: [] };
            
            for (callback in deferred.done) { jqXHR.done(deferred.done[callback]) }
            var done = jqXHR.done;
            jqXHR.done = function(callback) { options.deferred.done.push(callback); return done.call(jqXHR, callback); }
            jqXHR.success = jqXHR.done
            
            for (callback in deferred.fail) { jqXHR.fail(deferred.fail[callback]) }
            var fail = jqXHR.fail;
            jqXHR.fail = function(callback) { options.deferred.fail.push(callback); return fail.call(jqXHR, callback); }
            jqXHR.error = jqXHR.fail
            
            for (callback in deferred.always) { jqXHR.always(deferred.always[callback]) }
            var always = jqXHR.always;
            jqXHR.always = function(callback) { options.deferred.always.push(callback); return always.call(jqXHR, callback); }
            jqXHR.complete = jqXHR.always
        });
    }
    
    if (typeof define === 'function' && define.amd) {
        define(['jquery'], plugin);
    } else { plugin(window.jQuery) }
})();