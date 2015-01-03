rem ==========================================
rem Telemetry Plugin Installer for Steam users
rem ==========================================

@echo off
setlocal

rem Steam path detector script is copied from:
rem http://stackoverflow.com/questions/12069300/batch-file-discovered-program-path-variable-run-program-from-discovered-re
regedit /e "%TEMP%\reg_exported.tmp" "HKEY_CURRENT_USER\Software\Valve\Steam"
find "SteamExe" "%TEMP%\reg_exported.tmp" | findstr "SteamExe" >> "%TEMP%\line_exported.tmp"
set /p SteamPath= < "%TEMP%\line_exported.tmp"
set SteamPath=%SteamPath:~11%
set SteamPath=%SteamPath:~1,-11%
del "%TEMP%\reg_exported.tmp"
del "%TEMP%\line_exported.tmp"

rem Plugin installation
set Ets2BinPath=%SteamPath%\SteamApps\common\Euro Truck Simulator 2\bin
set Ets2x86PluginPath=%Ets2BinPath%\win_x86\plugins
set Ets2x64PluginPath=%Ets2BinPath%\win_x64\plugins
echo "Discovered ETS2 path: %Ets2BinPath%"
if not exist "%Ets2x86PluginPath%" mkdir "%Ets2x86PluginPath%"
if not exist "%Ets2x64PluginPath%" mkdir "%Ets2x64PluginPath%"
echo "Copying 32-bit version of ets2-telemetry.dll..."
copy win_x86\plugins\ets2-telemetry.dll "%Ets2x86PluginPath%\ets2-telemetry.dll"
echo "Copying 64-bit version of ets2-telemetry.dll..."
copy win_x64\plugins\ets2-telemetry.dll "%Ets2x64PluginPath%\ets2-telemetry.dll"
echo "Done!"

endlocal
pause