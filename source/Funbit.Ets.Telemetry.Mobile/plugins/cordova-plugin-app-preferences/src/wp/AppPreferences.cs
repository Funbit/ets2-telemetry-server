using System;
using System.Diagnostics;
using System.IO.IsolatedStorage;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Collections.Generic;
using System.Globalization;

namespace WPCordovaClassLib.Cordova.Commands
{

    // settings gui, the first one
    // http://msdn.microsoft.com/en-us/library/windowsphone/develop/ff769510(v=vs.105).aspx
    // http://windowsphone.interoperabilitybridges.com/articles/chapter-7-iphone-to-wp7-application-preference-migration#h2Section3

    // http://msdn.microsoft.com/en-us/library/microsoft.phone.net.networkinformation(v=VS.92).aspx
    // http://msdn.microsoft.com/en-us/library/microsoft.phone.net.networkinformation.devicenetworkinformation(v=VS.92).aspx

    public class AppPreferences : BaseCommand
    {
        public AppPreferences()
        {
        }

        public class JSONString
        {
            public string contents;
        }

        [DataContract]
        public class AppPreferenceArgs
        {
            [DataMember(Name = "key", IsRequired = true)]
            public string key;
            [DataMember(Name = "dict", IsRequired = false)]
            public string dict;
            [DataMember(Name = "value", IsRequired = false)]
            public string value;
            [DataMember(Name = "type", IsRequired = false)]
            public string type;

            public object parsedValue()
            {
                if (type == "boolean")
                {
                    return value == "true" ? true : false;
                }
                else if (type == "number")
                {
                    return float.Parse(value, CultureInfo.InvariantCulture);
                }
                else if (type == "string")
                {
                    return JSON.JsonHelper.Deserialize<string>(value);
                }
                else
                {
                    var jsonString = new JSONString();
                    jsonString.contents = value;
                    // System.Diagnostics.Debug.WriteLine("\njsonString type for " + (string)value + " is: " + jsonString.GetType() + "\n");
                    return jsonString;
                }
            }

            public string fullKey()
            {
                if (this.dict != null)
                {
                    return this.dict + '.' + this.key;
                }
                else
                {
                    return this.key;
                }
            }
        }

        public void fetch(string argsString)
        {
            AppPreferenceArgs preference;
            object value;
            string returnVal;
            string[] args = JSON.JsonHelper.Deserialize<string[]>(argsString);
            string optionsString = args[0];
            string callbackId = args[1];
            // System.Diagnostics.Debug.WriteLine("\nfetch args: " + argsString + "\n");
            //BrowserOptions opts = JSON.JsonHelper.Deserialize<BrowserOptions>(options);

            try
            {
                preference = JSON.JsonHelper.Deserialize<AppPreferenceArgs>(optionsString);
                IsolatedStorageSettings userSettings = IsolatedStorageSettings.ApplicationSettings;

                userSettings.TryGetValue<object>(preference.fullKey(), out value);
                // System.Diagnostics.Debug.WriteLine("\ntype is: " + value.GetType() + "\n");
                if (value is JSONString)
                {
                    returnVal = ((JSONString)value).contents;
                }
                else
                {
                    returnVal = JSON.JsonHelper.Serialize(value);

                }
                // System.Diagnostics.Debug.WriteLine("\nserialized value for " + value.GetType() + " is: " + returnVal + "\n");

            }
            catch (NullReferenceException)
            {
                DispatchCommandResult(new PluginResult(PluginResult.Status.JSON_EXCEPTION), callbackId);
                return;
            }
            catch (Exception)
            {
                DispatchCommandResult(new PluginResult(PluginResult.Status.JSON_EXCEPTION), callbackId);
                return;
            }
            DispatchCommandResult(new PluginResult(PluginResult.Status.OK, returnVal), callbackId);
        }

        public void store(string argsString)
        {
            AppPreferenceArgs preference;
            //BrowserOptions opts = JSON.JsonHelper.Deserialize<BrowserOptions>(options);
            string[] args = JSON.JsonHelper.Deserialize<string[]>(argsString);
            string optionsString = args[0];
            string callbackId = args[1];

            try
            {
                preference = JSON.JsonHelper.Deserialize<AppPreferenceArgs>(optionsString);
                IsolatedStorageSettings userSettings = IsolatedStorageSettings.ApplicationSettings;
                if (userSettings.Contains(preference.fullKey()))
                {
                    userSettings[preference.fullKey()] = preference.parsedValue();
                }
                else
                {
                    userSettings.Add(preference.fullKey(), preference.parsedValue());
                }
                userSettings.Save();
            }
            catch (Exception)
            {
                DispatchCommandResult(new PluginResult(PluginResult.Status.JSON_EXCEPTION), callbackId);
                // System.Diagnostics.Debug.WriteLine("\nJSON Exception was thrown\n");
                return;
            }
            DispatchCommandResult(new PluginResult(PluginResult.Status.OK, ""), callbackId);
        }
    }
}
