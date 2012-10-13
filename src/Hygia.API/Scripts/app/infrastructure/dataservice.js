define('dataservice',
    [
        'dataservice.fault',
        'dataservice.user',
        'dataservice.environment'
    ],
    function (fault, user, environment) {
        return {
            fault: fault,
            user: user,
            environment: environment
        };
    });