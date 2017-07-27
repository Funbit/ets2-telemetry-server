function testPlugin (cb) {
var tests = {
	"bool-test": true,
	"false-test": false,
	"float-test": 123.456,
	"int-test": 1,
	"zero-test": 0,
	"string-test": "xxx",
	"empty-string-test": "",
	"string-with-quotes-test": "xx\"xx",
	"obj-test": {a: "b"},
	"arr-test": ["a", "b"],
	"empty-arr-test": []
};

var fail = [];
var pass = 0;

var nonExistingKeyName = 'test-key-must-not-exists';

var appp = (typeof AppPreferences !== "undefined") ? new AppPreferences () : plugins.appPreferences;

function fetchIncrementStore (keyName) {
	var testRunCount;
	appp.fetch (keyName).then (function (value) {
		testRunCount = value || 0;
		testRunCount++;
		pass++;
	}, function (err) {
		console.error (err);
		fail.push ('promise '+keyName+' failed');
	}).then (function () {
		appp.store (keyName, testRunCount)
	}).then (function () {
		console.info ("test run #"+testRunCount);
	}, function (err) {
		console.error (err);
	});
}

fetchIncrementStore ("test-run-count");

appp.fetch ("test-promise").then (function () {
	pass++;
}, function (err) {
	fail.push ('promise fetch failed');
});

appp.fetch (function (ok) {
	if (ok === null) {
		console.log ("non existing key fetch result is success with null");
		pass++;
		appp.store (function (ok) {
			pass++;
			appp.fetch (function (ok) {
				if (ok !== null && ok) {
					pass++;
				} else {
					fail.push ('fetch>store>fetch '+nonExistingKeyName);
				}
				appp.remove (function (ok) {
					pass++;
				}, function (err) {
					fail.push ('fetch>store>fetch>remove '+nonExistingKeyName + ', error: '+err);
				}, nonExistingKeyName);
			}, function (err) {
				fail.push ('fetch>store>fetch null '+nonExistingKeyName);
			}, nonExistingKeyName);
		}, function (err) {
			fail.push ('fetch>store '+nonExistingKeyName);
		}, nonExistingKeyName, true);
	} else {
		appp.remove (function (ok) {
			pass++;
		}, function (err) {
			fail.push ('fetch>remove '+nonExistingKeyName + '="'+err+'"');
		}, nonExistingKeyName);
		fail.push ('fetch exists '+nonExistingKeyName + '="'+ok+'"');
	}
}, function (err) {
	fail.push ('fetch '+nonExistingKeyName);
}, nonExistingKeyName);

appp.fetch (function (ok) {
	if (ok === null) {
		pass++;
	} else {
		fail.push ('fetch not null '+'dict2.'+nonExistingKeyName + '="'+ok+'"');
	}
}, function (err) {
	fail.push ('fetch '+'dict2.'+nonExistingKeyName);
}, "dict2", nonExistingKeyName);

for (var testK in tests) {
	(function (testName, testValue) {
		console.log ('trying to store', testName);
		appp.store (function (ok) {
			console.log ('stored', testName);
			pass ++;
			appp.fetch (function (ok) {
				if (ok == testValue || (typeof testValue == "object" && JSON.stringify (ok) == JSON.stringify (testValue)))
					pass ++;
				else {
					console.error ('fetched incorrect value for ' + testName + ': expected ' + JSON.stringify (testValue) + ' got ' + JSON.stringify (ok));
					fail.push ('store>fetch not equal '+testName);
				}
			}, function (err) {
				console.error ('fetch value failed for ' + testName + ' and value ' + testValue);
				fail.push ('store>fetch '+testName);
			}, testName);
		}, function (err) {
			console.error ('store value failed for ' + testName + ' and value ' + testValue);
			fail.push ('store '+testName);
		}, testName, testValue);
		console.log ('trying to store', "dict.x" + testName);
		appp.store (function (ok) {
			console.log ('stored', "dict.x" + testName);
			pass ++;
			appp.fetch (function (ok) {
				if (ok == testValue || (typeof testValue == "object" && JSON.stringify (ok) == JSON.stringify (testValue)))
					pass ++;
				else {
					console.error ('fetched incorrect value for dict.x' + testName + ': expected ' + JSON.stringify (testValue) + ' got ' + JSON.stringify (ok));
					fail.push ('store>fetch not equal '+'dict.x'+testName);
				}
			}, function (err) {
				console.error ('fetch value failed for ' + "dict.x" + testName + ' and value ' + testValue);
				fail.push ('store>fetch '+'dict.x'+testName);
			}, "dict", "x" + testName);
		}, function (err) {
			console.error ('store value failed for ' + "dictx" + testName + ' and value ' + testValue);
			fail.push ('store '+'dict.x'+testName);
		}, "dict", "x" + testName, testValue);

	}) (testK, tests[testK]);
}

setTimeout (function () {
	var prompt = 'AppPreferences plugin tests';

	if (fail && fail.length) {
		console.error ('%s passed: %d, failed: %d', prompt, pass, fail);
	} else {
		console.log ('%s — all %d passed', prompt, pass);
	}

	cb && cb (pass, fail);
}, 1000);
}

function testPluginAndCallback () {
    var contentTag = '<content src="http://127.0.0.1:50000" />';
	var url = contentTag.split ('"')[1];
    // location.href = url;
    var oReq = new XMLHttpRequest();
    oReq.addEventListener("load", function () {});

	testPlugin (function (pass, fail) {

		if (fail.length) {
			url += "/test/fail?" + fail.join (';');
		} else {
			url += "/test/success";
		}
        oReq.open("GET", url);
        oReq.send();
	});
}
