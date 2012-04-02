var Utils = (function () {

    var Utils = {};

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


    Utils.getCookie = function readCokie(name) {
        var nameEQ = name + "=";
        var ca = document.cookie.split(';');
        for (var i = 0; i < ca.length; i++) {
            var c = ca[i];
            while (c.charAt(0) == ' ') c = c.substring(1, c.length);
            if (c.indexOf(nameEQ) == 0) return c.substring(nameEQ.length, c.length);
        }
        return null;
    };
    
    return Utils;
})();
