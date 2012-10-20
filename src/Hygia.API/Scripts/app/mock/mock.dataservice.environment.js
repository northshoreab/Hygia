define('mock/mock.dataservice.environment',
    ['amplify'],
    function (amplify) {
        var 
            defineApi = function (model) {

                amplify.request.define('myEnvironments', function (settings) {
                    settings.success(model.generateMyEnvironments());
                });

                amplify.request.define('environmentsAdd', function(settings, data) {
                    settings.success({
                        data: data
                    });
                });
            };

        return {
            defineApi: defineApi
        };
    });
