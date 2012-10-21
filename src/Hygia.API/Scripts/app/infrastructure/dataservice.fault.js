define('dataservice.fault',
    ['amplify'],
    function (amplify) {
        var init = function() {

            amplify.request.define('faults', 'ajax', {
                url: '/api/environments/{environmentId}/faults',
                dataType: 'json',
                type: 'GET'
                //cache:
            });
        },
            getFaults = function(callbacks, environmentId) {
                return amplify.request({
                    resourceId: 'faults',
                    data: {environmentId: environmentId},
                    success: callbacks.success,
                    error: callbacks.error
                });
        };

        init();

        return {
            getFaults: getFaults
        };
    });


