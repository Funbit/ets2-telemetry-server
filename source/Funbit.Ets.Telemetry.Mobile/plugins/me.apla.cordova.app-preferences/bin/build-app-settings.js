#!/usr/bin/env node

'use strict';

var fs      = require('fs');
var plist   = require('plist');
var libxml  = require('libxmljs');

function ucfirst(s) {
    return s.charAt(0).toUpperCase() + s.substring(1);
}

function ConfigMap(config) {
// iOS
// https://developer.apple.com/library/ios/documentation/cocoa/Conceptual/UserDefaults/Preferences/Preferences.html
/*

mkdir Settings.bundle
cd Settings.bundle
touch Root.plist
mkdir en.lproj
cd en.lproj
touch Root.strings

Identifier

PSGroupSpecifier
Type
Title
FooterText

PSToggleSwitchSpecifier
Title
Key
DefaultValue

PSSliderSpecifier
Key
DefaultValue
MinimumValue
MaximumValue

PSTitleValueSpecifier
Title
Key
DefaultValue

PSTextFieldSpecifier
Title
Key
DefaultValue
IsSecure
KeyboardType (Alphabet , NumbersAndPunctuation , NumberPad , URL , EmailAddress)
AutocapitalizationType
AutocorrectionType

PSMultiValueSpecifier
Title
Key
DefaultValue
Values
Titles

PSRadioGroupSpecifier
Title
FooterText???
Key
DefaultValue
Values
Titles


*/

    if (config.type) {
        
        if (config.type == 'group') {
            config.type = 'PSGroupSpecifier';
        }
        else {     
            config.DefaultValue = config['default'];
            delete config['default'];

            config.Key = config.name;
            delete config['name'];

            switch (config.type) {

                case 'textfield':
                    config.type = 'PSTextFieldSpecifier';                
                    break;

                case 'switch':
                    config.type = 'PSToggleSwitchSpecifier';
                    break;

                case 'combo':
                    config.type = 'PSMultiValueSpecifier';

                    config.titles = [];
                    config.values = [];
                    config.items.forEach(function(a) {
                        config.values.push(a.id || a.value);
                        config.titles.push(a.title || a.name);
                    });
                    delete config.items;
                    break;
            }
        }
    }

	Object.keys(config).forEach(function(k) {
		var uc = ucfirst(k);
		config[uc] = config[k];
		if (uc != k)
			delete config[k];
	})

	return config;
}



fs.readFile('app-settings.json', function(err, data) {
	if (err) {
		throw err;
    }
    
	var iosData = JSON.parse(data);
	var aData = iosData;


	// build iOS settings bundle

	var items = [];
	while (iosData.length) {
		var src = iosData.shift();
		if (src.type == 'group') {
			src.items.forEach(function(s) {
				iosData.unshift(s);
			});
			delete src['items'];
		}
		items.push(ConfigMap(src));
	}

	var plistXml = plist.build({ PreferenceSpecifiers: items });
	fs.exists('platforms/ios', function(exists) {
		if (!exists) {
			console.error('platform ios not found');
            return;
        }
        
		fs.mkdir('platforms/ios/Settings.bundle', function(e) {
			if (e && e.code != 'EEXIST') {
				throw e;
            }
            
			// Write settings plist
			fs.writeFile('platforms/ios/Settings.bundle/Root.plist', plistXml, function(err) {
				if (err) {
					throw err;
                }
				console.log('ios settings bundle was successfully generated');
			});

			// Write localization resource file
			fs.mkdir('platforms/ios/Settings.bundle/en.lproj', function(e) {
				if (e && e.code != 'EEXIST') {
					throw e;
                }
				fs.writeFile('platforms/ios/Settings.bundle/en.lproj/Root.strings', '/* */', function(err) {
					if (err) {
						throw err;
                    }
				});
			});
		});
	});



	// build Android settings XML

	var doc = new libxml.Document();
	var strings = [];
	var n = doc
		.node('PreferenceScreen')
		.attr({'xmlns:android': 'http://schemas.android.com/apk/res/android'});


	var addSettings = function(parent, config) {
		if (config.type == 'group') {
			var g = parent
				.node('PreferenceCategory')
				.attr({'android:title': config.name || config.title});

			config.items.forEach(function(item) {
				addSettings(g, item);
			});
            
		} else {

			var attr = {
				'android:title': config.title,
				'android:key': config.name,
				'android:defaultValue': config['default']
			}

			switch (config.type) {
				case 'combo':
					// Generate resource file
					var d = new libxml.Document();
					var res = d.node('resources');
					var titles = res.node('string-array').attr({name: config.name}),
					    values = res.node('string-array').attr({name: config.name + 'Values'});

					config.items.forEach(function(item) {
						titles.node('item', item.name || item.title);
						values.node('item', item.id || item.value);
					});

					strings.push({
						name: config.name,
						xml: d.toString()
					});

					attr['android:entries'] = '@array/' + config.name;
					attr['android:entryValues'] = '@array/' + config.name + 'Values';

					parent
						.node('ListPreference')
						.attr(attr)
				break;
			}
		}
	}
	aData.forEach(function(item) {
		addSettings(n, item);
	});


	fs.exists('platforms/android', function(exists) {
		if (!exists) {
			console.error('platform android not found');
            return;
        }

		fs.mkdir('platforms/android/res/xml', function(e) {
			if (e && e.code != 'EEXIST') {
				throw e;
            }

			// Write settings plist
			fs.writeFile('platforms/android/res/xml/preference.xml', doc.toString(), function(err) {
				if (err) {
					throw err;
                }
				console.log('android preferences file was successfully generated');
			});

			// Write localization resource file
			fs.mkdir('platforms/android/res/values', function(e) {
				if (e && e.code != 'EEXIST') {
					throw e;
                }
				strings.forEach(function(file) {
					fs.writeFile('platforms/android/res/values/' + file.name + '.xml', file.xml, function(err) {
						if (err) {
							throw err;
                        }
					});
				});
			});
		});
	});

});