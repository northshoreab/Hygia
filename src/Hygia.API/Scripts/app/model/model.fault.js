define('model.fault',
    ['ko', 'config'],
    function (ko, config) {
        var 
            _dc = null,

            Fault = function () {
                var self = this;
                self.id = ko.observable();
                self.faultNumber = ko.observable();
                self.title = ko.observable();
                self.exceptionMessage = ko.observable();
                self.timeSent = ko.observable();
                self.timeSentFull = ko.computed(function() {
                    return self.timeSent() ? moment(self.timeSent()).format('YYYY-MM-DD H:mm:ss') : '';
                });
                self.retries = ko.observable();
                self.enclosedMessageTypes = ko.observable();
                self.bussinessService = ko.observable();
                self.url = ko.computed(function () {
                    return '#/faults/' + self.id();
                });
                return self;
            };

        Fault.Nullo = new Fault()
            .id(0)
            .faultNumber(0)
            .title('')
            .exceptionMessage('')
            .timeSent('')
            .retries(0)
            .enclosedMessageTypes('')
            .bussinessService('');

        // static member
        Fault.datacontext = function (dc) {
            if (dc) { _dc = dc; }
            return _dc;
        };

        return Fault;
    });