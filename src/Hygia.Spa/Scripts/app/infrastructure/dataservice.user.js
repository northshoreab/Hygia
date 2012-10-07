define('dataservice.user',
    ['amplify'],
    function (amplify) {
        var init = function() {

            amplify.request.define('me', 'ajax', {
                url: 'http://localhost:38105/api/users/me',
                dataType: 'json',
                type: 'GET',
                xhrFields: {
                    withCredentials: true
                }
                //cache:
            });
        },
            getMe = function(callbacks) {
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


