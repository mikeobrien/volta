define ['jquery', 'backbone', 'underscore', 'postal', 'data', 
        'text!error-template.html', 'text!about.html']
        , ($, Backbone, _, postal, data, errorTemplate, aboutTemplate) ->

    class MenuView extends Backbone.View
        initialize: (options) ->
            _.bindAll @, 'route'
            postal.subscribe 'route', @route
        route: (url) ->
            @$el.find('li').removeClass('active')
            baseRoute = url.split('/')[0]
            @$el.find("li[data-route='#{baseRoute}']").addClass('active')
    
    class ErrorView extends Backbone.View
        initialize: (options) ->
            _.bindAll @, 'render'
            postal.channel('ajax.error.*').subscribe @render
            postal.channel('error').subscribe @render
            @template = options.template
        render: (error) -> 
            message = $ @template { message: error.message }
            @$el.append message
            message.fadeIn 'slow'
            message.delay(3000).fadeOut('slow').hide

    class Router extends Backbone.Router
        initialize: (options) ->
            @content = options.content
            @aboutTemplate = _.template aboutTemplate, data.SystemInfo
        routes:
            'about': 'about'
            'logout': 'logout'
        about: -> @content.html(@aboutTemplate)
        logout: -> $.post('logout', -> window.location = 'login/')

    start: (menu, messages, content) ->
        @menuView = new MenuView el: menu
        @errorView = new ErrorView el: messages, template: _.template errorTemplate
        @router = new Router content: content