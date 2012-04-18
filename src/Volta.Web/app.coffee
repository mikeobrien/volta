define ['jquery', 'backbone', 'underscore', 'postal', 'data', 
        'text!error-template.html', 'text!about.html', 'text!login-template.html']
        , ($, Backbone, _, postal, data, errorTemplate, aboutTemplate, loginTemplate) ->

    class MenuView extends Backbone.View
        initialize: (options) ->
            _.bindAll @, 'route'
            postal.subscribe 'route', @route
        route: (url) ->
            @$el.find('li').removeClass('active')
            baseRoute = url.split('/')[0]
            @$el.find("li[data-route='#{baseRoute}']").addClass('active')
    
    class ErrorView extends Backbone.View
        template: _.template errorTemplate
        initialize: (options) ->
            _.bindAll @, 'render'
            postal.channel('ajax.error.*').subscribe @render
            postal.channel('error').subscribe @render
        render: (error) -> 
            if error.status == 401 then return
            message = $ @template { message: error.message }
            @$el.append message
            message.fadeIn 'slow'
            message.delay(3000).fadeOut('slow').hide

    class LoginView extends Backbone.View
        template: loginTemplate
        initialize: (options) ->
            _.bindAll @, 'render'
            postal.channel('ajax.error.401').subscribe @render
        render: (error) -> 
            options = error.settings
            $.dialog
                title: 'Login'
                body: @template
                button: 'Login'
                initialize: (dialog) -> 
                    dialog.find('.username').focus()
                    dialog.find('.password').keyup((e) => if e.keyCode == 13 then dialog.find('.ok').click())
                command: (dialog) =>
                    showError = (message) =>  
                        error = dialog.find('.error-message')
                        error.html message
                        error.show()
                        error.delay(3000).fadeOut('slow').hide
                    request =
                        Username: dialog.find('.username').val()
                        Password: dialog.find('.password').val()
                    $.post('login', request)
                        .success (response) =>
                            if response.Success 
                                $.ajax(options)
                                dialog.modal('hide')
                            else showError 'Your username or password was not valid.'
                        .error (response) => 
                            showError 'Unable to access the server.'
                    false

    class Router extends Backbone.Router
        aboutTemplate: _.template aboutTemplate, data.SystemInfo
        initialize: (options) ->
            @content = options.content
        routes:
            'about': 'about'
            'logout': 'logout'
        about: -> 
            @content.empty()
            @content.html @aboutTemplate
        logout: -> $.post('logout', -> window.location = 'login/')

    start: (menu, messages, content) ->
        @menuView = new MenuView el: menu
        @errorView = new ErrorView el: messages
        @loginView = new LoginView 
        @router = new Router content: content