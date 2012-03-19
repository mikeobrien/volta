define ['backbone'], (Backbone) ->
    class Widget extends Backbone.Model
        setName: (first, last) ->
            this.set({name: first + ' ' + last})