define('vm.login',
    ['jquery', 'ko', 'datacontext', 'config', 'router', 'messenger', 'utils','model.mapper'],
    function ($, ko, datacontext, config, router, messenger, utils, modelmapper) {
        var canLeave = function () {
                return true;
            },
            user = ko.observable(),
            activate = function (routeData, callback) {
                messenger.publish.viewModelActivated({ canleaveCallback: canLeave });
                
                if (routeData.loginStatus || !config.isLoggedIn()) {
                    getMe(callback);
                }
                else if(!config.isLoggedIn()) {
                    login(callback);
                }
            },
            login = function (completeCallback) {
                window.location.href = "http://localhost:8088/api/login?provider=github&returnUrl=http://localhost:8088/#/login?loginStatus=success";
            },
            getMe = function (completeCallback) {
                var callback = completeCallback || function () { };

                datacontext.users.getMe({
                    success: function (u) {
                        user(u);
                        config.user(u);
                        callback();
                    },
                    error: function () { callback(); }
                });
            };

        return {
            activate: activate,
            canLeave: canLeave,
            user: user
        };
    });