define ['backbone'], (Backbone) ->
	class Router extends Backbone.Router
		routes:
			'': 'dashboard'
		dashboard: ->
			console.log 'dashboard...'

	Router: Router