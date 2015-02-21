var Funbit;
(function (Funbit) {
    (function (Ets) {
        (function (Telemetry) {
            var Ets2TelemetryData = (function () {
                function Ets2TelemetryData() {
                    this.gameTime = '';
                    this.jobDeadlineTime = '';
                    this.jobRemainingTime = '';
                    this.connected = false;
                }
                return Ets2TelemetryData;
            })();

            var Dashboard = (function () {
                function Dashboard(telemetryEndpointUrl, skinConfig) {
                    this.$cache = [];
                    this.lastDisconnectTimestamp = 0;
                    this.endpointUrl = telemetryEndpointUrl;
                    this.skinConfig = skinConfig;
                    this.adjustRefreshRate();

                    this.initialize(skinConfig);

                    this.initializeSignalR();
                }
                Dashboard.prototype.initializeMeters = function () {
                    var $animated = $('[data-type="meter"]').add('.animated');
                    var ie = /Trident/.test(navigator.userAgent);

                    var dataLatency = ie ? -17 : +17;
                    var value = ((this.skinConfig.refreshRate + dataLatency) / 1000.0) + 's linear';
                    $animated.css({
                        '-webkit-transition': value,
                        '-moz-transition': value,
                        '-o-transition': value,
                        '-ms-transition': value,
                        'transition': value
                    });
                };

                Dashboard.prototype.adjustRefreshRate = function () {
                    if (!this.skinConfig.refreshRate)
                        this.skinConfig.refreshRate = 0;
                    var now = Date.now();
                    var lastDisconnectInterval = now - this.lastDisconnectTimestamp;
                    if (lastDisconnectInterval < 1 * 60 * 1000)
                        this.skinConfig.refreshRate += 20;
                    if (lastDisconnectInterval < 2 * 60 * 1000)
                        this.skinConfig.refreshRate += 10;
                    if (lastDisconnectInterval < 3 * 60 * 1000)
                        this.skinConfig.refreshRate += 5;

                    this.skinConfig.refreshRate = Math.min(250, this.skinConfig.refreshRate);
                    this.skinConfig.refreshRate = Math.max(50, this.skinConfig.refreshRate);
                    this.initializeMeters();
                };

                Dashboard.prototype.initializeSignalR = function () {
                    var _this = this;
                    $.connection.hub.url = Telemetry.Configuration.getUrl('/signalr');
                    this.ets2TelemetryHub = $.connection['ets2TelemetryHub'];
                    this.ets2TelemetryHub.client['updateData'] = function (data) {
                        var $processed = _this.process(JSON.parse(data));
                        $.when.apply($, $processed).done(function () {
                            _this.ets2TelemetryHub.server['requestData']();
                        });
                    };
                    $.connection.hub.reconnecting(function () {
                        _this.process(null, Telemetry.Strings.connectingToServer);
                    });
                    $.connection.hub.disconnected(function () {
                        _this.process(null, Telemetry.Strings.couldNotConnectToServer);
                        _this.adjustRefreshRate();
                        _this.lastDisconnectTimestamp = Date.now();
                        setTimeout(function () {
                            $.connection.hub.start();
                        }, Dashboard.reconnectDelay);
                    });
                    $.connection.hub.start().done(function () {
                        _this.ets2TelemetryHub.server['requestData']();
                    }).fail(function () {
                        _this.process(null, Telemetry.Strings.couldNotConnectToServer);
                    });
                };

                Dashboard.prototype.process = function (data, reason) {
                    if (typeof reason === "undefined") { reason = ''; }
                    if (data != null && !data.connected) {
                        reason = Telemetry.Strings.connectedAndWaitingForDrive;
                        data = null;
                    }

                    $('.statusMessage').html(reason);

                    var data = data === null ? new Ets2TelemetryData() : data;

                    data = this.filter(data);

                    data = this.internalFilter(data);

                    var $renderFinished = this.internalRender(data);

                    this.render(data);

                    return $renderFinished;
                };

                Dashboard.prototype.internalFilter = function (data) {
                    data.gameTime = Dashboard.timeToReadableString(data.gameTime);
                    data.jobDeadlineTime = Dashboard.timeToReadableString(data.jobDeadlineTime);
                    data.jobRemainingTime = Dashboard.timeDifferenceToReadableString(data.jobRemainingTime);
                    return data;
                };

                Dashboard.prototype.internalRender = function (data) {
                    var $animations = [];

                    for (var name in data) {
                        var value = data[name];
                        var $e = this.$cache[name] !== undefined ? this.$cache[name] : this.$cache[name] = $('.' + name);
                        if (typeof value == "boolean") {
                            if (value) {
                                $e.addClass('yes');
                            } else {
                                $e.removeClass('yes');
                            }
                        } else if (typeof value == "number") {
                            var $meter = $e.filter('[data-type="meter"]');
                            if ($meter.length > 0) {
                                var minValue = $meter.data('min');
                                if (/[a-z]/i.test(minValue)) {
                                    minValue = data[minValue];
                                }
                                var maxValue = $meter.data('max');
                                if (/[a-z]/i.test(maxValue)) {
                                    maxValue = data[maxValue];
                                }
                                $animations.push(this.setMeter($meter, value, parseFloat(minValue), parseFloat(maxValue)));
                            }
                            var $value = $e.not('[data-type="meter"]');
                            if ($value.length > 0) {
                                $value.html(value);
                            }
                        } else if (typeof value == "string") {
                            $e.html(value);
                        }

                        $e.attr('data-value', value);
                    }
                    return $animations;
                };

                Dashboard.prototype.setMeter = function ($meter, value, minValue, maxValue) {
                    var maxValue = maxValue ? maxValue : $meter.data('max');
                    var minAngle = $meter.data('min-angle');
                    var maxAngle = $meter.data('max-angle');
                    value = Math.min(value, maxValue);
                    value = Math.max(value, minValue);
                    var offset = (value - minValue) / (maxValue - minValue);
                    var angle = (maxAngle - minAngle) * offset + minAngle;
                    var updateTransform = function (v) {
                        $meter.css({
                            'transform': v,
                            '-webkit-transform': v,
                            '-moz-transform': v,
                            '-ms-transform': v
                        });
                    };
                    updateTransform('rotate(' + angle + 'deg)');
                    var $animationFinished = $.Deferred();
                    setTimeout(function () {
                        $animationFinished.resolve();
                    }, this.skinConfig.refreshRate);
                    return $animationFinished;
                };

                Dashboard.formatNumber = function (num, digits) {
                    var output = num + "";
                    while (output.length < digits)
                        output = "0" + output;
                    return output;
                };

                Dashboard.isIso8601 = function (date) {
                    return /(\d{4})-(\d{2})-(\d{2})T(\d{2})\:(\d{2})\:(\d{2})Z/.test(date);
                };

                Dashboard.timeToReadableString = function (date) {
                    if (this.isIso8601(date)) {
                        var d = new Date(date);
                        return Telemetry.Strings.dayOfTheWeek[d.getUTCDay()] + ' ' + Dashboard.formatNumber(d.getUTCHours(), 2) + ':' + Dashboard.formatNumber(d.getUTCMinutes(), 2);
                    }
                    return date;
                };

                Dashboard.timeDifferenceToReadableString = function (date) {
                    if (this.isIso8601(date)) {
                        var d = new Date(date);
                        var dys = d.getUTCDate() - 1;
                        var hrs = d.getUTCHours();
                        var mnt = d.getUTCMinutes();
                        var o = dys > 1 ? dys + ' days ' : (dys != 0 ? dys + ' day ' : '');
                        if (hrs > 0)
                            o += hrs > 1 ? hrs + ' hours ' : hrs + ' hour ';
                        if (mnt > 0)
                            o += mnt > 1 ? mnt + ' minutes' : mnt + ' minute';
                        if (!o)
                            o = Telemetry.Strings.noTimeLeft;
                        return o;
                    }
                    return date;
                };

                Dashboard.prototype.filter = function (data) {
                    return data;
                };

                Dashboard.prototype.render = function (data) {
                    return;
                };

                Dashboard.prototype.initialize = function (skinConfig) {
                    return;
                };
                Dashboard.reconnectDelay = 3000;
                return Dashboard;
            })();
            Telemetry.Dashboard = Dashboard;
        })(Ets.Telemetry || (Ets.Telemetry = {}));
        var Telemetry = Ets.Telemetry;
    })(Funbit.Ets || (Funbit.Ets = {}));
    var Ets = Funbit.Ets;
})(Funbit || (Funbit = {}));
