## ETS2 Telemetry Web Server + Mobile Gauge (Version 1.0.3)

This is a modern ETS2 Telemetry Web Server written in C# and WebApi. The server exposes the following endpoints:

### Telemetry REST API
  
    GET http://localhost:25555/api/ets2/telemetry

It returns IEts2TelemetryData JSON object with the latest telemetry data read from the game. The state is updated upon every API call. You may use this REST API for your own Applications. Here is a short explanation of some IEts2TelemetryData properties:

- DateTime values are serialized to ISO 8601 strings relative to the server's timezone. Dates always start from 0001 year when 1st January is Monday.    
- Gear values: -1 = R, 0 = N, 1 = D1, 2 = D2, etc.
- Mass is expressed in kilograms
- Speed is expressed in m/sec (to convert it to km/sec just multiply it by 3.6)

Please note that GET responses may be cached by your HTTP client. To avoid caching you may use some random query string parameter or POST method which returns exactly the same result.

### Telemetry HTML5 Mobile Application
    http://localhost:25555/

This HTML5 gauge application is designed for mobile/desktop browsers running in landscape mode. You should be able to use the gauge just by navigating to the URL in your Mobile Safari (iOS 8+), Android 4+ browsers (Default or Chrome) or any modern desktop browser. **Android ETS2 mobile gauge application (APK) is also included!**  

Here is a screenshot of how your mobile gauge will look like in a browser:

![](https://raw.githubusercontent.com/Funbit/ets2-telemetry-server/master/Screenshot.png)

The gauge design is pretty customizable. All you have to do is to change style.less (and compile it to styles.min.css), index.html (there are some strings there and data attributes) and PNG files. No javascript changes required!

## Installation and Usage

### Supported OS

- Windows Vista, Windows 7 or Windows 8 (32-bit or 64-bit).
- .NET Framework 4.5 (pre-installed in Windows 8+). If it is not installed you will be prompted to install it when you run the server.

### Supported games

- Euro Truck Simulator 2 (32-bit or 64-bit). Multiplayer versions are supported as well. Steam version is preferred. 

### Supported browsers

- iOS 8+ running Mobile Safari
- Android 4+ Default or Chrome browsers
- Latest Firefox, Chrome or IE11

### Installation

**The following steps must be done only once! The installation is easy and will not take more than a couple of minutes!**

1. Download server bundle by clicking **Download ZIP** button at the right side of this page. 
2. **Unpack downloaded ZIP** file *anywhere* you want.
3. If you use Steam version of the ETS2 then run **plugins\Install-For-Steam-Ets2.bat** by right-clicking it and selecting "*Run as Administrator*" to install the telemetry plugin DLL to your game directory. *Proceed to the next step*. If you are a non-steam user or the installation script didn't work for you then you should copy ets2-telemetry.dll manually to your game's plugins directory: "Euro Truck Simulator 2\bin\win_x86\plugins" (win_x64 for 64-bit version). If plugins directory does not exist you must create it first. *Note: If you don't trust my compiled ets2-telemetry.dll you may compile it by yourself from [the official telemetry SDK](https://github.com/nlhans/ets2-sdk-plugin)*.
4. If you are going to connect to your server via Wi-Fi (from iOS or Android for example) then run **server\Add-25555-Rule-Local** shortcut to add an exception to your firewall to open 25555 port restricted to the local network (more secure). 
If you want to connect to your server from the Internet then use server\Add-25555-Rule-Internet shortcut instead.
5. Android users should install the provided "Ets2 Gauge" application. The APK file is located in **mobile/Android/Ets2Gauge.apk**. Copy it to your device and install via Android's File Manager. The main benefit of using the application instead of a browser is that it will prevent your device from going into sleep mode. Also, the application will remember entered server's IP address which is very useful if you use it frequently.

### Usage

1. Run **server/Ets2Telemetry.exe** (you have to run it *as Administrator* if you want to connect from other devices connected to your network). 
2. Run the game (the order is not important though).
3. **iOS users**: connect your iPhone or iPad to the same Wi-Fi network as your PC, open Safari and navigate to the "*ETS2 App URL*" displayed by the server. **Android users**: run "*Ets2 Gauge*" application, enter server IP (*without http part*) and press OK. If IP address is correct it will be remembered for the next time.
4. Enjoy your mobile gauge while playing your favorite simulator! ;)

### Known problems:

- Sometimes D1 gear is not properly displayed on the screen

## Version history

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