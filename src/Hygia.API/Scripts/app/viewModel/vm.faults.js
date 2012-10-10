define('vm.faults',
    ['ko', 'datacontext', 'config', 'router', 'messenger', 'utils'],
    function (ko, datacontext, config, router, messenger, utils) {
        var faults = ko.observableArray(),
            faulsPerPage = 10,
            faultTemplate = 'faults.view',
            canLeave = function () {
                return true;
            },
            pageIndex = ko.observable(0),
            visibleFaults = ko.computed(function () {
                return faults.slice(pageIndex() * faulsPerPage, (pageIndex() * faulsPerPage) + faulsPerPage);
            }),
            canMoveNext = ko.computed(function () {
                return faults().length > ((pageIndex() * faulsPerPage) + faulsPerPage);
            }),
            canMovePrevious = ko.computed(function () {
                return pageIndex() > 0;
            }),
            previousPage = function () {
                if (canMovePrevious())
                    pageIndex(pageIndex() - 1);
            },
            nextPage = function () {
                if (canMoveNext())
                    pageIndex(pageIndex() + 1);
            },
            canLeave = function () {
                return true;
            },
            activate = function (routeData, callback) {
                messenger.publish.viewModelActivated({
                    canleaveCallback: canLeave
                });
                getFaults(callback);
            },
            getFaults = function (callback) {
                $.when(datacontext.faults.getData({ results: faults, forceRefresh: false }))
                    .always(utils.invokeFunctionIfExists(callback));
            };
        
        return {
            activate: activate,
            canLeave: canLeave,
            faults: faults,
            faultTemplate: faultTemplate,
            previousPage: previousPage,
            nextPage: nextPage,
            pageIndex: pageIndex,
            canMovePrevious: canMovePrevious,
            canMoveNext: canMoveNext,
            visibleFaults: visibleFaults
        };
    });