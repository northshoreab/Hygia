define('mock/mock.dataservice.fault',
    ['amplify'],
    function (amplify) {
        var 
            defineApi = function (model) {

                amplify.request.define('faults', function (settings) {
                    settings.success(model.generateFaults().faults);
                });
            };

        return {
            defineApi: defineApi
        };
    });
