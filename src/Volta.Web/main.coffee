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

require ['jquery', 'backbone', 'app', 'batches/batches', 'admin/admin', 'bootstrap', '_config'], ($, Backbone, App, Batches, Admin) ->
    content = $ '#content'
    @appRouter = new App.Router(content: content)
    @batchesRouter = new Batches.Router(content: content)
    @adminRouter = new Admin.Router(content: content)
    Backbone.history.start()