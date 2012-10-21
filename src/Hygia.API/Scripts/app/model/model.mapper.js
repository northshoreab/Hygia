define('model.mapper',
['model'],
    function (model) {
        var fault = {
            getDtoId: function(dto) { return dto.data.faultId; },
            fromDto: function(dto, item) {
                item = item || new model.Fault().id(dto.data.faultId);
                item.faultNumber(dto.data.faultNumber)
                    .title(dto.data.title)
                    .exceptionMessage(dto.data.exceptionMessage)
                    .timeSent(dto.data.timeSent)
                    .retries(dto.data.retries)
                    .enclosedMessageTypes(dto.data.enclosedMessageTypes)
                    .businessService(dto.data.businessService);

                return item;
            }
        },
        user = {
            getDtoId: function(dto) { return dto.data.id; },
            fromDto: function(dto, item) {
                item = item || new model.User().id(dto.data.id);
                item.name(dto.data.name)
                    .accessToken(dto.data.accessToken);

                return item;
            }
        },
        environment = {
            getDtoId: function (dto) { return dto.data.id; },
            fromDto: function (dto, item) {
                item = item || new model.Environment().id(dto.data.id);
                item.name(dto.data.name)
                    .users(dto.data.users);

                return item;
            }
        };

        return {
            fault: fault,
            user: user,
            environment: environment
        };
    });