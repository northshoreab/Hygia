define('vm.login',
    ['ko', 'dataservice.user', 'config', 'router', 'messenger', 'utils','model.mapper'],
    function (ko, dataserviceUser, config, router, messenger, utils, modelmapper) {
        var canLeave = function () {
                return true;
            },
            activate = function (routeData, callback) {
                messenger.publish.viewModelActivated({ canleaveCallback: canLeave });
                getMe(callback);
            },
            getMe = function (completeCallback) {
                var callback = function () {
                    if (completeCallback) { completeCallback(); }
                };

                dataserviceUser.getMe({
                    success: function (dto) {
                        modelmapper.user.fromDto(dto, config.user());
                    },
                    error: function (response) {
                        
                    }
                });
                
                callback();
            };

        return {
            activate: activate,
            canLeave: canLeave,
            user: config.user
        };
    });