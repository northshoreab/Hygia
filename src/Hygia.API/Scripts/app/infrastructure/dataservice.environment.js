define('dataservice.environment',
    ['amplify'],
    function (amplify) {
        var init = function() {

            amplify.request.define('myEnvironments', 'ajax', {
                url: '/api/environment',
                dataType: 'json',
                type: 'GET'
                //cache:
            });
        },
            getMyEnvironments = function (callbacks) {
                return amplify.request({
                    resourceId: 'myEnvironments',
                    success: callbacks.success,
                    error: callbacks.error
                });
        };

        init();

        return {
            getMyEnvironments: getMyEnvironments
        };
    });


