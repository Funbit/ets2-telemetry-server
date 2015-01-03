//
//  AppPreferences.m
//
//
//  Created by Tue Topholm on 31/01/11.
//  Copyright 2011 Sugee. All rights reserved.
//
//  Modified by Ivan Baktsheev, 2012-2013
//
// THIS HAVEN'T BEEN TESTED WITH CHILD PANELS YET.

#import "AppPreferences.h"

@implementation AppPreferences

- (void)fetch:(CDVInvokedUrlCommand*)command
{

	__block CDVPluginResult* result = nil;

	NSDictionary* options = [[command arguments] objectAtIndex:0];

	if (!options) {
		result = [CDVPluginResult resultWithStatus:CDVCommandStatus_ERROR messageAsString:@"no options given"];
		[self.commandDelegate sendPluginResult:result callbackId:[command callbackId]];
		return;
	}

	NSString *settingsDict = [options objectForKey:@"dict"];
	NSString *settingsName = [options objectForKey:@"key"];

	[self.commandDelegate runInBackground:^{
	NSUserDefaults *defaults = [NSUserDefaults standardUserDefaults];

	id target = defaults;

	// NSMutableDictionary *mutable = [[dict mutableCopy] autorelease];
	// NSDictionary *dict = [[mutable copy] autorelease];

	@try {

		if (settingsDict) {
			target = [defaults dictionaryForKey:settingsDict];
		}

		NSString *returnVar;
		id settingsValue = [target objectForKey:settingsName];

		if (settingsValue != nil) {
			if ([settingsValue isKindOfClass:[NSString class]]) {
				returnVar = [NSString stringWithFormat:@"\"%@\"", (NSString*)settingsValue];
			} else if ([settingsValue isKindOfClass:[NSNumber class]]) {
				if ((NSNumber*)settingsValue == (void*)kCFBooleanFalse || (NSNumber*)settingsValue == (void*)kCFBooleanTrue) {
//					const char * x = [(NSNumber*)settingsValue objCType];
//					NSLog(@"boolean %@", [(NSNumber*)settingsValue boolValue] == NO ? @"false" : @"true");
					returnVar = [NSString stringWithFormat:@"%@", [(NSNumber*)settingsValue boolValue] == YES ? @"true": @"false"];
				} else {
					// TODO: int, float
//					NSLog(@"number");
					returnVar = [NSString stringWithFormat:@"%@", (NSNumber*)settingsValue];
				}

			} else if ([settingsValue isKindOfClass:[NSData class]]) { // NSData
				returnVar = [[NSString alloc] initWithData:(NSData*)settingsValue encoding:NSUTF8StringEncoding];
			}
		} else {
			returnVar = [self getSettingFromBundle:settingsName]; //Parsing Root.plist

//			if (returnVar == nil)
//				@throw [NSException exceptionWithName:nil reason:@"Key not found" userInfo:nil];;
		}

		result = [CDVPluginResult resultWithStatus:CDVCommandStatus_OK messageAsString:returnVar];

	} @catch (NSException * e) {

		result = [CDVPluginResult resultWithStatus:CDVCommandStatus_NO_RESULT messageAsString:[e reason]];

	} @finally {

		[self.commandDelegate sendPluginResult:result callbackId:[command callbackId]];
	}
	}];
}

- (void)store:(CDVInvokedUrlCommand*)command
{
    __block CDVPluginResult* result;

	NSDictionary* options = [[command arguments] objectAtIndex:0];

	if (!options) {
		result = [CDVPluginResult resultWithStatus:CDVCommandStatus_ERROR messageAsString:@"no options given"];
		[self.commandDelegate sendPluginResult:result callbackId:[command callbackId]];
		return;
	}

	NSString *settingsDict  = [options objectForKey:@"dict"];
    NSString *settingsName  = [options objectForKey:@"key"];
    NSString *settingsValue = [options objectForKey:@"value"];
	NSString *settingsType  = [options objectForKey:@"type"];

//	NSLog(@"%@ = %@ (%@)", settingsName, settingsValue, settingsType);

	[self.commandDelegate runInBackground:^{
	NSUserDefaults *defaults = [NSUserDefaults standardUserDefaults];

	id target = defaults;

	// NSMutableDictionary *mutable = [[dict mutableCopy] autorelease];
	// NSDictionary *dict = [[mutable copy] autorelease];

	if (settingsDict) {
		target = [[defaults dictionaryForKey:settingsDict] mutableCopy];
		if (!target) {
			target = [[NSMutableDictionary alloc] init];
#if !__has_feature(objc_arc)
			[target autorelease];
#endif
		}
	}

	NSError* error = nil;
	id JSONObj = [NSJSONSerialization JSONObjectWithData:[settingsValue dataUsingEncoding:NSUTF8StringEncoding]
                                                options:NSJSONReadingAllowFragments
                                                  error:&error];

	if (error != nil) {
        NSLog(@"NSString JSONObject error: %@", [error localizedDescription]);
    }

    @try {

		if ([settingsType isEqual: @"string"] && [JSONObj isKindOfClass:[NSString class]]) {
			[target setObject:(NSString*)JSONObj forKey:settingsName];
		} else if ([settingsType  isEqual: @"number"] && [JSONObj isKindOfClass:[NSNumber class]]) {
			[target setObject:(NSNumber*)JSONObj forKey:settingsName];
			// setInteger: forKey, setFloat: forKey:
		} else if ([settingsType  isEqual: @"boolean"]) {
			[target setObject:JSONObj forKey:settingsName];
		} else {
			// data
			[target setObject:[settingsValue dataUsingEncoding:NSUTF8StringEncoding] forKey:settingsName];
		}

		if (target != defaults)
			[defaults setObject:(NSMutableDictionary*)target forKey:settingsDict];
		[defaults synchronize];

        result = [CDVPluginResult resultWithStatus:CDVCommandStatus_OK];

    } @catch (NSException * e) {

        result = [CDVPluginResult resultWithStatus:CDVCommandStatus_NO_RESULT messageAsString:[e reason]];

	} @finally {

        [self.commandDelegate sendPluginResult:result callbackId:[command callbackId]];
    }
	}];
}
/*
  Parsing the Root.plist for the key, because there is a bug/feature in Settings.bundle
  So if the user haven't entered the Settings for the app, the default values aren't accessible through NSUserDefaults.
*/


- (NSString*)getSettingFromBundle:(NSString*)settingsName
{
	NSString *pathStr = [[NSBundle mainBundle] bundlePath];
	NSString *settingsBundlePath = [pathStr stringByAppendingPathComponent:@"Settings.bundle"];
	NSString *finalPath = [settingsBundlePath stringByAppendingPathComponent:@"Root.plist"];

	NSDictionary *settingsDict = [NSDictionary dictionaryWithContentsOfFile:finalPath];
	NSArray *prefSpecifierArray = [settingsDict objectForKey:@"PreferenceSpecifiers"];
	NSDictionary *prefItem;
	for (prefItem in prefSpecifierArray)
	{
		if ([[prefItem objectForKey:@"Key"] isEqualToString:settingsName])
			return [prefItem objectForKey:@"DefaultValue"];
	}
	return nil;

}
@end
