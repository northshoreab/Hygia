
WatchR.Model = Backbone.Model.extend({

});

WatchR.Session = Backbone.Model.extend({
    access_token: null,
    user_id: null,
    load: function () {
        access_token = Utils.getCookie("access_token");
        user_id = Utils.getCookie("user_id");
    },
    isAuthenticated: function () {
        return access_token != null;
    },
    hasUser: function () {
        return user_id != null;
    }

});
