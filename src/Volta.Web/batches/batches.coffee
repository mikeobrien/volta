define ['jquery', 'backbone', 'underscore', 'postal',
        'batches/schedules/schedules',
        'batches/templates/templates',
        'text!batches/list-template.html', 
        'text!batches/list-item-template.html', 
        'text!batches/edit-template.html', 
        'text!batches/add-template.html']
        , ($, Backbone, _, postal, Schedules, Templates, listTemplate, listItemTemplate, editTemplate, addTemplate) ->

    class Batch extends Backbone.Model
        urlRoot : 'batches'

    class Batches extends Backbone.LazyCollection
        model: Batch
        url: 'batches'
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
                body: 'Are you sure you want to delete this batch?'
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
            @model.save
                name: @$('#name').val()
            , 
                success: => @router.navigate 'batches', trigger: true
                wait: true
            return false

    class AddView extends Backbone.View
        events:
            'click .save': 'save'
        template: _.template addTemplate
        initialize: (options) ->
            _.bindAll @, 'render', 'save'
            @router = options.router
        render: ->
            @$el.html @template()
            @
        save: ->
            if @$el.validateHasValue('#file', 'No file selected') then return false
            $('.save').attr('disabled', true)
            progress = $ '.progress .bar'
            @$('form').ajaxSubmit
                url: 'batches'
                type: 'POST'
                dataType: 'json'
                success: (data) => @router.navigate 'batches/edit/' + data.id, trigger: true
                error: => $('.save').attr('disabled', false)
                uploadProgress: (event, position, total, percentComplete) ->
                    progress.width(percentComplete + '%')
            return false

    class Router extends Backbone.Router
        initialize: (options) ->
            @content = options.content
        routes:
            'batches': 'batches'
            'batches/add': 'addBatch'
            'batches/edit/:id': 'editBatch'
            'batches/schedules': 'schedules'
            'batches/schedules/add': 'addSchedule'
            'batches/schedules/edit/:id': 'editSchedule'
            'batches/templates': 'templates'
            'batches/templates/add': 'addTemplate'
            'batches/templates/edit/:id': 'editTemplate'
        batches: ->
            batches = new Batches()
            @render new ListView(collection: batches).render().el
            batches.fetch()
        addBatch: ->
            @render new AddView(router: @).render().el
        editBatch: (id) ->
            batch = new Batch id: id
            @render new EditView(model: batch, router: @).el
            batch.fetch()
        schedules: ->
            schedules = new Schedules.Schedules()
            @render new Schedules.ListView(collection: schedules).render().el
            schedules.fetch()
        addSchedule: ->
            @render new Schedules.AddView(router: @).render().el
        editSchedule: (id) ->
            schedule = new Schedules.Schedule id: id
            @render new Schedules.EditView(model: schedule, router: @).el
            schedule.fetch()
        templates: ->
            templates = new Templates.Templates()
            @render new Templates.ListView(collection: templates).render().el
            templates.fetch()
        addTemplate: ->
            @render new Templates.AddView(router: @).render().el
        editTemplate: (id) ->
            template = new Templates.Template id: id
            @render new Templates.EditView(model: template, router: @).el
            template.fetch()
        render: (el) ->
            @content.empty()
            @content.append el

    start: (content) ->
        @router = new Router(content: content)