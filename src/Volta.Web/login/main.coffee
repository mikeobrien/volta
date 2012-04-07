require.config
    paths:
        "jquery": "../scripts/jquery/jquery"
        "underscore": "../scripts/underscore/underscore"
        "backbone": "../scripts/backbone/backbone"
        "bootstrap": "../scripts/bootstrap/bootstrap"

require ['jquery', 'login', 'bootstrap'], ($, Login) ->
	new Login.View(el: $ '#login').render()