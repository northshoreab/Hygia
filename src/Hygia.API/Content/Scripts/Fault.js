(function ($) {

    window.Fault = Backbone.Model.extend({});

    window.Faults = Backbone.Collection.extend({
        model: Fault,
        url: '/faults'
    });

    window.FaultView = Backbone.View.extend({
        template: '/content/templates/fault.html',
        tagName: 'li',
        initialize: function () {
            _.bindAll(this, 'render');
            this.model.bind('change', this.render);
        },
        render: function (done) {
            var view = this;

            if (typeof this.doneFunc !== "undefined")
                done = this.doneFunc;

            this.doneFunc = done;

            // Fetch the template, render it to the View element and call done.
            hygia.fetchTemplate(this.template, function (tmpl) {
                view.el.innerHTML = tmpl(view.model.toJSON());

                done(view.el);
            });
        }
    });

    window.FaultsView = Backbone.View.extend({
        template: '/content/templates/faults.html',
        tagName: 'div',
        initialize: function () {
            _.bindAll(this, 'render');
            this.collection.bind('reset', this.render);
        },
        render: function (done) {
            var view = this;

            if (typeof this.doneFunc !== "undefined")
                done = this.doneFunc;

            this.doneFunc = done;

            // Fetch the template, render it to the View element and call done.
            hygia.fetchTemplate(this.template, function (tmpl) {
                view.el.innerHTML = tmpl({});

                done(view.el);
                var $faults,
                    collection = view.collection;
                $faults = view.$('.faults');
                collection.each(function (album) {
                    var view2 = new FaultView({
                        model: album,
                        collection: collection
                    });
                    view2.render(function (el) {
                        $faults.append(el);
                    });
                });

            });
        }
    });

    window.currentFaults = new Faults();

    window.MainRouter = Backbone.Router.extend({
        routes: {
            '': 'home'
        },
        initialize: function () {
            this.faultsView = new FaultsView({
                collection: window.currentFaults
            });
        },
        home: function () {
            $main = $('#main');
            $main.empty();

            this.faultsView.render(function (el) { $main.append(el); });
        }
    });

    $(function () {
        window.App = new MainRouter();
        Backbone.history.start();
    });
})(jQuery);