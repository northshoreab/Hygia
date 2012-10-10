define('vm.login',
    ['jquery', 'ko', 'datacontext', 'config', 'router', 'messenger', 'utils','model.mapper'],
    function ($, ko, datacontext, config, router, messenger, utils, modelmapper) {
        var canLeave = function () {
                return true;
            },
            user = ko.observable(),
            activate = function (routeData, callback) {
                messenger.publish.viewModelActivated({ canleaveCallback: canLeave });
                getMe(callback);
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