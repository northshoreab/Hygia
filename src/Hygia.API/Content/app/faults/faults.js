
WatchR.Faults = (function (WatchR, Backbone) {
	var Faults = {};
	
	// move to other files ---->
	Faults.Trend = Backbone.Model.extend({});
	Faults.Spread = Backbone.Model.extend({});
	// <----

	WatchR.vent.bind("faults:retry:all", function(){
		console.log("retry all event");
	});

	WatchR.vent.bind("faults:layout:show", function() {
      	Faults.showFaultList();
      	Faults.showSummary();		
	});

	Faults.show = function() {  				
		WatchR.layout.main.show(WatchR.Faults.layout); 
	};							
	
	Faults.showDetails = function(id) {
		
	}

	Faults.showFaultList = function () {
		var faultsListView = new WatchR.Faults.FaultList.FaultListView({ 
			collection: WatchR.Faults.faultsList 
		});		
		WatchR.Faults.layout.list.show(faultsListView);				
	};

	Faults.showSummary = function() {
		var summaryView = new WatchR.Faults.Summary.SummaryView({
			model: WatchR.Faults.summary
		});
		WatchR.Faults.layout.summary.show(summaryView);
	};

	WatchR.addInitializer(function () {
    	WatchR.Faults.spread  = new Faults.Spread();
    	WatchR.Faults.trend = new Faults.Trend();    	
  	});

	return Faults;
})(WatchR, Backbone);


