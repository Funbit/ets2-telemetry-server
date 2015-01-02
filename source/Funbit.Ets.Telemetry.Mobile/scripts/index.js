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

    };

    function onPause() {
        
    };

    function onResume() {
        
    };

})();
// ReSharper restore Html.EventNotResolved