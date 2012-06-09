

WatchR.Faults.FaultList = (function (WatchR, Backbone, $) {
	var FaultList = {};

	FaultList.Fault = Backbone.Model.extend({});
	FaultList.FaultsCollection = WatchR.Collection.extend({
		model: FaultList.Fault,
		url: '/api/faultmanagement/faults'		
	});

	FaultList.FaultItemView = WatchR.ItemView.extend({
		tagName: 'div',
		className: 'row-fluid',		
		template: '#faults-item-template',
        events: {
            "click a.action-retry-fault": "retry",
            "click a.action-archive-fault": "archive",
            "click a.faults-detail-link": "detail"
        },		
		initialize: function () {
			this.model.bind('change', this.render, this);
			this.model.bind('destroy', this.remove, this);
		},
		retry: function () {
			console.log("retry " + this.model.get('Title'));
			return false;
		},
		archive: function () {
			console.log("archive");
			return false;
		},
        detail: function () {
            console.log("retry " + this.model.get('id'));
        },
		onRender: function () {
			console.log("render item");
		}
	});

	FaultList.FaultListView = WatchR.CollectionView.extend({
		tagName: 'div',							
		itemView: FaultList.FaultItemView
	});

	WatchR.addInitializer(function () {
    	WatchR.Faults.faultsList = new FaultList.FaultsCollection();    	
    	WatchR.Faults.faultsList.fetch();    	 
	});

	return FaultList;
})(WatchR, Backbone, $);



