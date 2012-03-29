WatchR.Faults.Summary = (function (WatchR, Backbone, $) {
	var Summary = {};

	Summary.Summary = Backbone.Model.extend({
		url: '/api/faults/summary',
		defaults: function () {
			return { 
				total: 123,
				avg: 23
			};
		}		
	});

	Summary.SummaryView = WatchR.ItemView.extend({
		tagName: 'div',		
		template: '#faults-summary-item-template',	
		initialize: function () {
			this.model.bind('change', this.render, this);
			this.model.bind('destroy', this.remove, this);
		},
		onRender: function () {
			console.log("render summary view");
		}			
	});

	WatchR.addInitializer(function () {
		WatchR.Faults.summary = new Summary.Summary();
		WatchR.Faults.summary.fetch();   
	});

	return Summary;
})(WatchR, Backbone, $);