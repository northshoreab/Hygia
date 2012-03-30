
WatchR.Routing = (function (WatchR, Backbone) {
    var Routing = {};

    Routing.showRoute = function () {
        var route = getRoutePath(arguments);
        Backbone.history.navigate(route, false);
    };

    var getRoutePath = function(routeParts) {
        var base = routeParts[0];
        var length = routeParts.length;
        var route = base;

        if (length > 1) {
            for (var i = 1; i < length; i++) {
                var arg = routeParts[i];
                if (arg) {
                    route = route + "/" + arg;
                }
            }
        }

        return route;
    };

    return Routing;
})(WatchR, Backbone);

