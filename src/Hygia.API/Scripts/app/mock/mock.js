define('mock/mock',
    [
    'mock/mock.generator',
    'mock/mock.dataservice.fault',
    'mock/mock.dataservice.user',
    'mock/mock.dataservice.environment'
    ],
    function (generator, fault, user, environment) {
        var 
            model = generator.model,

            dataserviceInit = function () {
                fault.defineApi(model);
                user.defineApi(model);
                environment.defineApi(model);
            };

        return {
            dataserviceInit: dataserviceInit
        };
    });