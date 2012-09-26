define('binder',
    ['jquery', 'ko', 'config', 'vm'],
    function ($, ko, config, vm) {
        var 
            ids = config.viewIds,

            bind = function () {
                ko.applyBindings(vm.shell, getView(ids.shellTop));
                ko.applyBindings(vm.faults, getView(ids.faults));
                ko.applyBindings(vm.fault, getView(ids.fault));
            },

            getView = function (viewName) {
                return $(viewName).get(0);
            };

        return {
            bind: bind
        };
    });