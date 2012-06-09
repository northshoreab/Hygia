WatchR.SignUp.Finalize = (function (WatchR, Backbone, $) {
    var Finalize = {};

    Finalize.View = WatchR.ItemView.extend({
        tagName: 'div',
        className: 'row-fluid',
        template: '#signup-finalize-template',
        events: {
            "click #btn-finalize": "finalize"
        },
        finalize: function () {
            alert("Finalize");
            /*
            var emailField = $('input[name=email]');
            this.model.save({
            'email': emailField.val()
            },
            {
            success: function () {
            WatchR.vent.trigger("signup:registered");
            }
            });
            */
        }
    });


    return Finalize;
})(WatchR, Backbone, $);

