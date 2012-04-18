(function() {
    var plugin = function(_) {
        _.templateSettings = {
          evaluate: /\{\[([\s\S]+?)\]\}/g,
          interpolate: /\{\{([\s\S]+?)\}\}/g,
          escape: /\{\{-([\s\S]+?)\}\}/g
        };        
    }
    
    if (typeof define === 'function' && define.amd) {
        define(['underscore'], plugin);
    } else { plugin(_) }
})();