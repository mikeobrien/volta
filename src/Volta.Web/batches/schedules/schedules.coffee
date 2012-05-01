define ['jquery', 'backbone', 'underscore', 'postal', 'common/views',
        'text!batches/schedules/list-template.html', 
        'text!batches/schedules/list-item-template.html', 
        'text!batches/schedules/edit-template.html', 
        'text!batches/schedules/add-template.html']
        , ($, Backbone, _, postal, Views, listTemplate, listItemTemplate, editTemplate, addTemplate) ->

    class Schedule extends Backbone.Model
        urlRoot : 'batches/schedules'

    class AllSchedules extends Backbone.Collection
        model: Schedule
        url: 'batches/schedules'

    class Schedules extends Backbone.LazyCollection
        model: Schedule
        url: 'batches/schedules'
        batchSize: 20

    class ListItemView extends Views.ListItemView
        itemName: 'Schedule'
        template: _.template listItemTemplate

    class ListView extends Views.ListView
        template: listTemplate
        itemView: ListItemView

    class EditView extends Views.EditView
        template: _.template editTemplate
        save: ->
            if @$el.validateHasValue('#name', 'Name cannot be blank') |
               @$el.validateHasValue('#file', 'File cannot be empty') then return false
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
            if @$el.validateHasValue('#name', 'Name cannot be blank') |
               @$el.validateHasValue('#file', 'No file selected') then return false
            @$('form').ajaxSubmit
                url: 'batches/schedules'
                type: 'POST'
                success: => @router.navigate 'batches/schedules', trigger: true
            return false

    Schedule: Schedule
    Schedules: Schedules
    AllSchedules: AllSchedules
    ListView : ListView
    EditView: EditView
    AddView: AddView