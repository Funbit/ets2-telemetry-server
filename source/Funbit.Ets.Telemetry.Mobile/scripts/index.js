// For an introduction to the Blank template, see the following documentation:
// http://go.microsoft.com/fwlink/?LinkID=397704
// To debug code on page load in Ripple or on Android devices/emulators: launch your app, set breakpoints, 
// and then run "window.location.reload()" in the JavaScript Console.

// ReSharper disable Html.EventNotResolved
(function () {

    "use strict";

    document.addEventListener('deviceready', onDeviceReady.bind(this), false);

    function onDeviceReady() {

        document.addEventListener('pause', onPause.bind(this), false);
        document.addEventListener('resume', onResume.bind(this), false);

        // turn off sleep mode
        window.plugins.insomnia.keepAwake();

        // app preferences
        var prefs = plugins.appPreferences;
        function prefsOk(value) { }
        function prefsFail(error) { }
        function checkEndpoint(endpointUrl, done, fail) {
            $.ajax({
                url: endpointUrl,
                async: true,
                dataType: 'json',
                timeout: 5000
            }).done(function() { done(endpointUrl); }).fail(fail);
        }
        function askUserForEndpoint(prevEndpointUrl) {
            var ip = prompt("Please enter Telemetry Api server IP address", prevEndpointUrl);
            var endpointUrl = "http://" + ip + ":" + 25555 + "/api/ets2/telemetry";
            prefs.store(prefsOk, prefsFail, 'endpointUrl', endpointUrl);
            return endpointUrl;
        }

        // get saved endpoint and run the gauge
        prefs.fetch(function (endpointUrl) {
            checkEndpoint(endpointUrl, function (endpointUrl) {
                // endpoint seems to be fine
                window.gaugeStarter(endpointUrl);
            }, function () {
                // failed to connect, ask for a new endpoint
                var endpointUrl = askUserForEndpoint(endpointUrl);
                window.gaugeStarter(endpointUrl);
            });
        }, function () {
            // first run, ask for a new endpoint
            var endpointUrl = askUserForEndpoint();
            window.gaugeStarter(endpointUrl);
        }, 'endpointUrl');
        
    };

    function onPause() {
        
    };

    function onResume() {
        
    };

})();
// ReSharper restore Html.EventNotResolved