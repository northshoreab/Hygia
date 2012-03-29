(function(WatchR, Backbone, $){
	var Layout = Backbone.Marionette.Layout.extend({
		template: "#faults-layout-template",

		regions: {      
			summary: '#faults-summary',
			trend: '#faults-trend',
			spread: '#faults-spread',
			list: '#faults-recent-list'
		},    
		events: {	
			"click #faults-btn-retry-all" : "retryAll"
		},
		onShow: function () {					
			WatchR.vent.trigger("faults:layout:show");
		},
		retryAll: function () {
			WatchR.vent.trigger("faults:retry:all");
		}		
	});

	WatchR.addInitializer(function(){
		WatchR.Faults.layout = new Layout();
	});
	

})(WatchR, Backbone, $);