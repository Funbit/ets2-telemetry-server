var fs = require("../lib/filesystem")(require("q"), require("fs"));
var ios = require("../lib/ios")(fs, require("plist"));

describe("ios functions", function() {
	
	it("exists", function() {
		expect(ios).not.toBeNull();
	});

	it("maps a texfield control", function() {

		var config = {
			type: "textfield",
			default: "test_value",
			key: "test_key",
			keyboard: "email"
		};

		var element = ios.mapConfig(config);

		expect(element.Key).toEqual(config.key);
		expect(element.DefaultValue).toEqual(config.default);
		expect(element.KeyboardType).toEqual("EmailAddress");
	});

	it("builds array of ios preference items", function() {

		var configs = [{
			type: "textfield",
			key: "test_key_1"
		},{
			type: "textfield",
			key: "test_key_1"
		}];

		var items = ios.buildItems(configs);
		expect(items.length).toEqual(2);
	});

	it("flattens group items", function() {

		var configs = [{
			type: "group",
			items: [
				{ type: "textfield", key: "child 1" },
				{ type: "textfield", key: "child 2" }
			]
		}];

		var items = ios.buildItems(configs);
		expect(items.length).toEqual(3);
	});

});
