WatchR.Home = (function (WatchR, Backbone) {

    var Home = {};


    WatchR.vent.bind("home:layout:show", function () {
        var signUpView = new WatchR.SignUp.Index.View({
            model: new WatchR.SignUp.Index.User({})
        });

        WatchR.Home.layout.signup.show(signUpView);
    });

    Home.show = function () {
        WatchR.layout.main.show(WatchR.Home.layout);
    };


    WatchR.addInitializer(function () {
    });

    return Home;

})(WatchR, Backbone);