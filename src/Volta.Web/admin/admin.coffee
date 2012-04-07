define ['jquery', 'backbone', 'underscore', 'admin/users/users'], ($, Backbone, _, Users) ->

    class Router extends Backbone.Router
        initialize: (options) ->
            @content = options.content
        routes:
            'admin/users': 'users'
            'admin/users/add': 'addUser'
            'admin/users/edit/:id': 'editUser'
        users: -> 
            users = new Users.Collection()
            users.fetch()
            new Users.ListView(el: @content, collection: users).render()
        addUser: -> (@view = new Users.AddView(el: @content)).render()
        editUser: (id) -> (@view = new Users.EditView(el: @content)).render()

    start: (content) ->
        @router = new Router(content: content)