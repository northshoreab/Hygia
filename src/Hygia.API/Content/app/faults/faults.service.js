WatchR.Faults.Service = (function (WatchR, Backbone) {
    var Service = {};

    WatchR.vent.bind("faults:retry:all", function () {
        Service.retryAll();
    });

    Service.retryAll = function () {
        console.log("retry all");
    };

    return Service;
})(WatchR, Backbone);