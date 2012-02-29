define([
  "namespace",

// Libs
  "use!backbone"

// Modules

// Plugins
],

function (namespace, Backbone) {

    var FaultTbl = namespace.module();

    FaultTbl.Model = Backbone.Model.extend({});

    FaultTbl.Collection = Backbone.Collection.extend({
        model: FaultTbl.Model,
        url: "/faults"
    });

    FaultTbl.Views.Item = Backbone.View.extend({
        template: '/content/app/templates/faults.tr.html',
        tagName: 'tr',
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

    FaultTbl.Views.List = Backbone.View.extend({
        template: '/content/app/templates/faults.tbl.html',
        tagName: 'div',
        className: 'faults',
        initialize: function () {
            _.bindAll(this, 'render');
            this.collection.bind('reset', this.render);
        },
        render: function (done) {
            var view = this;

            namespace.fetchTemplate(this.template, function (tmpl) {
                view.el.innerHTML = tmpl();

                $tbody = view.$('table > tbody');

                view.collection.each(function (fault) {
                    var itemView = new FaultTbl.Views.Item({ model: fault });
                    //view.el.appendChild(itemView.render().el);
                    $tbody.append(itemView.render().el);
                });

                $table = view.$('table');
                $table.append($tbody);
                
                if (_.isFunction(done)) {
                    done(view.el);
                }

            });

            return view;
        }
    });

    // Required, return the module for AMD compliance
    return FaultTbl;

});
