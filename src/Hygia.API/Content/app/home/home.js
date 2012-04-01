WatchR.Home = (function (WatchR, Backbone) {

    var Home = {};


    WatchR.vent.bind("home:layout:show", function () {
        var signUpView = new WatchR.SignUp.Index.View({
            model: new WatchR.SignUp.Index.User({})
        });

        WatchR.Home.layout.signup.show(signUpView);
    });

    WatchR.vent.bind("signup:registered", function () {
        WatchR.Home.layout.signup.show(new WatchR.SignUp.Index.Registered({}));
    });



    Home.show = function () {
        WatchR.layout.main.show(WatchR.Home.layout);
    };

   

    WatchR.addInitializer(function () {
    });

    return Home;

})(WatchR, Backbone);