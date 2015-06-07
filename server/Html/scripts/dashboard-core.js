var Funbit;
(function (Funbit) {
    (function (Ets) {
        (function (Telemetry) {
            var Ets2Game = (function () {
                function Ets2Game() {
                    this.connected = false;
                    this.paused = false;
                    this.time = "";
                    this.timeScale = 0;
                    this.nextRestStopTime = "";
                    this.version = "";
                    this.telemetryPluginVersion = "";
                }
                return Ets2Game;
            })();

            var Ets2Job = (function () {
                function Ets2Job() {
                    this.income = 0;
                    this.deadlineTime = "";
                    this.remainingTime = "";
                    this.sourceCity = "";
                    this.sourceCompany = "";
                    this.destinationCity = "";
                    this.destinationCompany = "";
                }
                return Ets2Job;
            })();

            var Ets2Truck = (function () {
                function Ets2Truck() {
                    this.id = "";
                    this.make = "";
                    this.model = "";
                    this.speed = 0;
                    this.cruiseControlSpeed = 0;
                    this.cruiseControlOn = false;
                    this.odometer = 0;
                    this.gear = 0;
                    this.displayedGear = 0;
                    this.forwardGears = 0;
                    this.reverseGears = 0;
                    this.shifterType = "";
                    this.engineRpm = 0;
                    this.engineRpmMax = 0;
                    this.fuel = 0;
                    this.fuelCapacity = 0;
                    this.fuelAverageConsumption = 0;
                    this.fuelWarningFactor = 0;
                    this.fuelWarningOn = false;
                    this.wearEngine = 0;
                    this.wearTransmission = 0;
                    this.wearCabin = 0;
                    this.wearChassis = 0;
                    this.wearWheels = 0;
                    this.userSteer = 0;
                    this.userThrottle = 0;
                    this.userBrake = 0;
                    this.userClutch = 0;
                    this.gameSteer = 0;
                    this.gameThrottle = 0;
                    this.gameBrake = 0;
                    this.gameClutch = 0;
                    this.shifterSlot = 0;
                    this.shifterToggle = 0;
                    this.engineOn = false;
                    this.electricOn = false;
                    this.wipersOn = false;
                    this.retarderBrake = 0;
                    this.retarderStepCount = 0;
                    this.parkBrakeOn = false;
                    this.motorBrakeOn = false;
                    this.brakeTemperature = 0;
                    this.adblue = 0;
                    this.adblueCapacity = 0;
                    this.adblueAverageConsumpton = 0;
                    this.adblueWarningOn = false;
                    this.airPressure = 0;
                    this.airPressureWarningOn = false;
                    this.airPressureWarningValue = 0;
                    this.airPressureEmergencyOn = false;
                    this.airPressureEmergencyValue = 0;
                    this.oilTemperature = 0;
                    this.oilPressure = 0;
                    this.oilPressureWarningOn = false;
                    this.oilPressureWarningValue = 0;
                    this.waterTemperature = 0;
                    this.waterTemperatureWarningOn = false;
                    this.waterTemperatureWarningValue = 0;
                    this.batteryVoltage = 0;
                    this.batteryVoltageWarningOn = false;
                    this.batteryVoltageWarningValue = 0;
                    this.lightsDashboardValue = 0;
                    this.lightsDashboardOn = false;
                    this.blinkerLeftActive = false;
                    this.blinkerRightActive = false;
                    this.blinkerLeftOn = false;
                    this.blinkerRightOn = false;
                    this.lightsParkingOn = false;
                    this.lightsBeamLowOn = false;
                    this.lightsBeamHighOn = false;
                    this.lightsAuxFrontOn = false;
                    this.lightsAuxRoofOn = false;
                    this.lightsBeaconOn = false;
                    this.lightsBrakeOn = false;
                    this.lightsReverseOn = false;
                    this.placement = new Ets2Placement();
                    this.acceleration = new Ets2Vector();
                    this.head = new Ets2Vector();
                    this.cabin = new Ets2Vector();
                    this.hook = new Ets2Vector();
                }
                return Ets2Truck;
            })();

            var Ets2Trailer = (function () {
                function Ets2Trailer() {
                    this.attached = false;
                    this.id = "";
                    this.name = "";
                    this.mass = 0;
                    this.wear = 0;
                    this.placement = new Ets2Placement();
                }
                return Ets2Trailer;
            })();

            var Ets2Navigation = (function () {
                function Ets2Navigation() {
                    this.estimatedTime = "";
                    this.estimatedDistance = 0;
                    this.speedLimit = 0;
                }
                return Ets2Navigation;
            })();

            var Ets2Vector = (function () {
                function Ets2Vector() {
                    this.x = 0;
                    this.y = 0;
                    this.z = 0;
                }
                return Ets2Vector;
            })();

            var Ets2Placement = (function () {
                function Ets2Placement() {
                    this.x = 0;
                    this.y = 0;
                    this.z = 0;
                    this.heading = 0;
                    this.pitch = 0;
                    this.roll = 0;
                }
                return Ets2Placement;
            })();

            var Ets2TelemetryData = (function () {
                function Ets2TelemetryData() {
                    this.game = new Ets2Game();
                    this.truck = new Ets2Truck();
                    this.trailer = new Ets2Trailer();
                    this.job = new Ets2Job();
                    this.navigation = new Ets2Navigation();
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

                    this.initialize(skinConfig, this.utils);

                    this.animationLoop();

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
                    var vendors = ['ms', 'moz', 'webkit', 'o'];
                    for (var x = 0; x < vendors.length && !window.requestAnimationFrame; ++x) {
                        window.requestAnimationFrame = window[vendors[x] + 'RequestAnimationFrame'];
                        window.cancelAnimationFrame = window[vendors[x] + 'CancelAnimationFrame'] || window[vendors[x] + 'CancelRequestAnimationFrame'];
                    }
                    if (!window.requestAnimationFrame)
                        window.requestAnimationFrame = function (callback) {
                            var now = Date.now();

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

                    if (this.latestData && this.prevData) {
                        this.internalRender();

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
                    if (data != null && data.game != null && !data.game.connected) {
                        reason = Telemetry.Strings.connectedAndWaitingForDrive;

                        data = new Ets2TelemetryData();
                    } else if (data === null) {
                        data = new Ets2TelemetryData();
                    }

                    $('.statusMessage').html(reason);

                    data = this.filter(data, this.utils);

                    data = this.internalFilter(data);

                    this.lastDataRequestFrameDiff = this.frame - this.lastDataRequestFrame;
                    this.prevData = this.latestData;
                    this.frameData = this.latestData;
                    this.latestData = data;
                };

                Dashboard.prototype.internalFilter = function (data) {
                    data.game.time = this.timeToReadableString(data.game.time);
                    data.job.deadlineTime = this.timeToReadableString(data.job.deadlineTime);
                    data.job.remainingTime = this.timeDifferenceToReadableString(data.job.remainingTime);
                    return data;
                };

                Dashboard.prototype.internalRender = function (parent, propNamePrefix) {
                    if (typeof parent === "undefined") { parent = null; }
                    if (typeof propNamePrefix === "undefined") { propNamePrefix = null; }
                    var propSplitter = '.';
                    var cssPropertySplitter = '-';
                    var frames = Math.max(1, this.lastDataRequestFrameDiff) * 1.0;
                    var object = parent != null ? parent : this.latestData;
                    for (var propName in object) {
                        var fullPropName = propNamePrefix != null ? propNamePrefix + propName : propName;
                        var value = object[propName];
                        var $e = this.$cache[fullPropName] !== undefined ? this.$cache[fullPropName] : this.$cache[fullPropName] = $('.' + this.replaceAll(fullPropName, propSplitter, cssPropertySplitter));
                        if (typeof value === "number") {
                            var prevValue = this.resolveObjectByPath(this.prevData, fullPropName);
                            value = this.resolveObjectByPath(this.frameData, fullPropName) + (value - prevValue) / frames;
                            if (propNamePrefix == null) {
                                this.frameData[propName] = value;
                            } else {
                                var parentPropName = fullPropName.substr(0, fullPropName.lastIndexOf(propSplitter));
                                this.resolveObjectByPath(this.frameData, parentPropName)[propName] = value;
                            }
                            var $meters = $e.filter('[data-type="meter"]');
                            if ($meters.length > 0) {
                                var minValue = $meters.data('min');
                                if (/^[a-z\.]+$/i.test(minValue)) {
                                    minValue = this.resolveObjectByPath(this.latestData, minValue);
                                }
                                var maxValue = $meters.data('max');
                                if (/^[a-z\.]+$/i.test(maxValue)) {
                                    maxValue = this.resolveObjectByPath(this.latestData, maxValue);
                                }
                                this.setMeter($meters, value, parseFloat(minValue), parseFloat(maxValue));
                            } else {
                                var $notMeters = $e.not('[data-type="meter"]');
                                if ($notMeters.length > 0) {
                                    value = "" + Math.round(value);
                                    $notMeters.html(value);
                                }
                            }
                        } else if (typeof value === "boolean") {
                            if (value) {
                                $e.addClass('yes');
                            } else {
                                $e.removeClass('yes');
                            }
                        } else if (typeof value === "string") {
                            $e.html(value);
                        } else if ($.isArray(value)) {
                            for (var j = 0; j < value.length; j++) {
                                this.internalRender(value[j], fullPropName + propSplitter + j + propSplitter);
                            }
                        } else if (typeof value === "object") {
                            this.internalRender(value, fullPropName + propSplitter);
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

                Dashboard.prototype.utilityFunctions = function (skinConfig) {
                    var _this = this;
                    return {
                        formatInteger: this.formatInteger,
                        formatFloat: this.formatFloat,
                        preloadImages: function (images) {
                            return _this.preloadImages(skinConfig, images);
                        },
                        resolveObjectByPath: this.resolveObjectByPath
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
                    if (this.isIso8601(date)) {
                        var d = new Date(date);
                        return Telemetry.Strings.dayOfTheWeek[d.getUTCDay()] + ' ' + this.formatInteger(d.getUTCHours(), 2) + ':' + this.formatInteger(d.getUTCMinutes(), 2);
                    }
                    return date;
                };

                Dashboard.prototype.timeDifferenceToReadableString = function (date) {
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

                Dashboard.prototype.replaceAll = function (input, search, replace) {
                    return input.replace(new RegExp('\\' + search, 'g'), replace);
                };

                Dashboard.prototype.resolveObjectByPath = function (obj, path) {
                    return path.split('.').reduce(function (prev, curr) {
                        return prev ? prev[curr] : undefined;
                    }, obj || self);
                };

                Dashboard.prototype.filter = function (data, utils) {
                    return data;
                };

                Dashboard.prototype.render = function (data, utils) {
                    return;
                };

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
