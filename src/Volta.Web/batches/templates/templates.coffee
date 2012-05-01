define ['jquery', 'backbone', 'underscore', 'postal', 'common/views',
        'text!batches/templates/list-template.html', 
        'text!batches/templates/list-item-template.html', 
        'text!batches/templates/edit-template.html', 
        'text!batches/templates/add-template.html']
        , ($, Backbone, _, postal, Views, listTemplate, listItemTemplate, editTemplate, addTemplate) ->

    class Template extends Backbone.Model
        urlRoot : 'batches/templates'

    class AllTemplates extends Backbone.Collection
        model: Template
        url: 'batches/templates'

    class Templates extends Backbone.LazyCollection
        model: Template
        url: 'batches/templates'
        batchSize: 20

    class ListItemView extends Views.ListItemView
        itemName: 'Template'
        template: _.template listItemTemplate

    class ListView extends Views.ListView
        template: listTemplate
        itemView: ListItemView

    class EditView extends Views.EditView
        template: _.template editTemplate
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