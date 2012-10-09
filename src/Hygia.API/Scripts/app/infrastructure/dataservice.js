define('dataservice',
    [
        'dataservice.fault',
        'dataservice.user'
    ],
    function (fault, user) {
        return {
            fault: fault,
            user: user
        };
    });