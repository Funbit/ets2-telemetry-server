var Funbit;
(function (Funbit) {
    (function (Ets) {
        (function (Telemetry) {
            var Configuration = (function () {
                function Configuration() {
                    this.skins = [];
                    this.serverIp = '';
                    if (!Configuration.isCordovaAvailable()) {
                        // if cordova is not available then
                        // we are in desktop environment
                        // so we use current host name as our IP
                        this.serverIp = window.location.hostname;
                    }
                }
                Configuration.getInstance = function () {
                    if (!Configuration.instance) {
                        Configuration.instance = new Configuration();
                        Configuration.instance.reload();
                    }
                    return Configuration.instance;
                };

                Configuration.getParameter = function (name) {
                    name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
                    var regex = new RegExp("[\\?&]" + name + "=([^&#]*)");
                    var results = regex.exec(location.search);
                    return results == null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
                };

                Configuration.prototype.getSkinConfiguration = function () {
                    var skinName = Configuration.getParameter('skin');
                    if (skinName) {
                        for (var i = 0; i < this.skins.length; i++) {
                            if (this.skins[i].name == skinName)
                                return this.skins[i];
                        }
                    }
                    return null;
                };

                Configuration.prototype.reload = function () {
                    var _this = this;
                    if (!this.serverIp)
                        return false;
                    var result = true;
                    $.ajax({
                        url: this.getUrlInternal('/config.json'),
                        async: false,
                        cache: true,
                        dataType: 'json',
                        timeout: 3000
                    }).done(function (json) {
                        _this.skins = json.skins;
                    }).fail(function () {
                        alert('Failed to load config.json');
                        result = false;
                    });

                    // ReSharper disable once ExpressionIsAlwaysConst
                    return result;
                };

                Configuration.isCordovaAvailable = function () {
                    return document.URL.indexOf('http://') === -1 && document.URL.indexOf('https://') === -1;
                };

                Configuration.prototype.getUrlInternal = function (path) {
                    var serverPort = 25555;
                    return "http://" + this.serverIp + ":" + serverPort + path;
                };

                Configuration.getUrl = function (path) {
                    return Configuration.getInstance().getUrlInternal(path);
                };
                return Configuration;
            })();
            Telemetry.Configuration = Configuration;
        })(Ets.Telemetry || (Ets.Telemetry = {}));
        var Telemetry = Ets.Telemetry;
    })(Funbit.Ets || (Funbit.Ets = {}));
    var Ets = Funbit.Ets;
})(Funbit || (Funbit = {}));
//# sourceMappingURL=config.js.map
