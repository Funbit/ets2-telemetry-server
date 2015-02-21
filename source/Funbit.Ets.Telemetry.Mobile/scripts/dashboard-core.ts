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
        // jquery element cache
        private $cache: any[] = [];
        private ets2TelemetryHub: any;
        
        private lastDisconnectTimestamp: number = 0;
        private static reconnectDelay: number = 3000;
        
        constructor(telemetryEndpointUrl: string, skinConfig: ISkinConfiguration) {
            this.endpointUrl = telemetryEndpointUrl;
            this.skinConfig = skinConfig;
            this.adjustRefreshRate();
            // call custom skin initialization function
            this.initialize(skinConfig);
            // initialize SignalR based sync (using WebSockets)
            this.initializeSignalR();
        }

        private initializeMeters() {
            var $animated = $('[data-type="meter"]').add('.animated');
            var ie = /Trident/.test(navigator.userAgent);
            // fix to make animation a bit longer for additional smoothness (but not in IE)
            var dataLatency = ie ? -17 : +17; 
            var value = ((this.skinConfig.refreshRate + dataLatency) / 1000.0) + 's linear';
            $animated.css({
                '-webkit-transition': value,
                '-moz-transition': value,
                '-o-transition': value,
                '-ms-transition': value,
                'transition': value
            });
        }

        private adjustRefreshRate() {
            if (!this.skinConfig.refreshRate) this.skinConfig.refreshRate = 0;
            var now = Date.now();
            var lastDisconnectInterval = now - this.lastDisconnectTimestamp;
            if (lastDisconnectInterval < 1 * 60 * 1000)  // 1 min
                this.skinConfig.refreshRate += 20;
            if (lastDisconnectInterval < 2 * 60 * 1000)  // 2 min
                this.skinConfig.refreshRate += 10;
            if (lastDisconnectInterval < 3 * 60 * 1000)  // 3 min
                this.skinConfig.refreshRate += 5;
            // adaptive refresh rate adjustment within range [50...250]
            this.skinConfig.refreshRate = Math.min(250, this.skinConfig.refreshRate);
            this.skinConfig.refreshRate = Math.max(50, this.skinConfig.refreshRate);
            this.initializeMeters();
        }

        private initializeSignalR() {
            $.connection.hub.url = Configuration.getUrl('/signalr');
            this.ets2TelemetryHub = $.connection['ets2TelemetryHub'];
            this.ets2TelemetryHub.client['updateData'] = data => {
                var $processed = this.process(JSON.parse(data));
                $.when.apply($, $processed).done(() => {
                    this.ets2TelemetryHub.server['requestData']();
                });
            };
            $.connection.hub.reconnecting(() => {
                this.process(null, Strings.connectingToServer);
            });
            $.connection.hub.disconnected(() => {
                this.process(null, Strings.couldNotConnectToServer);
                this.adjustRefreshRate();
                this.lastDisconnectTimestamp = Date.now();
                setTimeout(() => {
                    $.connection.hub.start();
                }, Dashboard.reconnectDelay);
            });
            $.connection.hub.start().done(() => {
                this.ets2TelemetryHub.server['requestData']();
            }).fail(() => {
                this.process(null, Strings.couldNotConnectToServer);
            });
        }
        
        private process(data: Ets2TelemetryData, reason: string = ''): JQueryDeferred<any>[]  {
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
            data = this.filter(data);
            // tweak data using default internal filter
            data = this.internalFilter(data);
            // render data using internal method first
            var $renderFinished = this.internalRender(data);
            // then use skin based renderer if defined
            this.render(data);
            // return deferred object that resolves when animation finishes
            return $renderFinished;
        }

        private internalFilter(data: Ets2TelemetryData): Ets2TelemetryData {
            // convert ISO8601 to default readable format
            data.gameTime = Dashboard.timeToReadableString(data.gameTime);
            data.jobDeadlineTime = Dashboard.timeToReadableString(data.jobDeadlineTime);
            data.jobRemainingTime = Dashboard.timeDifferenceToReadableString(data.jobRemainingTime);
            return data;
        }

        private internalRender(data: any): JQueryDeferred<any>[] {
            var $animations = [];
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
                    var $meter = $e.filter('[data-type="meter"]');
                    if ($meter.length > 0) {
                        // if type is set to meter 
                        // then we use this HTML element 
                        // as a rotating meter "arrow"
                        var minValue = $meter.data('min');
                        if (/[a-z]/i.test(minValue)) {
                            // if data-min attribute points
                            // to a property name then we use its value
                            minValue = data[minValue];
                        }
                        var maxValue = $meter.data('max');
                        if (/[a-z]/i.test(maxValue)) {
                            // if data-max attribute points
                            // to a property name then we use its value
                            maxValue = data[maxValue];
                        }
                        $animations.push(this.setMeter($meter, value,
                            parseFloat(minValue), parseFloat(maxValue)));
                    }
                    var $value = $e.not('[data-type="meter"]');
                    if ($value.length > 0) {
                        // just display the number
                        $value.html(value);
                    }
                } else if (typeof value == "string") {
                    // just display string as is
                    $e.html(value);
                }
                // set data-value attribute 
                // to allow attribute based custom CSS selectors 
                $e.attr('data-value', value);
            }
            return $animations;
        }

        private setMeter($meter: any, value: number,
            minValue: number, maxValue: number): JQueryDeferred<any> {
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
            var $animationFinished = $.Deferred<any>();
            setTimeout(() => { $animationFinished.resolve(); }, this.skinConfig.refreshRate);
            return $animationFinished;
        }

        // utility functions available for custom skins:

        public static formatNumber(num: number, digits: number): string {
            var output = num + "";
            while (output.length < digits) output = "0" + output;
            return output;
        }

        public static isIso8601(date: string): boolean {
            return /(\d{4})-(\d{2})-(\d{2})T(\d{2})\:(\d{2})\:(\d{2})Z/.test(date);
        }

        public static timeToReadableString(date: string): string {
            // if we have ISO8601 (in UTC) then make it readable
            // in the following default format: "Wednesday 08:26"
            if (this.isIso8601(date)) {
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
        private filter(data: Ets2TelemetryData): Ets2TelemetryData {
            return data;
        }

        // define custom data render method for skins
        private render(data: Ets2TelemetryData) {
            return;
        }

        // define custom initialization function
        private initialize(skinConfig: ISkinConfiguration) {
            return;
        }
        
    }

}