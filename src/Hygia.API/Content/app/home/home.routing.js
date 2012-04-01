WatchR.Routing.HomeRouting = (function (WatchR, Backbone) {
	var HomeRouting = {};

	HomeRouting.Router = Backbone.Marionette.AppRouter.extend({
    	appRoutes: {      	
			   "" : "show",
			   "home": "show"
		} 	
  	});	

  	WatchR.addInitializer(function () {
    	HomeRouting.router = new HomeRouting.Router({
      		controller: WatchR.Home
    	});
  	});

	return HomeRouting;
})(WatchR, Backbone);