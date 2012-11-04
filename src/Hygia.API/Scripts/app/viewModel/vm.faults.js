define('vm.faults',
    ['ko', 'datacontext', 'config', 'router', 'messenger','utils', 'jquery'],
    function (ko, datacontext, config, router, messenger, utils, $) {
        var faults = ko.observableArray(),
            faulsPerPage = 10,
            faultTemplate = 'faults.view',
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
            getFaults = function (completeCallback) {
                var callback = completeCallback || function () { };

                datacontext.faults.getFaults({
                    success: function (u) {
                        user(u);
                        config.user(u);
                        callback();
                    },
                    error: function () { callback(); }
                });
                //$.when(datacontext.faults.getData({ results: faults, param: config.selectedEnvironment().id() }))
                //    .always(utils.invokeFunctionIfExists(callback));
            },
            activate = function (routeData, callback) {
                messenger.publish.viewModelActivated({
                    canleaveCallback: canLeave
                });
                getFaults(callback);
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