define ['backbone', 'underscore', 'text!about.html', 'data'], (Backbone, _, template, data) ->
    class Router extends Backbone.Router
        initialize: (options) ->
            @content = options.content
            @template = _.template(template, data.SystemInfo)
        routes:
            'about': 'about'
        about: ->
            @content.html(@template)

    Router: Router