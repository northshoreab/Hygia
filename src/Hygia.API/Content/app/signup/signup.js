
WatchR.SignUp = (function (WatchR, Backbone) {
    var SignUp = {};

    SignUp.show = function () {
    };

    SignUp.GithubSignUp = WatchR.Model.extend({
        url: "/signup/github"
    });

    SignUp.signUpWithGithub = function () {
        var code = Utils.getParameterByName('code');

        var model = new SignUp.GithubSignUp();
        model.save({
            'code': code
        },
            {
                success: function () {
                    WatchR.session.load();
                }
            });
    };

    return SignUp;
})(WatchR, Backbone);


