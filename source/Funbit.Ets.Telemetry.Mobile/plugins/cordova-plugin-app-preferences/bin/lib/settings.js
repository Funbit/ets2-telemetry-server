/**
 * Settings management methods
 *
 *
 */
'use strict';

var appSettingsPath = 'app-settings.json';

module.exports = function(fs, path) {
	
	function get() {
		return fs.exists(appSettingsPath)
			.then(function() { return fs.readFile(appSettingsPath); })
			.then(JSON.parse);
	}
	
	function create() {
		var basePath = path.resolve(__dirname, '../../' + appSettingsPath);
		
		return fs.copy(basePath, appSettingsPath);
	}
	
	function remove() {
		return fs.unlink(appSettingsPath)
			.catch(function(err) {
				if (err && err.code !== 'ENOENT') {
					throw err;
				}
			});
	}
	
	return {
		get: get,
		create: create,
		remove: remove
	};
};