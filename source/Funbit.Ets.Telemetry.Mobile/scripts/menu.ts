/// <reference path="typings/jquery.d.ts" />
/// <reference path="typings/jqueryui.d.ts" />
/// <reference path="typings/dot.d.ts" />

module Funbit.Ets.Telemetry {

    export class Menu {

        private config: Configuration;

        constructor() {
            this.config = Configuration.getInstance();
            this.initializeEvents();
            this.buildSkinTable();
            $.when(this.config.initialized).done(config => {
                if (!config.serverIp)
                    this.promptServerIp();
                else
                    this.connectToServer(config.serverIp);
            });
        }

        private buildSkinTable() {
            var $tableSkins = $('table.skins');
            $tableSkins.empty();
            var skins = this.config.skins;
            if (skins.length == 0) {
                $tableSkins.append(doT.template($('#skin-empty-row-template').html())({}));
            } else {
                var skinTemplateDo = doT.template($('#skin-row-template').html());
                var html = '';
                for (var i = 0; i < skins.length; i++) {
                    html += skinTemplateDo(skins[i]);
                }
                $tableSkins.append(html);    
            }
        }

        private connectToServer(serverIp: string) {
            if (!serverIp) return;
            var $serverStatus = $('.server-status');
            $serverStatus.removeClass('connected')
                .addClass('disconnected')
                .html('Connecting...');
            $('.server-ip').html(serverIp);
            $('table.skins').empty();
            this.config.reload(serverIp, () => {
                $serverStatus.removeClass('disconnected')
                    .addClass('connected')
                    .html('Connected');
                this.buildSkinTable();
            }, () => {
                $serverStatus.removeClass('connected')
                    .addClass('disconnected')
                    .html('Disconnected');
                this.buildSkinTable();
            });
        }

        private promptServerIp() {
            var ip = prompt("Please enter " +
                "server IP address (aa.bb.cc.dd)", this.config.serverIp);
            var correct = /^[a-zA-Z0-9\.\-]+$/.test(ip);
            if (!correct)
                alert('Entered server IP or hostname has incorrect format.');
            else
                this.connectToServer(ip);
        }

        private initializeEvents() {
            $(document).on('click', 'td.skin-image,td.skin-desc', e => {
                var $this = $(e.currentTarget);
                var skinName = $this.closest('tr').data('name');
                window.location.href = "dashboard-host.html?skin=" + skinName +
                    "&ip=" + this.config.serverIp;
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
        (new Funbit.Ets.Telemetry.Menu());
    });
} else {
    (new Funbit.Ets.Telemetry.Menu());
}