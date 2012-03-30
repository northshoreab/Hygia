WatchR.SignUp.Index = (function (WatchR, Backbone, $) {
    var Index = {};

    Index.User = WatchR.Model.extend({
        url: "/signup"
    });

    Index.View = WatchR.ItemView.extend({
        tagName: 'div',
        className: 'row-fluid',
        template: '#signup-index-template',
        events: { "submit #signupForm": "apply" },
        apply: function () {
            var emailField = $('input[name=email]');
            this.model.save({
                'email': emailField.val()
            },
            {
                success: function () {
                    //var complete = new SignUp.Views.SignupComplete({});
                    console.log("Signup complete");
                    //complete.render(function (el) { $("#main").html(el); });
                }
            });
        }
    });

    return Index;
})(WatchR, Backbone, $);

