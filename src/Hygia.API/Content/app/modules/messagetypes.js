define([
  "namespace",

// Libs
  "use!backbone"

// Modules

// Plugins
],

function (namespace, Backbone) {

    var MessageTypes = namespace.module();

    MessageTypes.Models.Item = Backbone.Model.extend({});

    MessageTypes.Models.Detail = Backbone.Model.extend({
        url: function () { return '/messagetypes/' + this.id; }
    });

    MessageTypes.Collections.List = Backbone.Collection.extend({
        model: MessageTypes.Models.Item,
        url: "/messagetypes"
    });

    MessageTypes.Views.Detail = Backbone.View.extend({
        template: '/content/app/templates/messagetypes.detail.html',
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

    MessageTypes.Views.Item = Backbone.View.extend({
        template: '/content/app/templates/messagetypes.tr.html',
        tagName: 'tr',
        events: { },
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

    MessageTypes.Views.List = Backbone.View.extend({
        template: '/content/app/templates/messagetypes.tbl.html',
        tagName: 'div',
        className: 'messagetypes',
        events: {
            "click #btn-reload-messagetypes": "reload"
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

                view.collection.each(function (messagetype) {
                    var itemView = new MessageTypes.Views.Item({ model: messagetype });
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

    MessageTypes.Router = Backbone.Router.extend({
        routes: {
            "messageType": "messageTypes",
            "messageType/:id": "messageTypeDetail"
        },
        messageTypeDetail: function (id) {
            var route = this;

            var detailModel = new MessageTypes.Models.Detail({ "id": id });

            detailModel.fetch({
                success: function (model, response) {
                    var detailView = new MessageTypes.Views.Detail({ "model": model });
                    detailView.render(function (el) {
                        $("#main").html(el);
                    });
                }
            });
        },
        messageTypes: function (hash) {
            var route = this;

            var currentMessageTypes = new MessageTypes.Collections.List();

            currentMessageTypes.fetch({
                success: function (collection, response) {
                    var messageTypes = new MessageTypes.Views.List({ "collection": collection });
                    messageTypes.render(function (el) {
                        $("#main").html(el);

                        if (hash && !route._alreadyTriggered) {
                            Backbone.history.navigate("", false);
                            location.hash = hash;
                            route._alreadyTriggered = true;
                        }
                    });                     
                }
            });            
        }
    });

    return MessageTypes;
});
