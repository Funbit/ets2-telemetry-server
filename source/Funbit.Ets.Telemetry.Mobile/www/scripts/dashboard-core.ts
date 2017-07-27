/// <reference path="typings/jquery.d.ts" />
/// <reference path="typings/jqueryui.d.ts" />
/// <reference path="typings/signalr.d.ts" />

module Funbit.Ets.Telemetry {

    // default telemetry values used when game connection is lost
   
    class Ets2Game {
        connected: boolean = false;
        gameName: string = "";
        paused: boolean = false;
        time: string = "";
        timeScale: number = 0;
        nextRestStopTime: string = "";
        version: string = "";
        telemetryPluginVersion: string = "";
    }

    class Ets2Job {
        income: number = 0;
        deadlineTime: string = "";
        remainingTime: string = "";
        sourceCity: string = "";
        sourceCompany: string = "";
        destinationCity: string = "";
        destinationCompany: string = "";
    }

    class Ets2Truck {
        id: string = "";
        make: string = "";
        model: string = "";
        speed: number = 0;
        cruiseControlSpeed: number = 0;
        cruiseControlOn: boolean = false;
        odometer: number = 0;
        gear: number = 0;
        displayedGear: number = 0;
        forwardGears: number = 0;
        reverseGears: number = 0;
        shifterType: string = "";
        engineRpm: number = 0;
        engineRpmMax: number = 0;
        fuel: number = 0;
        fuelCapacity: number = 0;
        fuelAverageConsumption: number = 0;
        fuelWarningFactor: number = 0;
        fuelWarningOn: boolean = false;
        wearEngine: number = 0;
        wearTransmission: number = 0;
        wearCabin: number = 0;
        wearChassis: number = 0;
        wearWheels: number = 0;
        userSteer: number = 0;
        userThrottle: number = 0;
        userBrake: number = 0;
        userClutch: number = 0;
        gameSteer: number = 0;
        gameThrottle: number = 0;
        gameBrake: number = 0;
        gameClutch: number = 0;
        shifterSlot: number = 0;
        shifterToggle: number = 0;
        engineOn: boolean = false;
        electricOn: boolean = false;
        wipersOn: boolean = false;
        retarderBrake: number = 0;
        retarderStepCount: number = 0;
        parkBrakeOn: boolean = false;
        motorBrakeOn: boolean = false;
        brakeTemperature: number = 0;
        adblue: number = 0;
        adblueCapacity: number = 0;
        adblueAverageConsumption: number = 0;
        adblueWarningOn: boolean = false;
        airPressure: number = 0;
        airPressureWarningOn: boolean = false;
        airPressureWarningValue: number = 0;
        airPressureEmergencyOn: boolean = false;
        airPressureEmergencyValue: number = 0;
        oilTemperature: number = 0;
        oilPressure: number = 0;
        oilPressureWarningOn: boolean = false;
        oilPressureWarningValue: number = 0;
        waterTemperature: number = 0;
        waterTemperatureWarningOn: boolean = false;
        waterTemperatureWarningValue: number = 0;
        batteryVoltage: number = 0;
        batteryVoltageWarningOn: boolean = false;
        batteryVoltageWarningValue: number = 0;
        lightsDashboardValue: number = 0;
        lightsDashboardOn: boolean = false;
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
        placement: Ets2Placement;
        acceleration: Ets2Vector;
        head: Ets2Vector;
        cabin: Ets2Vector;
        hook: Ets2Vector;
        constructor() {
            this.placement = new Ets2Placement();
            this.acceleration = new Ets2Vector();
            this.head = new Ets2Vector();
            this.cabin = new Ets2Vector();
            this.hook = new Ets2Vector();
        }
    }

    class Ets2Trailer {
        attached: boolean = false;
        id: string = "";
        name: string = "";
        mass: number = 0;
        wear: number = 0;
        placement: Ets2Placement;
        constructor() {
            this.placement = new Ets2Placement();
        }
    }

    class Ets2Navigation {
        estimatedTime: string = "";
        estimatedDistance: number = 0;
        speedLimit: number = 0;
    }

    class Ets2Vector {
        x: number = 0;
        y: number = 0;
        z: number = 0;
    }

    class Ets2Placement {
        x: number = 0;
        y: number = 0;
        z: number = 0;
        heading: number = 0;
        pitch: number = 0;
        roll: number = 0;
    }

    class Ets2TelemetryData {
        game: Ets2Game;
        truck: Ets2Truck;
        trailer: Ets2Trailer;
        job: Ets2Job;
        navigation: Ets2Navigation;
        constructor() {
            this.game = new Ets2Game();
            this.truck = new Ets2Truck();
            this.trailer = new Ets2Trailer();
            this.job = new Ets2Job();
            this.navigation = new Ets2Navigation();
        }
    }

    export class Dashboard {

        private endpointUrl: string;
        private skinConfig: ISkinConfiguration;
        private utils: any;
        
        private $cache: any[] = [];

        private ets2TelemetryHub: any;
        private lastDataRequestFrame: number = 0;
        private lastDataRequestFrameDiff: number = 0;

        private frame: number = 0;
        private latestData: any = null;
        private prevData: any = null;
        private frameData: any = null;
        private lastRafShimTime = 0;

        private reconnectTimer: any;
        private static reconnectDelay: number = 1000;
        
        constructor(telemetryEndpointUrl: string, skinConfig: ISkinConfiguration) {
            this.endpointUrl = telemetryEndpointUrl;
            this.skinConfig = skinConfig;
            this.utils = this.utilityFunctions(skinConfig);
            this.initializeRequestAnimationFrame();
            // call custom skin initialization function
            this.initialize(skinConfig, this.utils);
            // run infinite animation loop
            this.animationLoop();
            // initialize SignalR after some time to overcome some browser bugs
            this.reconnectTimer = this.setTimer(this.reconnectTimer, () => {
                this.initializeHub();
                this.connectToHub();
            }, 100);
        }

        private setTimer(timer: any, func: any, delay: number): any {
            if (timer)
                clearTimeout(timer);
            return setTimeout(() => func(), delay);
        }

        private initializeRequestAnimationFrame() {
            // requestAnimationFrame polyfill
            var vendors = ['ms', 'moz', 'webkit', 'o'];
            for (var x = 0; x < vendors.length && !window.requestAnimationFrame; ++x) {
                window.requestAnimationFrame = window[vendors[x] + 'RequestAnimationFrame'];
                window.cancelAnimationFrame = window[vendors[x] + 'CancelAnimationFrame']
                    || window[vendors[x] + 'CancelRequestAnimationFrame'];
            }
            if (!window.requestAnimationFrame)
                window.requestAnimationFrame = callback => {
                    var now = Date.now();
                    // on old devices shim to set timer with target frame rate of 30 fps
                    var timeToCall = Math.max(0, (1000 / 30.0) - (now - this.lastRafShimTime));
                    var id = window.setTimeout(() => {
                        callback(now + timeToCall);
                    }, timeToCall);
                    this.lastRafShimTime = now + timeToCall;
                    return id;
                };
            if (!window.cancelAnimationFrame)
                window.cancelAnimationFrame = id => {
                    clearTimeout(id);
                }; 
        }

        private animationLoop() {
            this.frame++;
            window.requestAnimationFrame(() => this.animationLoop());
            // render animated elements
            if (this.latestData && this.prevData) {
                // use our internal renderer first
                this.internalRender();
                // and then use skin based renderer
                this.render(this.frameData, this.utils);
            }
        }
        
        private initializeHub() {
            $.connection.hub.logging = false;
            $.connection.hub.url = Configuration.getUrl('/signalr');
            this.ets2TelemetryHub = $.connection['ets2TelemetryHub'];
            window.onbeforeunload = () => {
                $.connection.hub.stop();
            };
        }

        private connectToHub() {
            $.connection.hub.stop();
            this.ets2TelemetryHub.client['updateData'] = json => {
                this.dataUpdateCallback(json);
            };
            $.connection.hub.reconnected(() => {
                this.requestDataUpdate();
            });
            $.connection.hub.reconnecting(() => {
                this.process(null, Strings.connectingToServer);
            });
            $.connection.hub.disconnected(() => {
                this.process(null, Strings.disconnectedFromServer);
                this.reconnectToHubAfterDelay();
            });
            $.connection.hub.start().done(() => {
                this.requestDataUpdate();
            }).fail(() => {
                this.process(null, Strings.couldNotConnectToServer);
                this.reconnectToHubAfterDelay();
            });
        }

        private reconnectToHubAfterDelay() {
            this.process(null, Strings.connectingToServer);
            this.reconnectTimer = this.setTimer(this.reconnectTimer, () => {
                this.connectToHub();
            }, Dashboard.reconnectDelay);
        }
        
        private requestDataUpdate() {
            this.lastDataRequestFrame = this.frame;
            this.ets2TelemetryHub.server['requestData']();
        }

        private dataUpdateCallback(jsonData: any) {
            var data = JSON.parse(jsonData);
            this.process(data);
            this.requestDataUpdate();
        }
        
        private process(data: Ets2TelemetryData, reason: string = '') {
            if (data != null && data.game != null && !data.game.connected) {
                // if we're not connected we reset the data 
                reason = Strings.connectedAndWaitingForDrive;
                // use default values
                data = new Ets2TelemetryData();
            } else if (data === null) {
                // if we don't have real data we use default values
                data = new Ets2TelemetryData();
            }
            // update status message with failure reason
            $('.statusMessage').html(reason);
            // tweak data using custom skin based filter
            data = this.filter(data, this.utils);
            // tweak data using default internal filter
            data = this.internalFilter(data);
            // update data buffers
            this.lastDataRequestFrameDiff = this.frame - this.lastDataRequestFrame;
            this.prevData = this.latestData;
            this.frameData = this.latestData;
            this.latestData = data;
        }

        private internalFilter(data: Ets2TelemetryData): Ets2TelemetryData {
            // convert ISO8601 to default readable format
            data.game.time = this.timeToReadableString(data.game.time);
            data.job.deadlineTime = this.timeToReadableString(data.job.deadlineTime);
            data.job.remainingTime = this.timeDifferenceToReadableString(data.job.remainingTime);
            return data;
        }

        private internalRender(parent: any = null, propNamePrefix: string = null) {
            var propSplitter = '.';
            var cssPropertySplitter = '-';
            var frames = Math.max(1, this.lastDataRequestFrameDiff) * 1.0;
            var object = parent != null ? parent : this.latestData;
            for (var propName in object) {
                var fullPropName = propNamePrefix != null ? propNamePrefix + propName : propName;
                var value = object[propName];
                var $e = this.$cache[fullPropName] !== undefined
                    ? this.$cache[fullPropName]
                    : this.$cache[fullPropName] = $('.' + this
                        .replaceAll(fullPropName, propSplitter, cssPropertySplitter));
                if (typeof value === "number") {
                    // calculate interpolated value for current frame
                    var prevValue = this.resolveObjectByPath(this.prevData, fullPropName);
                    value = this.resolveObjectByPath(
                        this.frameData, fullPropName) + (value - prevValue) / frames;
                    if (propNamePrefix == null) {
                        this.frameData[propName] = value;
                    } else {
                        var parentPropName = fullPropName.substr(
                            0, fullPropName.lastIndexOf(propSplitter));
                        this.resolveObjectByPath(
                            this.frameData, parentPropName)[propName] = value;
                    }
                    var $meters = $e.filter('[data-type="meter"]');
                    if ($meters.length > 0) {
                        // if type is set to meter 
                        // then we use this HTML element 
                        // as a rotating meter "arrow"
                        var minValue = $meters.data('min');
                        if (/^[a-z\.]+$/i.test(minValue)) {
                            // if data-min attribute points
                            // to a property name then we use its value
                            minValue = this.resolveObjectByPath(
                                this.latestData, minValue);
                        }
                        var maxValue = $meters.data('max');
                        if (/^[a-z\.]+$/i.test(maxValue)) {
                            // if data-max attribute points
                            // to a property name then we use its value
                            maxValue = this.resolveObjectByPath(
                                this.latestData, maxValue);
                        }
                        this.setMeter($meters, value,
                            parseFloat(minValue), parseFloat(maxValue));
                    } else {
                        var $notMeters = $e.not('[data-type="meter"]');
                        if ($notMeters.length > 0) {
                            // convert number to a string
                            // and render it by updating HTML element content
                            value = "" + Math.round(value);
                            $notMeters.html(value);
                        }
                    }
                } else if (typeof value === "boolean") {
                    // render boolean by adding/removing "yes" CSS class
                    if (value) {
                        $e.addClass('yes');
                    } else {
                        $e.removeClass('yes');
                    }
                } else if (typeof value === "string") {
                    // render string value by updating HTML element content
                    $e.html(value);
                } else if ($.isArray(value)) {
                    // recursively process arrays
                    for (var j = 0; j < value.length; j++) {
                        this.internalRender(value[j],
                            fullPropName + propSplitter + j + propSplitter);
                    }
                } else if (typeof value === "object") {
                    // recursively process complex objects
                    this.internalRender(value,
                        fullPropName + propSplitter);
                }
                $e.attr('data-value', value);
            }
        }

        private setMeter($meter: any, value: number, minValue: number, maxValue: number) {
            var maxValue: number = maxValue ? maxValue : $meter.data('max');
            var minAngle: number = $meter.data('min-angle');
            var maxAngle: number = $meter.data('max-angle');
            value = Math.min(value, maxValue);
            value = Math.max(value, minValue);
            var offset = (value - minValue) / (maxValue - minValue);
            var angle = (maxAngle - minAngle) * offset + minAngle;
            var updateTransform = v => {
                $meter.css({
                    'transform': v,
                    '-webkit-transform': v,
                    '-moz-transform': v,
                    '-ms-transform': v
                });
            };
            updateTransform('rotate(' + angle + 'deg)');
        }

        // utility functions available for custom skins:

        private utilityFunctions(skinConfig: ISkinConfiguration): any {
            return {
                formatInteger: this.formatInteger,
                formatFloat: this.formatFloat,
                preloadImages: images => this.preloadImages(skinConfig, images),
                resolveObjectByPath: this.resolveObjectByPath
            }
        }

        private preloadImages(skinConfig: ISkinConfiguration, images: string[]) {
            $(images).each(function () {
                $('<img/>')[0]['src'] = Configuration.getInstance()
                    .getSkinResourceUrl(skinConfig, this);
            });
        }

        private formatInteger(num: number, digits: number): string {
            var output = num + "";
            while (output.length < digits) output = "0" + output;
            return output;
        }

        private formatFloat(num: number, digits: number): string {
            var power = Math.pow(10, digits || 0);
            return String((Math.round(num * power) / power).toFixed(digits));
        }

        private isIso8601(date: string): boolean {
            return /(\d{4})-(\d{2})-(\d{2})T(\d{2})\:(\d{2})\:(\d{2})Z/.test(date);
        }

        private timeToReadableString(date: string): string {
            // if we have ISO8601 (in UTC) then make it readable
            // in the following default format: "Wednesday 08:26"
            if (this.isIso8601(date)) {
                var d = new Date(date);
                return Strings.dayOfTheWeek[d.getUTCDay()] + ' '
                    + this.formatInteger(d.getUTCHours(), 2) + ':'
                    + this.formatInteger(d.getUTCMinutes(), 2);
            }
            return date;
        }

        private timeDifferenceToReadableString(date: string): string {
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
                    o = Strings.noTimeLeft;
                return o;
            }
            return date;
        }

        private replaceAll(input: string, search: string, replace: string): string {
            return input.replace(new RegExp('\\' + search, 'g'), replace);
        }

        private resolveObjectByPath(obj: any, path: string): any {
            // access obj by property path, 
            // example:
            // "truck.speed"
            // "truck.wheels"
            // or for array elements:
            // "truck.wheels.0.steerable"
            return path.split('.').reduce(
                (prev, curr) =>
                    prev ? prev[curr] : undefined, obj || self)
        }

        // "forward" declarations for custom skins:

        // define custom data filter method for skins
        private filter(data: Ets2TelemetryData, utils: any): any {
            return data;
        }

        // define custom data render method for skins
        private render(data: any, utils: any) {
            return;
        }

        // define custom initialization function
        private initialize(skinConfig: ISkinConfiguration, utils: any) {
            return;
        }
    }

}