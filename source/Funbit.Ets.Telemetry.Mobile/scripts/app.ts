/// <reference path="typings/jquery.d.ts" />
/// <reference path="typings/jqueryui.d.ts" />

module Funbit.Ets.Telemetry {

    export class App {

        private config: Configuration;
        private skinConfig: ISkinConfiguration;
        private dashboard: Dashboard;
        private anticacheSeed: number = Date.now();
        private resizeTimer: any;

        public static instance: App;

        constructor() {
            $.when(Configuration.getInstance().initialized).done(config => {
                this.config = config;
                this.skinConfig = config.getSkinConfiguration();
                this.initializeViewport();
                this.initializeDashboard();
            });
        }

        private initializeViewport() {
            // If the skin defines positive dimensions, we honor those.
            // If the skin defines zero or negative dimensions, it means the skin will handle the viewport by itself.
            if (this.skinConfig.width < 1 || this.skinConfig.height < 1) {
                return;
            }

            var ie = /Trident/.test(navigator.userAgent);
            var firefox = /Firefox/i.test(navigator.userAgent);
            var ios = /iPhone|iPad|iPod/i.test(navigator.userAgent);

            // fit viewport into the screen
            var dashboardWidth: number = this.skinConfig.width * 1.0;
            var dashboardHeight: number = this.skinConfig.height * 1.0;
            var windowWidth = $(window).width();
            var windowHeight = $(window).height();
            var ratio = windowHeight / dashboardHeight;
            if ((windowHeight / dashboardHeight) > (windowWidth / dashboardWidth)) {
                ratio = windowWidth / dashboardWidth;
            }
            var scale = 'scale(' + ratio + ')';
            var $body = $('body');
            $body.css('transform', scale);
            if (firefox) {
                $body.css('-moz-transform', scale);
            } else if (ie) {
                $body.css('-ms-transform', scale);
            } else {
                $body.css('-webkit-transform', scale);
            }

            // reload page when orientation changes
            $(window).on('orientationchange', () => {
                window.location.reload();
            });

            // setup window size tracking timer (after 1/4 sec delay)
            if (!ios) { // (Safari on iOS goes crazy with resize event, so we disable it)
                $(window).resize(() => {
                    clearTimeout(this.resizeTimer);
                    this.resizeTimer = setTimeout(() => {
                        window.location.reload();
                    }, 250);
                });
            }

            // prevent iOS device from sleeping
            if (ios && !Configuration.isCordovaAvailable()) {
                // the technique is very simple:
                // we just tell the browser that we are going to change the url
                // and at the last moment we abort the operation
                setInterval(() => {
                    window.location.href = "/";
                    window.setTimeout(() => {
                        window['stop']();
                    }, 0);
                }, 30000);
            }

        }

        public initializeDashboard() {
            var skinCssUrl = this.config.getSkinResourceUrl(this.skinConfig, 'dashboard.css');
            var skinHtmlUrl = this.config.getSkinResourceUrl(this.skinConfig, 'dashboard.html');
            var skinJsUrl = this.config.getSkinResourceUrl(this.skinConfig, 'dashboard.js');
            var signalrUrl = Configuration.getUrl('/signalr/hubs?seed=' + this.anticacheSeed++);
            // preload skin css
            $("head link[rel='stylesheet']").last()
                .after('<link rel="stylesheet" href="' + skinCssUrl + '" type="text/css">');
            // load skin html (synchronously)
            $.ajax({
                url: skinHtmlUrl,
                dataType: 'html',
                timeout: 3000
            }).done(html => {
                // include dashboard custom script
                html += '<script src="' + signalrUrl + '"></script>';
                html += '<script src="' + skinJsUrl + '"></script>';
                $('body').append(html);

                // If the skin defines positive dimensions, we honor those.
                // If the skin defines zero or negative dimensions, it means the skin will handle the viewport by itself.
                if (this.skinConfig.width > 0 && this.skinConfig.height > 0) {
                    $('.dashboard').css({
                        position: 'absolute',
                        left: '0px',
                        top: '0px',
                        width: this.skinConfig.width + 'px',
                        height: this.skinConfig.height + 'px'
                    });
                }
                this.dashboard = new Funbit.Ets.Telemetry.Dashboard(
                    Configuration.getUrl('/api/ets2/telemetry'), this.skinConfig);
            }).fail(() => {
                alert(Strings.dashboardHtmlLoadFailed + this.skinConfig.name);
            });
        }

    }

}

//
// Application "entry-point"
//

if (Funbit.Ets.Telemetry.Configuration.isCordovaAvailable()) {
    $(document).on('deviceready', () => {
        Funbit.Ets.Telemetry.App.instance = new Funbit.Ets.Telemetry.App();
    });
} else {
    Funbit.Ets.Telemetry.App.instance = new Funbit.Ets.Telemetry.App();
}
