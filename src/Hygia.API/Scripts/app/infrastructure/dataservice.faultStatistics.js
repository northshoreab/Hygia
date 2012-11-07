define('dataservice.faultStatistics',
    ['amplify'],
    function (amplify) {
        var init = function() {

            amplify.request.define('faultStatistics', 'ajax', {
                url: '/api/environments/{environmentId}/faultmanagement/statistics/numberoffaultsperinterval/lastweek',
                dataType: 'json',
                type: 'GET'
                //cache:
            });
        },
            getFaultStatistics = function(callbacks, environmentId) {
                return amplify.request({
                    resourceId: 'faultStatistics',
                    data: {environmentId: environmentId},
                    success: callbacks.success,
                    error: callbacks.error
                });
        };

        init();

        return {
            getFaultStatistics: getFaultStatistics
        };
    });


