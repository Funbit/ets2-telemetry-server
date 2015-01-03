var define;
if (typeof define === "undefined")
	define = function (classInstance) {
		classInstance (require, exports, module);
	}

define (function (require, exports, module) {

	var dataflows = require ('dataflo.ws');

	var taskBase = dataflows.task ('base');

	var AppPreferenceTask = module.exports = function (config) {
		// there is no options to netinfo class
		this.init (config);
	};

	util.inherits (AppPreferenceTask, taskBase);

	util.extend (AppPreferenceTask.prototype, {

		fetch: function () {
			var self  = this;

			console.log('MOBRO PREFERENCE GET PREPARE');

			var successCallback = function (response) {
				var result;
				try {
					result = JSON.parse (response);
				} catch (e) {
					result = response;
				}
				var returnValue = {forKey: self.forKey};
				if (result) {
					returnValue.value = result;
				} else {
					returnValue.noValue = true;
				}

				console.log ('MOBRO PREFERENCE GET DONE');
				console.log (returnValue);

				self.completed (returnValue);
			};

			var errorCallback = function (error) {
				console.log (error);
				self.completed ({
					forKey: self.forKey,
					noValue: true
				});
			};

			// if (device.platform == "BlackBerry" && parseInt(device.version) == 10) {
			// 	self.completed ({
			// 		forKey: self.forKey,
			// 		noValue: true
			// 	});
			// 	return;
			// }

			var cordovaModule = cordova.require ('me.apla.cordova.app-preferences.apppreferences');
			cordovaModule.fetch (successCallback, errorCallback, this.forKey, this.inDict);
		},
		store: function () {
			var self = this;

			var args   = {};
			args.key   = this.forKey;
			args.dict  = this.inDict;
			args.value = this.value;

			if (!this.forKey || !this.value) {
				self.completed ();
				return;
			}

			console.log ('MOBRO PREFERENCE SET PREPARE');
			console.log (this.value);

			var successCallback = function (response) {
				self.completed ();
			};

			var errorCallback = function (error) {
				self.failed ({'undefined': true});
				console.log (error);
			};

			if (device.platform == "BlackBerry" && parseInt(device.version) == 10) {
				self.completed ();
				return;
			}

			var cordovaModule = cordova.require ('me.apla.cordova.app-preferences.apppreferences');
			cordovaModule.store (successCallback, errorCallback, this.forKey, this.inDict, this.value);
		}

	});

	dataflows.register ('task', 'AppPreferences', AppPreferenceTask);

	return AppPreferenceTask;

});