define('dataservice.environment',
    ['amplify'],
    function (amplify) {
        var init = function() {
                amplify.request.define('myEnvironments', 'ajax', {
                    url: '/api/environments',
                    dataType: 'json',
                    type: 'GET'
                    //cache:
                });

                amplify.request.define('environmentsAdd', 'ajax', {
                    url: '/api/environments',
                    dataType: 'json',
                    type: 'POST',
                    contentType: 'application/json; charset=utf-8'
                });
            },
            getMyEnvironments = function (callbacks) {
                return amplify.request({
                    resourceId: 'myEnvironments',
                    success: callbacks.success,
                    error: callbacks.error
                });
            },

            addEnvironment = function (callbacks, data) {
                return amplify.request({
                    resourceId: 'environmentsAdd',
                    data: data,
                    success: callbacks.success,
                    error: callbacks.error
                });
            };

        init();

        return {
            getMyEnvironments: getMyEnvironments,
            addEnvironment: addEnvironment
        };
    });


