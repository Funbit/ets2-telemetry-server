/// <reference path="typings/jquery.d.ts" />
/// <reference path="typings/jqueryui.d.ts" />

module Funbit.Ets.Telemetry {

    export class App {

        private skinConfig: ISkinConfiguration;
        private dashboard: Dashboard;
        private anticacheSeed: number = Date.now();
        
        constructor() {
            this.skinConfig = Configuration.getInstance().getSkinConfiguration();
            this.initializeViewport();
            this.loadDashboardResources();
        }

        private initializeViewport() {

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
            var scale = 'scale(' + (ratio * 0.99) + ')'; // leave 1% padding
            var $body = $('body');
            $body.css('transform', scale);
            if (firefox) {
                $body.css('-moz-transform', scale);
            } else if (ie) {
                $body.css('-ms-transform', scale);
            } else {
                $body.css('-webkit-transform', scale);
            }

            // return to menu by a click
            $(document).add($body).on('click', () => {
                window.history.back();
            });

            // reload page when orientation changes
            $(window).on('orientationchange', () => {
                window.location.reload();
            });

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

        private getSkinResourceUrl(name: string): string {
            return Configuration.getUrl('/skins/' +
                this.skinConfig.name + '/' + name + '?seed=' + this.anticacheSeed++);
        }
        
        public loadDashboardResources() {
            var skinCssUrl = this.getSkinResourceUrl('dashboard.css');
            var skinHtmlUrl = this.getSkinResourceUrl('dashboard.html');
            var skinJsUrl = this.getSkinResourceUrl('dashboard.js');
            var signalrUrl = Configuration.getUrl('/signalr/hubs?seed=' + this.anticacheSeed++);
            // preload skin css
            $("head link[rel='stylesheet']").last()
                .after('<link rel="stylesheet" href="' + skinCssUrl + '" type="text/css">');
            // load skin html (synchronously)
            $.ajax({
                url: skinHtmlUrl,
                async: false,
                dataType: 'html',
                timeout: 3000
            }).done(html => {
                // include dashboard custom script
                html += '<script src="' + signalrUrl + '"></script>';
                html += '<script src="' + skinJsUrl + '"></script>';
                $('body').append(html);
                $('.dashboard').css({
                    position: 'absolute',
                    left: '0px',
                    top: '0px',
                    width: this.skinConfig.width + 'px',
                    height: this.skinConfig.height + 'px'
                });
            }).fail(() => {
                alert(Strings.dashboardHtmlLoadFailed + this.skinConfig.name);
            });
        }

        public connectDashboard() {
            if (!this.dashboard)
                this.dashboard = new Funbit.Ets.Telemetry.Dashboard(
                    Configuration.getUrl('/api/ets2/telemetry'), this.skinConfig);
        }

    }

}

//
// Application "entry-point"
//

if (Funbit.Ets.Telemetry.Configuration.isCordovaAvailable()) {
    $(document).on('deviceready', () => {
        (new Funbit.Ets.Telemetry.App()).connectDashboard();
    });
} else {
    (new Funbit.Ets.Telemetry.App()).connectDashboard();
}