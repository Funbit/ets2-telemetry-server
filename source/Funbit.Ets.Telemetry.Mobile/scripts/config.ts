declare var plugins: any; // cordova plugins

module Funbit.Ets.Telemetry {

    export interface IConfiguration {
        skins: ISkinConfiguration[];
        serverIp: string;
    } 

    export interface ISkinConfiguration {
        name: string;
        title: string;
        author: string;
        refreshDelay: number;
        width: number;
        height: number;
    } 

    interface IPrefsPlugin {
        fetch(ok: Function, fail: Function, key: string);
        store(ok: Function, fail: Function, key: string, value: string);
    }

    interface IInsomniaPlugin {
        keepAwake();
    }

    export class Configuration implements IConfiguration {
        
        public skins: ISkinConfiguration[];
        public serverIp: string;
        public initialized: JQueryDeferred<Configuration>;
        
        private prefs: IPrefsPlugin;
        private insomnia: IInsomniaPlugin;

        private static instance: Configuration;
        public static getInstance(): Configuration {
            if (!Configuration.instance) {
                Configuration.instance = new Configuration();
                Configuration.instance.reload(Configuration.instance.serverIp);
            }
            return Configuration.instance;
        }

        public static getParameter(name: string) {
            name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
            var regex = new RegExp("[\\?&]" + name + "=([^&#]*)");
            var results = regex.exec(location.search);
            return results == null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
        }

        public getSkinConfiguration(): ISkinConfiguration {
            var skinName = Configuration.getParameter('skin');
            if (skinName) {
                for (var i = 0; i < this.skins.length; i++) {
                    if (this.skins[i].name == skinName)
                        return this.skins[i];
                }
            }
            return null;
        }
        
        public reload(newServerIp: string, done: Function = null, fail: Function = null) {
            if (!newServerIp)
                return;
            this.serverIp = newServerIp;
            $.ajax({
                url: this.getUrlInternal('/config.json'),
                async: (done != null),
                cache: true,
                dataType: 'json',
                timeout: 5000
            }).done(json => {
                this.prefs.store(() => {}, () => {}, 'serverIp', this.serverIp);
                this.skins = json.skins;
                if (done) done();
            }).fail(() => {
                this.skins = [];
                if (fail) fail();
            });
        }
        
        public static isCordovaAvailable(): boolean {
            return document.URL.indexOf('http://') === -1 && document.URL.indexOf('https://') === -1;
        }

        private getUrlInternal(path: string): string {
            var serverPort: number = 25555;
            return "http://" + this.serverIp + ":" + serverPort + path;
        }
        
        public static getUrl(path: string): string {
            return Configuration.getInstance().getUrlInternal(path);
        }
        
        constructor() {
            this.initialized = $.Deferred<Configuration>();
            this.skins = [];
            // initialize structures
            if (!Configuration.isCordovaAvailable()) {
                this.insomnia = {
                    keepAwake: () => { }
                };
                this.prefs = {
                    fetch: () => { },
                    store: () => { }
                };
            } else {
                this.insomnia = <IInsomniaPlugin>plugins.insomnia;
                this.prefs = <IPrefsPlugin>plugins.appPreferences;
                // turn off sleep mode
                this.insomnia.keepAwake();
            }
            // if server IP was passed in the query string use it then
            var ip = Configuration.getParameter('ip');
            if (ip) {
                this.serverIp = ip;
                this.initialized.resolve(this);
                return;
            }
            this.serverIp = '';
            if (!Configuration.isCordovaAvailable()) {
                // if cordova is not available then 
                // we are in desktop environment 
                // so we use current host name as our IP
                this.serverIp = window.location.hostname;
                this.initialized.resolve(this);
            } else {
                this.prefs.fetch(savedIp => {
                    this.serverIp = savedIp;
                    this.initialized.resolve(this);
                }, () => {
                    this.initialized.resolve(this);
                }, 'serverIp');
            }
        }
    }
}