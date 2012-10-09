define('vm.signup',
    ['ko', 'datacontext', 'config', 'router', 'messenger', 'utils'],
    function (ko, datacontext, config, router, messenger, utils) {
        var signUpTemplate = 'signup.view',            
            canLeave = function () {
                return true;
            },
            activate = function (routeData, callback) {
                messenger.publish.viewModelActivated({ canleaveCallback: canLeave });
            },
            signUp = function () {
                //TODO call signup in api
            };

        return {
            activate: activate,
            canLeave: canLeave,
            signUpTemplate: signUpTemplate,
            signUp: signUp
        };
    });