WatchR.SignUp.Index = (function (WatchR, Backbone, $) {
    var Index = {};

    Index.User = WatchR.Model.extend({
        url: "/api/usermanagement/useraccounts/"
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
                    WatchR.vent.trigger("signup:registered");
                }
            });
        }
    });

    Index.Registered = WatchR.ItemView.extend({
        tagName: 'div',
        className: 'row-fluid',
        template: '#signup-registered-template'
    });

    return Index;
})(WatchR, Backbone, $);

