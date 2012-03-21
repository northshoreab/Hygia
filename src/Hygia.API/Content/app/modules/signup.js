define([
  "watchr",

// Libs
  "use!backbone"

// Modules

// Plugins
],

function (watchr, Backbone) {

    var SignUp = watchr.module();

    SignUp.Models.User = Backbone.Model.extend({
        url: '/signup'
    });


    SignUp.Views.Index = Backbone.View.extend({
        template: '/content/app/templates/signup.index.html',
        initialize: function () {

        },
        events: { "submit #signupForm": "apply" },
        apply: function () {
            var emailField = $('input[name=email]');
            this.model.save({
                'email': emailField.val()
            },
            { 
                success: function () {
                    var complete = new SignUp.Views.SignupComplete({});

                    complete.render(function (el) { $("#main").html(el); });
                } 
            });
        },
        render: function (done) {
            var view = this;

            watchr.fetchTemplate(this.template, function (tmpl) {
                view.el.innerHTML = tmpl({});

                if (_.isFunction(done)) {
                    done(view.el);
                }
            });

            return view;
        }
    });

    SignUp.Views.SignupComplete = Backbone.View.extend({
        template: '/content/app/templates/signup.complete.html',
        initialize: function () {

        },
        render: function (done) {
            var view = this;

            watchr.fetchTemplate(this.template, function (tmpl) {
                view.el.innerHTML = tmpl({});

                if (_.isFunction(done)) {
                    done(view.el);
                }
            });

            return view;
        }
    });


    SignUp.Router = Backbone.Router.extend({
        routes: {
            "signup": "index"
        },
        index: function (hash) {
            var route = this;

            var model = new SignUp.Models.User({});
            var username = new SignUp.Views.Index({
                model: model
            });

            username.render(function (el) {
                $("#main").html(el);

                if (hash && !route._alreadyTriggered) {
                    Backbone.history.navigate("", false);
                    location.hash = hash;
                    route._alreadyTriggered = true;
                }
            });
        }
    });

    return SignUp;
});
