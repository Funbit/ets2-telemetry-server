module Funbit.Ets.Telemetry {

    export interface IConfiguration {
        skins: ISkinConfiguration[];
    } 

    export interface ISkinConfiguration {
        name: string;
        title: string;
        author: string;
        refreshDelay: number;
        width: number;
        height: number;
    } 

    export class ConfigurationManager {
        
        private static instance: ConfigurationManager;
        private configuration: IConfiguration;

        // ReSharper disable once InconsistentNaming
        public static getConfiguration(url: string): IConfiguration {
            if (!ConfigurationManager.instance) {
                ConfigurationManager.instance = new Funbit.Ets.Telemetry.ConfigurationManager(url);
            }
            return ConfigurationManager.instance.configuration;
        }
        
        constructor(url: string) {
            if (!this.configuration) {
                $.ajax({
                    url: url,
                    async: false,
                    cache: true,
                    dataType: 'json',
                    timeout: 5000
                }).done(json => {
                    this.configuration = json;
                }).fail(() => {
                    alert('Failed to load config.json');
                });
            }
        }
        
    }
}