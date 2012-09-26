define('model',
    [
        'model.fault'
    ],
    function (fault) {
        var 
            model = {
                Fault: fault
            };

            model.setDataContext = function (dc) {
            model.Fault.datacontext(dc);
            // Model's that have navigation properties 
            // need a reference to the datacontext.
            //            model.Attendance.datacontext(dc);
            //            model.Person.datacontext(dc);
            //            model.Session.datacontext(dc);
        };

        return model;
    });