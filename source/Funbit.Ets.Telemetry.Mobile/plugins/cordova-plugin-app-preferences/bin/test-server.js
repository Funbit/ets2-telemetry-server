var fs = require ('fs');
var exec    = require('child_process').exec;

var ServeMe = require("serve-me");

var dir = './www';
var cmd = '';
var cmdPrepare = '';
var host = '';
// var confFile = 'config.xml';
var confFile   = 'www/js/apppreferences-test.js'

if (process.argv[2] === 'ios') {
	dir        = './platforms/ios/www';
	// cmd = 'ios-sim launch ./platforms/ios/build/emulator/HelloCordova.app --log ./console.log --devicetypeid iPhone-6 --exit';
	cmd        = 'cordova emulate ios';
	cmdPrepare = 'cordova prepare ios';
	host       = '127.0.0.1';
} else if (process.argv[2] === 'android') {
	dir        = './platforms/android/assets/www';
	cmd        = 'cordova emulate android';
	cmdPrepare = 'cordova prepare android';
	host       = '10.0.2.2';

	if (process.argv[3]) {
		cmdPrepare += " --target=" + process.argv[3];
	}
} else if (process.argv[2] === 'windows') {
	dir        = './platforms/windows/www';
	cmd        = 'cordova emulate windows';
	cmdPrepare = 'cordova prepare windows';
	host       = '127.0.0.1';
	confFile   = 'www/js/apppreferences-test.js'
}

var serveMe = ServeMe ({
	// debug: true,
	// log: true,
	directory: dir,
	secure: false
});

function handleError (err, stdout, stderr) {
	if (err) {
		if (stdout) console.log (stdout);
		if (stderr) console.error (stderr);
		process.exit (1);
	}
}

fs.readFile (confFile, function (err, buf) {
	handleError (err, null, err);

	serveMe.start("0", function() {

		var port = serveMe.server.server.address().port;

		var configXml = buf.toString('utf-8').replace (/<content src="[^"]+/m, '<content src="http://' + host + ':' + port);

		// console.log (configXml);

		fs.writeFile (confFile, configXml, function (err) {
			handleError (err, null, err);

			console.log ('Changes applied to the', confFile);

			exec(cmdPrepare, function callback(error, stdout, stderr){
				handleError (error, stdout, stderr);

				console.log ('Prepare completed');
				exec(cmd, function callback(error, stdout, stderr){
					handleError (error, stdout, stderr);

					console.log ('Emulator running');
					setTimeout (function () {process.exit(1)}, 30000);
				});
			});
		});
	});
})

serveMe.on("http_request", function(data){
	// console.log("http request", data);
});

serveMe.get ("/test/success", function(){
	console.log ("test success");
	process.exit ();
});

serveMe.get ("/test/fail", function (req, res){
	console.log ("test failures: ", req.url);
	process.exit (1);
});
