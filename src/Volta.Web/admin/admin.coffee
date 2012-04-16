define ['jquery', 'backbone', 'underscore', 'admin/users/users'], ($, Backbone, _, Users) ->

    class Router extends Backbone.Router
        initialize: (options) ->
            @content = options.content
        routes:
            'admin/users': 'users'
            'admin/users/add': 'addUser'
            'admin/users/edit/:id': 'editUser'
        users: -> 
            users = new Users.Users()
            @render new Users.ListView(collection: users).render().el
            users.fetch()
        addUser: -> 
            @render new Users.AddView(router: @).render().el
        editUser: (id) -> 
            user = new Users.User id: id
            @render new Users.EditView(model: user, router: @).el
            user.fetch()
        render: (el) ->
            @content.empty()
            @content.append el

    start: (content) ->
        @router = new Router(content: content)