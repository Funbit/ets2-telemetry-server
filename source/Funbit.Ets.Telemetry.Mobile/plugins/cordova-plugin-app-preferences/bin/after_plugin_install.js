'use strict';

module.exports = function (context) {
	var req = context.requireCordovaModule,
		Q = req('q'),
		path = req('path'),
		fs = require("./lib/filesystem")(Q, req('fs'), path),
		settings = require("./lib/settings")(fs, path),
		android = require("./lib/android")(context),
		ios = require("./lib/ios")(Q, fs, path, req('plist'), req('xcode'));

	return settings.get()
		.then(function (config) {
			return Q.all([
				android.afterPluginInstall(config),
				// ios.afterPluginInstall(config) // not implemented for iOS
			]);
		})
		.catch(function(err) {
			if (err.code === 'NEXIST') {
				console.log("app-settings.json not found: creating a sample file");
				return settings.create();
			}

			console.log ('unhandled exception', err);

			throw err;
		});
};
