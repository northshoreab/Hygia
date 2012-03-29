WatchR.Faults.Spread = (function (WatchR, Backbone) {
    var Spread = {};

    Spread.Spread = Backbone.Model.extend({});

    WatchR.addInitializer(function () {
        WatchR.Faults.spread = new Spread.Spread();        
    });

    return Spread;
})(WatchR, Backbone);