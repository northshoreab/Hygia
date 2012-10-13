define('mock/mock.dataservice.user',
    ['amplify'],
    function (amplify) {
        var 
            defineApi = function (model) {

                amplify.request.define('me', function (settings) {
                    settings.success(model.generateMe());
                });
            };

        return {
            defineApi: defineApi
        };
    });
