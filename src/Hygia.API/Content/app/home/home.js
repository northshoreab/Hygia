WatchR.Home = (function (WatchR, Backbone){

	var Home = {};

	Home.show = function() {
		WatchR.layout.main.show(WatchR.Home.layout);

		WatchR.Home.layout.on("item:rendered", function(){
			// render home screens				
		}, this);		
	};

	WatchR.addInitializer(function () {
  	});	

	return Home;

})(WatchR, Backbone);