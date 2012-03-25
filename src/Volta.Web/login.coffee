define ['backbone'], (Backbone) ->
	class Router extends Backbone.Router
		routes:
			'login': 'login'
		login: ->
			console.log 'login...'