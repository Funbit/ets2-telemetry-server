var Funbit;
(function (Funbit) {
    (function (Ets) {
        (function (Telemetry) {
            var App = (function () {
                function App() {
                    this.anticacheSeed = 0;
                    this.skinConfig = Telemetry.Configuration.getInstance().getSkinConfiguration();
                    this.initializeViewport();
                    this.loadDashboardResources();
                }
                App.prototype.initializeViewport = function () {
                    var ie = /Trident/.test(navigator.userAgent);
                    var firefox = /Firefox/i.test(navigator.userAgent);
                    var ios = /iPhone|iPad|iPod/i.test(navigator.userAgent);

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

                    $(document).add($body).on('click', function () {
                        window.history.back();
                    });

                    $(window).on('orientationchange', function () {
                        window.location.reload();
                    });

                    if (ios && !Telemetry.Configuration.isCordovaAvailable()) {
                        setInterval(function () {
                            window.location.href = "/";
                            window.setTimeout(function () {
                                window['stop']();
                            }, 0);
                        }, 30000);
                    }
                };

                App.prototype.loadDashboardResources = function () {
                    var _this = this;
                    var skinCssUrl = Telemetry.Configuration.getUrl('/skins/' + this.skinConfig.name + '/dashboard.css?seed=' + this.anticacheSeed++);
                    var skinHtmlUrl = Telemetry.Configuration.getUrl('/skins/' + this.skinConfig.name + '/dashboard.html?seed=' + this.anticacheSeed++);

                    $("head link[rel='stylesheet']").last().after('<link rel="stylesheet" href="' + skinCssUrl + '" type="text/css">');

                    $.ajax({
                        url: skinHtmlUrl,
                        async: false,
                        dataType: 'html',
                        timeout: 5000
                    }).done(function (html) {
                        $('body').append(html);
                    }).fail(function () {
                        alert('Failed to load dashboard.html for skin: ' + _this.skinConfig.name);
                    });
                };

                App.prototype.connectDashboard = function () {
                    if (!this.dashboard)
                        this.dashboard = new Funbit.Ets.Telemetry.Dashboard(Telemetry.Configuration.getUrl('/api/ets2/telemetry'), this.skinConfig);
                };
                return App;
            })();
            Telemetry.App = App;
        })(Ets.Telemetry || (Ets.Telemetry = {}));
        var Telemetry = Ets.Telemetry;
    })(Funbit.Ets || (Funbit.Ets = {}));
    var Ets = Funbit.Ets;
})(Funbit || (Funbit = {}));

if (Funbit.Ets.Telemetry.Configuration.isCordovaAvailable()) {
    $(document).on('deviceready', function () {
        (new Funbit.Ets.Telemetry.App()).connectDashboard();
    });
} else {
    (new Funbit.Ets.Telemetry.App()).connectDashboard();
}
