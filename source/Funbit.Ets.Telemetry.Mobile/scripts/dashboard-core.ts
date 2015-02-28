/// <reference path="typings/jquery.d.ts" />
/// <reference path="typings/jqueryui.d.ts" />
/// <reference path="typings/signalr.d.ts" />

module Funbit.Ets.Telemetry {

    class Ets2TelemetryData {
        gameTime: string = '';                   // absolute time in ISO8601
        jobDeadlineTime: string = '';            // absolute time in ISO8601
        jobRemainingTime: string = '';           // time difference in ISO8601
        connected: boolean = false;
        // the rest properties are not defined here
        // (see IEts2TelemetryData.cs or JSON contents for reference)
    }

    export class Dashboard {

        private endpointUrl: string;
        private skinConfig: ISkinConfiguration;
        
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
            this.initializeRequestAnimationFrame();
            // call custom skin initialization function
            this.initialize(skinConfig, this.utilityFunctions(skinConfig));
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
                this.render(this.frameData, this.utilityFunctions(this.skinConfig));
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
            if (data != null && !data.connected) {
                // if we're not connected we reset the data 
                reason = Strings.connectedAndWaitingForDrive;
                data = null;
            }
            // update status message with failure reason
            $('.statusMessage').html(reason);
            // if we don't have real data we use default values
            var data = data === null ? new Ets2TelemetryData() : data;
            // tweak data using custom skin based filter
            data = this.filter(data, this.utilityFunctions(this.skinConfig));
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
            data.gameTime = this.timeToReadableString(data.gameTime);
            data.jobDeadlineTime = this.timeToReadableString(data.jobDeadlineTime);
            data.jobRemainingTime = this.timeDifferenceToReadableString(data.jobRemainingTime);
            return data;
        }

        private internalRender() {
            var frames = Math.max(1, this.lastDataRequestFrameDiff) * 1.0;
            for (var name in this.latestData) {
                var $e = this.$cache[name] !== undefined
                    ? this.$cache[name]
                    : this.$cache[name] = $('.' + name);
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
                        this.setMeter($meters, value,
                            parseFloat(minValue), parseFloat(maxValue));
                    } else {
                        var $notMeters = $e.not('[data-type="meter"]');
                        if ($notMeters.length > 0) {
                            $notMeters.html(value);
                        }
                    }
                } else if (typeof value == "boolean") {
                    if (value) {
                        $e.addClass('yes');
                    } else {
                        $e.removeClass('yes');
                    }
                } else if (typeof value == "string") {
                    $e.html(value);
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
                preloadImages: images => this.preloadImages(skinConfig, images)
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
            return String(Math.round(num * power) / power);
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