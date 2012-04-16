(function() {
    var plugin = function($) {
        $.dialog = function (option) {
            var content = '<div class="modal hide ' + option.css + '">';
            content +=   '<div class="modal-header">';
            content +=     '<a class="close" data-dismiss="modal">&times;</a><h3>' + option.title + '</h3>';
            content +=   '</div>';
            content +=   '<div class="modal-body">' + option.body + '</div>';
            content +=   '<div class="modal-footer">';
            content +=     '<a class="btn btn-primary ok">' + option.button + '</a>';
            content +=     '<a class="btn cancel">Cancel</a>';
            content +=   '</div>';
            content += '</div>';
            var dialog = $(content);
            dialog.find('.cancel').click(function() { dialog.modal('hide') });
            dialog.find('.ok').click(function() { option.command(); dialog.modal('hide') });
            dialog.on('hidden', function () { dialog.remove() });
            dialog.modal('show');
            dialog.css('top', '50%');
            dialog.css('margin-top', -1 * (dialog.height() / 2));
        }
    }
    
    if (typeof define === 'function' && define.amd) {
        define(['jquery'], function($) { plugin($) });
    } else { plugin(window.jQuery) }
})();


