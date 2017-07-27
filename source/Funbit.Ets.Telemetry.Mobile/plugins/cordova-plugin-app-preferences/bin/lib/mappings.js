/**
 * Decode a custom config file format into the elements needed to build iOS and Android preference xml
 *
 *
 */
'use strict';
	
var commonMappings = {
	title: {
		ios: "Title",
		android: "@android:title"
	},
	key: {
		ios: "Key",
		android: "@android:key"
	},
	default: {
		ios: "DefaultValue",
		android: "@android:defaultValue"
	},
	description: {
		ios: "FooterText",
		android: "@android:summary"
	},
};

module.exports = {
	group: {
		ios: "PSGroupSpecifier",
		android: "PreferenceCategory",
		attrs: {
			description: commonMappings.description,
			title: commonMappings.title
		}
	},
	selectNotSupported: {
		ios: "PSMultiValueSpecifier",
		android: "MultiSelectListPreference",
		attrs: {
			key:     commonMappings.key,
			title:   commonMappings.title,
			default: commonMappings.default,
		}
	},
	radio: {
		ios: "PSRadioGroupSpecifier",
		android: "ListPreference",
		required: ["title", "key", "default"],
		attrs: {
			key:     commonMappings.key,
			title:   commonMappings.title,
			default: commonMappings.default,
			description: commonMappings.description,
		},
		fixup: {
			ios: function (element, config) {
				element.Titles = [];
				element.Values = [];
				config.items.forEach(function(a) {
					element.Values.push(a.id || a.value);
					element.Titles.push(a.title || a.name);
				});
			},
			android: function (element, config) {
				var name = config.name || config.key,
					titles = [], values = [];

				config.items.forEach(function(item) {
					titles.push(item.name || item.title);
					values.push(item.id || item.value);
				});

				element.strings = {
					name: name,
					titles: titles,
					values: values
				};

				element.attrs['android:entries'] = '@array/apppreferences_' + name;
				element.attrs['android:entryValues'] = '@array/apppreferences_' + name + 'Values';
			}
		}
	},
	toggle: {
		ios: "PSToggleSwitchSpecifier",
		android: "SwitchPreference",
		types: "boolean",
		required: ["title", "key", "default"],
		attrs: {
			key:     commonMappings.key,
			title:   commonMappings.title,
			default: commonMappings.default,
		}
	},
	textfield: {
		ios: "PSTextFieldSpecifier",
		android: "EditTextPreference",
		types: "string",
		required: ["key"],
		attrs: {
			keyboard: {
				android: "@android:inputType",
				ios: "KeyboardType",
				value: {
					// Alphabet , NumbersAndPunctuation , NumberPad , URL , EmailAddress
					// text, number, textUri, textEmailAddress
					// ios: https://developer.apple.com/library/ios/documentation/PreferenceSettings/Conceptual/SettingsApplicationSchemaReference/Articles/PSTextFieldSpecifier.html#//apple_ref/doc/uid/TP40007011-SW1
					// android is little weird http://developer.android.com/reference/android/widget/TextView.html#attr_android:inputType
					number: {ios: "NumberPad", android: "number"},
					text: {ios: "Alphabet", android: "text"},
					uri: {ios: "URL", android: "textUri"},
					email: {ios: "EmailAddress", android: "textEmailAddress"}
				}
			},
			// need a different handling for ios and android
			// IsSecure
			// AutocapitalizationType
			// AutocorrectionType
			key:     commonMappings.key,
			title:   commonMappings.title,
			default: commonMappings.default,
		}
	},
	sliderNotSupported: {
		// slider is not supported for android
		// iOS:
		//@TODO: PSSliderSpecifier
		//Key
		//DefaultValue
		//MinimumValue
		//MaximumValue
	},
	titleNotSupported: {
		// please use group for this, ios only
		// TODO: probably it is good idea to add title automatically:
		// 1. if you want to show wide text input without title
		// 2. for a slider
		// 3. to simulate android summary for fields
	}
};