var Funbit;
(function (Funbit) {
    (function (Ets) {
        (function (Telemetry) {
            var ConfigurationManager = (function () {
                function ConfigurationManager(url) {
                    var _this = this;
                    if (!this.configuration) {
                        $.ajax({
                            url: url,
                            async: false,
                            cache: true,
                            dataType: 'json',
                            timeout: 5000
                        }).done(function (json) {
                            _this.configuration = json;
                        }).fail(function () {
                            alert('Failed to load config.json');
                        });
                    }
                }
                ConfigurationManager.getConfiguration = function (url) {
                    if (!ConfigurationManager.instance) {
                        ConfigurationManager.instance = new Funbit.Ets.Telemetry.ConfigurationManager(url);
                    }
                    return ConfigurationManager.instance.configuration;
                };
                return ConfigurationManager;
            })();
            Telemetry.ConfigurationManager = ConfigurationManager;
        })(Ets.Telemetry || (Ets.Telemetry = {}));
        var Telemetry = Ets.Telemetry;
    })(Funbit.Ets || (Funbit.Ets = {}));
    var Ets = Funbit.Ets;
})(Funbit || (Funbit = {}));
//# sourceMappingURL=config.js.map
