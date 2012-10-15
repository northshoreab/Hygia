define('vm.home',
    ['ko', 'datacontext', 'config', 'router', 'messenger', 'utils'],
    function (ko, datacontext, config, router, messenger, utils) {
        var canLeave = function () {
                return true;
            },
            environments = ko.observableArray(),
            environmentTemplate = 'environmentlist.view',
            activate = function (routeData, callback) {
                messenger.publish.viewModelActivated({ canleaveCallback: canLeave });
                getMyEnvironments();
            },
            getMyEnvironments = function (callback) {
                $.when(datacontext.environments.getData({ results: environments }))
                    .always(utils.invokeFunctionIfExists(callback));
            },
            selectEnvironment = function () {
                //set selected environment to config
            },
            isLoggedIn = ko.computed(function () {
                return config.isLoggedIn();
            });

        return {
            activate: activate,
            canLeave: canLeave,
            environments: environments,
            selectEnvironment: selectEnvironment,
            environmentTemplate: environmentTemplate,
            isLoggedIn: isLoggedIn
        };
    });