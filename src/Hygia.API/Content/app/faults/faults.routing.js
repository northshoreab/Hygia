
WatchR.Routing.FaultsRouting = (function (WatchR, Backbone) {
	var FaultsRouting = {};

	FaultsRouting.Router = Backbone.Marionette.AppRouter.extend({
    	appRoutes: {      				   
      	 "api/faultmanagement/faults": "show",
         "api/faultmanagement/faults/:id" : "showDetails"         
    	} 	
  	});	

  	WatchR.addInitializer(function () {
    	FaultsRouting.router = new FaultsRouting.Router({
      		controller: WatchR.Faults
    	});
  	});

	return FaultsRouting;
})(WatchR, Backbone);


