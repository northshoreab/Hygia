define([
  "namespace"

// Libs  

// Modules

// Plugins
],

function (namespace) {

    // Create a new module
    var Utils = namespace.module();

    Utils.getParameterByName = function (name) {
        name = name.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
        var regexS = "[\\?&]" + name + "=([^&#]*)";
        var regex = new RegExp(regexS);
        var results = regex.exec(window.location.search);
        if (results == null)
            return "";
        else
            return decodeURIComponent(results[1].replace(/\+/g, " "));
    };

    Utils.setCookie = function () {
        var today = new Date();
        var expire = new Date();
        var nDays = 10;
        var environment = Utils.getParameterByName('environment');

        if (nDays == null || nDays == 0) {
            nDays = 1;
        }

        expire.setTime(today.getTime() + 3600000 * 24 * nDays);

        document.cookie = 'environment' + "=" + escape(environment) + ";expires=" + expire.toGMTString();
    };

    Utils.deleteCookie = function () {
        var now = new Date();
        var environment = Utils.getParameterByName('environment');
        document.cookie = 'environment' + "=" + escape(environment) + ";expires=" + now.toGMTString();
    };

    return Utils;

}); 
