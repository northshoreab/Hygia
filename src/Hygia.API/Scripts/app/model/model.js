define('model',
    [
        'model.fault',
        'model.user',
        'model.environment'
    ],
    function (fault, user, environment) {
        var 
            model = {
                Fault: fault,
                User: user,
                Environment: environment
            };

            model.setDataContext = function (dc) {
                model.Fault.datacontext(dc);
                model.User.datacontext(dc);
                model.Environment.datacontext(dc);
            // Model's that have navigation properties 
            // need a reference to the datacontext.
            //            model.Attendance.datacontext(dc);
            //            model.Person.datacontext(dc);
            //            model.Session.datacontext(dc);
        };

        return model;
    });