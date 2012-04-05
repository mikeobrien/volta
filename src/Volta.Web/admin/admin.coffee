define ['jquery', 'backbone', 'underscore'], ($, Backbone, _) ->

    class Router extends Backbone.Router
        initialize: (options) ->
            @content = options.content
        routes:
            'admin/users': 'users'
            'admin/users/add': 'addUser'
            'admin/users/edit/:id': 'editUser'
        users: -> console.log 'users...'
        addUser: -> console.log 'addUser...'
        editUser: (id) -> console.log "editUser(#{id})..."

    Router: Router