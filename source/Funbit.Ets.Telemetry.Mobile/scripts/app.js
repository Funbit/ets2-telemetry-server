var Funbit;
(function (Funbit) {
    (function (Ets) {
        (function (Telemetry) {
            var App = (function () {
                function App() {
                    var _this = this;
                    this.anticacheSeed = Date.now();
                    $.when(Telemetry.Configuration.getInstance().initialized).done(function (config) {
                        _this.config = config;
                        _this.skinConfig = config.getSkinConfiguration();
                        _this.initializeViewport();
                        _this.initializeDashboard();
                    });
                }
                App.prototype.initializeViewport = function () {
                    var _this = this;
                    if (this.skinConfig.width < 1 || this.skinConfig.height < 1) {
                        return;
                    }

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
                    var scale = 'scale(' + ratio + ')';
                    var $body = $('body');
                    $body.css('transform', scale);
                    if (firefox) {
                        $body.css('-moz-transform', scale);
                    } else if (ie) {
                        $body.css('-ms-transform', scale);
                    } else {
                        $body.css('-webkit-transform', scale);
                    }

                    $(window).on('orientationchange', function () {
                        window.location.reload();
                    });

                    if (!ios) {
                        $(window).resize(function () {
                            clearTimeout(_this.resizeTimer);
                            _this.resizeTimer = setTimeout(function () {
                                window.location.reload();
                            }, 250);
                        });
                    }

                    if (ios && !Telemetry.Configuration.isCordovaAvailable()) {
                        setInterval(function () {
                            window.location.href = "/";
                            window.setTimeout(function () {
                                window['stop']();
                            }, 0);
                        }, 30000);
                    }
                };

                App.prototype.initializeDashboard = function () {
                    var _this = this;
                    var skinCssUrl = this.config.getSkinResourceUrl(this.skinConfig, 'dashboard.css');
                    var skinHtmlUrl = this.config.getSkinResourceUrl(this.skinConfig, 'dashboard.html');
                    var skinJsUrl = this.config.getSkinResourceUrl(this.skinConfig, 'dashboard.js');
                    var signalrUrl = Telemetry.Configuration.getUrl('/signalr/hubs?seed=' + this.anticacheSeed++);

                    $("head link[rel='stylesheet']").last().after('<link rel="stylesheet" href="' + skinCssUrl + '" type="text/css">');

                    $.ajax({
                        url: skinHtmlUrl,
                        dataType: 'html',
                        timeout: 3000
                    }).done(function (html) {
                        html += '<script src="' + signalrUrl + '"></script>';
                        html += '<script src="' + skinJsUrl + '"></script>';
                        $('body').append(html);

                        if (_this.skinConfig.width > 0 && _this.skinConfig.height > 0) {
                            $('.dashboard').css({
                                position: 'absolute',
                                left: '0px',
                                top: '0px',
                                width: _this.skinConfig.width + 'px',
                                height: _this.skinConfig.height + 'px'
                            });
                        }
                        _this.dashboard = new Funbit.Ets.Telemetry.Dashboard(Telemetry.Configuration.getUrl('/api/ets2/telemetry'), _this.skinConfig);
                    }).fail(function () {
                        alert(Telemetry.Strings.dashboardHtmlLoadFailed + _this.skinConfig.name);
                    });
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
        Funbit.Ets.Telemetry.App.instance = new Funbit.Ets.Telemetry.App();
    });
} else {
    Funbit.Ets.Telemetry.App.instance = new Funbit.Ets.Telemetry.App();
}
