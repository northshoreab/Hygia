define('vm.home',
    ['ko', 'datacontext', 'config', 'router', 'messenger', 'utils', 'jquery'],
    function (ko, datacontext, config, router, messenger, utils, $) {
        var canLeave = function () {
                return true;
            },
            environments = ko.observableArray(),
            newEnvName = ko.observable(),
            environmentTemplate = 'environmentlist.view',
            activate = function (routeData, callback) {
                messenger.publish.viewModelActivated({ canleaveCallback: canLeave });
                if(config.isLoggedIn())
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
            }),
            createEnvironment = function () {
                datacontext.environments.addData(
                {
                    id: null,
                    name: newEnvName()
                },
                {
                    success: function (data) { /*environments().add(data);*/ },
                    error: function() { }
                });
            };

        return {
            activate: activate,
            canLeave: canLeave,
            environments: environments,
            selectEnvironment: selectEnvironment,
            environmentTemplate: environmentTemplate,
            isLoggedIn: isLoggedIn,
            createEnvironment: createEnvironment,
            newEnvName: newEnvName
        };
    });