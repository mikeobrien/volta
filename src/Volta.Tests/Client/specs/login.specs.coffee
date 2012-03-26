define ['login'], (Login) ->
	describe 'Login', ->
		describe 'login method', ->
			originalLocation = window.location
			afterEach ->
				window.location = originalLocation
			it 'should redirect to login page', ->
				window.location = '#users/enumerate'
				Login.login()
				expect(window.location.hash).toBe('#login')