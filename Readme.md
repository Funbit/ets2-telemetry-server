## ETS2 Telemetry Web Server 2.0.0 + Mobile Dashboard

This is a free Telemetry Web Server for Euro Truck Simulator 2 written in C#. The client side consists of a skinnable HTML5 mobile dashboard application that works in any modern desktop or mobile browser. Android users may also use provided native Android application.   

## Main Features

- Free and open source
- Automated installation
- REST API for ETS2 telemetry data 
- Support for custom dashboard skins
- Telemetry data broadcasting to a given URL via HTTP protocol

### Telemetry REST API
  
    GET http://localhost:25555/api/ets2/telemetry

Returns JSON object with the latest telemetry data read from the game. The state is updated upon every API call. You may use this REST API for your own Applications. Here is a short explanation of some telemetry properties:

- DateTime values are serialized to ISO 8601 strings in UTC time zone. Dates always start from 0001 year when 1st January is Monday.    
- Gear values: -1 = R, 0 = N, 1 = D1, 2 = D2, etc.
- Mass is expressed in kilograms
- Speed is expressed in km/sec

Please note that GET responses may be cached by your HTTP client. To avoid caching you may use some random query string parameter or POST method which returns exactly the same result.

### HTML5 Mobile Dashboard Application
    http://localhost:25555/

This HTML5 dashboard application is designed for mobile and desktop browsers. You should be able to use the dashboard just by navigating to the URL in your Mobile Safari (iOS 8+), Android 4+ browsers (Default or Chrome) or any modern desktop browser. 

Here is a screenshot of how your mobile dashboard will look like in a browser:

![](https://raw.githubusercontent.com/Funbit/ets2-telemetry-server/master/source/Funbit.Ets.Telemetry.Mobile/skins/default/dashboard.jpg)

Dashboard design is very customizable. All you have to do is to change dashboard.css, dashboard.html and dashboard.js (if needed). 

## Installation and Usage

### Supported OS

- Windows Vista, Windows 7 or Windows 8 (32-bit or 64-bit).
- .NET Framework 4.5 (pre-installed in Windows 8+). If it is not installed you will be prompted to install it when you run the server.

### Supported games

- Euro Truck Simulator 2 (32-bit or 64-bit). Multiplayer versions are supported as well. Steam version is preferred (non Steam users must edit configuration file prior to the installation). 

### Tested browsers

- iOS 8+ running Mobile Safari
- Android 4+ Default or Chrome browsers
- Latest Firefox, Chrome or IE11

### Installation

1. Download bundle by clicking **Download ZIP** button at the right side of this page. 
2. **Unpack downloaded ZIP** file *anywhere* you want.
3. Run **server\Ets2Telemetry.exe** 
4. Click "**Install**" button to perform the installation (see below for details) 
5. When installation finishes click "**OK**", select your network interface and click "**HTML5 App URL**" link to open your dashboard!

If installer reports that it can't find your ETS2 game directory (when you don't use Steam for example) you must set it manually inside **server\Ets2Telemetry.exe.config** file (search for *Ets2GamePath* setting). 

Android users should install the provided "Ets2 Dashboard" application. The APK file is located in **mobile/Android/Ets2Dashboard.apk**. Copy it to your device and install via Android's File Manager. The application will prevent your device from going into sleep mode and will remember server IP address which is very useful if you are going to use the app frequently.

***Security notes***: The installation must be done only once and requires Administrator privileges. If you mind what exactly server does to the system at this point here is the detailed information:

1. Tries to find your ETS2 game directory and copy ets2-telemetry.dll plugin there (avoiding overwrites)
2. Creates a new Firewall rule for 25555 port named "ETS2 TELEMETRY SERVER (PORT 25555)" opened only for local subnet (i.e. it won't be visible from Internet, so you are safe)
3. Creates a new ACL rule for HTTP URL bound on 25555 port for OWIN's HttpListener ([more details](http://msdn.microsoft.com/en-us/library/ms733768%28v=vs.110%29.aspx))
4. Creates a new file for storing application settings inside "\Users\USERNAME\AppData\Local\Ets2 Telemetry Server".

Also, if you don't trust my compiled ets2-telemetry.dll you may compile it by yourself from [the official telemetry SDK](https://github.com/nlhans/ets2-sdk-plugin).

### Usage

1. Run **server/Ets2Telemetry.exe**  
2. Run Euro Truck Simulator 2 (the order is not important though).
3. **iOS users**: connect your iPhone or iPad to the same Wi-Fi network as your PC, open Safari and navigate to the "*HTML5 App URL*" displayed by the server. **Android users**: run "*Ets2 Dashboard*" application, enter server IP (*without http and port*) and press OK. If IP address is correct it will be remembered for the next time.
4. Enjoy your mobile dashboard while playing your favorite simulator! ;)

### Uninstallation

If server hasn't fulfilled your expectations and you decide to uninstall it, then:

1. Exit from the Euro Truck Simulator
1. Run **server\Uninstall.bat**
2. Click "**Uninstall**" button
3. Done

At this moment your system will be in exactly the same state as it were before the installation. The only difference is that ets2-telemetry.dll plugin files are not deleted but renamed to .bak.

### Known problems:

- Sometimes D1 gear is not properly displayed on the screen (telemetry SDK limitation)
- Cruise control is not properly updated (telemetry SDK limitation)

## Dashboard skin tutorial

Not yet available. Please stay tuned.

## Version history

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

- [Server discussion on the official SCS forum](http://forum.scssoft.com/viewtopic.php?f=41&t=171000)
- [Server discussion on the ets2mp.com forum](http://forum.ets2mp.com/index.php?/topic/3058-ets2-telemetry-web-server-mobile-gauge-for-all-phones/) 

## License

MIT.