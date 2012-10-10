define('dataservice.fault',
    ['amplify'],
    function (amplify) {
        var init = function() {

            amplify.request.define('faults', 'ajax', {
                url: '/Hygia.API/api/faults',
                dataType: 'json',
                type: 'GET'
                //cache:
            });
        },
            getFaults = function(callbacks) {
                return amplify.request({
                    resourceId: 'faults',   
                    success: callbacks.success,
                    error: callbacks.error
                });
        };

        init();

        return {
            getFaults: getFaults
        };
    });


