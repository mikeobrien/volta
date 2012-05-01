define ['jquery', 'backbone', 'underscore', 'postal',
        'text!batches/templates/list-template.html', 
        'text!batches/templates/list-item-template.html', 
        'text!batches/templates/edit-template.html', 
        'text!batches/templates/add-template.html']
        , ($, Backbone, _, postal, listTemplate, listItemTemplate, editTemplate, addTemplate) ->

    class Template extends Backbone.Model
        urlRoot : 'batches/templates'

    class AllTemplates extends Backbone.Collection
        model: Template
        url: 'batches/templates'

    class Templates extends Backbone.LazyCollection
        model: Template
        url: 'batches/templates'
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
                title: 'Delete Template'
                body: 'Are you sure you want to delete this template?'
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
            if @$el.validateHasValue('#name', 'Name cannot be blank') then return false
            @$('form').ajaxSubmit
                url: 'batches/templates/' + @model.id
                type: 'POST'
                success: => @router.navigate 'batches/templates', trigger: true
            return false

    class AddView extends Backbone.View
        events:
            'click .save': 'save'
            'change #file': 'setName'
        template: addTemplate
        initialize: (options) ->
            _.bindAll @, 'render', 'save', 'setName'
            @router = options.router
        render: ->
            @$el.html @template
            @
        setName: (input) ->
            name = @$('#name')
            if name.val() then return
            path = @$('#file').val()
            start = path.lastIndexOf('\\') + 1
            name.val(path.substr(start, path.lastIndexOf('.') - start))
        save: ->
            if @$el.validateHasValue('#name', 'Name cannot be blank') |
               @$el.validateHasValue('#file', 'No file selected') then return false
            @$('form').ajaxSubmit
                url: 'batches/templates'
                type: 'POST'
                success: => @router.navigate 'batches/templates', trigger: true
            return false

    Template: Template
    Templates: Templates
    AllTemplates: AllTemplates
    ListView : ListView
    EditView: EditView
    AddView: AddView