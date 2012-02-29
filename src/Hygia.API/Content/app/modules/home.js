define([
  "namespace",

// Libs
  "use!backbone"

// Modules

// Plugins
],

function (namespace, Backbone) {

    // Create a new module
    var Home = namespace.module();

    // Example extendings
    Home.Model = Backbone.Model.extend({ /* ... */});
    Home.Collection = Backbone.Collection.extend({ /* ... */});
    Home.Router = Backbone.Router.extend({ /* ... */});

    // This will fetch the tutorial template and render it.
    Home.Views.Index = Backbone.View.extend({
        template: "/content/app/templates/home.index.html",

        render: function (done) {
            var view = this;

            // Fetch the template, render it to the View element and call done.
            namespace.fetchTemplate(this.template, function (tmpl) {
                view.el.innerHTML = tmpl();

                // If a done function is passed, call it with the element
                if (_.isFunction(done)) {
                    done(view.el);
                }
            });
        }
    });

    // Required, return the module for AMD compliance
    return Home;

});
