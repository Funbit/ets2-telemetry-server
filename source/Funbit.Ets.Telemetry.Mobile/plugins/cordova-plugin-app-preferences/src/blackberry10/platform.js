function AppPreferencesLocalStorage() {

}

AppPreferencesLocalStorage.prototype.fetch = function(successCallback, errorCallback, dict, key) {

	var self = this;

	var args = this.prepareKey ('get', dict, key);

	if (!args.key) {
		errorCallback ();
		return;
	}

	var key = args.key;

	if (args.dict)
		key = args.dict + '.' + args.key;
	
	var result = window.localStorage.getItem (key);

	var value = result;
	if (result) {
		try {
			value = JSON.parse (result);
		} catch (e) {
		}
		successCallback (value);
	} else {
		errorCallback();
	}
};

AppPreferencesLocalStorage.prototype.store = function(successCallback, errorCallback, dict, key, value) {

	var self = this;

	var args = this.prepareKey ('set', dict, key, value);

	if (!args.key || args.value === null || args.value === undefined) {
		errorCallback ();
		return;
	}

	var key = args.key;

	if (args.dict)
		key = args.dict + '.' + args.key;

	var value = JSON.stringify (args.value);

	window.localStorage.setItem (key, value);

	successCallback ();
};

module.exports = new AppPreferencesLocalStorage();
