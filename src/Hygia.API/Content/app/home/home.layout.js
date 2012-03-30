(function(WatchR, Backbone, $){
	var Layout = Backbone.Marionette.Layout.extend({
		template: "#home-layout-template",

		regions: {      
            signup: "#signup-index"
		},
        onShow: function () {					
			WatchR.vent.trigger("home:layout:show");
		},
		events: {				
		}
	});

	WatchR.addInitializer(function(){
		WatchR.Home.layout = new Layout();
	});
	

})(WatchR, Backbone, $);