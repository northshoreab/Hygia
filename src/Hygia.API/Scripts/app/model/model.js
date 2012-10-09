define('model',
    [
        'model.fault',
        'model.user'
    ],
    function (fault, user) {
        var 
            model = {
                Fault: fault,
                User: user
            };

            model.setDataContext = function (dc) {
                model.Fault.datacontext(dc);
                model.User.datacontext(dc);
            // Model's that have navigation properties 
            // need a reference to the datacontext.
            //            model.Attendance.datacontext(dc);
            //            model.Person.datacontext(dc);
            //            model.Session.datacontext(dc);
        };

        return model;
    });