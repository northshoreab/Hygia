define([
  "watchr",

// Libs
  "use!backbone"

// Modules

// Plugins
],

function (watchr, Backbone) {

    var Faults = watchr.module();

    Faults.Models.Item = Backbone.Model.extend({});

    Faults.Models.Detail = Backbone.Model.extend({
        url: function () { return '/faults/' + this.id; }
    });

    Faults.Collections.List = Backbone.Collection.extend({
        model: Faults.Models.Item,
        url: "/faults"
    });

    Faults.Views.Detail = Backbone.View.extend({
        template: '/content/app/templates/faults.detail.html',
        initialize: function () {
            _.bindAll(this, 'render');
            this.model.bind('change', this.render);
        },
        render: function (done) {
            var view = this;

            watchr.fetchTemplate(this.template, function (tmpl) {
                view.el.innerHTML = tmpl(view.model.toJSON());

                if (_.isFunction(done)) {
                    done(view.el);
                }
            });

            return view;
        }
    });

    Faults.Views.Item = Backbone.View.extend({
        template: '/content/app/templates/faults.tr.html',
        tagName: 'tr',
        events: {
            "click a.action-retry-fault": "retry",
            "click a.action-archive-fault": "archive"
        },
        initialize: function () {
            _.bindAll(this, 'render');
            this.model.bind('change', this.render);
        },
        retry: function () {
            $.ajax({
                type: 'POST',
                url: '/faults/retry',
                data: { FaultId: this.model.get('FaultId') },
                success: function () {
                    console.log("now, remove this model: " + this.model.id + " - not impl.");
                },
                dataType: "json"
            });
        },
        archive: function () {
            $.ajax({
                type: 'POST',
                url: '/faults/archive',
                data: { FaultId: this.model.get('FaultId') },
                success: function () { console.log("success!"); },
                dataType: "json"
            });
        },
        render: function (done) {
            var view = this;

            watchr.fetchTemplate(this.template, function (tmpl) {
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

            watchr.fetchTemplate(this.template, function (tmpl) {
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

    Faults.Router = Backbone.Router.extend({
        routes: {
            "fault": "list",
            "fault/:id": "detail"
        },
        detail: function (id) {
            var route = this;

            var detailModel = new Faults.Models.Detail({ "id": id });

            detailModel.fetch({
                success: function (model, response) {
                    var detailView = new Faults.Views.Detail({ "model": model });
                    detailView.render(function (el) {
                        $("#main").html(el);
                    });
                }
            });
        },
        list: function (hash) {
            var route = this;

            var faults = new Faults.Views.List({ "collection": watchr.app.Current.Faults });

            faults.render(function (el) {
                $("#main").html(el);

                if (hash && !route._alreadyTriggered) {
                    Backbone.history.navigate("", false);
                    location.hash = hash;
                    route._alreadyTriggered = true;
                }
            });
        }
    });

    return Faults;
});
