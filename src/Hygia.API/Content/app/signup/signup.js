
WatchR.SignUp = (function (WatchR, Backbone) {
    var SignUp = {};

    SignUp.show = function () {
    };

    SignUp.GithubSignUp = WatchR.Model.extend({
        url: "/signup/github"
    });

    SignUp.CurrentUser = WatchR.Model.extend({
    });


    SignUp.signUpWithGithub = function () {
        var code = Utils.getParameterByName('code');

        var model = new SignUp.GithubSignUp();
        model.save({ 'code': code },
            {
                success: function (userAccount) {
                    WatchR.currentUser = new SignUp.CurrentUser(userAccount);

                    WatchR.vent.trigger("signup:verified");
                }
            });
    };

    WatchR.vent.bind("signup:verified", function () {
        var finalizeView = new WatchR.SignUp.Finalize.View({
            model: WatchR.currentUser
        });

        WatchR.layout.main.show(finalizeView);
    });



    return SignUp;
})(WatchR, Backbone);


