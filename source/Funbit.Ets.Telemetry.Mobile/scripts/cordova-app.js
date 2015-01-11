/* 
    *** DO NOT CHANGE THIS SCRIPT ***

    Cordova based mobile application wrapper script.
*/

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
        // ReSharper disable once UnusedParameter
        function prefsOk(value) { }
        // ReSharper disable once UnusedParameter
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
            var ip = prompt("Please enter server IP address (aa.bb.cc.dd)", prevEndpointUrl);
            var endpointUrl = "http://" + ip + ":" + globalConfig.port + globalConfig.endpointPath;
            prefs.store(prefsOk, prefsFail, 'endpointUrl', endpointUrl);
            return endpointUrl;
        }
        function startDashboard(endpointUrl) {
            $(document).trigger('dashboard-ready', endpointUrl);
        }

        startDashboard('http://192.168.1.5:25555/api/ets2/telemetry');
        /*

        // get saved endpoint and run the dashboard
        prefs.fetch(function (rememberedEndpointUrl) {
            checkEndpoint(rememberedEndpointUrl, function (endpointUrl) {
                // endpoint seems to be fine
                startDashboard(endpointUrl);
            }, function () {
                // failed to connect, ask for a new endpoint
                var endpointUrl = askUserForEndpoint(rememberedEndpointUrl);
                startDashboard(endpointUrl);
            });
        }, function () {
            // first run, ask for a new endpoint
            var endpointUrl = askUserForEndpoint();
            startDashboard(endpointUrl);
        }, 'endpointUrl');*/
        
    };

    function onPause() {
        
    };

    function onResume() {
        
    };

})();
// ReSharper restore Html.EventNotResolved