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

require ['args', 'jquery', 'dashboard'], (args, $, Dashboard) ->
	content = $ '#content'
	@dashboardRouter = new Dashboard.Router(content: content)
	Backbone.history.start()