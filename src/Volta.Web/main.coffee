require.config
    paths:
        "jquery": "scripts/jquery/jquery"
        "underscore": "scripts/underscore/underscore"
        "_config": "scripts/underscore/underscore-config"
        "backbone": "scripts/backbone"
        "text": "scripts/require/text"
        "postal": "scripts/postal/postal"
        "postalajax": "scripts/postal/postal.ajax"
        "postalscroll": "scripts/postal/postal.scroll"
        "bootstrap": "scripts/bootstrap/bootstrap"

require ['jquery', 'backbone', 'postal', 'postalajax', 'postalscroll', 'app', 'batches/batches', 
         'admin/admin', 'bootstrap', '_config'], ($, Backbone, postal, postalAjax, postalScroll, App, Batches, Admin) ->
    
    window.onerror = (message, source, line) -> 
        $.post 'errors', { Source: source, Line: line, Message: message }
        postal.publish('error', message: 'Oops! A browser error has occured. This has been logged and will be fixed as soon as possible.')

    postalAjax.errors[0] = 
        message: 'Unable to communicate with the server. Make sure you are connected to the internet and try again.'
    postalAjax.errors[500] = 
        message: 'Oops! A server error has occured. This has been logged and will be fixed as soon as possible.'

    postalScroll.bottomOffset = 100

    content = $ '#content'
    @appRouter = new App.Router(content: content, menu: $('#menu'), messages: $('#messages'))
    @batchesRouter = new Batches.Router(content: content)
    @adminRouter = new Admin.Router(content: content)

    Backbone.history.on 'route', -> postal.publish('route', window.location.hash.substr(1))
    Backbone.history.start()