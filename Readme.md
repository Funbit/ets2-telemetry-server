## ETS2 Telemetry Web Server (Version 1.0.1)

This is a modern ETS2 Telemetry Web Server written in C# and WebApi. The server exposes the following endpoints:

### Telemetry REST API
  
    GET http://localhost:25555/api/ets2/telemetry

It returns IEts2TelemetryData JSON object with the latest telemetry data read from the game. The state is updated upon every API call. You may use this REST API for your own Applications. 

Please note that GET responses may be cached by your HTTP client. To avoid caching you may use some random query string parameter or use POST method instead.

### Telemetry HTML5 Mobile Application
    http://localhost:25555/apps/ets2/index.htm

This application is designed for mobile browsers running in landscape mode. You should open the URL in your Mobile Safari (iPhone 8+) or Android 4+ Browsers (Default or Chrome).  

Here is a screenshot of how your mobile gauge will look like:

![](https://raw.githubusercontent.com/Funbit/ets2-telemetry-server/master/Screenshot.png)

## Installation and Usage

### Supported OS and Devices

- Windows Vista, Windows 7 or Windows 8 (32-bit or 64-bit)
- Euro Truck Simulator 2 (**32-bit only for now**!)
- .NET Framework 4.5 (pre-installed in Windows 8+)
- iPhone OS 8+ (using built-in Mobile Safari) or Android 4+ devices (using Default or Chrome browser). **Warning:** since it is not possible to prevent Android device from going into sleep from a web page you must turn sleep mode off in the device's settings (or use third party apps that are capable of turning off sleep mode by application)

### Installation

1. Copy **plugins/ets2-telemetry.dll** to your **SteamApps\common\Euro Truck Simulator 2\bin\win_x86\plugins** directory. If **plugins** directory does not exist you must create it first. *Note: If you don't trust my compiled ets2-telemetry.dll you may compile it by yourself from [the official SDK](https://github.com/nlhans/ets2-sdk-plugin)*. 
2. Make sure that **25555** (default) port in opened in your Firewall (you may change the port number inside Ets2Telemetry.exe.config). If you are not sure how to configure the Firewall just run cmd.exe as Administrator and execute the following command: 

	`netsh advfirewall firewall add rule name="ETS2 TELEMETRY SERVER" dir=in action=allow protocol=TCP localport=25555 remoteip=localsubnet`

### Usage

1. Run **server/Ets2Telemetry.exe** (you have to run it **as Administrator** if you want to connect from other devices connected to your local network!). 
2. Run the game.
3. Connect your iPhone or Android to the same network as your PC (Wi-Fi for example), open *ETS2 App URL* in a browser and rotate device to landscape mode.
4. Enjoy your mobile gauge while playing your favorite simulator!

### Known problems:

- Sometimes due to browser bugs the gauge may not fit exactly in your mobile screen. If it happens to you the first thing you should try is to rotate the device to portrait mode and then back to landscape. Usually it fixes the problem.
- Sometimes D1 gear is not properly displayed on the screen
- [Sometimes day of the week displayed in the gauge is incorrect](https://github.com/Funbit/ets2-telemetry-server/issues/6)

## Version history

### 1.0.2
- Refactored gauge screen fitting algorithm, the app should work in any browser now (including desktop)

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