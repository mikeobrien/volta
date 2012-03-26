define ['backbone', 'jquery', 'text!login.html'], (Backbone, $, loginTemplate) ->

	class View extends Backbone.View
		events:
			'click .login': 'login'
		initialize: (options) ->
			@router = options.router
			_.bindAll @, 'render', 'login', 'showError'
		render: ->
			@$el.html loginTemplate
			@$('.username').focus()
		login: (event) ->
			event.preventDefault()
			request =
				username: @$('.username').val()
				password: @$('.password').val()
			$.post('login', request)
				.success (response) =>
					if response.success then @router.navigate '', trigger: true
					else @showError 'Your username or password was not valid.'
				.error (response) => @showError if response.status == 500 then 'A system error has occured.' else 'Unable to access the server.'
		showError: (message) ->
			errorMessage = @$('.login-error-message')
			errorMessage.text(message)
			errorMessage.fadeIn 'slow'
			errorMessage.delay(2000).fadeOut('slow')

	class Router extends Backbone.Router
		initialize: (options) ->
			@content = options.content
			_.bindAll @, 'login'
		routes:
			'login': 'login'
			'logout': 'logout'
		login: ->
			new View(el: @content, router: @).render()
		logout: ->
			$.post 'logout', request, (response) =>
				if response.success then @router.navigate '', trigger: true
				else $('.access-denied-message').show()

	Router: Router
	login: ->
		loginUrl = '#login'
		if window.location.hash.substr(0, 6) != loginUrl then window.location = loginUrl