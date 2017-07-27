var android = require("../lib/android")();

describe("android functions", function() {

	it("exists", function() {
		expect(android).not.toBeNull();
	});

	it("generates group items", function() {

		var config = {
			type: "group",
			title: "test group",
			items: [
				{ type: "textfield", key: "child 1" },
				{ type: "textfield", key: "child 2" }
			]
		};

		var item = android.mapConfig(config);
		console.log(item);
		expect(item.tagname).toEqual('PreferenceCategory');
		expect(item.children).not.toBeNull();
	});

	it("maps a texfield control", function() {

		var config = {
			type: "textfield",
			default: "test_value",
			key: "test_key"
		};

		var element = android.mapConfig(config);

		expect(element.tagname).toEqual('EditTextPreference');
		expect(element.atts).not.toBeNull();
	});

	it("builds the item array", function() {

		var configs = [{
			type: "group",
			title: "test group",
			items: [
				{ type: "textfield", key: "child 1", title: "child 1" },
				{ type: "textfield", key: "child 2", title: "child 2" }
			]
		}];

		var prefsDocuments = android.buildSettings(configs);
		console.log(prefsDocuments);
		expect(prefsDocuments.preferencesDocument).not.toBeNull();
		expect(prefsDocuments.preferencesStringDocument).not.toBeNull();
	});

	it ("extended radio play", function () {
		var configs = [
			{
				"type":"group",
				"title":"Measurement Units",
				"key":"measurement_units",
				"description":"Define which measurement unit is prefered",
				"items":[
					{
						"type":"radio",
						"items":[
							{
								"value":"kilometers_litres",
								"title":"Use kilometers / litres"
							},
							{
								"value":"miles_gallons",
								"title":"Use miles / gallons"
							}
						],
						"default":"kilometers_litres",
						"title":"Measurement unit",
						"key":"measurement_unit",
						"name":"measurementunit"
					}
				]
			}
		];

		var prefsDocuments = android.buildSettings(configs);
		console.log(prefsDocuments);
		expect(prefsDocuments.preferencesDocument).not.toBeNull();
		expect(prefsDocuments.preferencesStringDocument).not.toBeNull();
	});

});
