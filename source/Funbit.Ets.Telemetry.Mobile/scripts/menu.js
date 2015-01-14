var Funbit;
(function (Funbit) {
    (function (Ets) {
        (function (Telemetry) {
            var Menu = (function () {
                function Menu() {
                    var _this = this;
                    this.config = Telemetry.Configuration.getInstance();
                    this.initializeEvents();
                    this.buildSkinTable([]);
                    $.when(this.config.initialized).done(function (config) {
                        if (!config.serverIp) {
                            _this.promptServerIp();
                        } else {
                            _this.connectToServer(config.serverIp);
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
                            html += skinTemplateDo(skins[i]);
                        }
                        $tableSkins.append(html);
                    }
                };

                Menu.prototype.connectToServer = function (serverIp) {
                    var _this = this;
                    if (!serverIp)
                        return;
                    var $serverStatus = $('.server-status');
                    $serverStatus.removeClass('connected').addClass('disconnected').html('Connecting...');
                    $('.server-ip').html(serverIp);
                    this.buildSkinTable([]);
                    this.config.reload(serverIp, function () {
                        $serverStatus.removeClass('disconnected').addClass('connected').html('Connected');
                        _this.buildSkinTable(_this.config.skins);
                    }, function () {
                        $serverStatus.removeClass('connected').addClass('disconnected').html('Disconnected');
                        _this.buildSkinTable(_this.config.skins);
                    });
                };

                Menu.prototype.promptServerIp = function () {
                    var ip = prompt("Please enter " + "server IP address (aa.bb.cc.dd)", this.config.serverIp);
                    var correct = /^[a-zA-Z0-9\.\-]+$/.test(ip);
                    if (!correct)
                        alert('Entered server IP or hostname has incorrect format.');
                    else
                        this.connectToServer(ip);
                };

                Menu.prototype.initializeEvents = function () {
                    var _this = this;
                    $(document).on('click', 'td.skin-image,td.skin-desc', function (e) {
                        var $this = $(e.currentTarget);
                        var skinName = $this.closest('tr').data('name');
                        window.location.href = "dashboard-host.html?skin=" + skinName + "&ip=" + _this.config.serverIp;
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
        (new Funbit.Ets.Telemetry.Menu());
    });
} else {
    (new Funbit.Ets.Telemetry.Menu());
}
