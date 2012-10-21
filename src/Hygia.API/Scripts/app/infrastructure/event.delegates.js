define('event.delegates',
    ['jquery', 'ko', 'config'],
    function ($, ko, config) {
        var
            faultSelector = '.fault',
            environmentSelector = '.environment',
            bindEventToList = function (rootSelector, selector, callback, eventName) {
                var eName = eventName || 'click';
                $(rootSelector).on(eName, selector, function () {
                    //var context = ko.contextFor(this);
                    //var session = context.$data;
                    var session = ko.dataFor(this);
                    callback(session);
                    return false;
                });
            },

            environmentListItem = function (callback, eventName) {
                bindEventToList(config.viewIds.home, environmentSelector, callback, eventName);
            },
            faultListItem = function (callback, eventName) {
                bindEventToList(config.viewIds.faults, faultSelector, callback, eventName);
            };

        return {
            faultListItem: faultListItem,
            environmentListItem: environmentListItem
        };
    });

