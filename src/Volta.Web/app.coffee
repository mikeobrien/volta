define ['jquery', 'backbone', 'underscore', 'postal', 'mustache', 'data', 
        'text!error-template.html', 
        'text!about.html', 
        'text!login-template.html',
        'text!dashboard-template.html']
        , ($, Backbone, _, postal, Mustache, data, errorTemplate, aboutTemplate, loginTemplate, dashboardTemplate) ->

    class NavBarView extends Backbone.View
        events:
            'click .logout': 'logout'
        initialize: (options) ->
            _.bindAll @, 'route', 'logout'
            postal.subscribe 'route', @route
        route: (url) ->
            @$el.find('.nav li').removeClass('active')
            baseRoute = url.split('/')[0]
            @$el.find(".nav li[data-route='#{baseRoute}']").addClass('active')
        logout: -> $.post('logout', -> window.location = 'login/')
    
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
            '': 'dashboard'
            'about': 'about'
        dashboard: ->
            $.get 'dashboard', (data) => @render(Mustache.render(dashboardTemplate, data))
        about: -> @render @aboutTemplate
        render: (el) ->
            @content.empty()
            @content.append el

    start: (menu, messages, content) ->
        @navBarView = new NavBarView el: menu
        @errorView = new ErrorView el: messages
        @loginView = new LoginView 
        @router = new Router content: content