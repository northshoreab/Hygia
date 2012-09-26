define('vm.faults',
    ['ko', 'datacontext', 'config', 'router', 'messenger', 'utils'],
    function (ko, datacontext, config, router, messenger, utils) {
        var faults = ko.observableArray(),
            faultTemplate = 'faults.view',
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
            faultTemplate: faultTemplate
        };
    });