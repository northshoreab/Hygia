require([
  "namespace",
// Libs
  "jquery",
  "use!backbone",
  "use!bootstrap",
// Modules  
  "modules/home",
  "modules/faults",
  "modules/messagetypes",
  "modules/utils"

],

function (namespace, jQuery, Backbone, Bootstrap, Home, Faults, MessageTypes, Utils) {

    // Defining the application router, you can attach sub routers here.
    var Router = Backbone.Router.extend({
        routes: {
            "": "index"
        },

        index: function (hash) {
            var route = this;
            var home = new Home.Views.Index();

            home.render(function (el) {
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

    // Shorthand the application namespace
    var app = namespace.app;

    // Treat the jQuery ready function as the entry point to the application.
    // Inside this function, kick-off all initialization, everything up to this
    // point should be definitions.
    jQuery(function ($) {
        app.router = new Router();
        app.faultRouter = new Faults.Router();
        app.messageTypeRouter = new MessageTypes.Router();

        Utils.setCookie();

        Backbone.history.start({ pushState: true });
    });

    // All navigation that is relative should be passed through the navigate
    // method, to be processed by the router.  If the link has a data-bypass
    // attribute, bypass the delegation completely.
    $(document).on("click", "a:not([data-bypass])", function (evt) {
        // Get the anchor href and protcol
        var href = $(this).attr("href");
        var protocol = this.protocol + "//";

        // Ensure the protocol is not part of URL, meaning its relative.
        if (href && href.slice(0, protocol.length) !== protocol &&
        href.indexOf("javascript:") !== 0) {
            // Stop the default event to ensure the link will not cause a page
            // refresh.
            evt.preventDefault();

            // This uses the default router defined above, and not any routers
            // that may be placed in modules.  To have this work globally (at the
            // cost of losing all route events) you can change the following line
            // to: Backbone.history.navigate(href, true);
            app.router.navigate(href, true);
        }
    });
});
