define ['jquery', 'backbone', 'underscore', 'postal',
        'text!batches/schedules/list-template.html', 
        'text!batches/schedules/list-item-template.html', 
        'text!batches/schedules/edit-template.html', 
        'text!batches/schedules/add-template.html']
        , ($, Backbone, _, postal, listTemplate, listItemTemplate, editTemplate, addTemplate) ->

    class Schedule extends Backbone.Model
        urlRoot : '/batches/schedules'

    class AllSchedules extends Backbone.Collection
        model: Schedule
        url: '/batches/schedules'

    class Schedules extends Backbone.LazyCollection
        model: Schedule
        url: '/batches/schedules'
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
                title: 'Delete Schedule'
                body: 'Are you sure you want to delete this schedule?'
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
            if @$el.validate('#name', ((x) -> x == ''), 'Name cannot be blank') |
               @$el.validate('#file', ((x) -> x == ''), 'File cannot be empty') then return false
            @model.save
                name: @$('#name').val()
                file: @$('#file').val()
            , 
                success: => @router.navigate 'batches/schedules', trigger: true
                wait: true
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
            if @$el.validate('#name', ((x) -> x == ''), 'Name cannot be blank') |
               @$el.validate('#file', ((x) -> x == ''), 'No file selected') then return false
            @$('form').ajaxSubmit
                url: '/batches/schedules'
                type: 'POST'
                success: => @router.navigate 'batches/schedules', trigger: true
            return false

    Schedule: Schedule
    Schedules: Schedules
    AllSchedules: AllSchedules
    ListView : ListView
    EditView: EditView
    AddView: AddView