/* 
    *** DO NOT CHANGE THIS SCRIPT ***

    HTML5 application startup script.
*/

var globalConfig = {
    endpointPath: '/api/ets2/telemetry',
    port: 25555
};

(function () {
    
    //
    // ENTRY POINT FUNCTION
    //
    var entryPoint = function (skinConfig) {

        var ie = /Trident/.test(navigator.userAgent);
        var firefox = /Firefox/i.test(navigator.userAgent);
        var ios = /iPhone|iPad|iPod/i.test(navigator.userAgent);
        var cordovaAvailable = document.URL.indexOf('http://') === -1 && document.URL.indexOf('https://') === -1;
        
        // fit dashboard into the screen
        var dashboardHeight = parseFloat($('.dashboard').css('height'));
        var dashboardWidth = parseFloat($('.dashboard').css('width'));
        var windowHeight = $(window).height();
        var windowWidth = $(window).width();
        var ratio = windowHeight / dashboardHeight;
        if ((windowHeight / dashboardHeight) > (windowWidth / dashboardWidth)) {
            ratio = windowWidth / dashboardWidth;
        }
        var scale = 'scale(' + (ratio * 0.99) + ')'; // leave 1% padding
        var $body = $('body');
        $body.css('transform', scale);
        if (firefox) {
            $body.css('-moz-transform', scale);
        } else if (ie) {
            $body.css('-ms-transform', scale);
        } else {
            $body.css('-webkit-transform', scale);
        }

        // reload page when orientation changes
        $(window).on('orientationchange', function () {
            window.location.reload();
        });

        // prevent iOS device from sleeping
        if (ios && !cordovaAvailable) {
            // the technique is very simple:
            // we just tell the browser that we are going to change the url
            // and at the last moment we abort the operation
            window.sleepPreventInterval = setInterval(function () {
                window.location.href = "/";
                window.setTimeout(function () {
                    window.stop();
                }, 0);
            }, 30000);
        }

        // listen for a special event to be fired when everything is ready
        $(document).on('dashboard-ready', function (e, endpointUrl) {
            window.dashboardInstance =
                new Funbit.Ets.Telemetry.Components.Dashboard(endpointUrl, skinConfig);
        });

        // start the app!
        if (cordovaAvailable) {
            // Cordova will fire dashboard-ready event inside cordova-app.js onDeviceReady
        } else {
            // start manually in desktop mode
            $(document).trigger('dashboard-ready', globalConfig.endpointPath);
        }
    }
    
    // load application configuration
    $.get('config.json', function (config) {
        // get name of the skin to load this time
        var skinConfig = config.skins[0];
        // load dashboard skin CSS
        var $dashboardCss = $('<link rel="stylesheet" type="text/css" href="skins/' + skinConfig.name + '/dashboard.css"/>').load(function () {
            $().ready(function () {
                // load dashboard HTML only when CSS is preloaded
                $('body').load('skins/' + skinConfig.name + '/dashboard.html', function () {
                    // everything is loaded at this moment
                    entryPoint(skinConfig);
                });
            });
        });
        $('head').append($dashboardCss);
    });

})();