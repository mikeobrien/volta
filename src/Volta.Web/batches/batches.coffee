define ['jquery', 'backbone', 'underscore', 'batches/schedules/schedules'], ($, Backbone, _, Schedules) ->

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
        batches: -> console.log 'batches...'
        addBatch: -> console.log 'addBatch...'
        editBatch: (id) -> console.log "editBatch(#{id})..."
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
        render: (el) ->
            @content.empty()
            @content.append el

    start: (content) ->
        @router = new Router(content: content)