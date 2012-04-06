define ['jquery', 'backbone', 'underscore', 'admin/users/users'], ($, Backbone, _, Users) ->

    class Router extends Backbone.Router
        initialize: (options) ->
            @content = options.content
        routes:
            'admin/users': 'users'
            'admin/users/add': 'addUser'
            'admin/users/edit/:id': 'editUser'
        users: -> (@view = new Users.EnumView(el: @content)).render()
        addUser: -> (@view = new Users.AddView(el: @content)).render()
        editUser: (id) -> (@view = new Users.EditView(el: @content)).render()

    Router: Router