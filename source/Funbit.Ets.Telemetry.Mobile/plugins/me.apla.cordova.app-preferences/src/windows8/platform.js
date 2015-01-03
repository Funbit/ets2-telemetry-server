function AppPreferencesW8() {

}

// http://blogs.msdn.com/b/going_metro/archive/2012/04/22/integrating-with-windows-8-settings-charm.aspx
// http://msdn.microsoft.com/en-us/library/windows/apps/hh770544.aspx
// http://www.silverlightshow.net/items/Windows-8-Metro-Add-settings-to-your-application.aspx

AppPreferencesW8.prototype.fetch = function(successCallback, errorCallback, dict, key) {

	var self = this;

	var args = this.prepareKey ('get', dict, key);

	if (!args.key) {
		errorCallback ();
		return;
	}

	// no support for windows phone 8
	var settings = Windows.Storage.ApplicationData.current.localSettings;

	var hasContainer;
	if (args.dict)
		hasContainer = settings.containers.hasKey(args.dict);

	var result;
	if (hasContainer) {
		// Access data in: 
		result = settings.containers.lookup(args.dict).values.hasKey(args.key);
	} else {
		result = settings.values.hasKey(args.key);
	}

	var value;
	if (result) {
		try {
			value = JSON.parse (result);
		} catch (e) {
			value = result;
		}
		successCallback (value);
	} else {
		errorCallback();
	}

    // argscheck.checkArgs('fF', 'Device.getInfo', arguments);
    // exec(successCallback, errorCallback, "Device", "getDeviceInfo", []);
};

AppPreferencesW8.prototype.store = function(successCallback, errorCallback, dict, key, value) {

	var self = this;

	var args = this.prepareKey ('set', dict, key, value);

	if (!args.key || !args.value) {
		errorCallback ();
		return;
	}

	args.value = JSON.stringify(args.value);

	// no support for windows phone 8
	var settings = Windows.Storage.ApplicationData.current.localSettings;

	if (args.dict) {
		var hasContainer = settings.containers.hasKey(args.dict);

		// debugger;

		if (!hasContainer) {
			var container = settings.createContainer(dict, Windows.Storage.ApplicationDataCreateDisposition.Always);
		}
		settings = settings.containers[args.dict];			
	}
	
	settings.values[args.key] = args.value;

	successCallback ();
	
    // argscheck.checkArgs('fF', 'Device.getInfo', arguments);
    // exec(successCallback, errorCallback, "Device", "getDeviceInfo", []);
};

module.exports = new AppPreferencesW8();
