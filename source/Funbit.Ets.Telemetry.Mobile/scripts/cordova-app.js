/* 
    *** DO NOT CHANGE THIS SCRIPT ***

    Cordova app bootstrapper.
*/

// ReSharper disable Html.EventNotResolved
(function () {

    document.addEventListener('deviceready', onDeviceReady.bind(this), false);

    function onDeviceReady() {

        document.addEventListener('pause', onPause.bind(this), false);
        document.addEventListener('resume', onResume.bind(this), false);

        // load and connect to the dashboard
        (new Funbit.Ets.Telemetry.App()).connectDashboard();
        
    };

    function onPause() {
    };

    function onResume() {
    };

})();
// ReSharper restore Html.EventNotResolved