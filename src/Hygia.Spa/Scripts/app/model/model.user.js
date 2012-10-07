define('model.user',
    ['ko'],
    function (ko) {
        var 
            _dc = null,

            User = function () {
                var self = this;
                self.id = ko.observable();
                self.name = ko.observable();
                self.accessToken = ko.observable();
                return self;
            };

        User.Nullo = new User()
            .id(0)
            .name('')
            .accessToken('');

        // static member
        User.datacontext = function (dc) {
            if (dc) { _dc = dc; }
            return _dc;
        };

        return User;
    });