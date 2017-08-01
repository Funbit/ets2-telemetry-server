#!/usr/bin/env node

'use strict';

var cordova_util, ConfigParser;
(function() {
	var cordovaLib = 'cordova',
		configParserLib = 'ConfigParser';
	
	try {
		cordova_util = require (cordovaLib + '/src/util');
	} catch (e) {
		cordovaLib = 'cordova/node_modules/cordova-lib';
		configParserLib = 'configparser/ConfigParser';
	}
	
	try {
		cordova_util = require (cordovaLib + '/src/cordova/util');
	} catch (e) {
		console.error ('cordova error', e);
	}
	
	try {
		ConfigParser = cordova_util.config_parser || cordova_util.configparser;
		
		if (!ConfigParser) {
			ConfigParser = require(cordovaLib + '/src/' + configParserLib);
		}
	} catch (e) {
		console.error ('cordova error', e);
	}
})();

var Q = require('q');
var path = require('path');
var fs = require('./lib/filesystem')(Q, require('fs'), path);
var settings = require("./lib/settings")(fs, path);

var android = require('./lib/android')(fs, path, require('elementtree'), cordova_util, ConfigParser);
var ios = require('./lib/ios')(Q, fs, path, require('plist'), require('xcode'));

return settings.get()
	.then(function (config) {
		return Q.all([
			android.build(config),
			ios.build(config)
		]);
	})
	.catch(function(err) {
		if (err.code === 'NEXIST') {
			console.error('app-settings.json not found');
			return;
		}

		console.error(err);
		console.log(err.stack);
		throw err;
	});
