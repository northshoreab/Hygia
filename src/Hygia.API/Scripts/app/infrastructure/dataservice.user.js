﻿define('dataservice.user',
    ['amplify'],
    function (amplify) {
        var init = function() {

            amplify.request.define('me', 'ajax', {
                url: '/api/users/me',
                dataType: 'json',
                type: 'GET'
                //cache:
            });
        },
            getMe = function (callbacks) {
                return amplify.request({
                    resourceId: 'me',
                    success: callbacks.success,
                    error: callbacks.error
                });
        };

        init();

        return {
            getMe: getMe
        };
    });

