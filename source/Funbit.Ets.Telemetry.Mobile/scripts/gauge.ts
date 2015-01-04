/// <reference path="typings/jquery.d.ts" />
/// <reference path="typings/jqueryui.d.ts" />

module Funbit.Ets.Telemetry.Components {

    interface IEts2TelemetryData {
        connected: boolean;
        gameTime: string;
        gamePaused: boolean;
        telemetryPluginVersion: string;
        gameVersion: string;
        trailerAttached: boolean;
        truckSpeed: number;
        accelerationX: number;
        accelerationY: number;
        accelerationZ: number;
        coordinateX: number;
        coordinateY: number;
        coordinateZ: number;
        rotationX: number;
        rotationY: number;
        rotationZ: number;
        gear: number;
        gears: number;
        gearRanges: number;
        gearRangeActive: number;
        engineRpm: number;
        engineRpmMax: number;
        fuel: number;
        fuelCapacity: number;
        fuelAverageConsumption: number;
        userSteer: number;
        userThrottle: number;
        userBrake: number;
        userClutch: number;
        gameSteer: number;
        gameThrottle: number;
        gameBrake: number;
        gameClutch: number;
        truckMass: number;
        truckModelLength: number;
        truckModelOffset: number;
        trailerMass: number;
        trailerId: string;
        trailerName: string;
        jobIncome: number;
        jobDeadlineTime: string;
        sourceCity: string;
        destinationCity: string;
        sourceCompany: string;
        destinationCompany: string;
        retarderBrake: number;
        shifterSlot: number;
        shifterToggle: number;
        cruiseControlOn: boolean;
        wipersOn: boolean;
        parkBrakeOn: boolean;
        motorBrakeOn: boolean;
        electricOn: boolean;
        engineOn: boolean;
        blinkerLeftActive: boolean;
        blinkerRightActive: boolean;
        blinkerLeftOn: boolean;
        blinkerRightOn: boolean;
        lightsParkingOn: boolean;
        lightsBeamLowOn: boolean;
        lightsBeamHighOn: boolean;
        lightsAuxFrontOn: boolean;
        lightsAuxRoofOn: boolean;
        lightsBeaconOn: boolean;
        lightsBrakeOn: boolean;
        lightsReverseOn: boolean;
        batteryVoltageWarning: boolean;
        airPressureWarning: boolean;
        airPressureEmergency: boolean;
        adblueWarning: boolean;
        oilPressureWarning: boolean;
        waterTemperatureWarning: boolean;
        airPressure: number;
        brakeTemperature: number;
        fuelWarning: number;
        adblue: number;
        adblueConsumpton: number;
        oilPressure: number;
        oilTemperature: number;
        waterTemperature: number;
        batteryVoltage: number;
        lightsDashboard: number;
        wearEngine: number;
        wearTransmission: number;
        wearCabin: number;
        wearChassis: number;
        wearWheels: number;
        wearTrailer: number;
        truckOdometer: number;
    }

    export class Gauge {

        private refreshDelay: number;
        private endpointUrl: string;
        private firstRun: boolean = true;
        private timer: any;
        private failCount: number = 0;
        private minFailCount: number = 2;
        private endpointSeed: number = 0;
        
        private static dayOfTheWeek = [
            'Sunday',
            'Monday',
            'Tuesday',
            'Wednesday',
            'Thursday',
            'Friday',
            'Saturday'
        ];

        private static minDateValue: string = "0001-01-01T00:00:00";

        private static connectionTimeout: number = 5000;

        constructor(telemetryEndpointUrl: string, telemetryRefreshDelay: number) {
            this.endpointUrl = telemetryEndpointUrl;
            this.refreshDelay = telemetryRefreshDelay;
            this.initialize();
        }

        private initialize() {
            this.refreshData();
        }

        private refreshData() {
            var url: string = this.endpointUrl + "?seed=" + this.endpointSeed++;
            $.ajax({
                    url: url,
                    async: true,
                    dataType: 'json',
                    timeout: Gauge.connectionTimeout
                })
                .done(d => {
                    this.dataRefreshSucceeded(d);
                    this.failCount = 0;
                })
                .fail(() => {
                    this.failCount++;
                    if (this.failCount > this.minFailCount) {
                        this.dataRefreshFailed('Could not connect to the server');
                    }
                })
                .always(() => {
                    this.timer = setTimeout(
                        this.refreshData.bind(this), this.refreshDelay);
                });
        }

        private formatNumber(num: number, digits: number): string {
            var output = num + "";
            while (output.length < digits) output = "0" + output;
            return output;
        }

        private isoToReadableDate(date: string): string {
            var d = new Date(date);
            return Gauge.dayOfTheWeek[d.getDay()] + ' '
                + this.formatNumber(d.getHours(), 2) + ':'
                + this.formatNumber(d.getMinutes(), 2);
        }

        private turnIndicator(name: string, condition: boolean) {
            var className = '.' + name;
            if (condition)
                $(className).addClass('on');
            else
                $(className).removeClass('on');
        }

        private setMeter(name: string, value: number, maxValue: number = null) {
            var className = '.' + name;
            var $meter = $(className);
            var minValue: number = $meter.data('min');
            var maxValue: number = maxValue ? maxValue : $meter.data('max');
            var minAngle: number = $meter.data('min-angle');
            var maxAngle: number = $meter.data('max-angle');
            value = Math.min(value, maxValue);
            value = Math.max(value, minValue);
            var offset = (value - minValue) / (maxValue - minValue);
            var angle = (maxAngle - minAngle) * offset + minAngle;
            var prevAngle = parseInt($(className).data('prev'));
            $(className).data('prev', angle);
            var updateTransform = v => {
                $meter.css({
                    'transform': v,
                    '-webkit-transform': v,
                    '-moz-transform': v,
                    '-ms-transform': v,
                    '-o-transform': v
                });
            }
            if (Math.abs(prevAngle - angle) < (maxAngle - minAngle) * 0.005) {
                // fast update
                updateTransform('rotate(' + angle + 'deg)');
                return;
            }
            // animated update
            $({ a: prevAngle }).animate({ a: angle }, {
                duration: this.refreshDelay * 1.1,
                step: now => {
                    updateTransform('rotate(' + now + 'deg)');
                }
            });
        }

        private setSpeedometer(value: number) {
            this.setMeter('speedometer-arrow', value);
        }

        private setTachometer(value: number) {
            this.setMeter('tachometer-arrow', value / 100);
        }

        private setFuel(value: number, maxValue: number) {
            this.setMeter('fuel-arrow', value, maxValue);
        }

        private setTemperature(value: number) {
            this.setMeter('temperature-arrow', value);
        }

        private setIndicator(name: string, value: string) {
            var className = '.' + name;
            $(className).html(value);
        }

        private dataRefreshSucceeded(data: IEts2TelemetryData) {
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
                // we have job info set
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
            this.setIndicator('gear', data.gear > 0 ? 'D' + data.gear :
                (data.gear < 0 ? 'R' : 'N'));
            this.turnIndicator('trailer', data.trailerAttached);
            this.turnIndicator('blinker-left', data.blinkerLeftOn);
            this.turnIndicator('blinker-right', data.blinkerRightOn);
            this.turnIndicator('cruise', data.cruiseControlOn);
            this.turnIndicator('parking-lights', data.lightsParkingOn);
            this.turnIndicator('highbeam', data.lightsBeamHighOn);
            this.turnIndicator('lowbeam', data.lightsBeamLowOn && !data.lightsBeamHighOn);
            this.setSpeedometer(data.truckSpeed * 3.6); // convert to km/h
            this.setTachometer(data.engineRpm);
            this.setFuel(data.fuel, data.fuelCapacity);
            this.setTemperature(data.waterTemperature);
        }

        private dataRefreshFailed(reason: string) {
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
        }
        
    }

}