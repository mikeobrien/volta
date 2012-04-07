define ['jquery', 'backbone', 'underscore'], ($, Backbone, _) ->

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
        schedules: -> console.log 'schedules...'
        addSchedule: -> console.log 'addSchedule...'
        editSchedule: (id) -> console.log "editSchedule(#{id})..."

    start: (content) ->
        @router = new Router(content: content)