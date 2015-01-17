/// <reference path="typings/jquery.d.ts" />
/// <reference path="typings/jqueryui.d.ts" />
var Funbit;
(function (Funbit) {
    (function (Ets) {
        (function (Telemetry) {
            var Ets2TelemetryData = (function () {
                function Ets2TelemetryData() {
                    // dates
                    this.gameTime = '';
                    this.jobDeadlineTime = '';
                    this.jobRemainingTime = '';
                    // booleans
                    this.connected = false;
                    this.gamePaused = false;
                    this.hasJob = false;
                    this.trailerAttached = false;
                    this.cruiseControlOn = false;
                    this.wipersOn = false;
                    this.parkBrakeOn = false;
                    this.motorBrakeOn = false;
                    this.electricOn = false;
                    this.engineOn = false;
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
                    this.batteryVoltageWarning = false;
                    this.airPressureWarning = false;
                    this.airPressureEmergency = false;
                    this.adblueWarning = false;
                    this.oilPressureWarning = false;
                    this.waterTemperatureWarning = false;
                    // strings
                    this.telemetryPluginVersion = '';
                    this.gameVersion = '';
                    this.trailerId = '';
                    this.trailerName = '';
                    this.sourceCity = '';
                    this.destinationCity = '';
                    this.sourceCompany = '';
                    this.destinationCompany = '';
                    // numbers
                    this.jobIncome = 0;
                    this.truckSpeed = 0;
                    this.accelerationX = 0;
                    this.accelerationY = 0;
                    this.accelerationZ = 0;
                    this.coordinateX = 0;
                    this.coordinateY = 0;
                    this.coordinateZ = 0;
                    this.rotationX = 0;
                    this.rotationY = 0;
                    this.rotationZ = 0;
                    this.gear = 0;
                    this.gears = 1;
                    this.gearRanges = 0;
                    this.gearRangeActive = 0;
                    this.engineRpm = 0;
                    this.engineRpmMax = 1;
                    this.fuel = 0;
                    this.fuelCapacity = 1;
                    this.fuelAverageConsumption = 0;
                    this.userSteer = 0;
                    this.userThrottle = 0;
                    this.userBrake = 0;
                    this.userClutch = 0;
                    this.gameSteer = 0;
                    this.gameThrottle = 0;
                    this.gameBrake = 0;
                    this.gameClutch = 0;
                    this.truckMass = 0;
                    this.truckModelLength = 0;
                    this.truckModelOffset = 0;
                    this.trailerMass = 0;
                    this.retarderBrake = 0;
                    this.shifterSlot = 0;
                    this.shifterToggle = 0;
                    this.airPressure = 0;
                    this.brakeTemperature = 0;
                    this.fuelWarning = 0;
                    this.adblue = 0;
                    this.adblueConsumpton = 0;
                    this.oilPressure = 0;
                    this.oilTemperature = 0;
                    this.waterTemperature = 0;
                    this.batteryVoltage = 0;
                    this.lightsDashboard = 0;
                    this.wearEngine = 0;
                    this.wearTransmission = 0;
                    this.wearCabin = 0;
                    this.wearChassis = 0;
                    this.wearWheels = 0;
                    this.wearTrailer = 0;
                    this.truckOdometer = 0;
                }
                return Ets2TelemetryData;
            })();

            var Dashboard = (function () {
                function Dashboard(telemetryEndpointUrl, skinConfig) {
                    this.failCount = 0;
                    // minimum number of fails before turning dashboard off
                    this.minFailCount = 2;
                    this.anticacheSeed = 0;
                    // jquery element cache
                    this.$cache = [];
                    this.endpointUrl = telemetryEndpointUrl;
                    this.skinConfig = skinConfig;
                    jQuery.fx.interval = 1000 / this.skinConfig.animationFps; // default animation interval

                    // here we are going into infinite refresh timer cycle
                    this.refreshData();
                }
                Dashboard.prototype.refreshData = function () {
                    var _this = this;
                    var url = this.endpointUrl + "?seed=" + this.anticacheSeed++;
                    $.ajax({
                        url: url,
                        async: true,
                        dataType: 'json',
                        timeout: Dashboard.connectionTimeout
                    }).done(function (d) {
                        if (!d.connected) {
                            _this.process(null, Telemetry.Strings.connectedAndWaitingForDrive);
                            return;
                        }
                        _this.process(d);
                        _this.failCount = 0;
                    }).fail(function () {
                        _this.failCount++;
                        if (_this.failCount > _this.minFailCount) {
                            _this.process(null, Telemetry.Strings.couldNotConnectToServer);
                        }
                    }).always(function () {
                        _this.timer = setTimeout(_this.refreshData.bind(_this), _this.skinConfig.refreshDelay);
                    });
                };

                Dashboard.formatNumber = function (num, digits) {
                    var output = num + "";
                    while (output.length < digits)
                        output = "0" + output;
                    return output;
                };

                Dashboard.timeToReadableString = function (date) {
                    // if we have ISO8601 (in UTC) then make it readable
                    // in the following default format: "Wednesday 08:26"
                    if (/(\d{4})-(\d{2})-(\d{2})T(\d{2})\:(\d{2})\:(\d{2})Z/.test(date)) {
                        var d = new Date(date);
                        return Telemetry.Strings.dayOfTheWeek[d.getUTCDay()] + ' ' + Dashboard.formatNumber(d.getUTCHours(), 2) + ':' + Dashboard.formatNumber(d.getUTCMinutes(), 2);
                    }
                    return date;
                };

                Dashboard.timeDifferenceToReadableString = function (date) {
                    // if we have ISO8601 (in UTC) then make it readable
                    // in the following default format: "1 day 8 hours 26 minutes"
                    if (/(\d{4})-(\d{2})-(\d{2})T(\d{2})\:(\d{2})\:(\d{2})Z/.test(date)) {
                        var d = new Date(date);
                        var dys = d.getUTCDate();
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

                Dashboard.prototype.setMeter = function (name, value, maxValue) {
                    if (typeof maxValue === "undefined") { maxValue = null; }
                    var className = '.' + name;
                    var $meter = $(className);
                    var minValue = $meter.data('min');
                    var maxValue = maxValue ? maxValue : $meter.data('max');
                    var minAngle = $meter.data('min-angle');
                    var maxAngle = $meter.data('max-angle');
                    value = Math.min(value, maxValue);
                    value = Math.max(value, minValue);
                    var offset = (value - minValue) / (maxValue - minValue);
                    var angle = (maxAngle - minAngle) * offset + minAngle;
                    var prevAngle = parseInt($(className).data('prev'));
                    $(className).data('prev', angle);
                    var updateTransform = function (v) {
                        $meter.css({
                            'transform': v,
                            '-webkit-transform': v,
                            '-moz-transform': v,
                            '-ms-transform': v,
                            '-o-transform': v
                        });
                    };
                    if (Math.abs(prevAngle - angle) < (maxAngle - minAngle) * 0.01) {
                        // fast update
                        updateTransform('rotate(' + angle + 'deg)');
                        return;
                    }

                    // animated update
                    $({ a: prevAngle }).animate({ a: angle }, {
                        duration: this.skinConfig.refreshDelay,
                        queue: false,
                        step: function (now) {
                            updateTransform('rotate(' + now + 'deg)');
                        }
                    });
                };

                Dashboard.prototype.process = function (data, reason) {
                    if (typeof reason === "undefined") { reason = ''; }
                    // update status message with failure reason
                    $('.statusMessage').html(reason);

                    // if we don't have real data we use default values
                    var data = data === null ? new Ets2TelemetryData() : data;

                    // tweak data using custom skin based filter
                    data = this.filter(data);

                    // tweak data using default internal filter
                    data = this.internalFilter(data);

                    // render data using internal method first
                    this.internalRender(data);

                    // then use skin based renderer if defined
                    this.render(data);
                };

                Dashboard.prototype.internalFilter = function (data) {
                    // convert ISO8601 to default readable format
                    data.gameTime = Dashboard.timeToReadableString(data.gameTime);
                    data.jobDeadlineTime = Dashboard.timeToReadableString(data.jobDeadlineTime);
                    data.jobRemainingTime = Dashboard.timeDifferenceToReadableString(data.jobRemainingTime);
                    return data;
                };

                Dashboard.prototype.internalRender = function (data) {
                    for (var name in data) {
                        var value = data[name];
                        var $e = this.$cache[name] !== undefined ? this.$cache[name] : this.$cache[name] = $('.' + name);
                        if (typeof value == "boolean") {
                            // all booleans will have "yes" class
                            // attached if value is true
                            if (value) {
                                $e.addClass('yes');
                            } else {
                                $e.removeClass('yes');
                            }
                        } else if (typeof value == "number") {
                            if ($e.data('type') == 'meter') {
                                // if type is set to meter
                                // then we use this HTML element
                                // as a rotating meter "arrow"
                                var maxValue = $e.data('max');
                                if (/[a-z]/i.test(maxValue)) {
                                    // if data-max attribute points
                                    // to a property name then we use its value
                                    maxValue = data[maxValue];
                                }
                                this.setMeter(name, value, parseFloat(maxValue));
                            } else {
                                // just display the number
                                $e.html(value);
                            }
                        } else if (typeof value == "string") {
                            // just display string as is
                            $e.html(value);
                        }

                        // set data-value attribute
                        // to allow attribute based custom CSS selectors
                        $e.attr('data-value', value);
                    }
                };

                // "forward" declarations for custom skins:
                // define custom data filter method for skins
                Dashboard.prototype.filter = function (data) {
                    return data;
                };

                // define custom data render method for skins
                Dashboard.prototype.render = function (data) {
                    return;
                };
                Dashboard.connectionTimeout = 3000;
                return Dashboard;
            })();
            Telemetry.Dashboard = Dashboard;
        })(Ets.Telemetry || (Ets.Telemetry = {}));
        var Telemetry = Ets.Telemetry;
    })(Funbit.Ets || (Funbit.Ets = {}));
    var Ets = Funbit.Ets;
})(Funbit || (Funbit = {}));
//# sourceMappingURL=dashboard-core.js.map
