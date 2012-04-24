//     backbone.lazy.js 0.1.0
//     (c) 2012 Mike O'Brien
//     May be freely distributed under the MIT license.
//     https://github.com/mikeobrien/jsplugins

(function() {
    var plugin = function(Backbone) {

        LazyCollection = Backbone.Collection.extend();

        LazyCollection.prototype.indexQuerystring = 'index';
        LazyCollection.prototype.index = 0;
        LazyCollection.prototype.lastLength = -1;

        LazyCollection.prototype.fetch = function(options) {
            var error, success, that = this;
            options || (options = {});
            
            if (options.reset) {
                this.index = 1;
                this.lastLength = 0;
            } else {
                if (this.lastLength === this.length) return;
                this.index++;
                this.lastLength = this.length;
                options.add = true;
            }
            
            options.data || (options.data = {});
            options.data[this.indexQuerystring] = this.index;
            success = options.success;
            
            options.success = function(model, resp) {
                that.trigger('fetch:end', that.length > 0 && that.batchSize <= that.length - that.lastLength);
                if (success) return success(model, resp);
            };
            
            error = options.error;
            
            options.error = function(originalModel, resp, options) {
                that.trigger('fetch:end', false);
                if (error) return error(originalModel, resp, options);
            };
            
            this.trigger('fetch:start');
            Backbone.Collection.prototype.fetch.call(this, options);
        };

        LazyItemsView = Backbone.View.extend();

        LazyItemsView.prototype.initialize = function(options) {
            this.itemView = options.itemView;
            _.bindAll(this, 'render', 'renderResult');
            this.collection.on('reset', this.render, this);
            this.collection.on('add', this.renderResult, this);
        };

        LazyItemsView.prototype.render = function() {
            this.$el.empty();
            this.collection.each(this.renderResult);
        };

        LazyItemsView.prototype.renderResult = function(result) {
            var view;
            view = new this.itemView({ model: result });
            this.$el.append(view.render().el);
        };

        LazyView = Backbone.View.extend();
        
        LazyView.prototype.moreSelector = '.more'
        LazyView.prototype.loadingSelector = '.loading'
        
        LazyView.prototype.initialize = function() {
            _.bindAll(this, 'render', 'more', 'start', 'end');
            this.$el.delegate(this.moreSelector, 'click', this.more);
            this.collection.on('fetch:start', this.start, this);
            this.collection.on('fetch:end', this.end, this);
        };

        LazyView.prototype.render = function() {
            this.$el.html(this.template);
            this.itemsView = new LazyItemsView({
                el: this.$(this.itemsSelector),
                collection: this.collection,
                itemView: this.itemView
            });
            return this;
        }

        LazyView.prototype.more = function() {
            this.collection.fetch();
        };

        LazyView.prototype.start = function() {
            this.$(this.loadingSelector).show();
            this.$(this.moreSelector).hide();
        };

        LazyView.prototype.end = function(more) {
            this.$(this.loadingSelector).hide();
            if (more) {
                this.$(this.moreSelector).show();
            } else {
                this.$(this.moreSelector).hide();
            }
        };

        Backbone.LazyCollection = LazyCollection  
        Backbone.LazyView = LazyView
    }
    
    if (typeof define === 'function' && define.amd) {
        define(['backbone'], plugin);
    } else { plugin(Backbone) }
})();