define('vm.shell',
    ['ko', 'config'],
    function (ko, config) {
        var menuHashes = config.hashes,
            isLoggedIn = ko.computed(function () {
                return config.isLoggedIn();
            }),
            userName = ko.computed(function () {
                if (config.user())
                    return config.user().name();

                return '';
            }),
            activate = function (routeData) {
                
            },
            
            selectedEnvironmentName = ko.computed(function () {
                if (config.selectedEnvironment())
                    return config.selectedEnvironment().name();

                return '';
            }),
            environmentSelected = ko.computed(function () {
                if (config.selectedEnvironment())
                    return true;

                return false;
            }),

            init = function () {
                activate();
            };

        init();

        return {
            activate: activate,
            menuHashes: menuHashes,
            isLoggedIn: isLoggedIn,
            userName: userName,
            environmentSelected: environmentSelected,
            selectedEnvironmentName: selectedEnvironmentName
        };
    });