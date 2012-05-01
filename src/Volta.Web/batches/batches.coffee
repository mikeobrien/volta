define ['jquery', 'backbone', 'underscore', 'postal', 'common/views', 
        'text!batches/list-template.html', 
        'text!batches/list-item-template.html', 
        'text!batches/edit-template.html', 
        'text!batches/add-template.html']
        , ($, Backbone, _, postal, Views, listTemplate, listItemTemplate, editTemplate, addTemplate) ->

    class Batch extends Backbone.Model
        urlRoot : 'batches'

    class Batches extends Backbone.LazyCollection
        model: Batch
        url: 'batches'
        batchSize: 20

    class ListItemView extends Views.ListItemView
        itemName: 'Batch'
        template: _.template listItemTemplate

    class ListView extends Views.ListView
        template: listTemplate
        itemView: ListItemView

    class EditView extends Views.EditView
        template: _.template editTemplate
        save: ->
            if @$el.validateHasValue('#name', 'Name cannot be blank') then return false
            @model.save
                name: @$('#name').val()
            , 
                success: => @router.navigate 'batches', trigger: true
                wait: true
            return false

    class AddView extends Views.AddView
        template: _.template addTemplate
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

    Batch: Batch
    Batches: Batches
    ListView : ListView
    EditView: EditView
    AddView: AddView