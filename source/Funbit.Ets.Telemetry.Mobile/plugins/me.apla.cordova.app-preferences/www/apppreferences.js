/**
 * This module contains basic cross-platform request. platform's request override common one
 * @constructor
 */
var platform = {};
try {
	platform = require ('./platform');
} catch (e) {
	
}

function AppPreferences() {

}

AppPreferences.prototype.prepareKey = platform.prepareKey || function (mode, dict, key, value) {
	var argList = [].slice.apply(arguments);
	argList.shift();
	if (
		(mode == 'get' && argList.length == 1) ||
		(mode == 'get' && argList.length == 2 && argList[1] == null) ||
		(mode == 'set' && argList.length == 2) ||
		(mode == 'set' && argList.length == 3 && argList[2] == null)
	) {
		argList.unshift (undefined);
	}
	var args = {
		key: argList[1]
	};
	if (argList[0] !== undefined)
		args.dict = argList[0]
	
	if (mode == 'set')
		args.value = argList[2];
	
	// console.log (JSON.stringify (argList), JSON.stringify (args));
	return args;
}

/**
 * Get a preference value
 *
 * @param {Function} successCallback The function to call when the value is available
 * @param {Function} errorCallback The function to call when value is unavailable
 * @param {String} dict Dictionary for key (OPTIONAL)
 * @param {String} key Key
 */
AppPreferences.prototype.fetch = platform.fetch || function (
	successCallback, errorCallback, dict, key
	) {

		var args = this.prepareKey ('get', dict, key);

		if (!args.key) {
			errorCallback ();
			return;
		}

		_successCallback = function (_value) {
			var value = _value;
			try {
				value = JSON.parse (_value);
			} catch (e) {
			}
			successCallback (value);
		}

		var execStatus = cordova.exec (
			_successCallback, errorCallback,
			"AppPreferences", "fetch", [args]
		);
};

/**
 * Set a preference value
 *
 * @param {Function} successCallback The function to call when the value is set successfully
 * @param {Function} errorCallback The function to call when value is not set
 * @param {String} dict Dictionary for key (OPTIONAL)
 * @param {String} key Key
 * @param {String} value Value
 */
AppPreferences.prototype.store = platform.store || function (
	successCallback, errorCallback, dict, key, value
	) {

		var args = this.prepareKey ('set', dict, key, value);

		if (!args.key || args.value === null || args.value === undefined) {
			errorCallback ();
			return;
		}

		args.type  = typeof args.value;

		// VERY IMPORTANT THING
		// WP platform has some limitations, so we need to encode all values to JSON.
		// On plugin side we store value according to it's type.
		// So, every platform plugin must check for type, decode JSON and store
		// value decoded for basic types.
		// TODO: don't think about array of strings, it's android only.
		// Complex structures must be stored as JSON string.
		// On iOS strings stored as strings and JSON stored as NSData
		// Android:
		// Now, interesting thing: how to differentiate between string value
		// and complex value, encoded as json and stored as string?
		// I'm introduce setting named _<preference>_type with value "JSON"
		// Windows Phone ?
		args.value = JSON.stringify (args.value);

		var execStatus = cordova.exec (
			successCallback, errorCallback,
			"AppPreferences", "store", [args]
		);
};


module.exports = new AppPreferences();
