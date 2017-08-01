//
//  AppPreferences.h
//
//
//  Created by Tue Topholm on 31/01/11.
//  Copyright 2011 Sugee. All rights reserved.
//
//  Modified by Ivan Baktsheev, 2012-2015
//

#import <Foundation/Foundation.h>

#import <Cordova/CDV.h>

@interface AppPreferences : CDVPlugin

- (void)defaultsChanged:(NSNotification *)notification;
- (void)watch:(CDVInvokedUrlCommand*)command;
- (void)fetch:(CDVInvokedUrlCommand*)command;
- (void)remove:(CDVInvokedUrlCommand*)command;
- (void)clearAll:(CDVInvokedUrlCommand*)command;
- (void)show:(CDVInvokedUrlCommand*)command;
- (void)store:(CDVInvokedUrlCommand*)command;
- (NSString*)getSettingFromBundle:(NSString*)settingsName;


@end
