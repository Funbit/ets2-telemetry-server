/// <reference path="typings/jquery.d.ts" />
/// <reference path="typings/jqueryui.d.ts" />

module Funbit.Ets.Telemetry {

    export class App {

        private skinConfig: ISkinConfiguration;
        private dashboard: Dashboard;
        private anticacheSeed: number = 0;

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
        
        public loadDashboardResources() {
            var skinCssUrl = Configuration.getUrl('/skins/' +
                this.skinConfig.name + '/dashboard.css?seed=' + this.anticacheSeed++);
            var skinHtmlUrl = Configuration.getUrl('/skins/' +
                this.skinConfig.name + '/dashboard.html?seed=' + this.anticacheSeed++);
            // preload skin css (synchronously)
            $("head link[rel='stylesheet']").last()
                .after('<link rel="stylesheet" href="' + skinCssUrl + '" type="text/css">');
            // load skin html (synchronously)
            $.ajax({
                url: skinHtmlUrl,
                async: false,
                dataType: 'html',
                timeout: 5000
            }).done(html => {
                $('body').append(html);
            }).fail(() => {
                alert('Failed to load dashboard.html for skin: ' + this.skinConfig.name);
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