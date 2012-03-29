WatchR.Faults.Trend = (function (WatchR, Backbone) {
    var Trend = {};
    Trend.Trend = Backbone.Model.extend({});

    WatchR.addInitializer(function () {
        WatchR.Faults.trend = new Trend.Trend();
    });

    return Trend;
})(WatchR, Backbone);