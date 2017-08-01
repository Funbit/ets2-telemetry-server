/// <reference path="typings/jquery.d.ts" />
/// <reference path="typings/jqueryui.d.ts" />
/// <reference path="typings/dot.d.ts" />

module Funbit.Ets.Telemetry {

    export class Menu {

        private config: Configuration;
        private reconnectionTimer: any;

        public static instance: Menu;

        constructor() {
            $.when(Configuration.getInstance().initialized).done(config => {
                this.config = config;
                this.initializeEvents();
                this.buildSkinTable([]);
                if (!config.serverIp) {
                    this.promptServerIp();
                } else {
                    $('.server-ip').html(config.serverIp);
                    this.connectToServer();
                }
            });
        }

        private buildSkinTable(skins: ISkinConfiguration[]) {
            var $tableSkins = $('table.skins');
            $tableSkins.empty();
            if (skins.length == 0) {
                $tableSkins.append(doT.template($('#skin-empty-row-template').html())({}));
            } else {
                var skinTemplateDo = doT.template($('#skin-row-template').html());
                var html = '';
                for (var i = 0; i < skins.length; i++) {
                    var skinConfig: any = skins[i];
                    skinConfig.splashUrl = Configuration.getUrl(
                        "/skins/" + skinConfig.name + "/dashboard.jpg");
                    html += skinTemplateDo(skinConfig);
                }
                $tableSkins.append(html);    
            }
        }
        
        private connectToServer() {
            var serverIp: string = $('.server-ip').html();
            if (!serverIp) return;
            var $serverStatus = $('.server-status');
            $serverStatus.removeClass('connected')
                .addClass('disconnected')
                .html(Strings.connecting);
            this.buildSkinTable([]);
            this.config.reload(serverIp, () => {
                $serverStatus.removeClass('disconnected')
                    .addClass('connected')
                    .html(Strings.connected);
                this.buildSkinTable(this.config.skins);
            }, () => {
                $serverStatus.removeClass('connected')
                    .addClass('disconnected')
                    .html(Strings.disconnected);
                this.buildSkinTable(this.config.skins);
                this.reconnectionTimer = setTimeout(
                    this.connectToServer.bind(this,
                        [$('.server-ip').html()]), 3000);
            });
        }

        private promptServerIp() {
            var ip = prompt(Strings.enterServerIpMessage, this.config.serverIp);
            if (!ip) return;
            var correct = /^[a-zA-Z0-9\.\-]+$/.test(ip);
            if (!correct) {
                alert(Strings.incorrectServerIpFormat);
            } else {
                $('.server-ip').html(ip);
                this.connectToServer();
            }
        }

        private initializeEvents() {
            $(document).on('click', 'td.skin-image,td.skin-desc', e => {
                var $this = $(e.currentTarget);
                var skinName = $this.closest('tr').data('name');
                var dashboardHostUrl = Configuration.getUrl(
                    "/dashboard-host.html?skin=" + skinName +
                    "&ip=" + this.config.serverIp);
                window.location.href = dashboardHostUrl;
            });
            $('.edit-server-ip').click(() => {
                this.promptServerIp();
            });
        }

    }
}

//
// Menu "entry-point"
//

if (Funbit.Ets.Telemetry.Configuration.isCordovaAvailable()) {
    $(document).on('deviceready', () => {
        Funbit.Ets.Telemetry.Menu.instance = new Funbit.Ets.Telemetry.Menu();
    });
} else {
    Funbit.Ets.Telemetry.Menu.instance = new Funbit.Ets.Telemetry.Menu();
}