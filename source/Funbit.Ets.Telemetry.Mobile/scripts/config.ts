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

    export class Configuration implements IConfiguration {
        
        public skins: ISkinConfiguration[];
        public serverIp: string;

        private static instance: Configuration;
        public static getInstance(): Configuration {
            if (!Configuration.instance) {
                Configuration.instance = new Configuration();
                Configuration.instance.reload();
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
        
        public reload(): boolean {
            if (!this.serverIp)
                return false;
            var result: boolean = true;
            $.ajax({
                url: this.getUrlInternal('/config.json'),
                async: false,
                cache: true,
                dataType: 'json',
                timeout: 3000
            }).done(json => {
                this.skins = json.skins;
            }).fail(() => {
                alert('Failed to load config.json');
                result = false;
            });
            // ReSharper disable once ExpressionIsAlwaysConst
            return result;
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
            this.skins = [];
            this.serverIp = '';
            if (!Configuration.isCordovaAvailable()) {
                // if cordova is not available then 
                // we are in desktop environment 
                // so we use current host name as our IP
                this.serverIp = window.location.hostname;
            }
        }
        
    }
}