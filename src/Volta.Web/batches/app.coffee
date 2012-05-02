define ['jquery', 'backbone', 'underscore', 'postal',
        'batches/batches',
        'batches/schedules/schedules',
        'batches/templates/templates']
        , ($, Backbone, _, postal, Batches, Schedules, Templates) ->

    class Router extends Backbone.Router
        initialize: (options) ->
            @content = options.content
        routes:
            'batches': 'batches'
            'batches/add': 'addBatch'
            'batches/edit/:id': 'editBatch'
            'batches/presentation/:id': 'generatePresentation'
            'batches/schedules': 'schedules'
            'batches/schedules/add': 'addSchedule'
            'batches/schedules/edit/:id': 'editSchedule'
            'batches/templates': 'templates'
            'batches/templates/add': 'addTemplate'
            'batches/templates/edit/:id': 'editTemplate'
        batches: ->
            batches = new Batches.Batches()
            @render new Batches.ListView(collection: batches).render().el
            batches.fetch()
        addBatch: ->
            @render new Batches.AddView(router: @).render().el
        editBatch: (id) ->
            batch = new Batches.Batch id: id
            @render new Batches.EditView(model: batch, router: @).el
            batch.fetch()
        generatePresentation: (id) ->
            @render new Batches.PresentationView(batchId: id, router: @).el
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