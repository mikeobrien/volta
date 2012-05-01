define ['jquery', 'backbone', 'underscore', 'postal', 'common/views',
        'text!admin/users/list-template.html', 
        'text!admin/users/list-item-template.html', 
        'text!admin/users/edit-template.html', 
        'text!admin/users/add-template.html']
        , ($, Backbone, _, postal, Views, listTemplate, listItemTemplate, editTemplate, addTemplate) ->

    class User extends Backbone.Model
        urlRoot : '/admin/users'

    class Users extends Backbone.LazyCollection
        model: User
        url: '/admin/users'
        batchSize: 20

    class ListItemView extends Views.ListItemView
        itemName: 'User'
        template: _.template listItemTemplate

    class ListView extends Views.ListView
        template: listTemplate
        itemView: ListItemView

    class EditView extends Views.EditView
        template: _.template editTemplate
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

    class AddView extends Views.AddView
        template: addTemplate
        save: ->
            if @$el.validateHasValue('#username', 'Username cannot be blank') |
               (@$el.validateHasValue('#password', 'Password cannot be blank') ||
                @$el.validateMatch('#password', '#password2', 'Passwords do not match')) then return false
            new User().save
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