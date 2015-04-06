/// <reference path="typings/jquery.d.ts" />
/// <reference path="typings/jqueryui.d.ts" />
/// <reference path="typings/signalr.d.ts" />
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
                    var _this = this;
                    this.$cache = [];
                    this.lastDataRequestFrame = 0;
                    this.lastDataRequestFrameDiff = 0;
                    this.frame = 0;
                    this.latestData = null;
                    this.prevData = null;
                    this.frameData = null;
                    this.lastRafShimTime = 0;
                    this.endpointUrl = telemetryEndpointUrl;
                    this.skinConfig = skinConfig;
                    this.utils = this.utilityFunctions(skinConfig);
                    this.initializeRequestAnimationFrame();

                    // call custom skin initialization function
                    this.initialize(skinConfig, this.utils);

                    // run infinite animation loop
                    this.animationLoop();

                    // initialize SignalR after some time to overcome some browser bugs
                    this.reconnectTimer = this.setTimer(this.reconnectTimer, function () {
                        _this.initializeHub();
                        _this.connectToHub();
                    }, 100);
                }
                Dashboard.prototype.setTimer = function (timer, func, delay) {
                    if (timer)
                        clearTimeout(timer);
                    return setTimeout(function () {
                        return func();
                    }, delay);
                };

                Dashboard.prototype.initializeRequestAnimationFrame = function () {
                    var _this = this;
                    // requestAnimationFrame polyfill
                    var vendors = ['ms', 'moz', 'webkit', 'o'];
                    for (var x = 0; x < vendors.length && !window.requestAnimationFrame; ++x) {
                        window.requestAnimationFrame = window[vendors[x] + 'RequestAnimationFrame'];
                        window.cancelAnimationFrame = window[vendors[x] + 'CancelAnimationFrame'] || window[vendors[x] + 'CancelRequestAnimationFrame'];
                    }
                    if (!window.requestAnimationFrame)
                        window.requestAnimationFrame = function (callback) {
                            var now = Date.now();

                            // on old devices shim to set timer with target frame rate of 30 fps
                            var timeToCall = Math.max(0, (1000 / 30.0) - (now - _this.lastRafShimTime));
                            var id = window.setTimeout(function () {
                                callback(now + timeToCall);
                            }, timeToCall);
                            _this.lastRafShimTime = now + timeToCall;
                            return id;
                        };
                    if (!window.cancelAnimationFrame)
                        window.cancelAnimationFrame = function (id) {
                            clearTimeout(id);
                        };
                };

                Dashboard.prototype.animationLoop = function () {
                    var _this = this;
                    this.frame++;
                    window.requestAnimationFrame(function () {
                        return _this.animationLoop();
                    });

                    // render animated elements
                    if (this.latestData && this.prevData) {
                        // use our internal renderer first
                        this.internalRender();

                        // and then use skin based renderer
                        this.render(this.frameData, this.utils);
                    }
                };

                Dashboard.prototype.initializeHub = function () {
                    $.connection.hub.logging = false;
                    $.connection.hub.url = Telemetry.Configuration.getUrl('/signalr');
                    this.ets2TelemetryHub = $.connection['ets2TelemetryHub'];
                    window.onbeforeunload = function () {
                        $.connection.hub.stop();
                    };
                };

                Dashboard.prototype.connectToHub = function () {
                    var _this = this;
                    $.connection.hub.stop();
                    this.ets2TelemetryHub.client['updateData'] = function (json) {
                        _this.dataUpdateCallback(json);
                    };
                    $.connection.hub.reconnected(function () {
                        _this.requestDataUpdate();
                    });
                    $.connection.hub.reconnecting(function () {
                        _this.process(null, Telemetry.Strings.connectingToServer);
                    });
                    $.connection.hub.disconnected(function () {
                        _this.process(null, Telemetry.Strings.disconnectedFromServer);
                        _this.reconnectToHubAfterDelay();
                    });
                    $.connection.hub.start().done(function () {
                        _this.requestDataUpdate();
                    }).fail(function () {
                        _this.process(null, Telemetry.Strings.couldNotConnectToServer);
                        _this.reconnectToHubAfterDelay();
                    });
                };

                Dashboard.prototype.reconnectToHubAfterDelay = function () {
                    var _this = this;
                    this.process(null, Telemetry.Strings.connectingToServer);
                    this.reconnectTimer = this.setTimer(this.reconnectTimer, function () {
                        _this.connectToHub();
                    }, Dashboard.reconnectDelay);
                };

                Dashboard.prototype.requestDataUpdate = function () {
                    this.lastDataRequestFrame = this.frame;
                    this.ets2TelemetryHub.server['requestData']();
                };

                Dashboard.prototype.dataUpdateCallback = function (jsonData) {
                    var data = JSON.parse(jsonData);
                    this.process(data);
                    this.requestDataUpdate();
                };

                Dashboard.prototype.process = function (data, reason) {
                    if (typeof reason === "undefined") { reason = ''; }
                    if (data != null && !data.connected) {
                        // if we're not connected we reset the data
                        reason = Telemetry.Strings.connectedAndWaitingForDrive;
                        data = null;
                    }

                    // update status message with failure reason
                    $('.statusMessage').html(reason);

                    // if we don't have real data we use default values
                    var data = data === null ? new Ets2TelemetryData() : data;

                    // tweak data using custom skin based filter
                    data = this.filter(data, this.utils);

                    // tweak data using default internal filter
                    data = this.internalFilter(data);

                    // update data buffers
                    this.lastDataRequestFrameDiff = this.frame - this.lastDataRequestFrame;
                    this.prevData = this.latestData;
                    this.frameData = this.latestData;
                    this.latestData = data;
                };

                Dashboard.prototype.internalFilter = function (data) {
                    // convert ISO8601 to default readable format
                    data.gameTime = this.timeToReadableString(data.gameTime);
                    data.jobDeadlineTime = this.timeToReadableString(data.jobDeadlineTime);
                    data.jobRemainingTime = this.timeDifferenceToReadableString(data.jobRemainingTime);
                    return data;
                };

                Dashboard.prototype.internalRender = function () {
                    var frames = Math.max(1, this.lastDataRequestFrameDiff) * 1.0;
                    for (var name in this.latestData) {
                        var $e = this.$cache[name] !== undefined ? this.$cache[name] : this.$cache[name] = $('.' + name);
                        var value = this.latestData[name];
                        if (typeof value == "number") {
                            // calculate interpolated value for current frame
                            var prevValue = this.prevData[name];
                            value = this.frameData[name] + (value - prevValue) / frames;
                            this.frameData[name] = value;
                            var $meters = $e.filter('[data-type="meter"]');
                            if ($meters.length > 0) {
                                // if type is set to meter
                                // then we use this HTML element
                                // as a rotating meter "arrow"
                                var minValue = $meters.data('min');
                                if (/[a-z]/i.test(minValue)) {
                                    // if data-min attribute points
                                    // to a property name then we use its value
                                    minValue = this.latestData[minValue];
                                }
                                var maxValue = $meters.data('max');
                                if (/[a-z]/i.test(maxValue)) {
                                    // if data-max attribute points
                                    // to a property name then we use its value
                                    maxValue = this.latestData[maxValue];
                                }
                                this.setMeter($meters, value, parseFloat(minValue), parseFloat(maxValue));
                            } else {
                                var $notMeters = $e.not('[data-type="meter"]');
                                if ($notMeters.length > 0) {
                                    // convert number to a string
                                    // and render it by updating HTML element content
                                    value = "" + Math.round(value);
                                    $notMeters.html(value);
                                }
                            }
                        } else if (typeof value == "boolean") {
                            // render boolean by adding/removing "yes" CSS class
                            if (value) {
                                $e.addClass('yes');
                            } else {
                                $e.removeClass('yes');
                            }
                        } else if (typeof value == "string") {
                            // render string value by updating HTML element content
                            $e.html(value);
                        }
                        $e.attr('data-value', value);
                    }
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
                };

                // utility functions available for custom skins:
                Dashboard.prototype.utilityFunctions = function (skinConfig) {
                    var _this = this;
                    return {
                        formatInteger: this.formatInteger,
                        formatFloat: this.formatFloat,
                        preloadImages: function (images) {
                            return _this.preloadImages(skinConfig, images);
                        }
                    };
                };

                Dashboard.prototype.preloadImages = function (skinConfig, images) {
                    $(images).each(function () {
                        $('<img/>')[0]['src'] = Telemetry.Configuration.getInstance().getSkinResourceUrl(skinConfig, this);
                    });
                };

                Dashboard.prototype.formatInteger = function (num, digits) {
                    var output = num + "";
                    while (output.length < digits)
                        output = "0" + output;
                    return output;
                };

                Dashboard.prototype.formatFloat = function (num, digits) {
                    var power = Math.pow(10, digits || 0);
                    return String((Math.round(num * power) / power).toFixed(digits));
                };

                Dashboard.prototype.isIso8601 = function (date) {
                    return /(\d{4})-(\d{2})-(\d{2})T(\d{2})\:(\d{2})\:(\d{2})Z/.test(date);
                };

                Dashboard.prototype.timeToReadableString = function (date) {
                    // if we have ISO8601 (in UTC) then make it readable
                    // in the following default format: "Wednesday 08:26"
                    if (this.isIso8601(date)) {
                        var d = new Date(date);
                        return Telemetry.Strings.dayOfTheWeek[d.getUTCDay()] + ' ' + this.formatInteger(d.getUTCHours(), 2) + ':' + this.formatInteger(d.getUTCMinutes(), 2);
                    }
                    return date;
                };

                Dashboard.prototype.timeDifferenceToReadableString = function (date) {
                    // if we have ISO8601 (in UTC) then make it readable
                    // in the following default format: "1 day 8 hours 26 minutes"
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

                // "forward" declarations for custom skins:
                // define custom data filter method for skins
                Dashboard.prototype.filter = function (data, utils) {
                    return data;
                };

                // define custom data render method for skins
                Dashboard.prototype.render = function (data, utils) {
                    return;
                };

                // define custom initialization function
                Dashboard.prototype.initialize = function (skinConfig, utils) {
                    return;
                };
                Dashboard.reconnectDelay = 1000;
                return Dashboard;
            })();
            Telemetry.Dashboard = Dashboard;
        })(Ets.Telemetry || (Ets.Telemetry = {}));
        var Telemetry = Ets.Telemetry;
    })(Funbit.Ets || (Funbit.Ets = {}));
    var Ets = Funbit.Ets;
})(Funbit || (Funbit = {}));
//# sourceMappingURL=dashboard-core.js.map
