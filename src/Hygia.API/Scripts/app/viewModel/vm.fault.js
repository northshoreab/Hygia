define('vm.fault',
    ['ko', 'datacontext', 'config', 'router', 'messenger', 'utils'],
    function (ko, datacontext, config, router, messenger, utils) {
        var fault = ko.observable(),
            currentFaultId = ko.observable(),
            faultTemplate = 'fault.view',
            
            canLeave = function () {
                return true;
            },
            activate = function (routeData, callback) {
                messenger.publish.viewModelActivated({ canleaveCallback: canLeave });
                currentFaultId(routeData.id);
                getFault(callback);
            },
            getFault = function (completeCallback) {
                var callback = function () {
                    if (completeCallback) { completeCallback(); }
                };

                fault(datacontext.faults.getLocalById(currentFaultId()));
                callback();
            };

        return {
            activate: activate,
            canLeave: canLeave,
            fault: fault,
            faultTemplate: faultTemplate
        };
    });