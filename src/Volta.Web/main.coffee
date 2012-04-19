require.config
    paths:
        "jquery": "scripts/jquery/jquery"
        "underscore": "scripts/underscore/underscore"
        "backbone": "scripts/backbone/backbone"
        "text": "scripts/require/text"
        "postal": "scripts/postal/postal"
        "postalajax": "scripts/postal/postal.ajax-events"
        "postalscroll": "scripts/postal/postal.window-scroll-events"

plugins = [
    'scripts/jquery/jquery.ajax-dateparser', 
    'scripts/jquery/jquery.cache-ajax-deferreds',
    'scripts/bootstrap/bootstrap', 
    'scripts/bootstrap/bootstrap.dialog', 
    'scripts/bootstrap/bootstrap.validate',
    'scripts/underscore/underscore.config', 
    'scripts/backbone/backbone.lazy']

require ['jquery', 'backbone', 'postal', 'postalajax', 'postalscroll', 'app', 'batches/batches', 'admin/admin'].concat(plugins)
        , ($, Backbone, postal, postalAjax, postalScroll, App, Batches, Admin) ->
    
    window.onerror = (message, source, line) -> 
        $.post 'errors', { Source: source, Line: line, Message: message }
        postal.publish('error', message: 'Oops! A browser error has occured. This has been logged and will be fixed as soon as possible.')

    postalAjax.errors[0] = 
        message: 'Unable to communicate with the server. Make sure you are connected to the internet and try again.'
    postalAjax.errors[500] = 
        message: 'Oops! A server error has occured. This has been logged and will be fixed as soon as possible.'

    postalScroll.bottomOffset = 100

    content = $ '#content'

    App.start $('#menu'), $('#messages'), content
    Admin.start content
    Batches.start content

    Backbone.history.on 'route', -> postal.publish('route', window.location.hash.substr(1))
    Backbone.history.start()