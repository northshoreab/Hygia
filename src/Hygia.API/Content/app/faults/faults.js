
WatchR.Faults = (function (WatchR, Backbone) {
    var Faults = {};

    WatchR.vent.bind("faults:layout:show", function () {
        Faults.showFaultList();
        Faults.showSummary();
    });

    Faults.show = function () {
        WatchR.layout.main.show(WatchR.Faults.layout);
    };

    Faults.showDetails = function(id) {
        console.log(id);
    };

    Faults.showFaultList = function () {
        var faultsListView = new WatchR.Faults.FaultList.FaultListView({
            collection: WatchR.Faults.faultsList
        });
        WatchR.Faults.layout.list.show(faultsListView);
    };

    Faults.showSummary = function () {
        var summaryView = new WatchR.Faults.Summary.SummaryView({
            model: WatchR.Faults.summary
        });
        WatchR.Faults.layout.summary.show(summaryView);
    };

    return Faults;
})(WatchR, Backbone);


