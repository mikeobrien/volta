//     bootstrap.validate.js 0.1.0
//     (c) 2012 Mike O'Brien
//     May be freely distributed under the MIT license.
//     https://github.com/mikeobrien/jsplugins

(function() {
    var plugin = function($) {
        $.fn.validate = function (selectors, predicate, message) {
            if (!$.isArray(selectors)) selectors = [selectors];
            var controlGroups = [];
            var values = [];
            var form;
            for (index in selectors) {
                var input = this.find(selectors[index]);
                if (input.length == 0) throw 'Selector invalid: ' + selectors[index];
                input = $(input[0]);
                values.push(input.val());
                var element = input;
                var controlGroup;
                while (true) {
                    element = element.parent();
                    if (element == null) break;
                    if (element.hasClass('control-group')) controlGroup = element;
                    if (element.hasClass('form-horizontal')) {
                        form = element;
                        break;
                    }
                }
                if (controlGroup) {
                    controlGroups.push(controlGroup);
                    controlGroup.removeClass('error');
                    controlGroup.find('.help-inline').html('');
                }
            }
            if (form) {     
                form.find('.form-actions').css('background-color', '');
                form.find('.form-actions .help-inline').css('color', '').html('');   
            }
            var result = !predicate.apply(this, values);
            if (result) {
                for (index in controlGroups) {
                    controlGroups[index].addClass('error')
                    controlGroups[index].find('.help-inline').html(message)  
                }
                if (form) {
                    form.find('.form-actions').css('background-color', '#F2DEDE');
                    form.find('.form-actions .help-inline').css('color', '#B94A48').html('Some values entered above were not valid.');
                }
            }
            return result;
        }
        
        $.fn.validateHasValue = function (selectors, message) {
            return this.validate(selectors, function(value) { return value != '' }, message || 'Value cannot be empty.');
        }
        
        $.fn.validateMatch = function (selectorX, selectorY, message) {
            return this.validate([selectorX, selectorY], function(x, y) { return x === y }, message || 'Value cannot be empty.');
        }
        
        $.fn.validateInteger = function (selectors, message) {
            return this.validate(selectors, function(value) { return !isNaN(parseFloat(value)) && isFinite(value) && parseFloat(value) == parseInt(value) }, message || 'Value must be an integer.');
        }
        
        $.fn.validateDecimal = function (selectors, message) {
            return this.validate(selectors, function(value) { return !isNaN(parseFloat(value)) && isFinite(value) }, message || 'Value must be a decimal.');
        }
    }
    
    if (typeof define === 'function' && define.amd) {
        define(['jquery'], plugin);
    } else { plugin(window.jQuery) }
})();