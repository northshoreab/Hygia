WatchR.Routing.SignUpRouting = (function (WatchR, Backbone) {
    var SignUpRouting = {};

    SignUpRouting.Router = Backbone.Marionette.AppRouter.extend({
    	appRoutes: {
    	    "github-signup": "signUpWithGithub" 
    	} 	
  	});	

  	WatchR.addInitializer(function () {
  	    SignUpRouting.router = new SignUpRouting.Router({
      		controller: WatchR.SignUp
    	});
  	});

    return SignUpRouting;
})(WatchR, Backbone);