function testPlugin () {
var tests = {
	"bool-test": true,
	"false-test": false,
	"float-test": 123.456,
	"int-test": 1,
	"zero-test": 0,
	"string-test": "xxx",
	"empty-string-test": "xxx",
	"obj-test": {a: "b"},
	"arr-test": ["a", "b"],
	"empty-arr-test": []
};

var fail = 0;
var pass = 0;

var appp = plugins.appPreferences;
for (var testK in tests) {
	(function (testName, testValue) {
		appp.store (function (ok) {
			pass ++;
			appp.fetch (function (ok) {
				if (ok == testValue || (typeof testValue == "object" && JSON.stringify (ok) == JSON.stringify (testValue)))
					pass ++;
				else {
					console.error ('fetched incorrect value for ' + testName + ': expected ' + JSON.stringify (testValue) + ' got ' + JSON.stringify (ok));
					fail ++;
				}
			}, function (err) {
				console.error ('fetch value failed for ' + testName + ' and value ' + testValue);
				fail ++;
			}, testName);
		}, function (err) {
			console.error ('store value failed for ' + testName + ' and value ' + testValue);
			fail ++;
		}, testName, testValue);
		appp.store (function (ok) {
			pass ++;
			appp.fetch (function (ok) {
				if (ok == testValue || (typeof testValue == "object" && JSON.stringify (ok) == JSON.stringify (testValue)))
					pass ++;
				else {
					console.error ('fetched incorrect value for x' + testName + ': expected ' + JSON.stringify (testValue) + ' got ' + JSON.stringify (ok));
					fail ++;
				}
			}, function (err) {
				console.error ('fetch value failed for ' + "x" + testName + ' and value ' + testValue);
				fail ++;
			}, "dict", "x" + testName);
		}, function (err) {
			console.error ('store value failed for ' + "x" + testName + ' and value ' + testValue);
			fail ++;
		}, "dict", "x" + testName, testValue);

	}) (testK, tests[testK]);
}

setTimeout (function () {
	console.log (pass + ' tests passed');
	if (fail)
		console.error (fail + ' tests failed');
}, 1000);
}