(function() {
    var plugin = function($) {
        $.fn.validate = function (selectors, predicate, message) {
            if (!$.isArray(selectors)) selectors = [selectors];
            var controlGroups = [];
            var values = [];
            for (index in selectors) {
                var input = this.find(selectors[index]);
                if (input.length == 0) throw 'Selector invalid: ' + selectors[index];
                input = $(input[0]);
                values.push(input.val());
                var element = input;
                while (true) {
                    element = element.parent();
                    if (element.hasClass('control-group') || element == null) break;
                }
                if (element) {
                    controlGroups.push(element);
                    element.removeClass('error');
                    element.find('.help-inline').html('');
                }
            }
            var result = predicate.apply(this, values);
            if (result) {
                for (index in controlGroups) {
                    controlGroups[index].addClass('error')
                    controlGroups[index].find('.help-inline').html(message)  
                }
            }
            return result;
        }
    }
    
    if (typeof define === 'function' && define.amd) {
        define(['jquery'], plugin);
    } else { plugin(window.jQuery) }
})();