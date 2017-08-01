var Funbit;
(function (Funbit) {
    (function (Ets) {
        (function (Telemetry) {
            var Menu = (function () {
                function Menu() {
                    var _this = this;
                    $.when(Telemetry.Configuration.getInstance().initialized).done(function (config) {
                        _this.config = config;
                        _this.initializeEvents();
                        _this.buildSkinTable([]);
                        if (!config.serverIp) {
                            _this.promptServerIp();
                        } else {
                            $('.server-ip').html(config.serverIp);
                            _this.connectToServer();
                        }
                    });
                }
                Menu.prototype.buildSkinTable = function (skins) {
                    var $tableSkins = $('table.skins');
                    $tableSkins.empty();
                    if (skins.length == 0) {
                        $tableSkins.append(doT.template($('#skin-empty-row-template').html())({}));
                    } else {
                        var skinTemplateDo = doT.template($('#skin-row-template').html());
                        var html = '';
                        for (var i = 0; i < skins.length; i++) {
                            var skinConfig = skins[i];
                            skinConfig.splashUrl = Telemetry.Configuration.getUrl("/skins/" + skinConfig.name + "/dashboard.jpg");
                            html += skinTemplateDo(skinConfig);
                        }
                        $tableSkins.append(html);
                    }
                };

                Menu.prototype.connectToServer = function () {
                    var _this = this;
                    var serverIp = $('.server-ip').html();
                    if (!serverIp)
                        return;
                    var $serverStatus = $('.server-status');
                    $serverStatus.removeClass('connected').addClass('disconnected').html(Telemetry.Strings.connecting);
                    this.buildSkinTable([]);
                    this.config.reload(serverIp, function () {
                        $serverStatus.removeClass('disconnected').addClass('connected').html(Telemetry.Strings.connected);
                        _this.buildSkinTable(_this.config.skins);
                    }, function () {
                        $serverStatus.removeClass('connected').addClass('disconnected').html(Telemetry.Strings.disconnected);
                        _this.buildSkinTable(_this.config.skins);
                        _this.reconnectionTimer = setTimeout(_this.connectToServer.bind(_this, [$('.server-ip').html()]), 3000);
                    });
                };

                Menu.prototype.promptServerIp = function () {
                    var ip = prompt(Telemetry.Strings.enterServerIpMessage, this.config.serverIp);
                    if (!ip)
                        return;
                    var correct = /^[a-zA-Z0-9\.\-]+$/.test(ip);
                    if (!correct) {
                        alert(Telemetry.Strings.incorrectServerIpFormat);
                    } else {
                        $('.server-ip').html(ip);
                        this.connectToServer();
                    }
                };

                Menu.prototype.initializeEvents = function () {
                    var _this = this;
                    $(document).on('click', 'td.skin-image,td.skin-desc', function (e) {
                        var $this = $(e.currentTarget);
                        var skinName = $this.closest('tr').data('name');
                        var dashboardHostUrl = Telemetry.Configuration.getUrl("/dashboard-host.html?skin=" + skinName + "&ip=" + _this.config.serverIp);
                        window.location.href = dashboardHostUrl;
                    });
                    $('.edit-server-ip').click(function () {
                        _this.promptServerIp();
                    });
                };
                return Menu;
            })();
            Telemetry.Menu = Menu;
        })(Ets.Telemetry || (Ets.Telemetry = {}));
        var Telemetry = Ets.Telemetry;
    })(Funbit.Ets || (Funbit.Ets = {}));
    var Ets = Funbit.Ets;
})(Funbit || (Funbit = {}));

if (Funbit.Ets.Telemetry.Configuration.isCordovaAvailable()) {
    $(document).on('deviceready', function () {
        Funbit.Ets.Telemetry.Menu.instance = new Funbit.Ets.Telemetry.Menu();
    });
} else {
    Funbit.Ets.Telemetry.Menu.instance = new Funbit.Ets.Telemetry.Menu();
}
