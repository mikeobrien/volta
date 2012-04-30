require.config
    paths:
        "jquery": "../scripts/jquery/jquery"
        "underscore": "../scripts/underscore/underscore"
        "backbone": "../scripts/backbone/backbone"
        "bootstrap": "../scripts/bootstrap/bootstrap"
        "postal": "../scripts/postal/postal"

require ['jquery', 'login', 'postal', 'bootstrap'], ($, Login, postal) ->
    
    window.onerror = (message, source, line) -> 
        $.post '../errors', { source: source, line: line, message: message }
        postal.publish('error', message: 'Oops! A browser error has occured. This has been logged and will be fixed as soon as possible.')

    new Login.View(el: $ '#login').render()