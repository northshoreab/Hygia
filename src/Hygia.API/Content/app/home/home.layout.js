(function(WatchR, Backbone, $){
	var Layout = Backbone.Marionette.Layout.extend({
		template: "#home-layout-template",

		regions: {      

		},
    
		events: {				
		}
	});

	WatchR.addInitializer(function(){
		WatchR.Home.layout = new Layout();
	});
	

})(WatchR, Backbone, $);