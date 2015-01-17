/// <reference path="typings/jquery.d.ts" />
/// <reference path="typings/jqueryui.d.ts" />

module Funbit.Ets.Telemetry {

    class Ets2TelemetryData {
        // dates
        gameTime: string = '';                   // absolute time in ISO8601
        jobDeadlineTime: string = '';            // absolute time in ISO8601
        jobRemainingTime: string = '';           // time difference in ISO8601
        // booleans
        connected: boolean = false;
        gamePaused: boolean = false;
        hasJob: boolean = false;
        trailerAttached: boolean = false;
        cruiseControlOn: boolean = false;
        wipersOn: boolean = false;
        parkBrakeOn: boolean = false;
        motorBrakeOn: boolean = false;
        electricOn: boolean = false;
        engineOn: boolean = false;
        blinkerLeftActive: boolean = false;
        blinkerRightActive: boolean = false;
        blinkerLeftOn: boolean = false;
        blinkerRightOn: boolean = false;
        lightsParkingOn: boolean = false;
        lightsBeamLowOn: boolean = false;
        lightsBeamHighOn: boolean = false;
        lightsAuxFrontOn: boolean = false;
        lightsAuxRoofOn: boolean = false;
        lightsBeaconOn: boolean = false;
        lightsBrakeOn: boolean = false;
        lightsReverseOn: boolean = false;
        batteryVoltageWarning: boolean = false;
        airPressureWarning: boolean = false;
        airPressureEmergency: boolean = false;
        adblueWarning: boolean = false;
        oilPressureWarning: boolean = false;
        waterTemperatureWarning: boolean = false;
        // strings
        telemetryPluginVersion: string = '';
        gameVersion: string = '';
        trailerId: string = '';
        trailerName: string = '';
        sourceCity: string = '';
        destinationCity: string = '';
        sourceCompany: string = '';
        destinationCompany: string = '';
        // numbers
        jobIncome: number = 0;
        truckSpeed: number = 0;
        accelerationX: number = 0;
        accelerationY: number = 0;
        accelerationZ: number = 0;
        coordinateX: number = 0;
        coordinateY: number = 0;
        coordinateZ: number = 0;
        rotationX: number = 0;
        rotationY: number = 0;
        rotationZ: number = 0;
        gear: number = 0;
        gears: number = 1;
        gearRanges: number = 0;
        gearRangeActive: number = 0;
        engineRpm: number = 0;
        engineRpmMax: number = 1;
        fuel: number = 0;
        fuelCapacity: number = 1;
        fuelAverageConsumption: number = 0;
        userSteer: number = 0;
        userThrottle: number = 0;
        userBrake: number = 0;
        userClutch: number = 0;
        gameSteer: number = 0;
        gameThrottle: number = 0;
        gameBrake: number = 0;
        gameClutch: number = 0;
        truckMass: number = 0;
        truckModelLength: number = 0;
        truckModelOffset: number = 0;
        trailerMass: number = 0;
        retarderBrake: number = 0;
        shifterSlot: number = 0;
        shifterToggle: number = 0;
        airPressure: number = 0;
        brakeTemperature: number = 0;
        fuelWarning: number = 0;
        adblue: number = 0;
        adblueConsumpton: number = 0;
        oilPressure: number = 0;
        oilTemperature: number = 0;
        waterTemperature: number = 0;
        batteryVoltage: number = 0;
        lightsDashboard: number = 0;
        wearEngine: number = 0;
        wearTransmission: number = 0;
        wearCabin: number = 0;
        wearChassis: number = 0;
        wearWheels: number = 0;
        wearTrailer: number = 0;
        truckOdometer: number = 0;
    }

    export class Dashboard {

        private endpointUrl: string;
        private timer: any;

        private failCount: number = 0;
        // minimum number of fails before turning dashboard off
        private minFailCount: number = 2;

        private anticacheSeed: number = 0;
        private skinConfig: ISkinConfiguration;
        // jquery element cache
        private $cache: any[] = [];
        
        private static connectionTimeout: number = 3000;

        constructor(telemetryEndpointUrl: string, skinConfig: ISkinConfiguration) {
            this.endpointUrl = telemetryEndpointUrl;
            this.skinConfig = skinConfig;
            jQuery.fx.interval = 1000 / this.skinConfig.animationFps; // default animation interval
            // here we are going into infinite refresh timer cycle
            this.refreshData();
        }

        private refreshData() {
            var url: string = this.endpointUrl + "?seed=" + this.anticacheSeed++;
            $.ajax({
                    url: url,
                    async: true,
                    dataType: 'json',
                    timeout: Dashboard.connectionTimeout
                })
                .done(d => {
                    if (!d.connected) {
                        this.process(null, Strings.connectedAndWaitingForDrive);
                        return;
                    }
                    this.process(d);
                    this.failCount = 0;
                })
                .fail(() => {
                    this.failCount++;
                    if (this.failCount > this.minFailCount) {
                        this.process(null, Strings.couldNotConnectToServer);
                    }
                })
                .always(() => {
                    this.timer = setTimeout(
                        this.refreshData.bind(this), this.skinConfig.refreshDelay);
                });
        } 
        
        public static formatNumber(num: number, digits: number): string {
            var output = num + "";
            while (output.length < digits) output = "0" + output;
            return output;
        }

        public static timeToReadableString(date: string): string {
            // if we have ISO8601 (in UTC) then make it readable
            // in the following default format: "Wednesday 08:26"
            if (/(\d{4})-(\d{2})-(\d{2})T(\d{2})\:(\d{2})\:(\d{2})Z/.test(date)) {
                var d = new Date(date);
                return Strings.dayOfTheWeek[d.getUTCDay()] + ' '
                    + Dashboard.formatNumber(d.getUTCHours(), 2) + ':'
                    + Dashboard.formatNumber(d.getUTCMinutes(), 2);
            }
            return date;
        }

        public static timeDifferenceToReadableString(date: string): string {
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
                    o = Strings.noTimeLeft;
                return o;
            }
            return date;
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
            if (Math.abs(prevAngle - angle) < (maxAngle - minAngle) * 0.01) {
                // fast update
                updateTransform('rotate(' + angle + 'deg)');
                return;
            }
            // animated update
            $({ a: prevAngle })
                .animate({ a: angle }, {
                    duration: this.skinConfig.refreshDelay,
                    queue: false,
                    step: now => {
                        updateTransform('rotate(' + now + 'deg)');
                    }
                });
        }
        
        private process(data: Ets2TelemetryData, reason: string = '') {
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
        }

        private internalFilter(data: Ets2TelemetryData): Ets2TelemetryData {
            // convert ISO8601 to default readable format
            data.gameTime = Dashboard.timeToReadableString(data.gameTime);
            data.jobDeadlineTime = Dashboard.timeToReadableString(data.jobDeadlineTime);
            data.jobRemainingTime = Dashboard.timeDifferenceToReadableString(data.jobRemainingTime);
            return data;
        }

        private internalRender(data: any) {
            // handle all telemetry properties by type
            for (var name in data) {
                var value = data[name];
                var $e = this.$cache[name] !== undefined
                    ? this.$cache[name]
                   : this.$cache[name] = $('.' + name);
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
        }

        // "forward" declarations for custom skins:

        // define custom data filter method for skins
        private filter(data: Ets2TelemetryData): Ets2TelemetryData {
            return data;
        }

        // define custom data render method for skins
        private render(data: Ets2TelemetryData) {
            return;
        }
        
    }

}