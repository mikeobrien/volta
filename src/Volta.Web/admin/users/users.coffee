define ['jquery', 'backbone', 'underscore', 'postal',
        'text!admin/users/list-template.html', 
        'text!admin/users/list-item-template.html', 
        'text!admin/users/edit-template.html', 
        'text!admin/users/add-template.html']
        , ($, Backbone, _, postal, listTemplate, listItemTemplate, editTemplate, addTemplate) ->

    class User extends Backbone.Model
        urlRoot : '/admin/users'

    class Users extends Backbone.LazyCollection
        model: User
        url: '/admin/users'
        batchSize: 20

    class ListItemView extends Backbone.View
        tagName: 'tr'
        events:
            'click .delete': 'delete'
        template: _.template listItemTemplate
        initialize: ->
            _.bindAll @, 'render', 'delete'
            @model.on 'destroy', @remove, @
        render: ->
            @$el.html @template(@model.toJSON())
            @
        delete: -> 
            $.dialog
                title: 'Delete User'
                body: 'Are you sure you want to delete this user?'
                button: 'Delete'
                command: => 
                    @model.destroy wait: true
                    true

    class ListView extends Backbone.LazyView
        template: listTemplate
        itemsSelector: '.list-items'
        itemView: ListItemView
        initialize: ->
            super()
            postal.channel('window.scroll.bottom').subscribe @more

    class EditView extends Backbone.View
        events:
            'click .save': 'save'
        template: _.template editTemplate
        initialize: (options) ->
            _.bindAll @, 'render', 'save'
            @router = options.router
            @model.on 'change', @render, @
        render: ->
            @$el.html @template(@model.toJSON())
            @
        save: ->
            if @$el.validateHasValue('#username', 'Username cannot be blank') |
               @$el.validateMatch('#password', '#password2', 'Passwords do not match') then return false
            @model.save
                username: @$('#username').val()
                email: @$('#email').val()
                password: @$('#password').val()
                administrator: @$('#administrator').is(':checked')
            , 
                success: => @router.navigate 'admin/users', trigger: true
                wait: true
            return false

    class AddView extends Backbone.View
        events:
            'click .save': 'save'
        template: addTemplate
        initialize: (options) ->
            _.bindAll @, 'render', 'save'
            @router = options.router
            @model = new User()
        render: ->
            @$el.html @template
            @
        save: ->
            if @$el.validateHasValue('#username', 'Username cannot be blank') |
               (@$el.validateHasValue('#password', 'Password cannot be blank') ||
                @$el.validateMatch('#password', '#password2', 'Passwords do not match')) then return false
            @model.save
                username: @$('#username').val()
                email: @$('#email').val()
                password: @$('#password').val()
                administrator: @$('#administrator').is(':checked')
            , 
                success: => @router.navigate 'admin/users', trigger: true
                wait: true
            return false

    User: User
    Users: Users
    ListView : ListView
    EditView: EditView
    AddView: AddView