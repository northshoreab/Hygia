define('vm.shell',
    ['ko', 'config'],
    function (ko, config) {
        var menuHashes = config.hashes,
            activate = function (routeData) {
                //TODO: Check if logged in
                //TODO: Call API to get what user initially can do and apply navigation accordingly (urls should be loaded from api)
            },

            init = function () {
                activate();
            };

        init();

        return {
            activate: activate,
            menuHashes: menuHashes,
            isLoggedIn: config.isLoggedIn
        };
    });