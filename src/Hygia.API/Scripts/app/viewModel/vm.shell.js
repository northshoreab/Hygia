define('vm.shell',
    ['ko', 'config'],
    function (ko, config) {
        var menuHashes = config.hashes,
            loginUrl = ko.observable("http://localhost:8088/Hygia.API/api/login?provider=github&returnUrl=http://localhost:8088/Hygia.API/#/login"),
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
            loginUrl: loginUrl
        };
    });