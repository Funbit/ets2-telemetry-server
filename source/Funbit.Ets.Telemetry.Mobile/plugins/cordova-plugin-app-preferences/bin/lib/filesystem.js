/**
 * Filesystem wrapper with promises
 *
 *
 */
'use strict';

module.exports = function(Q, fs, path) {
	
	function exists(src) {
		var defer = Q.defer();
		
		fs.exists(src, function (exists) {
			if (!exists) {
				var err = new Error(src + 'does not exists');
				err.code = 'NEXIST';
				return defer.reject(err);
			}
			
			defer.resolve(exists);
		});
		
		return defer.promise;
	}
	
	function find(dir, pattern) {
		var defer = Q.defer();
		
		fs.readdir(dir, function (err, entries) {
			if (err) {
				return defer.reject(err);
			}
			
			var entry = entries.filter(pattern.test.bind(pattern))[0];
			if (!entry) {
				err = new Error('.xcodeproj cannot be found in ' + dir);
				err.code = 'NEXIST';
				return defer.reject(err);
			}
			
			defer.resolve(path.join(dir, entry));
		});
		
		return defer.promise;
	}
	
	function readFile(src) {
		var defer = Q.defer();
		
		fs.readFile(src, function (err, data) {
			if (err) {
				return defer.reject(err);
			}
		
			defer.resolve(data);
		});
		
		return defer.promise;
	}
	
	function writeFile(dest, content) {
		var defer = Q.defer();

		fs.writeFile(dest, content, function (err) {
			if (err) {
				return defer.reject(err);
			}
			
			defer.resolve();
		});
			
		return defer.promise;
	}
	
	function copy(src, dest) {
		return readFile(src).then(function(content) {
			return writeFile(dest, content);
		});
	}
	
	function unlink(dest) {
		var defer = Q.defer();

		fs.unlink(dest, function (err) {
			if (err) {
				return defer.reject(err);
			}
			
			defer.resolve();
		});
			
		return defer.promise;
	}
	
	function mkdir(dest) {
		var defer = Q.defer();

		fs.mkdir(dest, function (err) {
			if (err && err.code != 'EEXIST') {
				return defer.reject(err);
			}
			
			defer.resolve();
		});
			
		return defer.promise;
	}
	
	function rmdir(dest) {
		var defer = Q.defer();

		fs.rmdir(dest, function (err) {
			if (err) {
				return defer.reject(err);
			}
			
			defer.resolve();
		});
			
		return defer.promise;
	}
	
	return {
		exists: exists,
		find: find,
		
		readFile: readFile,
		writeFile: writeFile,
		copy: copy,
		unlink: unlink,
		
		mkdir: mkdir,
		rmdir: rmdir
	};
};