define ['jquery', 'backbone', 'underscore', 'postal']
        , ($, Backbone, _, postal) ->

    class ListItemView extends Backbone.View
        itemName: 'Item'
        tagName: 'tr'
        events:
            'click .delete': 'delete'
        initialize: ->
            _.bindAll @, 'render', 'delete'
            @model.on 'destroy', @remove, @
        render: ->
            @$el.html @template(@model.toJSON())
            @
        delete: -> 
            $.dialog
                title: 'Delete ' + @itemName
                body: 'Are you sure you want to delete this ' + @itemName.toLowerCase() + '?'
                button: 'Delete'
                command: => 
                    @model.destroy wait: true
                    true

    class ListView extends Backbone.LazyView
        itemsSelector: '.list-items'
        initialize: ->
            super()
            postal.channel('window.scroll.bottom').subscribe @more

    class EditView extends Backbone.View
        initialize: (options) ->
            _.bindAll @, 'render', 'save'
            @$el.delegate('.save', 'click', @save);
            @router = options.router
            @model.on 'change', @render, @
        render: ->
            @$el.html @template(@model.toJSON())
            @

    class AddView extends Backbone.View
        initialize: (options) ->
            _.bindAll @, 'render', 'save'
            @$el.delegate('.save', 'click', @save);
            @router = options.router
        render: ->
            @$el.html(if typeof @template == 'function' then @template() else @template)
            @

    ListItemView: ListItemView
    ListView : ListView
    EditView: EditView
    AddView: AddView