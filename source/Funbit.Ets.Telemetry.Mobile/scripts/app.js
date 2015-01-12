/*
*** DO NOT CHANGE THIS SCRIPT ***
HTML5 application.
*/
var Funbit;
(function (Funbit) {
    (function (Ets) {
        (function (Telemetry) {
            var App = (function () {
                function App(serverIp) {
                    this.serverIp = serverIp;
                    this.config = Funbit.Ets.Telemetry.ConfigurationManager.getConfiguration(this.getUrl('/config.json'));
                    this.skinConfig = this.config.skins[0];
                    this.initializeViewport();
                    this.loadDashboard();
                }
                App.isCordovaAvailable = function () {
                    return document.URL.indexOf('http://') === -1 && document.URL.indexOf('https://') === -1;
                };

                App.prototype.getUrl = function (path) {
                    var serverPort = 25555;
                    return "http://" + this.serverIp + ":" + serverPort + path;
                };

                App.prototype.initializeViewport = function () {
                    var ie = /Trident/.test(navigator.userAgent);
                    var firefox = /Firefox/i.test(navigator.userAgent);
                    var ios = /iPhone|iPad|iPod/i.test(navigator.userAgent);

                    // fit viewport into the screen
                    var dashboardWidth = this.skinConfig.width * 1.0;
                    var dashboardHeight = this.skinConfig.height * 1.0;
                    var windowWidth = $(window).width();
                    var windowHeight = $(window).height();
                    var ratio = windowHeight / dashboardHeight;
                    if ((windowHeight / dashboardHeight) > (windowWidth / dashboardWidth)) {
                        ratio = windowWidth / dashboardWidth;
                    }
                    var scale = 'scale(' + (ratio * 0.99) + ')';
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
                    if (ios && !App.isCordovaAvailable()) {
                        // the technique is very simple:
                        // we just tell the browser that we are going to change the url
                        // and at the last moment we abort the operation
                        setInterval(function () {
                            window.location.href = "/";
                            window.setTimeout(function () {
                                window['stop']();
                            }, 0);
                        }, 30000);
                    }
                };

                App.prototype.loadDashboard = function () {
                    var skinConfig = this.config.skins[0];
                    var skinCssUrl = this.getUrl('/skins/' + skinConfig.name + '/dashboard.css');
                    var skinHtmlUrl = this.getUrl('/skins/' + skinConfig.name + '/dashboard.html');

                    // preload skin css (synchronously)
                    $("head link[rel='stylesheet']").last().after('<link rel="stylesheet" href="' + skinCssUrl + '" type="text/css">');

                    // load skin html (synchronously)
                    $.ajax({
                        url: skinHtmlUrl,
                        async: false,
                        cache: true,
                        dataType: 'html',
                        timeout: 5000
                    }).done(function (html) {
                        $('body').append(html);
                    }).fail(function () {
                        alert('Failed to load dashboard.html for skin: ' + skinConfig.name);
                    });
                };

                App.prototype.connectDashboard = function () {
                    if (!this.dashboard)
                        this.dashboard = new Funbit.Ets.Telemetry.Dashboard(this.getUrl('/api/ets2/telemetry'), this.skinConfig);
                };
                return App;
            })();
            Telemetry.App = App;
        })(Ets.Telemetry || (Ets.Telemetry = {}));
        var Telemetry = Ets.Telemetry;
    })(Funbit.Ets || (Funbit.Ets = {}));
    var Ets = Funbit.Ets;
})(Funbit || (Funbit = {}));

//
// Application "entry-point"
//
if (Funbit.Ets.Telemetry.App.isCordovaAvailable()) {
    // Cordova will call connectDashboard inside cordova-app.js onDeviceReady
} else {
    // start manually in desktop mode using relative endpoint URL
    (new Funbit.Ets.Telemetry.App(window.location.hostname)).connectDashboard();
}
//# sourceMappingURL=app.js.map
