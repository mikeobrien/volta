define ['jquery', 'backbone', 'underscore', 'postal', 'mustache', 
        'common/views', 'batches/templates/templates',
        'text!batches/list-template.html', 
        'text!batches/list-item-template.html', 
        'text!batches/edit-template.html', 
        'text!batches/add-template.html',
        'text!batches/presentation-template.html']
        , ($, Backbone, _, postal, Mustache, Views, Templates, listTemplate, listItemTemplate, editTemplate, addTemplate, presentationTemplate) ->

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
        
    class EditView extends Backbone.View
        events:
            'click .save': 'save'
            'click .save-presentation': 'saveAndPresentation'
        initialize: (options) ->
            _.bindAll @, 'render', 'save', 'saveAndPresentation'
            @router = options.router
            @model.on 'change', @render, @
        render: ->
            @$el.html @template(@model.toJSON())
            @
        template: _.template editTemplate
        saveAndPresentation: -> @save(true)
        save: (createPresentation) ->
            if @$el.validateHasValue('#name', 'Name cannot be blank') then return false
            @model.save
                name: @$('#name').val()
            , 
                success: => 
                    if createPresentation == true
                        @router.navigate 'batches/presentation/' + @model.id, trigger: true
                    else @router.navigate 'batches', trigger: true  
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

    class PresentationView extends Backbone.View
        events:
            'click .generate': 'generate'
        template: presentationTemplate
        initialize: (options) ->
            _.bindAll @, 'render', 'generate', 'showError'
            @router = options.router
            @templates = new Templates.Templates()
            @batch = new Batch id: options.batchId
            templatesLoaded = new $.Deferred()
            batchLoaded = new $.Deferred()
            @templates.fetch(all: true, success: -> templatesLoaded.resolve())
            @batch.fetch(success: -> batchLoaded.resolve())
            $.when(templatesLoaded, batchLoaded).done(@render)
        render: ->
            @$el.html Mustache.render(@template, { batch: @batch.toJSON(), templates: @templates.toJSON() })
            @
        generate: ->
            format = @$('input[#format]:checked').val()
            request = batchId: @batch.id, templateId: @$('#templateId').val(), format: format
            @$('.progress .bar').width('100%')
            @$('.generate').attr("disabled", true)
            success = (result) => 
                if result.error == true then @showError(result.errorMessage) 
                else window.location = 'download/' + result.fileId + '/' + 
                        @batch.get('name').replace(/[\<\>\:\"\/\\\|\?\*]/g, '_') + '.' + format
            $.post('batches/presentation', request, success, 'json').always(=>             
                    @$('.progress .bar').width(0)
                    @$('.generate').attr("disabled", false))
            return false
        showError: (message) ->
            $.dialog
                title: 'Render Error'
                body: message.replace(/\r\n/g, '<br/>')
                button: 'Ok'
                cancel: false
                command: (dialog) => dialog.modal('hide') 

    Batch: Batch
    Batches: Batches
    ListView : ListView
    EditView: EditView
    AddView: AddView
    PresentationView: PresentationView