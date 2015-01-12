/* 
    *** DO NOT CHANGE THIS SCRIPT ***

    Cordova app bootstrapper.
*/

// ReSharper disable Html.EventNotResolved
(function () {

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
        }).done(function () { done(endpointUrl); }).fail(fail);
    }

    function askUserForEndpoint(prevEndpointUrl) {
        return '192.168.1.5';
        var ip = prompt("Please enter server IP address (aa.bb.cc.dd)", prevEndpointUrl);
        var config = Funbit.Ets.Telemetry.ConfigurationManager.getConfiguration();
        var endpointUrl = "http://" + ip + ":" + config.port + config.endpointPath;
        plugins.appPreferences.store(prefsOk, prefsFail, 'endpointUrl', endpointUrl);
        return endpointUrl;
    }

    document.addEventListener('deviceready', onDeviceReady.bind(this), false);

    function onDeviceReady() {

        document.addEventListener('pause', onPause.bind(this), false);
        document.addEventListener('resume', onResume.bind(this), false);

        // turn off sleep mode
        window.plugins.insomnia.keepAwake();

        // get saved endpoint and run the dashboard
        plugins.appPreferences.fetch(function (rememberedEndpointUrl) {
            checkEndpoint(rememberedEndpointUrl, function (endpointUrl) {
                // endpoint seems to be fine
                (new Funbit.Ets.Telemetry.App(endpointUrl)).connectDashboard();
            }, function () {
                // failed to connect, ask for a new endpoint
                var endpointUrl = askUserForEndpoint(rememberedEndpointUrl);
                (new Funbit.Ets.Telemetry.App(endpointUrl)).connectDashboard();
            });
        }, function () {
            // first run, ask for a new endpoint
            var endpointUrl = askUserForEndpoint();
            (new Funbit.Ets.Telemetry.App(endpointUrl)).connectDashboard();
        }, 'endpointUrl');
        
    };

    function onPause() {
    };

    function onResume() {
    };

})();
// ReSharper restore Html.EventNotResolved