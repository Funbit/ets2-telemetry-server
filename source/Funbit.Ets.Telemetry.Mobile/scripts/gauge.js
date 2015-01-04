var Funbit;
(function (Funbit) {
    (function (Ets) {
        (function (Telemetry) {
            (function (Components) {
                var Gauge = (function () {
                    function Gauge(telemetryEndpointUrl, telemetryRefreshDelay) {
                        this.firstRun = true;
                        this.failCount = 0;
                        this.minFailCount = 2;
                        this.endpointSeed = 0;
                        this.endpointUrl = telemetryEndpointUrl;
                        this.refreshDelay = telemetryRefreshDelay;
                        this.initialize();
                    }
                    Gauge.prototype.initialize = function () {
                        this.refreshData();
                    };

                    Gauge.prototype.refreshData = function () {
                        var _this = this;
                        var url = this.endpointUrl + "?seed=" + this.endpointSeed++;
                        $.ajax({
                            url: url,
                            async: true,
                            dataType: 'json',
                            timeout: Gauge.connectionTimeout
                        }).done(function (d) {
                            _this.dataRefreshSucceeded(d);
                            _this.failCount = 0;
                        }).fail(function () {
                            _this.failCount++;
                            if (_this.failCount > _this.minFailCount) {
                                _this.dataRefreshFailed('Could not connect to the server');
                            }
                        }).always(function () {
                            _this.timer = setTimeout(_this.refreshData.bind(_this), _this.refreshDelay);
                        });
                    };

                    Gauge.prototype.formatNumber = function (num, digits) {
                        var output = num + "";
                        while (output.length < digits)
                            output = "0" + output;
                        return output;
                    };

                    Gauge.prototype.isoToReadableDate = function (date) {
                        var d = new Date(date);
                        return Gauge.dayOfTheWeek[d.getDay()] + ' ' + this.formatNumber(d.getHours(), 2) + ':' + this.formatNumber(d.getMinutes(), 2);
                    };

                    Gauge.prototype.turnIndicator = function (name, condition) {
                        var className = '.' + name;
                        if (condition)
                            $(className).addClass('on');
                        else
                            $(className).removeClass('on');
                    };

                    Gauge.prototype.setMeter = function (name, value, maxValue) {
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
                            updateTransform('rotate(' + angle + 'deg)');
                            return;
                        }

                        $({ a: prevAngle }).animate({ a: angle }, {
                            duration: this.refreshDelay * 1.1,
                            step: function (now) {
                                updateTransform('rotate(' + now + 'deg)');
                            }
                        });
                    };

                    Gauge.prototype.setSpeedometer = function (value) {
                        this.setMeter('speedometer-arrow', value);
                    };

                    Gauge.prototype.setTachometer = function (value) {
                        this.setMeter('tachometer-arrow', value / 100);
                    };

                    Gauge.prototype.setFuel = function (value, maxValue) {
                        this.setMeter('fuel-arrow', value, maxValue);
                    };

                    Gauge.prototype.setTemperature = function (value) {
                        this.setMeter('temperature-arrow', value);
                    };

                    Gauge.prototype.setIndicator = function (name, value) {
                        var className = '.' + name;
                        $(className).html(value);
                    };

                    Gauge.prototype.dataRefreshSucceeded = function (data) {
                        if (data.connected && data.gameTime.indexOf(Gauge.minDateValue) == 0) {
                            this.dataRefreshFailed('Connected, waiting for the drive...');
                            return;
                        }
                        if (!data.connected) {
                            this.dataRefreshFailed('Waiting for the simulator to run...');
                            return;
                        }
                        data.gameTime = this.isoToReadableDate(data.gameTime);
                        data.jobDeadlineTime = this.isoToReadableDate(data.jobDeadlineTime);
                        if (!$('.gauge').hasClass('on')) {
                            $('.gauge').addClass('on');
                        }
                        $('.time').removeClass('error');
                        this.setIndicator('time', data.gameTime);
                        if (data.sourceCity.length > 0) {
                            this.setIndicator('source', data.sourceCity + ' (' + data.sourceCompany + ')');
                            this.setIndicator('destination', data.destinationCity + ' (' + data.destinationCompany + ')');
                            this.setIndicator('deadline', data.jobDeadlineTime);
                        } else {
                            this.setIndicator('source', '');
                            this.setIndicator('destination', '');
                            this.setIndicator('deadline', '');
                        }
                        if (data.trailerAttached) {
                            this.setIndicator('trailer-mass', (data.trailerMass / 1000) + 't');
                            this.setIndicator('trailer-name', data.trailerName);
                        } else {
                            this.setIndicator('trailer-mass', '');
                            this.setIndicator('trailer-name', '');
                        }
                        this.setIndicator('odometer', (Math.round(data.truckOdometer * 10) / 10).toFixed(1));
                        this.setIndicator('gear', data.gear > 0 ? 'D' + data.gear : (data.gear < 0 ? 'R' : 'N'));
                        this.turnIndicator('trailer', data.trailerAttached);
                        this.turnIndicator('blinker-left', data.blinkerLeftOn);
                        this.turnIndicator('blinker-right', data.blinkerRightOn);
                        this.turnIndicator('cruise', data.cruiseControlOn);
                        this.turnIndicator('parking-lights', data.lightsParkingOn);
                        this.turnIndicator('highbeam', data.lightsBeamHighOn);
                        this.turnIndicator('lowbeam', data.lightsBeamLowOn && !data.lightsBeamHighOn);
                        this.setSpeedometer(data.truckSpeed * 3.6);
                        this.setTachometer(data.engineRpm);
                        this.setFuel(data.fuel, data.fuelCapacity);
                        this.setTemperature(data.waterTemperature);
                    };

                    Gauge.prototype.dataRefreshFailed = function (reason) {
                        this.setIndicator('time', reason);
                        if ($('.gauge').hasClass('on') || this.firstRun) {
                            this.firstRun = false;
                            $('.gauge').removeClass('on');
                            $('.time').addClass('error');
                            this.setIndicator('source', '');
                            this.setIndicator('destination', '');
                            this.setIndicator('deadline', '');
                            this.setIndicator('trailer-mass', '');
                            this.setIndicator('trailer-name', '');
                            this.setIndicator('odometer', '');
                            this.setIndicator('gear', '');
                            this.turnIndicator('trailer', false);
                            this.turnIndicator('blinker-left', false);
                            this.turnIndicator('blinker-right', false);
                            this.turnIndicator('cruise', false);
                            this.turnIndicator('parking-lights', false);
                            this.turnIndicator('highbeam', false);
                            this.turnIndicator('lowbeam', false);
                            this.setSpeedometer(0);
                            this.setTachometer(0);
                            this.setFuel(0, 1);
                            this.setTemperature(0);
                        }
                    };
                    Gauge.dayOfTheWeek = [
                        'Sunday',
                        'Monday',
                        'Tuesday',
                        'Wednesday',
                        'Thursday',
                        'Friday',
                        'Saturday'
                    ];

                    Gauge.minDateValue = "0001-01-01T00:00:00";

                    Gauge.connectionTimeout = 5000;
                    return Gauge;
                })();
                Components.Gauge = Gauge;
            })(Telemetry.Components || (Telemetry.Components = {}));
            var Components = Telemetry.Components;
        })(Ets.Telemetry || (Ets.Telemetry = {}));
        var Telemetry = Ets.Telemetry;
    })(Funbit.Ets || (Funbit.Ets = {}));
    var Ets = Funbit.Ets;
})(Funbit || (Funbit = {}));
