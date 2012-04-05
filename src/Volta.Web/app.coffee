define ['jquery', 'backbone', 'underscore', 'text!about.html', 'data'], ($, Backbone, _, aboutTemplate, data) ->

    class MenuView extends Backbone.View
        initialize: (options) ->
            Backbone.history.on 'route', @route, @
            _.bindAll @, 'route'
        route: (router, route) ->
            @$el.find('li').removeClass('active')
            baseRoute = window.location.hash.substr(1).split('/')[0]
            @$el.find("li[data-route='#{baseRoute}']").addClass('active')
    
    class Router extends Backbone.Router
        initialize: (options) ->
            @content = options.content
            @menuView = new MenuView(el: $('#menu'))
            @aboutTemplate = _.template(aboutTemplate, data.SystemInfo)
        routes:
            'about': 'about'
            'logout': 'logout'
        about: -> @content.html(@aboutTemplate)
        logout: -> $.post('logout', -> window.location = 'login/')

    Router: Router