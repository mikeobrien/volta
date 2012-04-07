define(['backbone'], function(Backbone) {

    LazyCollection = Backbone.Collection.extend();

    LazyCollection.prototype.indexQuerystring = 'index';
    LazyCollection.prototype.index = 0;
    LazyCollection.prototype.lastLength = -1;
    LazyCollection.prototype.window = 0;

    LazyCollection.prototype.fetch = function(options) {
        var error, success, that = this;
        options || (options = {});
        
        if (options.reset) {
            this.index = 1;
            this.lastLength = 0;
            this.window = 0;
        } else {
            if (this.lastLength === this.length) return;
            this.index++;
            this.lastLength = this.length;
            this.window || (this.window = this.length);
            options.add = true;
        }
        
        options.data || (options.data = {});
        options.data[this.indexQuerystring] = this.index;
        success = options.success;
        
        options.success = function(model, resp) {
            that.window || (that.window = that.length);
            that.trigger('fetch:end', that.length > 0 && that.window <= that.length - that.lastLength);
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

    Backbone.LazyCollection = LazyCollection
});