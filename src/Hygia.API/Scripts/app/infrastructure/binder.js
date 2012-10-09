define('binder',
    ['jquery', 'ko', 'config', 'vm'],
    function ($, ko, config, vm) {
        var 
            ids = config.viewIds,

            bind = function () {
                ko.applyBindings(vm.shell, getView(ids.shellTop));
                ko.applyBindings(vm.faults, getView(ids.faults));
                ko.applyBindings(vm.fault, getView(ids.fault));
                ko.applyBindings(vm.signUp, getView(ids.signUp));
                ko.applyBindings(vm.faultOverview, getView(ids.faultOverview));
                ko.applyBindings(vm.login, getView(ids.login));
            },

            getView = function (viewName) {
                return $(viewName).get(0);
            };

        return {
            bind: bind
        };
    });