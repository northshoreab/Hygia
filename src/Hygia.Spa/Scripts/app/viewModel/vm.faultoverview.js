define('vm.faultoverview',
    ['ko', 'datacontext', 'config', 'router', 'messenger', 'utils', 'highcharts'],
    function (ko, datacontext, config, router, messenger, utils, highcharts) {
        var faultOverviewTemplate = 'faultoverview.view',
            canLeave = function () {
                return true;
            },
            activate = function (routeData, callback) {
                messenger.publish.viewModelActivated({ canleaveCallback: canLeave });
                renderCharts();
            },
            renderCharts = function () {
                //TODO
            };

        return {
            activate: activate,
            canLeave: canLeave,
            faultOverviewTemplate: faultOverviewTemplate
        };
    });