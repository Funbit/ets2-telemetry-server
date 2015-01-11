/* 
    *** DO NOT CHANGE THIS SCRIPT ***

    Dashboard engine script that works with the telemetry REST API.
*/

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

    export interface ISkinConfig {
        name: string;
        refreshDelay: number;
    }

    export class Dashboard {

        private endpointUrl: string;
        private firstRun: boolean = true;
        private timer: any;
        private failCount: number = 0;
        private minFailCount: number = 2;
        private endpointSeed: number = 0;
        private skinConfig: ISkinConfig;

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

        constructor(telemetryEndpointUrl: string, skinConfig: ISkinConfig) {
            this.endpointUrl = telemetryEndpointUrl;
            this.skinConfig = skinConfig;
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
                timeout: Dashboard.connectionTimeout
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
                        this.refreshData.bind(this), this.skinConfig.refreshDelay);
                });
        }

        private formatNumber(num: number, digits: number): string {
            var output = num + "";
            while (output.length < digits) output = "0" + output;
            return output;
        }

        private isoToReadableDate(date: string): string {
            var d = new Date(date);
            return Dashboard.dayOfTheWeek[d.getDay()] + ' '
                + this.formatNumber(d.getHours(), 2) + ':'
                + this.formatNumber(d.getMinutes(), 2);
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
                duration: this.skinConfig.refreshDelay * 1.1,
                step: now => {
                    updateTransform('rotate(' + now + 'deg)');
                }
            });
        }

        private setSpeedometerValue(value: number) {
            this.setMeter('speedometer-arrow', value);
        }

        private setTachometerValue(value: number) {
            this.setMeter('tachometer-arrow', value / 100);
        }

        private setFuelValue(value: number, maxValue: number) {
            this.setMeter('fuel-arrow', value, maxValue);
        }

        private setTemperatureValue(value: number) {
            this.setMeter('temperature-arrow', value);
        }

        private setIndicatorStatus(name: string, condition: boolean) {
            var className = '.' + name;
            if (condition)
                $(className).addClass('on');
            else
                $(className).removeClass('on');
        }

        private setIndicatorText(name: string, value: string) {
            var className = '.' + name;
            $(className).html(value);
        }

        private dataRefreshSucceeded(data: IEts2TelemetryData) {
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
            this.setIndicatorText('gear', data.gear > 0 ? 'D' + data.gear :
                (data.gear < 0 ? 'R' : 'N'));
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
        }

        private dataRefreshFailed(reason: string) {
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
        }
        
    }

}