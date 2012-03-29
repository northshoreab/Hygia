

WatchR.Collection = Backbone.Collection.extend({
  constructor: function(){
    var args = Array.prototype.slice.call(arguments);
    Backbone.Collection.prototype.constructor.apply(this, args);

    this.onResetCallbacks = new Backbone.Marionette.Callbacks();
    this.on("reset", this.runOnResetCallbacks, this);
  },

  onReset: function(callback){
    this.onResetCallbacks.add(callback);
  },

  runOnResetCallbacks: function(){
    this.onResetCallbacks.run(this, this);
  }
});