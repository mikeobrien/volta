define ['jquery', 'backbone', 'underscore', 
        'text!admin/users/enum-template.html', 
        'text!admin/users/edit-template.html', 
        'text!admin/users/add-template.html']
        , ($, Backbone, _, enumTemplate, editTemplate, addTemplate) ->

    class EnumView extends Backbone.View
        initialize: ->
            _.bindAll @, 'render'
        render: ->
            @$el.html enumTemplate

    class EditView extends Backbone.View
        initialize: ->
            _.bindAll @, 'render'
        render: ->
            @$el.html editTemplate

    class AddView extends Backbone.View
        initialize: ->
            _.bindAll @, 'render'
        render: ->
            @$el.html addTemplate

    EnumView : EnumView
    EditView: EditView
    AddView: AddView