'use strict';

function createCordovaUtil() {
	var 
		cordovaLib = 'cordova',
		configParserLib = 'ConfigParser',
		cordova_util;
	
	try {
		cordova_util = require (cordovaLib + '/src/util');
	} 
	catch (e) {
		cordovaLib = 'cordova/node_modules/cordova-lib';
		configParserLib = 'configparser/ConfigParser';
	}
	
	try {
		cordova_util = require (cordovaLib + '/src/cordova/util');
	
		if (!cordova_util.config_parser && !cordova_util.configparser) {
			cordova_util.configparser = require(cordovaLib + '/src/' + configParserLib);
		}
	} 
	catch (e) {
		console.error('cordova error', e);
	}
	
	return cordova_util || {
		isCordova: function (p) { return p; },
		configparser: function() {}
	};
}

module.exports = {
	createCordovaUtil: createCordovaUtil
};