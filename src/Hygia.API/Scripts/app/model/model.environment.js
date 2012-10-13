define('model.environment',
    ['ko'],
    function (ko) {
        var 
            _dc = null,

            Environment = function () {
                var self = this;
                self.id = ko.observable();
                self.name = ko.observable();
                self.users = ko.observableArray();
                return self;
            };

        Environment.Nullo = new Environment()
            .id(0)
            .name('')
            .users([]);

        // static member
        Environment.datacontext = function (dc) {
            if (dc) { _dc = dc; }
            return _dc;
        };

        return Environment;
    });