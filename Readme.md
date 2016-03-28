## ETS2 Telemetry Web Server 3.2.5 + Mobile Dashboard

This is a free Telemetry Web Server for [Euro Truck Simulator 2](http://www.eurotrucksimulator2.com/) and [American Truck Simulator](http://www.americantrucksimulator.com/) written in C# based on WebSockets and REST API. The client side consists of a skinnable HTML5 mobile dashboard application that works in any modern desktop or mobile browser. Android users may also use provided native Android application.   

## Main Features

- Free and open source
- Automated installation
- REST API for telemetry data
- HTML5 dashboard application for live telemetry data streaming based on WebSockets 
- Powerful support for custom dashboard skins (skin tutorial included)
- Telemetry data broadcasting to a given URL via HTTP protocol

### Telemetry REST API
  
    GET http://localhost:25555/api/ets2/telemetry

Returns structured JSON object with the latest telemetry data read from the game: 

    {    
		"game": {
			"connected": true,
			"paused": false,
			"gameName": "ETS2",
			"time": "0001-01-08T21:09:00Z",
			"timeScale": 19.0,
			"nextRestStopTime": "0001-01-01T10:11:00Z",
			"version": "1.10",
			"telemetryPluginVersion": "7"
		},
		"truck":{
			"id": "man",
			"make": "MAN",
			"model": "TGX",
			"speed": 53.82604,
			... 
    }

The state is updated upon every API call. You may use this REST API for your own Applications. 

**The complete telemetry property reference is available [here](Telemetry.md).**

Please note that GET responses may be cached by your HTTP client. To avoid caching you may use some random query string parameter or POST method which returns exactly the same result.

### HTML5 Mobile Dashboard Application
    http://localhost:25555/

This HTML5 dashboard application is designed for mobile and desktop browsers. You should be able to use the dashboard just by navigating to the URL in your Mobile Safari (iOS 8+), Android 4+ browsers (Default or Chrome) or any modern desktop browser. 

Here is a screenshot of how your mobile dashboard will look like in a browser:

![](https://raw.githubusercontent.com/Funbit/ets2-telemetry-server/master/source/Funbit.Ets.Telemetry.Mobile/skins/default/dashboard.jpg)

The package contains other photo realistic skins as well. For example, this is a skin for MAN-TGX:

![](https://raw.githubusercontent.com/Funbit/ets2-telemetry-server/master/server/Html/skins/man-tgx/dashboard.jpg)

As you can see dashboard design is completely customizable. With some basic knowledge of the HTML and CSS you can create your own skins. See Dashboard skin tutorial below for more information. 

## Setup

### Supported OS

- **Windows Vista, Windows 7, 8 or 10 (32-bit or 64-bit)**. Windows XP is not supported.
- **.NET Framework 4.5** (pre-installed in Windows 8+). If it is not installed you will be prompted to install it when you run the server. 

### Supported games

- Euro Truck Simulator 2 (32-bit or 64-bit) version 1.15+ (Steam or Standalone). Multiplayer versions are supported as well. 
- American Truck Simulator (Steam or Standalone)

### Tested browsers

- iOS 8+ running Mobile Safari (highly recommended, best user experience)
- Latest Firefox, Chrome or IE11 (Firefox or Chrome is recommended though)
- Android 4+ (4.4+ highly recommended) Default or Chrome browsers (see FAQ if you have performance issues)

### Installation

1. Download bundle by clicking **Download ZIP** button at the right side of this page. 
2. **Unpack downloaded ZIP** file *anywhere* you want.
3. Run **server\Ets2Telemetry.exe** 
4. Click "**Install**" button to perform the installation 
5. When installation finishes click "**OK**", select your network interface and click "**HTML5 App URL**" link to open your dashboard
6. **Done** (now you may read *Usage* topic to understand how to use the server)

Android users may install the provided "Ets2 Dashboard" application. The APK file is located in **mobile/Android/Ets2Dashboard.apk**. Copy it to your device and install via Android's File Manager. The application will prevent your device from going into sleep mode and will remember server IP address which is very useful if you are going to use the app frequently.

***Security notes***: The installation must be done only once and requires Administrator privileges. If you mind what exactly server does to the system at this point here is the detailed information:

1. Tries to find your ETS2 game directory and copy ets2-telemetry-server.dll plugin there
2. Creates a new Firewall rule for 25555 port named "ETS2 TELEMETRY SERVER (PORT 25555)" opened only for local subnet (i.e. it won't be visible from Internet, so you are safe)
3. Creates a new ACL rule for HTTP URL bound on 25555 port for OWIN's HttpListener ([more details](http://msdn.microsoft.com/en-us/library/ms733768%28v=vs.110%29.aspx))
4. Creates a new file for storing application settings inside "\Users\USERNAME\AppData\Local\Ets2 Telemetry Server".

The server also reports everything to the log file (Ets2Telemetry.log), so you may see the details there as well.

Also, if you don't trust my compiled ets2-telemetry-server.dll you may compile it by yourself using [plugin's source code](https://github.com/Funbit/ets2-sdk-plugin) and Visual Studio 2013+.

### Skin Installation

If you downloaded some third-party skin (which is folder, containing dashboard.html, css, js and image files, but **no EXE files**!) you may install it just by **copying to server/Html/skins** directory. It should appear in the skin menu as soon as you refresh your browser.

### Upgrade

If you already have a previous version installed, it is recommended to leave it as is and **unpack the new version into a separate directory**. This way you will never lose your configuration files, logs, etc. 

However, please keep in mind that if you plan to return back to previous version you have to uninstall the latest one first!

### Uninstallation

If server hasn't fulfilled your expectations and you decide to uninstall it, then:

1. Exit from the Euro Truck Simulator / American Truck Simulator
2. Select from the server's menu: Server -> Uninstall
3. Click "**Uninstall**" button in the popped up window
4. **Done**

At this moment your system will be in exactly the same state as it were before the installation. 

## Usage

1. Run **server/Ets2Telemetry.exe**  
2. Run Euro Truck Simulator 2 / American Truck Simulator.
3. **Desktop users**: connect your notebook to the same Wi-Fi/LAN network as your PC, open Firefox, Chrome or IE and navigate to the "*HTML5 App URL*" displayed by the server. 
3. **iOS users**: connect your iPhone or iPad to the same Wi-Fi network as your PC, open Safari and navigate to the "*HTML5 App URL*" displayed by the server. 
4. **Android users**: run "*Ets2 Dashboard*" application, enter server IP (*without http and port*, exactly in the same format as displayed by the server) and press OK. If IP address is correct it will be remembered for the next time.
5. **Enjoy** your mobile dashboard while playing your favorite simulator! ;)

## FAQ

> I ran the server and opened HTML5 App URL on a device but browser says "Page not found". What should I do?

First of all, make sure that your device is using Wi-Fi connection instead of mobile internet (3G, 4G, etc.). Then, make sure that you selected correct "Network interface" on the main server screen. You must select the interface that is directly connected to your Wi-Fi network, *usually* it is named as "Wi-Fi", "Ethernet" or "LAN". Also, make sure that "AP Isolation" is disabled on your Wi-Fi router ([more info](http://www.howtogeek.com/179089/lock-down-your-wi-fi-network-with-your-routers-wireless-isolation-option/)). If you still can't connect - try to temporarily disable firewalls (especially from 3rd-parties) or anti-viruses and check again. If problem persists then you should contact to your Administrator... 

> I installed provided Android application, but it always shows "Could not connect to the server" or "Disconnected" status. How do I fix that?

Please check if you can connect to the dashboard from a browser first (read the answer above). If you are able to connect via browser then there is something wrong with the application (or Android environment). You may try to restart it or reinstall.

> I started the game but dashboard is displaying "Connected, waiting for the drive..." message. What is wrong?

Please make sure that the server window is showing "Connected to the simulator" status message. If it is showing "Simulator is running" instead - then there is a problem with the telemetry plugin installation (ets2-telemetry-server.dll). If it is showing "Simulator is not running" but simulator is actually running then you have an incompatible ETS2 version.

> The dashboard UI animation (meters) sometimes stutters. Is my device not good enough? Is it possible to fix that?

The HTML5 dashboard is optimized for modern browsers and fast network connections (LAN, local Wi-Fi), so you may experience problems on old devices or devices having old web browsers. 

Some performance examples:

The dashboard will work smoothly on Samgung Galaxy Tab S (4.4.2), but not on Galaxy Note 1 or Kindle Fire HD due to slow GPU or turned off GPU graphics acceleration. To achieve the best performance on Androids you may try to use a standalone Chrome browser instead of an app (but you will need to turn off device sleep mode when you use the dashboard). 

The dashboard will work very smoothly on iOS 8.X devices (iPhone 6 or new iPads). But it will not properly work on iOS 6.X (iPhone 3GS, old iPods). If you are experiencing slow performance even on new devices you may try to close all opened apps (especially Safari) and try again, that helps a lot.

The dashboard will perfectly work on any PC or laptop inside latest Firefox, Chrome or IE11.  

Also, do not forget to turn off background downloads, especially Torrent clients, because they may dramatically slow your connection between devices!

> Is it safe to use the server? Can it crash my game? What about influence on the game performance, say FPS, processor load?

The server is written very carefully. It will not crash your game, because the telemetry plugin was created by the official developers. It also does not eat CPU (the load is less than 1%) and has small memory footprint (around 20MB). So you won't notice any difference in FPS. 

If you don't trust the compiled exe/dll files you are always welcome to check them for viruses by more than 50 different anti-virus programs on [VirusTotal](https://www.virustotal.com/) site.

> Can I use mobile dashboard on Android 2.x devices?

No. There is a chance that it will work, but it won't be supported.

> Is it possible to include sleep indicator, remaining distance, ETA, current speed limit (or whatever else)?

Starting from version 3.0.0 this is possible, but I haven't yet had time to update the default skins to display it. But I will ;)

## Dashboard skin tutorial

The tutorial is included in the ZIP package (see "Dashboard Skin Tutorial.pdf"). You may download it separately from [here](https://raw.githubusercontent.com/Funbit/ets2-telemetry-server/master/Dashboard%20Skin%20Tutorial.pdf).

## Support

The ETS2 Telemetry Web Server has evolved into a pretty complex open-source project that requires singificant amount of time to support. If you are interested in its future you may provide the author with some material support by clicking the button below.

[![](https://raw.githubusercontent.com/Funbit/ets2-telemetry-server/master/server/Html/images/donate-link.png)](http://funbit.info/ets2/donate.htm)

Thank you!

## Version history

### 3.2.5

- Another improvement for job information reset code (plugin DLL update).

### 3.2.4

- Added support for SVG, GIF, TTF and WOFF content types.

### 3.2.3

- Added support for up to 9 levels of sub directories for skins to use (previous version allowed only 5).
- Minor code refactoring.

### 3.2.2

- Fixed occasional bug in latest job information reset code (new job info might have been reset).

### 3.2.1

- Updated telemetry plugin DLL and fixed job information reset when job is done (trailer detached).
- Added support for sending UserId/Password for telemetry broadcaster to allow user identification.
- Fixed typo in property name: adblueAverageConsumption (was adblueAverageConsumpton).

### 3.2.0

- Added support for American Truck Simulator
- Added two new skins: MAN TGX (MPH) and Peterbilt 579 by NightstalkerPL, Lisek Chytrusek (by WEBX.PL)
- Added application menu to ease uninstallation and extending for future updates
- Added new GameName property to the telemetry (proposed by mkoch227)
- Fixed CPU overhead issue for some users (when CPU goes above 1%)
- Updated all project files for Visual Studio 2015 and C# 6.0

### 3.0.7

- Fixed 'Dynamic' skin size display issue.

### 3.0.6

- Fixed minor bug in the telemetry plugin with wrong hshifter.selector usage.
- Added new 'Dynamic' skin size type, cursor fixes and other refactorings (thanks to denilsonsa & Phil0499!).

### 3.0.5

- Removed small visual padding around skins. Now skins takes full canvas space (thanks to James).
- Fixed rounding problem with speed limit value, sometimes it was displayed as 79 instead of 80, etc (thanks to R0adrunner).

### 3.0.4

- Restored truck.engineOn property (separated from truck.electricOn).

### 3.0.3

- Removed truck.engineOn property which was a duplicate for truck.electricOn property causing it not to work properly inside the telemetry plugin (thanks to ivanrichwalski).

### 3.0.2

- Fixed bug with parsing date-min/max HTML attributes.
- Changed property name format inside date-min/max HTML attributes, object separation character was changed from '-' to '.', for example: **date-max="truck-fuelCapacity"** must be changed to: **date-max="truck.fuelCapacity"**. 

### 3.0.1

- Fixed iOS Safari window reload bug (window gets reloaded several times in a row).
- Added new skin "rd-info" created by van_argiano. Thank you very much!

### 3.0.0

- Changed telemetry JSON object structure by introducing complex nested types. If you develop or use 3rd party custom skin please note that it will not work as is with the new server version! You will have to update your skin files first. I have created a handy tool that will do 99% (or usually even 100%) work for you. You may [download the tool here](http://funbit.info/ets2/Telemetry-Dashboard-3.0.0-Skin-Upgrader.zip). Just drop your skin files to the  SkinFileUpgrader.exe and you will get the updated version of it (you need to upgrade dashboard.html, css and js files). The source code is included in the ZIP files as well, just in case.
- New telemetry JSON structure. All properties are now structurized in several categories: game, truck, trailer, job and navigation. To understand the new system better please refer to the [updated skin tutorial](https://raw.githubusercontent.com/Funbit/ets2-telemetry-server/master/Dashboard%20Skin%20Tutorial.pdf) and [complete telemetry property reference](Telemetry.md). 
- Removed hasJob property. You should use *trailer.attached* property instead (or add a custom *data.hasJob = data.trailer.attached;* to your dashboard.js).
- Added support for new telemetry properties: game.nextRestStopTime, game.timeScale, truck.forwardGears, truck.reverseGears, navigation.estimatedTime, navigation.estimatedDistance, navigation.speedLimit.
- Added [complete telemetry property reference](Telemetry.md). 
- Fixed aux light indicators (roof and front indicators didn't work)
- [Forked ETS2 telemetry plugin](https://github.com/Funbit/ets2-sdk-plugin) to make it a custom part of the server.
- Some bug fixes and improvements.

### 2.2.6

- Fixed trailerAttached property so it is properly changed when trailer is attached/detached. 
- Changed hasJob property, now it equals to the trailerAttached value.
- Fixed odometer for default MPH skin (was showing kilometers) *(thanks to kevindwood)* 

### 2.2.5

- Fixed truck speed rounding to avoid jumps between 0 and 1 km/h 
- Created new MPH version of the default skin
- Fixed minor bug with cruise control speed displayed as NaN sometimes (default skin)

### 2.2.4

- Fixed speedometer for Scania and Volvo skins (made the needle movement smoother) 
- Fixed floating point rendering (truck speed sometimes might have been displayed as XX.YYYY) *(big thanks to Jorji_costava and sketch)*

### 2.2.3

- Fixed speedometer for all built-in skins (DAF, MAN, Mercedes, Volvo)
- Implemented automatic window reloading on resize (for PC) *(greetings to denilsonsa)*
- Added skin ability to control user clicks (back to menu link moved to dashboard.js) *(greetings to mkoch227)*
- Changed speed rounding algorithm to match game's speed *(greetings to 
maysaraahmad)*
- Minor comment and typo fixes

### 2.2.2

- Fixed bug in Android APK (no IP address prompt)
- Fixed Scania skin (invalid speed limit)

### 2.2.1

- Completely revamped dashboard core, including rendering and connection layers. The mobile dashboard now reflects game changes almost instantly (within 5-10ms)!
- Removed refreshRate option (now it is adjusted automatically)
- Fixed fuelWarning telemetry property (updated telemetry plugin DLL)
- Fixed NaN trailer mass when dashboard is disconnected (default skin only)
- Added some utility functions to dashboard.js (see Skin Tutorial for more info)
- Added new server status: "Connected to Ets2TestTelemetry.json"

### 2.2.0

- Added Dashboard Skin Tutorial!
- Fixed support for Cruise Control indicator and added Cruise Control Speed
- Fixed deadline time bug
- Made speed value always positive (even when reversing)
- Significantly improved skin loading speed
- Added ability to skip certain setup steps to support 3rd-party firewalls
- Added ability to manually select ETS2 game path using standard UI when it is not detected automatically
- Added wear indicators to the default skin
- Added additional status message to check if server is connected to the telemetry plugin
- Added 5 new photo realistic skins made by Klauzzy (DAF-XF, MAN-TGX, Mercedes-Atego, Scania, Volvo-FH)
- Added simple template skin
- Changed telemetry plugin DLL name from ets2-telemetry.dll to ets2-telemetry-server.dll (previous version is not compatible anymore)
- Various refactoring and improvements

### 2.1.0

- Moved to WebSockets for low-latency data updates
- Optimized UI animation (now it is SUPER SMOOTH, especially in Desktop and Mobile Safari browsers)  
- Minor fixes

### 2.0.0

- Completely rewritten client side application. All code is now written in Typescript. 
- Full support for custom skins
- Automated server installer
- Telemetry broadcasting to external URLs (see Ets2Telemetry.exe.config)
- Updated default dashboard skin
- Administrator rights are now required only for installation. Server starts under user privileges.

### 1.0.4

- Added server IP to the server window
- Minor logging improvements
- Fixed IE behavior with ajax requests (should fix Windows Phone issues)

### 1.0.3

- Fixed bug with invalid day of the week
- Improved connection stability
- Completely decoupled gauge design and gauge update engine (coded in Typescript)
- Added some scripts to simplify the installation

### 1.0.2
- Refactored gauge screen fitting algorithm, the app should work in any modern browser now 
- Added logging
- Added support for binding on a particular network interface
- Added Cordova mobile application (compiled Android APK is included in the bundle)
- Various fixes and improvements
- Made HTML5 application URL shorter

### 1.0.1
- Fixed bug with multiple network interfaces (thanks to thorerik)
- Made application run under Administrator by default (thanks to thorerik)
- Updated application icon and added it to the HTML app
- Minor refactoring and bug fixes 

## External links

Forums

- [Discussion on the official SCS forum](http://forum.scssoft.com/viewtopic.php?f=41&t=171000)

Video

- [Dashboard overview from SimplySimulators](https://www.youtube.com/watch?v=mM13wfTYfM8)
- [Dashboard usage with OBS from A JonC](https://www.youtube.com/watch?v=yLFu4DPixCM)
- [Dashboard overview from MrSoundwaves Cubes](https://www.youtube.com/watch?v=2OCs9RwA0AI)
- [Dashboard usage from z5teve](https://www.youtube.com/watch?v=gdwpTwhzZIg)
- [Dashboard overview from Driver Geo (Romanian)](https://www.youtube.com/watch?v=zcrmyD5wq10)
- [Dashboard overview from MaRKiToX12 (Spanish)](https://www.youtube.com/watch?v=J_SpwY8RIX4)
- [Dashboard overview from Branislav Rác (Slovak)](https://www.youtube.com/watch?v=LpKyuNWxJTU)
- [Dashboard overview from 1Tera Games (Portuguese)](https://www.youtube.com/watch?v=hfUMWmuLToQ)
- [Обзор от Саши Плотникова (на русском)](https://www.youtube.com/watch?v=mmNm27eTTBs)
  
## License

[GNU General Public License v3 (GPL-3)](https://tldrlegal.com/license/gnu-general-public-license-v3-%28gpl-3%29).