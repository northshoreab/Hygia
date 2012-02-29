define([
  "namespace",

// Libs
  "use!backbone"

// Modules

// Plugins
],

function (namespace, Backbone) {

    var Faults = namespace.module();

    Faults.Model = Backbone.Model.extend({});

    Faults.Collection = Backbone.Collection.extend({
        model: Faults.Model,
        url: "/faults"
    });

    Faults.Router = Backbone.Router.extend({});

    Faults.Views.Item = Backbone.View.extend({
        template: '/content/app/templates/faults.item.html',
        tagName: 'li',
        initialize: function () {
            _.bindAll(this, 'render');
            this.model.bind('change', this.render);
        },

        render: function (done) {
            var view = this;

            namespace.fetchTemplate(this.template, function (tmpl) {
                view.el.innerHTML = tmpl(view.model.toJSON());

                if (_.isFunction(done)) {
                    done(view.el);
                }
            });

            return view;
        }
    });

    Faults.Views.List = Backbone.View.extend({
        template: '/content/app/templates/faults.list.html',
        tagName: 'ul',
        className: 'faults',
        initialize: function () {
            _.bindAll(this, 'render');
            this.collection.bind('reset', this.render);
        },
        render: function (done) {
            var view = this;

            namespace.fetchTemplate(this.template, function (tmpl) {
                view.el.innerHTML = tmpl();

                view.collection.each(function (fault) {
                    var itemView = new Faults.Views.Item({ model: fault });                    
                    view.el.appendChild(itemView.render().el);
                });

                if (_.isFunction(done)) {
                    done(view.el);
                }

            });

            return view;
        }
    });

    // Required, return the module for AMD compliance
    return Faults;

});
