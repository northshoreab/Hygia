define('mock/mock',
    [
    'mock/mock.generator',
    'mock/mock.dataservice.fault'
    ],
    function (generator, fault) {
        var 
            model = generator.model,

            dataserviceInit = function () {
                fault.defineApi(model);
            };

        return {
            dataserviceInit: dataserviceInit
        };
    });