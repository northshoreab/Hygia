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

    Faults.Views.Item = Backbone.View.extend({
        template: '/content/app/templates/faults.tr.html',
        tagName: 'tr',
        events: {
            "click button.btn-retry-fault": "retry",
            "click button.btn-archive-fault": "archive"
        },
        initialize: function () {
            _.bindAll(this, 'render');
            this.model.bind('change', this.render);
        },
        retry: function () {
            $.ajax({
                type: 'POST',
                url: '/faults/retry',
                data: { FaultEnvelopeId : this.model.get('FaultEnvelopeId') },
                success: function () { console.log("success!"); },
                dataType: "json"
            });
        },
        archive: function () {
            $.ajax({
                type: 'POST',
                url: '/faults/archive',
                data: { FaultEnvelopeId: this.model.get('FaultEnvelopeId') },
                success: function () { console.log("success!"); },
                dataType: "json"
            });
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
        template: '/content/app/templates/faults.tbl.html',
        tagName: 'div',
        className: 'faults',
        events: {
            "click #btn-reload-faults": "reload"
        },
        initialize: function () {
            _.bindAll(this, 'render');
            this.collection.bind('reset', this.render);
        },
        reload: function () {
            this.collection.fetch();
        },
        render: function (done) {
            var view = this;

            namespace.fetchTemplate(this.template, function (tmpl) {
                view.el.innerHTML = tmpl();

                $tbody = view.$('table > tbody');

                view.collection.each(function (fault) {
                    var itemView = new Faults.Views.Item({ model: fault });
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

    return Faults;
});
