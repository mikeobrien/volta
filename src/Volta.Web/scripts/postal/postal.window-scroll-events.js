//     postal.window-scroll-events.js 0.1.0
//     (c) 2012 Mike O'Brien
//     May be freely distributed under the MIT license.
//     https://github.com/mikeobrien/jsplugins

(function() {
    var plugin = function($, postal) {
        var top = true;
        var bottom = false;
        
        var options = {
            topOffset: 0,
            bottomOffset: 0
        };
        
        var $window = $(window);
        var $document = $(document);
        
        $window.scroll(function() {
            var topPosition = $window.scrollTop();
            var bottomPosition = topPosition + $window.height();
            
            if (topPosition <= options.topOffset) 
            {
                if (top) return;
                postal.channel('window.scroll.top').publish();
                top = true;
                bottom = false;
            } 
            else if (bottomPosition >= $document.height() - options.bottomOffset) 
            {
                if (bottom) return;
                postal.channel('window.scroll.bottom').publish();
                top = false;
                bottom = true;
            } 
            else 
            {
                top = false;
                bottom = false;
            }
        });
        return options;        
    }
    
    if (typeof define === 'function' && define.amd) {
        define(['jquery', 'postal'], plugin);
    } else { plugin(window.jQuery, window.postal) }
})();