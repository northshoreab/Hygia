define('mock/mock.dataservice.environment',
    ['amplify'],
    function (amplify) {
        var 
            defineApi = function (model) {

                amplify.request.define('myEnvironments', function (settings) {
                    settings.success(model.generateMyEnvironments());
                });
            };

        return {
            defineApi: defineApi
        };
    });
