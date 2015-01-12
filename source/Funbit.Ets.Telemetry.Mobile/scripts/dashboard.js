/*
*** DO NOT CHANGE THIS SCRIPT ***
Dashboard engine core that uses telemetry REST API.
*/
var Funbit;
(function (Funbit) {
    (function (Ets) {
        /// <reference path="typings/jquery.d.ts" />
        /// <reference path="typings/jqueryui.d.ts" />
        (function (Telemetry) {
            var Dashboard = (function () {
                function Dashboard(telemetryEndpointUrl, skinConfig) {
                    this.firstRun = true;
                    this.failCount = 0;
                    this.minFailCount = 2;
                    this.endpointSeed = 0;
                    this.endpointUrl = telemetryEndpointUrl;
                    this.skinConfig = skinConfig;
                    this.initialize();
                }
                Dashboard.prototype.initialize = function () {
                    this.refreshData();
                };

                Dashboard.prototype.refreshData = function () {
                    var _this = this;
                    var url = this.endpointUrl + "?seed=" + this.endpointSeed++;
                    $.ajax({
                        url: url,
                        async: true,
                        dataType: 'json',
                        timeout: Dashboard.connectionTimeout
                    }).done(function (d) {
                        _this.dataRefreshSucceeded(d);
                        _this.failCount = 0;
                    }).fail(function () {
                        _this.failCount++;
                        if (_this.failCount > _this.minFailCount) {
                            _this.dataRefreshFailed('Could not connect to the server');
                        }
                    }).always(function () {
                        _this.timer = setTimeout(_this.refreshData.bind(_this), _this.skinConfig.refreshDelay);
                    });
                };

                Dashboard.prototype.formatNumber = function (num, digits) {
                    var output = num + "";
                    while (output.length < digits)
                        output = "0" + output;
                    return output;
                };

                Dashboard.prototype.isoToReadableDate = function (date) {
                    var d = new Date(date);
                    return Dashboard.dayOfTheWeek[d.getDay()] + ' ' + this.formatNumber(d.getHours(), 2) + ':' + this.formatNumber(d.getMinutes(), 2);
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
                    if (Math.abs(prevAngle - angle) < (maxAngle - minAngle) * 0.005) {
                        // fast update
                        updateTransform('rotate(' + angle + 'deg)');
                        return;
                    }

                    // animated update
                    $({ a: prevAngle }).animate({ a: angle }, {
                        duration: this.skinConfig.refreshDelay * 1.1,
                        step: function (now) {
                            updateTransform('rotate(' + now + 'deg)');
                        }
                    });
                };

                Dashboard.prototype.setSpeedometerValue = function (value) {
                    this.setMeter('speedometer-arrow', value);
                };

                Dashboard.prototype.setTachometerValue = function (value) {
                    this.setMeter('tachometer-arrow', value / 100);
                };

                Dashboard.prototype.setFuelValue = function (value, maxValue) {
                    this.setMeter('fuel-arrow', value, maxValue);
                };

                Dashboard.prototype.setTemperatureValue = function (value) {
                    this.setMeter('temperature-arrow', value);
                };

                Dashboard.prototype.setIndicatorStatus = function (name, condition) {
                    var className = '.' + name;
                    if (condition)
                        $(className).addClass('on');
                    else
                        $(className).removeClass('on');
                };

                Dashboard.prototype.setIndicatorText = function (name, value) {
                    var className = '.' + name;
                    $(className).html(value);
                };

                Dashboard.prototype.dataRefreshSucceeded = function (data) {
                    if (data.connected && data.gameTime.indexOf(Dashboard.minDateValue) == 0) {
                        this.dataRefreshFailed('Connected, waiting for the drive...');
                        return;
                    }
                    if (!data.connected) {
                        this.dataRefreshFailed('Waiting for the simulator to run...');
                        return;
                    }
                    data.gameTime = this.isoToReadableDate(data.gameTime);
                    data.jobDeadlineTime = this.isoToReadableDate(data.jobDeadlineTime);
                    if (!$('.dashboard').hasClass('on')) {
                        $('.dashboard').addClass('on');
                    }
                    $('.time').removeClass('error');
                    this.setIndicatorText('time', data.gameTime);
                    if (data.sourceCity.length > 0) {
                        // we have job info set
                        this.setIndicatorText('source', data.sourceCity + ' (' + data.sourceCompany + ')');
                        this.setIndicatorText('destination', data.destinationCity + ' (' + data.destinationCompany + ')');
                        this.setIndicatorText('deadline', data.jobDeadlineTime);
                    } else {
                        this.setIndicatorText('source', '');
                        this.setIndicatorText('destination', '');
                        this.setIndicatorText('deadline', '');
                    }
                    if (data.trailerAttached) {
                        this.setIndicatorText('trailer-mass', (data.trailerMass / 1000) + 't');
                        this.setIndicatorText('trailer-name', data.trailerName);
                    } else {
                        this.setIndicatorText('trailer-mass', '');
                        this.setIndicatorText('trailer-name', '');
                    }
                    this.setIndicatorText('odometer', (Math.round(data.truckOdometer * 10) / 10).toFixed(1));
                    this.setIndicatorText('gear', data.gear > 0 ? 'D' + data.gear : (data.gear < 0 ? 'R' : 'N'));
                    this.setIndicatorStatus('trailer', data.trailerAttached);
                    this.setIndicatorStatus('blinker-left', data.blinkerLeftOn);
                    this.setIndicatorStatus('blinker-right', data.blinkerRightOn);
                    this.setIndicatorStatus('cruise', data.cruiseControlOn);
                    this.setIndicatorStatus('parking-lights', data.lightsParkingOn);
                    this.setIndicatorStatus('highbeam', data.lightsBeamHighOn);
                    this.setIndicatorStatus('lowbeam', data.lightsBeamLowOn && !data.lightsBeamHighOn);
                    this.setSpeedometerValue(data.truckSpeed * 3.6); // convert to km/h
                    this.setTachometerValue(data.engineRpm);
                    this.setFuelValue(data.fuel, data.fuelCapacity);
                    this.setTemperatureValue(data.waterTemperature);
                };

                Dashboard.prototype.dataRefreshFailed = function (reason) {
                    this.setIndicatorText('time', reason);
                    if ($('.dashboard').hasClass('on') || this.firstRun) {
                        this.firstRun = false;
                        $('.dashboard').removeClass('on');
                        $('.time').addClass('error');
                        this.setIndicatorText('source', '');
                        this.setIndicatorText('destination', '');
                        this.setIndicatorText('deadline', '');
                        this.setIndicatorText('trailer-mass', '');
                        this.setIndicatorText('trailer-name', '');
                        this.setIndicatorText('odometer', '');
                        this.setIndicatorText('gear', '');
                        this.setIndicatorStatus('trailer', false);
                        this.setIndicatorStatus('blinker-left', false);
                        this.setIndicatorStatus('blinker-right', false);
                        this.setIndicatorStatus('cruise', false);
                        this.setIndicatorStatus('parking-lights', false);
                        this.setIndicatorStatus('highbeam', false);
                        this.setIndicatorStatus('lowbeam', false);
                        this.setSpeedometerValue(0);
                        this.setTachometerValue(0);
                        this.setFuelValue(0, 1);
                        this.setTemperatureValue(0);
                    }
                };
                Dashboard.dayOfTheWeek = [
                    'Sunday',
                    'Monday',
                    'Tuesday',
                    'Wednesday',
                    'Thursday',
                    'Friday',
                    'Saturday'
                ];

                Dashboard.minDateValue = "0001-01-01T00:00:00";

                Dashboard.connectionTimeout = 5000;
                return Dashboard;
            })();
            Telemetry.Dashboard = Dashboard;
        })(Ets.Telemetry || (Ets.Telemetry = {}));
        var Telemetry = Ets.Telemetry;
    })(Funbit.Ets || (Funbit.Ets = {}));
    var Ets = Funbit.Ets;
})(Funbit || (Funbit = {}));
//# sourceMappingURL=dashboard.js.map
