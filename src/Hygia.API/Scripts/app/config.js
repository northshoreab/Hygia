﻿define('config',
    ['infuser','ko','mock/mock'],
    function (infuser, ko, mock) {

        var user = ko.observable(),
            isLoggedIn = ko.observable(false),
            selectedEnvironment = ko.observable(),
        // properties
        //-----------------

            hashes = {
                faults: '#/faults',
                home: '#/home',
                faultOverview: '#/faultoverview',
                login: '#/login'
            },
            //logger = toastr, // use toastr for the logger
            messages = {
                viewModelActivated: 'viewmodel-activation'
            },
            stateKeys = {
                lastView: 'state.active-hash'
            },
            //storeExpirationMs = (1000 * 60 * 60 * 24), // 1 day
            storeExpirationMs = (1000 * 5), // 5 seconds
            throttle = 400,
            title = 'WatchR > ',
            //toastrTimeout = 2000,

            _useMocks = false, // Set this to toggle mocks
            useMocks = function (val) {
                if (val) {
                    _useMocks = val;
                    init();
                }
                return _useMocks;
            },

            viewIds = {
                faults: '#faults-view',
                fault: '#fault-view',
                home: '#home-view',
                shellTop: '#shellTop-view',
                faultOverview: '#faultoverview-view',
                login: '#login-view'
            },

//            toasts = {
//                changesPending: 'Please save or cancel your changes before leaving the page.',
//                errorSavingData: 'Data could not be saved. Please check the logs.',
//                errorGettingData: 'Could not retrieve data.  Please check the logs.',
//                invalidRoute: 'Cannot navigate. Invalid route',
//                retreivedData: 'Data retrieved successfully',
//                savedData: 'Data saved successfully'
//            },

        // methods
        //-----------------

            dataserviceInit = function () { },

            validationInit = function () {
                ko.validation.configure({
                    registerExtenders: true,    //default is true
                    messagesOnModified: true,   //default is true
                    insertMessages: true,       //default is true
                    parseInputAttributes: true, //default is false
                    writeInputAttributes: true, //default is false
                    messageTemplate: null,      //default is null
                    decorateElement: true       //default is false. Applies the .validationElement CSS class
                });
            },

            configureExternalTemplates = function () {
                infuser.defaults.templatePrefix = "_";
                infuser.defaults.templateSuffix = ".tmpl.html";
                infuser.defaults.templateUrl = "/Tmpl";
            },

            init = function () {
                if (_useMocks) {
                    dataserviceInit = mock.dataserviceInit;
                }
                dataserviceInit();

                //toastr.options.timeOut = toastrTimeout;
                configureExternalTemplates();
                validationInit();
                
            };

        init();

        user.subscribe(function (newValue) {
            if (newValue)
                isLoggedIn(true);
            else
                isLoggedIn(false);
        });

        return {
//            currentUserId: currentUserId,
            //            currentUser: currentUser,
            user: user,
            isLoggedIn: isLoggedIn,
            dataserviceInit: dataserviceInit,
            hashes: hashes,
            //logger: logger,
            messages: messages,
            stateKeys: stateKeys,
            storeExpirationMs: storeExpirationMs,
            throttle: throttle,
            title: title,
            //toasts: toasts,
            useMocks: useMocks,
            viewIds: viewIds,
            window: window,
            selectedEnvironment: selectedEnvironment
        };
    });