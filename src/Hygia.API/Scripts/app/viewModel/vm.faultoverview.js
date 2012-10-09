define('vm.faultoverview',
    ['jquery','ko', 'datacontext', 'config', 'router', 'messenger', 'utils', 'underscore'],
    function ($, ko, datacontext, config, router, messenger, utils, _) {
        var faultSummaryTemplate = 'faultsummary.view',
            faults = ko.observableArray(),
            faultsHasItems = ko.computed(function () {
                return faults().length;
            }),
            pageIndex = ko.observable(0),
            visibleFaults = ko.computed(function () {
                return faults.slice(pageIndex() * 5, (pageIndex() * 5) + 5);
            }),
            canMoveNext = ko.computed(function () {
                return faults().length > ((pageIndex() * 5) + 5);
            }),
            canMovePrevious = ko.computed(function () {
                return pageIndex() > 0;
            }),
            canLeave = function() {
                return true;
            },
            activate = function(routeData, callback) {
                messenger.publish.viewModelActivated({ canleaveCallback: canLeave });
                getFaults(callback);
            },
            stacked = ko.observable(true),
            getFaults = function (callback) {
                $.when(datacontext.faults.getData({ results: faults, forceRefresh: false }))
                    .always(utils.invokeFunctionIfExists(callback));
            },
            seriesList = ko.observableArray([{
                label: 'Fault messages',
                legendEntry: true,
                data: {
                    x: ['UserSignedUp', 'SomeMessage', 'SomeOtherMessage'],
                    y: [35, 15, 19]
                }
            }]),
            previousPage = function () {
                if(canMovePrevious())
                    pageIndex(pageIndex() - 1);
            },
            nextPage = function () {
                if(canMoveNext())
                    pageIndex(pageIndex() + 1);
            };

        faults.subscribe(function () {
            
        });
        return {
            stacked: stacked,
            seriesList: seriesList,
            activate: activate,
            canLeave: canLeave,
            faultSummaryTemplate: faultSummaryTemplate,
            previousPage: previousPage,
            nextPage: nextPage,
            pageIndex: pageIndex,
            canMovePrevious: canMovePrevious,
            canMoveNext: canMoveNext,
            visibleFaults: visibleFaults,
            faultsHasItems: faultsHasItems
        };
    });