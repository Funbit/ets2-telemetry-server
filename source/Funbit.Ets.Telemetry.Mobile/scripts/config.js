var Funbit;
(function (Funbit) {
    (function (Ets) {
        (function (Telemetry) {
            var serverPort = 25555;

            var Strings = (function () {
                function Strings() {
                }
                Strings.dashboardHtmlLoadFailed = 'Failed to load dashboard.html for skin: ';

                Strings.connecting = 'Connecting...';
                Strings.connected = 'Connected';
                Strings.disconnected = 'Disconnected';
                Strings.enterServerIpMessage = 'Please enter server IP address (aa.bb.cc.dd)';
                Strings.incorrectServerIpFormat = 'Entered server IP or hostname has incorrect format.';

                Strings.dayOfTheWeek = [
                    'Sunday',
                    'Monday',
                    'Tuesday',
                    'Wednesday',
                    'Thursday',
                    'Friday',
                    'Saturday'
                ];
                Strings.noTimeLeft = 'Overdue';
                Strings.disconnectedFromServer = 'Disconnected from server';
                Strings.couldNotConnectToServer = 'Could not connect to the server';
                Strings.connectedAndWaitingForDrive = 'Connected, waiting for the drive...';
                Strings.connectingToServer = 'Connecting to the server...';
                return Strings;
            })();
            Telemetry.Strings = Strings;

            var Configuration = (function () {
                function Configuration() {
                    var _this = this;
                    this.anticacheSeed = Date.now();
                    this.initialized = $.Deferred();
                    this.skins = [];

                    if (!Configuration.isCordovaAvailable()) {
                        this.insomnia = {
                            keepAwake: function () {
                            }
                        };
                        this.prefs = {
                            fetch: function () {
                            },
                            store: function () {
                            }
                        };
                    } else {
                        this.insomnia = plugins.insomnia;
                        this.prefs = plugins.appPreferences;

                        this.insomnia.keepAwake();
                    }

                    var ip = Configuration.getParameter('ip');
                    if (ip) {
                        this.serverIp = ip;
                        this.initialize();
                        return;
                    }
                    this.serverIp = '';
                    if (!Configuration.isCordovaAvailable()) {
                        this.serverIp = window.location.hostname;
                        this.initialize();
                    } else {
                        this.prefs.fetch(function (savedIp) {
                            _this.serverIp = savedIp;
                            _this.initialize();
                        }, function () {
                            _this.initialize();
                        }, 'serverIp');
                    }
                }
                Configuration.getInstance = function () {
                    if (!Configuration.instance) {
                        Configuration.instance = new Configuration();
                    }
                    return Configuration.instance;
                };

                Configuration.getParameter = function (name) {
                    name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
                    var regex = new RegExp("[\\?&]" + name + "=([^&#]*)");
                    var results = regex.exec(location.search);
                    return results == null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
                };

                Configuration.prototype.getSkinConfiguration = function () {
                    var skinName = Configuration.getParameter('skin');
                    if (skinName) {
                        for (var i = 0; i < this.skins.length; i++) {
                            if (this.skins[i].name == skinName)
                                return this.skins[i];
                        }
                    }
                    return null;
                };

                Configuration.prototype.reload = function (newServerIp, done, fail) {
                    var _this = this;
                    if (typeof done === "undefined") { done = null; }
                    if (typeof fail === "undefined") { fail = null; }
                    if (!newServerIp)
                        return;
                    this.serverIp = newServerIp;
                    this.prefs.store(function () {
                    }, function () {
                    }, 'serverIp', this.serverIp);
                    $.ajax({
                        url: this.getUrlInternal('/config.json?seed=' + this.anticacheSeed++),
                        dataType: 'json',
                        timeout: 3000
                    }).done(function (json) {
                        _this.skins = json.skins;
                        if (done)
                            done();
                    }).fail(function () {
                        _this.skins = [];
                        if (fail)
                            fail();
                    });
                };

                Configuration.isCordovaAvailable = function () {
                    return document.URL.indexOf('http://') === -1 && document.URL.indexOf('https://') === -1;
                };

                Configuration.prototype.getUrlInternal = function (path) {
                    return "http://" + this.serverIp + ":" + serverPort + path;
                };

                Configuration.getUrl = function (path) {
                    return Configuration.getInstance().getUrlInternal(path);
                };

                Configuration.prototype.getSkinResourceUrl = function (skinConfig, name) {
                    return Configuration.getUrl('/skins/' + skinConfig.name + '/' + name + '?seed=' + this.anticacheSeed++);
                };

                Configuration.prototype.initialize = function () {
                    var _this = this;
                    if (!this.serverIp)
                        this.initialized.resolve(this);
                    this.reload(this.serverIp, function () {
                        return _this.initialized.resolve(_this);
                    }, function () {
                        return _this.initialized.resolve(_this);
                    });
                };
                return Configuration;
            })();
            Telemetry.Configuration = Configuration;
        })(Ets.Telemetry || (Ets.Telemetry = {}));
        var Telemetry = Ets.Telemetry;
    })(Funbit.Ets || (Funbit.Ets = {}));
    var Ets = Funbit.Ets;
})(Funbit || (Funbit = {}));
