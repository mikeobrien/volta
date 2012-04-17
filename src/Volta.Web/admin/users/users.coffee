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
        initialize: ->
            @template = _.template listItemTemplate
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

    class ListItemsView extends Backbone.View
        initialize: (options) ->
            _.bindAll @, 'render', 'renderResult'
            @collection.on 'reset', @render, @
            @collection.on 'add', @renderResult, @
        render: ->
            @$el.empty()
            @collection.each @renderResult
        renderResult: (result) ->
            view = new ListItemView 
                model: result
            @$el.append view.render().el

    class ListView extends Backbone.View
        events:
            'click .more': 'more'
        initialize: (options) ->
            _.bindAll @, 'render', 'more', 'start', 'end'
            @template = _.template listTemplate
            postal.channel('scroll.bottom').subscribe @more
            @collection.on 'fetch:start', @start, @
            @collection.on 'fetch:end', @end, @
        render: ->
            @$el.html @template()
            @itemsView = new ListItemsView
                el: @$ '.enum-items'
                collection: @collection
            @
        more: -> @collection.fetch()
        start: ->
            @$('.spinner').show()
            @$('.more').hide()
        end: (more) ->
            @$('.spinner').hide()
            if more then @$('.more').show() else @$('.more').hide()

    class EditView extends Backbone.View
        events:
            'click .save': 'save'
        initialize: (options) ->
            _.bindAll @, 'render', 'save'
            @router = options.router
            @template = _.template editTemplate
            @model.on 'change', @render, @
        render: ->
            @$el.html @template(@model.toJSON())
            @
        save: ->
            if @$('#password').val() != @$('#password2').val()
                @$('.password').addClass('error')
                @$('.password .message').html('Passwords do not match')
                return false
            else
                @$('.password').removeClass('error')
                @$('.password .message').html('')
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
        initialize: (options) ->
            _.bindAll @, 'render', 'save'
            @router = options.router
            @model = new User()
        render: ->
            @$el.html addTemplate
            @
        save: ->
            if @$('#username').val() == ''
                @$('.username').addClass('error')
                @$('.username .message').html('Username cannot be blank')
                return false
            if @$('#password').val() != @$('#password2').val()
                @$('.password').addClass('error')
                @$('.password .message').html('Passwords do not match')
                return false
            if @$('#password').val() == ''
                @$('.password').addClass('error')
                @$('.password .message').html('Password cannot be blank')
                return false
            else
                @$('.password').removeClass('error')
                @$('.password .message').html('')
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