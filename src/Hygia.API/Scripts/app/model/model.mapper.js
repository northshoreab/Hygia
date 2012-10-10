define('model.mapper',
['model'],
    function (model) {
        var fault = {
            getDtoId: function(dto) { return dto.data.id; },
            fromDto: function(dto, item) {
                item = item || new model.Fault().id(dto.data.id);
                item.faultNumber(dto.data.faultNumber)
                    .title(dto.data.title)
                    .exceptionMessage(dto.data.exceptionMessage)
                    .timeSent(dto.data.timeSent)
                    .retries(dto.data.retries)
                    .enclosedMessageTypes(dto.data.enclosedMessageTypes)
                    .bussinessService(dto.data.bussinessService);

                return item;
            }
        },
        user = {
            getDtoId: function(dto) { return dto.id; },
            fromDto: function(dto, item) {
                item = item || new model.User().id(dto.id);
                item.name(dto.name)
                    .accessToken(dto.accessToken);

                return item;
            }
        };
        

        //        var 
        //            attendance = {
        //                getDtoId: function (dto) {
        //                    return model.Attendance.makeId(dto.personId, dto.sessionId);
        //                },
        //                fromDto: function (dto, item) {
        //                    item = item || new model.Attendance();
        //                    item.personId(dto.personId)
        //                        .sessionId(dto.sessionId);
        //                    item.rating(dto.rating).text(dto.text);
        //                    item.dirtyFlag().reset();
        //                    return item;
        //                }
        //            },

        //            person = {
        //                getDtoId: function (dto) { return dto.id; },
        //                fromDto: function (dto, item) {
        //                    item = item || new model.Person().id(dto.id);
        //                    item.firstName(dto.firstName)
        //                        .lastName(dto.lastName)
        //                        .email(dto.email)
        //                        .blog(dto.blog)
        //                        .twitter(dto.twitter)
        //                        .gender(dto.gender)
        //                        .imageSource(dto.imageSource)
        //                        .bio(dto.bio);
        //                    item.dirtyFlag().reset();
        //                    item.isBrief(dto.bio === undefined); // detect if brief or full person
        //                    return item;
        //                }
        //            },

        //            room = {
        //                getDtoId: function (dto) { return dto.id; },
        //                fromDto: function (dto, item) {
        //                    item = item || new model.Room().id(dto.id);
        //                    return item.name(dto.name);
        //                }
        //            },

        //            session = {
        //                getDtoId: function (dto) { return dto.id; },
        //                fromDto: function (dto, item) {
        //                    item = item || new model.Session().id(dto.id);
        //                    item.title(dto.title)
        //                        .code(dto.code)
        //                        .description(dto.description)
        //                        .speakerId(dto.speakerId)
        //                        .trackId(dto.trackId)
        //                        .timeslotId(dto.timeSlotId)
        //                        .roomId(dto.roomId)
        //                        .level(dto.level)
        //                        .tags(dto.tags);
        //                    item.dirtyFlag().reset();
        //                    item.isBrief(dto.description === undefined); // detect if brief or full session
        //                    item.isFavoriteRefresh.valueHasMutated(); // when we reload sessions, favorites may have changed. 
        //                    return item;
        //                }
        //            },

        //            timeSlot = {
        //                getDtoId: function (dto) { return dto.id; },
        //                fromDto: function (dto, item) {
        //                    item = item || new model.TimeSlot().id(dto.id);
        //                    return item
        //                        .start(moment(dto.start).toDate())
        //                        .duration(dto.duration);
        //                }
        //            },

        //            track = {
        //                getDtoId: function (dto) { return dto.id; },
        //                fromDto: function (dto, item) {
        //                    item = item || new model.Track().id(dto.id);
        //                    return item.name(dto.name);
        //                }
        //            };

        return {
            fault: fault,
            user: user
            //            attendance: attendance,
            //            room: room,
            //            session: session,
            //            person: person,
            //            timeSlot: timeSlot,
            //            track: track
        };
    });