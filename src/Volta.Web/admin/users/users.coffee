define ['jquery', 'backbone', 'underscore', 'postal',
        'text!admin/users/list-template.html', 
        'text!admin/users/list-item-template.html', 
        'text!admin/users/edit-template.html', 
        'text!admin/users/add-template.html']
        , ($, Backbone, _, postal, listTemplate, listItemTemplate, editTemplate, addTemplate) ->

    class User extends Backbone.Model

    class Collection extends Backbone.LazyCollection
        model: User
        url: '/admin/users'

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
            $.modal('<div style="background-color"></div>')
            @model.destroy wait: true

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
        more: -> @collection.fetch()
        start: ->
            @$('.spinner').show()
            @$('.more').hide()
        end: (more) ->
            @$('.spinner').hide()
            if more then @$('.more').show() else @$('.more').hide()

    class EditView extends Backbone.View
        initialize: ->
            _.bindAll @, 'render'
        render: ->
            @$el.html editTemplate

    class AddView extends Backbone.View
        initialize: ->
            _.bindAll @, 'render'
        render: ->
            @$el.html addTemplate

    Collection: Collection
    ListView : ListView
    EditView: EditView
    AddView: AddView