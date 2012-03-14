define([
  "watchr",

// Libs
  "use!backbone"

// Modules

// Plugins
],

function (watchr, Backbone) {

    // Create a new module
    var Faq = watchr.module();

    // Example extendings
    Faq.Model = Backbone.Model.extend({ /* ... */ });
    Faq.Collection = Backbone.Collection.extend({ /* ... */ });
    Faq.Router = Backbone.Router.extend({ routes: { "faq": "index" },
        index: function (hash) {
            var route = this;
            var v = new Faq.Views.Index();

            v.render(function (el) {
                $("#main").html(el);

                // Fix for hashes in pushState and hash fragment
                if (hash && !route._alreadyTriggered) {
                    // Reset to home, pushState support automatically converts hashes
                    Backbone.history.navigate("", false);

                    // Trigger the default browser behavior
                    location.hash = hash;

                    // Set an internal flag to stop recursive looping
                    route._alreadyTriggered = true;
                }
            });
        } 
    });

    // This will fetch the tutorial template and render it.
    Faq.Views.Index = Backbone.View.extend({
        template: "/content/app/templates/faq.index.html",

        render: function (done) {
            var view = this;

            // Fetch the template, render it to the View element and call done.
            watchr.fetchTemplate(this.template, function (tmpl) {
                view.el.innerHTML = tmpl();

                // If a done function is passed, call it with the element
                if (_.isFunction(done)) {
                    done(view.el);
                }
            });
        }
    });

    // Required, return the module for AMD compliance
    return Faq;

});
