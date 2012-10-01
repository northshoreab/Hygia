define('vm.faultoverview',
    ['jquery','ko', 'datacontext', 'config', 'router', 'messenger', 'utils', 'highcharts'],
    function ($, ko, datacontext, config, router, messenger, utils, highcharts) {
        var faultOverviewTemplate = 'faultoverview.view',
            canLeave = function() {
                return true;
            },
            activate = function(routeData, callback) {
                messenger.publish.viewModelActivated({ canleaveCallback: canLeave });
            },
            stacked = ko.observable(true),
            seriesList = ko.observableArray([{
                label: 'Fault messages',
                legendEntry: true,
                data: {
                    x: ['UserSignedUp', 'SomeMessage', 'SomeOtherMessage'],
                    y: [35, 15, 19]
                }
            }]);        return {
            stacked: stacked,
            seriesList: seriesList,
            activate: activate,
            canLeave: canLeave,
            faultOverviewTemplate: faultOverviewTemplate
        };
    });